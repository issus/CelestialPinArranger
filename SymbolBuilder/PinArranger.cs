using AltiumSharp;
using AltiumSharp.BasicTypes;
using AltiumSharp.Records;
using SymbolBuilder.Mappers;
using SymbolBuilder.Readers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SymbolBuilder
{
    public class PinArranger
    {
        private PinMapper _pinMapper;
        private List<Package> _packages = new List<Package>();
        public IEnumerable<Package> Packages => _packages;

        public PinArranger(PinMapper pinMapper)
        {
            _pinMapper = pinMapper;
        }

        public void LoadFromFile(string fileName)
        {
            _packages.Clear();
            _packages.AddRange(PinDataReader.Load(fileName));
        }

        public void LoadFromText(string text)
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(text));
            LoadFromStream(stream);
        }

        public void LoadFromStream(Stream stream)
        {
            _packages.Clear();

            var reader = new DelimitedTextPinReader();
            _packages.AddRange(reader.LoadFromStream(stream));
        }

        /// <summary>
        /// Executes the pin mapping and arranges them in the schematic symbol for each
        /// type of packaging in the input.
        /// </summary>
        public SchLib Execute()
        {
            var components = _packages
                .Select(p => CreatePackageComponent(p));
            var schLib = new SchLib();
            foreach (var c in components) schLib.Add(c);
            return schLib;
        }

        /// <summary>
        /// Count number of positions where pins or spaces can exist in the symbol
        /// </summary>
        private int CountSlots(List<Pin> pins)
        {
            var pinCount = pins.Count;

            var functionSpacing = pins.OrderBy(p => p.Ordering)
                .GroupBy(p => p.Function)
                .Skip(1)
                .Sum(g => g.Key.FunctionSpacing);
            var groupSpacing = pins
                .GroupBy(p => p.Function)
                .Sum(g => g.Key.GroupSpacing * g.GroupBy(gg => gg.GroupName).Skip(1).Count());
            return pinCount + functionSpacing + groupSpacing;
        }

        /// <summary>
        /// Number of positions where pin or spaces can exist for pins of a given side
        /// </summary>
        private int CountSideSlots(Dictionary<PinSide, List<Pin>> pinsBySide, PinSide side)
        {
            return pinsBySide.TryGetValue(side, out var pins) ? CountSlots(pins) : 0;
        }

        /// <summary>
        /// Calculates text length approximation for pins of a given side
        /// </summary>
        private int CalcTextSideSize(Dictionary<PinSide, List<Pin>> pinsBySide, PinSide side)
        {
            return ((120 * (pinsBySide.TryGetValue(side, out var pins)
                ? pins.Select(p => (int)Math.Ceiling(p.NameClean.Length * 0.5)).DefaultIfEmpty().Max()
                : 0)) / 100) * 100;
        }

        /// <summary>
        /// Creates a component from the package information
        /// </summary>
        private SchComponent CreatePackageComponent(Package package)
        {
            foreach (var pin in package.Pins)
            {
                _pinMapper.Map(package.Name, pin);
            }

            // fix treating _N as active low when there is _P
            Regex findDiffPairP = new Regex("(?<PairName>[\\w-]+?)_P\\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            foreach (var pin in package.Pins.Where(p => findDiffPairP.IsMatch(p.Name)))
            {
                var pMatch = findDiffPairP.Match(pin.Name).Groups["PairName"].Value;
                var nName = string.Join("\\", pMatch.ToUpper().Replace("_P", "").ToArray()) + '\\';

                var nPin = package.Pins.FirstOrDefault(p => p.Name.ToUpper() == nName);
                if (nPin == null)
                    continue;

                nPin.UpdateName(nPin.Name.Replace(nName, pMatch + "_N"));
            }

            // Order non-port pins to make it less of a jumble
            foreach (var alignmentPins in package.Pins.Where(p => p.FunctionName.ToLower() != "port").GroupBy(p => p.Position?.Alignment))
            {
                int order = 0;
                foreach (var fn in alignmentPins.OrderByDescending(p => p.Name).GroupBy(p => p.FunctionName))
                {
                    foreach (var pin in fn)
                    {
                        pin.GroupIndex = order++;
                    }

                    order = 0;
                }

            }
            
            var pinsBySide = package.Pins.Where(p => p.Position.HasValue).GroupBy(p => p.Position.Value.Side)
            .ToDictionary(g => g.Key, g => g.OrderBy(p => p.Ordering).ToList());

            var hcount = Math.Max(CountSideSlots(pinsBySide, PinSide.Top), CountSideSlots(pinsBySide, PinSide.Bottom));
            var vcount = Math.Max(CountSideSlots(pinsBySide, PinSide.Left), CountSideSlots(pinsBySide, PinSide.Right));

            var pinSpacing = 100;
            var minWidth = 1000;
            var minHeight = 1000;

            var textWidth = 300 + CalcTextSideSize(pinsBySide, PinSide.Left) + CalcTextSideSize(pinsBySide, PinSide.Right);
            var textHeight = 300 + CalcTextSideSize(pinsBySide, PinSide.Top) + CalcTextSideSize(pinsBySide, PinSide.Bottom);

            var yIndent = textWidth > 0 ? Math.Max(CalcTextSideSize(pinsBySide, PinSide.Top), CalcTextSideSize(pinsBySide, PinSide.Bottom)) : 0;

            var width = new[] { minWidth, (hcount + 1) * pinSpacing, textWidth }.Max();
            var height = new[] { minHeight, (vcount + 1) * pinSpacing + yIndent * 2, textHeight }.Max();

            var schComponent = new SchComponent
            {
                Designator = { Text = "IC?" },
                LibReference = package.Name
            };

            schComponent.Add(new SchRectangle
            {
                Corner = new CoordPoint(Coord.FromMils(width), Coord.FromMils(height))
            });

            foreach (var kv in pinsBySide)
            {
                ArrangeSide(kv.Key, kv.Value, width, height, hcount, vcount, pinSpacing, yIndent, schComponent);
            }

            return schComponent;
        }

        /// <summary>
        /// Distributes pins of a side of the symbol and adds them to the given component
        /// </summary>
        /// <param name="side">Side to be processed</param>
        /// <param name="pins">Pins to be processed for the given side</param>
        /// <param name="width">Total width in mils of the symbol</param>
        /// <param name="height">Total height in mils of the symbol</param>
        /// <param name="hcount">Count of horizontal slots (pins + spacings)</param>
        /// <param name="vcount">Count of vertical slots (pins + spacings)</param>
        /// <param name="pinSpacing">Spacing distance between pins (and slots) measured in mils</param>
        /// <param name="yIndent">Vertical indent for symbols that have pins in ortogonal sides, so the pin text doesn't overlap</param>
        /// <param name="schComponent">Component in which to add the pins</param>
        private void ArrangeSide(PinSide side, List<Pin> pins, int width, int height, int hcount, int vcount, int pinSpacing, int yIndent,
            SchComponent schComponent)
        {
            int xMin = 0;
            int yMin = 0;
            int xMax = 0;
            int yMax = 0;
            int xStride = 0;
            int yStride = 0;
            int dirCount;
            var pinOrientation = TextOrientations.None;

            if (side == PinSide.Left)
            {
                yMin = height - pinSpacing - yIndent;
                yMax = yIndent;
                yStride = -pinSpacing;
                dirCount = vcount;
                pinOrientation = TextOrientations.Flipped;
            }
            else if (side == PinSide.Right)
            {
                xMin = width;
                yMin = height - pinSpacing - yIndent;
                yMax = yIndent;
                xMax = width;
                dirCount = vcount;
                yStride = -pinSpacing;
            }
            else if (side == PinSide.Top)
            {
                xMin = pinSpacing;
                yMin = height;
                xMax = width;
                yMax = height;
                xStride = pinSpacing;
                dirCount = hcount;
                pinOrientation = TextOrientations.Rotated;
            }
            else
            {
                xMin = pinSpacing;
                xMax = width;
                xStride = pinSpacing;
                dirCount = hcount;
                pinOrientation = TextOrientations.Flipped | TextOrientations.Rotated;
            }

            // iterate over each alignment option of the side in order (upper, middle, lower / left, center, right)
            var x = xMin;
            var y = yMin;
            foreach (var alignmentPins in pins.OrderBy(p => p.Ordering).GroupBy(p => p.Position?.Alignment))
            {
                if (alignmentPins.Key == PinAlignment.Middle)
                {
                    var slots = CountSlots(pins);
                    var spacingMid = (dirCount - slots) / 2;
                    x += xStride * spacingMid;
                    y += yStride * spacingMid;
                }
                else if (alignmentPins.Key == PinAlignment.Lower)
                {
                    // start from the bottom of the symbol moving up
                    x = xMax - xStride;
                    y = yMax - yStride;
                    xStride *= -1;
                    yStride *= -1;
                }

                var lastFunctionName = alignmentPins.FirstOrDefault()?.FunctionName;
                var lastGroupName = alignmentPins.FirstOrDefault()?.GroupName;

                foreach (var pin in alignmentPins)
                {
                    if (pin.Function.Name != lastFunctionName)
                    {
                        x += xStride * pin.Function.FunctionSpacing;
                        y += yStride * pin.Function.FunctionSpacing;
                    }
                    else if (pin.GroupName != lastGroupName)
                    {
                        x += xStride * pin.Function.GroupSpacing;
                        y += yStride * pin.Function.GroupSpacing;
                    }

                    pin.IsArranged = true;
                    schComponent.Add(new SchPin
                    {
                        Location = new CoordPoint(Coord.FromMils(x), Coord.FromMils(y)),
                        Designator = pin.Designator,
                        Name = pin.Name,
                        Electrical = pin.ElectricalType.GetValueOrDefault(PinElectricalType.Passive),
                        Orientation = pinOrientation
                    });

                    x += xStride;
                    y += yStride;
                    lastFunctionName = pin.Function.Name;
                    lastGroupName = pin.GroupName;
                }
            }
        }
    }
}
