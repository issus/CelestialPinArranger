using AltiumSharp;
using AltiumSharp.BasicTypes;
using AltiumSharp.Drawing;
using AltiumSharp.Records;
using SymbolBuilder.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace SymbolBuilder.Translators
{
    public class AltiumOutput : IEdaTranslator
    {
        public string ProgramName => "Altium Designer";

        int pinSpacing = 100;
        int characterWidth = 60;



        public void GenerateFile(IEnumerable<SymbolDefinition> symbols, string filePath)
        {
            using (var file = File.OpenWrite(filePath))
            {
                WriteStream(symbols, file);
                file.Close();
                file.Dispose();
            }
        }

        public IEnumerable<Bitmap> GeneratePreview(SymbolDefinition symbol, int sizeX = 1024, int sizeY = 1024)
        {
            var schLib = new SchLib();
            schLib.Add(CreateSchComponent(symbol));

            var renderer = new SchLibRenderer(schLib.Header, null)
            {
                BackgroundColor = Color.White
            };

            renderer.Component = schLib.Items[0];

            using (var image = new Bitmap(sizeX, sizeY))
            using (var g = Graphics.FromImage(image))
            {
                renderer.Render(g, sizeX, sizeY, true, false);

                return new List<Bitmap>() { image };
            }
        }

        public void WriteStream(IEnumerable<SymbolDefinition> symbols, Stream stream)
        {
            var writer = new SchLibWriter();

            SchLib lib = (SchLib)GenerateNativeType(symbols);

            writer.WriteStream(lib, stream);
        }

        public bool SupportsSymbol(SymbolDefinition symbol)
        {
            return true;
        }

        /// <summary>
        /// Generate an SchLib object for for the symbols
        /// </summary>
        /// <param name="symbols"></param>
        /// <returns></returns>
        public object GenerateNativeType(IEnumerable<SymbolDefinition> symbols)
        {
            var components = symbols.Select(p => CreateSchComponent(p));

            var schLib = new SchLib();

            foreach (var c in components)
            {
                schLib.Add(c);
            }

            return schLib;
        }


        /// <summary>
        /// Count number of positions where pins or spaces can exist in the symbol
        /// </summary>
        private int CountSlots(List<PinDefinition> pins)
        {
            var pinCount = pins.Count;

            var functionSpacing = pins.OrderBy(p => p.PortBit)
                .GroupBy(p => p.MappingFunction)
                .Skip(1)
                .Sum(g => g.Key.FunctionSpacing);
            var groupSpacing = pins
                .GroupBy(p => p.MappingFunction)
                .Sum(g => g.Key.GroupSpacing * g.GroupBy(gg => gg.Port).Skip(1).Count());
            return pinCount + functionSpacing + groupSpacing;
        }

        /// <summary>
        /// Number of positions where pin or spaces can exist for pins of a given side
        /// </summary>
        private int CountSideSlots(Dictionary<PinSide, List<PinDefinition>> pinsBySide, PinSide side)
        {
            return pinsBySide.TryGetValue(side, out var pins) ? CountSlots(pins) : 0;
        }

        /// <summary>
        /// Calculates text length approximation for pins of a given side
        /// </summary>
        private int CalcTextSideSize(Dictionary<PinSide, List<PinDefinition>> pinsBySide, PinSide side)
        {
            return 
                (
                    (120 * 
                        (pinsBySide.TryGetValue(side, out var pins)
                            ? pins.Select(p => (int)Math.Ceiling(p.FullNameSlashed.Length * 0.5)).DefaultIfEmpty().Max()
                            : 0
                        )
                    ) / 100
                ) * 100;
        }

        /// <summary>
        /// Creates a component from the package information
        /// </summary>
        private SchComponent CreateSchComponent(SymbolDefinition symbol)
        {
            var schComponent = new SchComponent
            {
                Designator = { Text = "IC?" },
                LibReference = $"{symbol.Manufacturer} {symbol.PartNumber} {symbol.DevicePackage}"
            };

            int partId = 1;
            foreach (var block in symbol.SymbolBlocks)
            {
                if (block != symbol.SymbolBlocks.First())
                {
                    schComponent.AddPart();
                    partId++;
                }

                Dictionary<PinPosition, List<SchPin>> positionedPins = new Dictionary<PinPosition, List<SchPin>>();

                var pinsByPosition = block.Pins.Where(p => p.PinPosition.HasValue).GroupBy(p => p.PinPosition);

                foreach (var position in pinsByPosition)
                {
                    var arranged = ArrangePosition(position.Key.Value, position.ToList(), partId);
                    if (arranged.Value.Any())
                    {
                        positionedPins.Add(arranged.Key, arranged.Value);
                    }
                }

                int leftSideTextWidth = positionedPins.Where(p => p.Key.Side == PinSide.Left).Any() ? positionedPins.Where(p => p.Key.Side == PinSide.Left).Max(p => p.Value.Max(v => v.Name.Where(c => c != '\\').Count()) * characterWidth) + 150 : 0;
                int rightSideTextWidth = positionedPins.Where(p => p.Key.Side == PinSide.Right).Any() ? positionedPins.Where(p => p.Key.Side == PinSide.Right).Max(p => p.Value.Max(v => v.Name.Where(c => c != '\\').Count()) * characterWidth) + 150 : 0;

                // set minium width for text
                leftSideTextWidth = leftSideTextWidth < 300 ? 300 : leftSideTextWidth;
                rightSideTextWidth = rightSideTextWidth < 300 ? 300 : rightSideTextWidth;

                // ensure pins will set on a grid square
                leftSideTextWidth += 100 - (leftSideTextWidth % 100);
                rightSideTextWidth += 200 - ((leftSideTextWidth + rightSideTextWidth) % 200);
                int horizontalPinOffset = (leftSideTextWidth + rightSideTextWidth) / 2;

                int minY = 0;
                int leftMinY = 0;
                int rightMinY = 0;

                // rough arrangement to get an idea of heights/widths
                foreach (var location in positionedPins.GroupBy(p => p.Key.Side))
                {
                    int xOffset = location.Key == PinSide.Left ? -horizontalPinOffset : horizontalPinOffset;

                    int yOffset = OffsetAlignment(location, PinAlignment.Upper, xOffset, 0);
                    yOffset = OffsetAlignment(location, PinAlignment.Middle, xOffset, yOffset);
                    yOffset = OffsetAlignment(location, PinAlignment.Lower, xOffset, yOffset);

                    if (minY > yOffset)
                        minY = yOffset;

                    if (location.Key == PinSide.Left && leftMinY > yOffset)
                        leftMinY = yOffset;

                    if (location.Key == PinSide.Right && rightMinY > yOffset)
                        rightMinY = yOffset;
                }

                // recalc min positions based on actual pins
                leftMinY = positionedPins.Any(p => p.Key.Side == PinSide.Left) ? (int)positionedPins.Where(p => p.Key.Side == PinSide.Left).Min(p => p.Value.Min(n => Utils.CoordToMils(n.Location.Y))) : 0;
                rightMinY = positionedPins.Any(p => p.Key.Side == PinSide.Right) ? (int)positionedPins.Where(p => p.Key.Side == PinSide.Right).Min(p => p.Value.Min(n => Utils.CoordToMils(n.Location.Y))) : 0;

                // left and right bottom blocks are probably not both aligned at the bottom of the symbol. Move the higher side down.
                if (leftMinY > rightMinY)
                {
                    // move bottom left pins down to bottom of symbol
                    var pins = positionedPins.Where(g => g.Key.Alignment == PinAlignment.Lower && g.Key.Side == PinSide.Left).SelectMany(p => p.Value);
                    if (pins.Any())
                    {
                        int yOffset = leftMinY - rightMinY;
                        foreach (var pin in pins)
                        {
                            pin.Location = CoordPoint.FromMils(Utils.CoordToMils(pin.Location.X), Utils.CoordToMils(pin.Location.Y) - yOffset);
                        }
                    }
                }
                else
                {
                    // move bottom right pins down to bottom of symbol
                    var pins = positionedPins.Where(g => g.Key.Alignment == PinAlignment.Lower && g.Key.Side == PinSide.Right).SelectMany(p => p.Value);
                    if (pins.Any())
                    {
                        int yOffset = rightMinY - leftMinY;
                        foreach (var pin in pins)
                        {
                            pin.Location = CoordPoint.FromMils(Utils.CoordToMils(pin.Location.X), Utils.CoordToMils(pin.Location.Y) - yOffset);
                        }
                    }
                }

                int totalPinHeight = (int)-positionedPins.Min(p => p.Value.Min(n => Utils.CoordToMils(n.Location.Y)));

                // todo: move everything up to centre it vertically

                int rectMinX = (int)positionedPins.Min(p => p.Value.Min(n => Utils.CoordToMils(n.Location.X)));
                int rectMaxX = (int)positionedPins.Max(p => p.Value.Max(n => Utils.CoordToMils(n.Location.X)));
                if (rectMinX == rectMaxX)
                {
                    // pins only on one side of sch symbol, calc text width
                    var pinsSide = positionedPins.Select(p => p.Key.Side).FirstOrDefault();
                    int textWidth = positionedPins.SelectMany(p => p.Value).Max(p => p.Name.Where(c => c != '\\').Count() * characterWidth);

                    if (pinsSide == PinSide.Left)
                    {
                        rectMaxX = rectMaxX + textWidth;
                    }
                    else
                    {
                        rectMinX = rectMaxX - textWidth;
                    }
                }

                SchRectangle rectangle = new SchRectangle
                {
                    Location = CoordPoint.FromMils(rectMinX, (int)positionedPins.Min(p => p.Value.Min(n => Utils.CoordToMils(n.Location.Y) - 100))),
                    Corner = CoordPoint.FromMils(rectMaxX, (int)positionedPins.Max(p => p.Value.Max(n => Utils.CoordToMils(n.Location.Y) + 100))),
                    OwnerPartId = partId
                };

                schComponent.Add(rectangle);

                foreach (var pin in positionedPins.SelectMany(p => p.Value))
                {
                    schComponent.Add(pin);
                }
            }

            return schComponent;
        }

        private static int OffsetAlignment(IGrouping<PinSide, KeyValuePair<PinPosition, List<SchPin>>> location, PinAlignment alignment, int xOffset, int yOffset)
        {
            var pins = location.FirstOrDefault(p => p.Key.Alignment == alignment).Value;
            if (pins != null && pins.Any())
            {
                OffsetPins(xOffset, yOffset, pins);

                return (int)pins.Min(p => Utils.CoordToMils(p.Location.Y)) - (alignment == PinAlignment.Lower ? 0 : 200);
            }

            return yOffset;
        }

        private static void OffsetPins(int xOffset, int yOffset, List<SchPin> pins)
        {
            foreach (var pin in pins)
            {
                pin.Location = new CoordPoint(Coord.FromMils(Utils.CoordToMils(pin.Location.X) + xOffset), Coord.FromMils(Utils.CoordToMils(pin.Location.Y) + yOffset));
            }
        }

        /// <summary>
        /// Layout pins in each position with correct spacing/order, converting PinDefinition to SchPin.
        /// </summary>
        /// <param name="position">Area getting arranged</param>
        /// <param name="pins">Pins to arrange</param>
        /// <param name="partId">OwnerPartId in the Altium symbol to assign to each pin</param>
        /// <returns></returns>
        private KeyValuePair<PinPosition, List<SchPin>> ArrangePosition(PinPosition position, IEnumerable<PinDefinition> pins, int partId)
        {
            KeyValuePair<PinPosition, List<SchPin>> arranged = new KeyValuePair<PinPosition, List<SchPin>>(position, new List<SchPin>());

            var pinOrientation = TextOrientations.None;

            int xStride = 0;
            int yStride = -pinSpacing;
            int xPos = 0;
            int yPos = 0;

            switch (position.Side)
            {
                case PinSide.Left:
                    pinOrientation = TextOrientations.Flipped;
                    break;
                case PinSide.Top:
                    yStride = 0;
                    xStride = pinSpacing;
                    pinOrientation = TextOrientations.Rotated;
                    break;
                case PinSide.Bottom:
                    yStride = 0;
                    xStride = pinSpacing;
                    pinOrientation = TextOrientations.Flipped | TextOrientations.Rotated;
                    break;
            }

            var groupedMappings = pins.GroupBy(p => p.MappingFunction);
            IOrderedEnumerable<IGrouping<PinFunction, PinDefinition>> pinMap;

            if (position.Alignment == PinAlignment.Lower)
                pinMap = groupedMappings.OrderByDescending(x => x.Key.OrderPriority);
            else
                pinMap = groupedMappings.OrderBy(x => x.Key.OrderPriority);

            foreach (var map in pinMap)
            {
                foreach (var port in map.GroupBy(p => p.Port))
                {
                    foreach (var pin in port.OrderBy(p => p.PortBit))
                    {
                        var schPin = new SchPin
                        {
                            Location = new CoordPoint(Coord.FromMils(xPos), Coord.FromMils(yPos)),
                            Designator = pin.Designator,
                            Name = GeneratePinName(pin),
                            Electrical = (AltiumSharp.Records.PinElectricalType)pin.ElectricalType.GetValueOrDefault(Model.PinElectricalType.Passive),
                            Orientation = pinOrientation,
                            OwnerPartId = partId
                        };
                        
                        arranged.Value.Add(schPin);

                        xPos += xStride;
                        yPos += yStride;
                    }

                    xPos += xStride * map.Key.GroupSpacing;
                    yPos += yStride * map.Key.GroupSpacing;
                }

                xPos += xStride * map.Key.FunctionSpacing;
                yPos += yStride * map.Key.FunctionSpacing;
            }

            return arranged;
        }


        private string AddActiveLowBars(string name)
        {
            return string.Join("\\", name.ToCharArray()) + "\\";
        }

        private string GeneratePinName(PinDefinition pin)
        {
            if (!pin.AlternativeSignals.Any())
            {
                return pin.SignalName.ActiveLow ? AddActiveLowBars(pin.SignalName.Name) : pin.SignalName.Name;
            }

            StringBuilder pinName = new StringBuilder();

            pinName.Append(pin.SignalName.ActiveLow ? AddActiveLowBars(pin.SignalName.Name) : pin.SignalName.Name);
            pinName.Append("/");

            pinName.Append(string.Join("/", pin.AlternativeSignals.Select(n => n.ActiveLow ? AddActiveLowBars(n.Name) : n.Name)));

            return pinName.ToString();
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
        /// <param name="yIndent">Vertical indent for symbols that have pins in orthogonal sides, so the pin text doesn't overlap</param>
        /// <param name="schComponent">Component in which to add the pins</param>
        private void ArrangeSide(PinSide side, List<PinDefinition> pins, int width, int height, int hcount, int vcount, int pinSpacing, int yIndent,
            SchComponent schComponent, int ownerId)
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
            foreach (var alignmentPins in pins.OrderBy(p => p.PortBit).GroupBy(p => p.PinPosition?.Alignment))
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

                var lastFunctionName = alignmentPins.FirstOrDefault()?.MappingFunction.Name;
                var lastGroupName = alignmentPins.FirstOrDefault()?.Port;

                foreach (var port in alignmentPins.GroupBy(p => p.Port).OrderBy(x => x.Key))
                {
                    foreach (var pin in port.OrderBy(p => p.PortBit))
                    {
                        if (pin.MappingFunction.Name != lastFunctionName)
                        {
                            x += xStride * pin.MappingFunction.FunctionSpacing;
                            y += yStride * pin.MappingFunction.FunctionSpacing;
                        }
                        else if (pin.Port != lastGroupName)
                        {
                            x += xStride * pin.MappingFunction.GroupSpacing;
                            y += yStride * pin.MappingFunction.GroupSpacing;
                        }

                        schComponent.Add(new SchPin
                        {
                            Location = new CoordPoint(Coord.FromMils(x), Coord.FromMils(y)),
                            Designator = pin.Designator,
                            Name = GeneratePinName(pin),
                            Electrical = (AltiumSharp.Records.PinElectricalType)pin.ElectricalType.GetValueOrDefault(Model.PinElectricalType.Passive),
                            Orientation = pinOrientation,
                            OwnerPartId = ownerId
                        });

                        x += xStride;
                        y += yStride;
                        lastFunctionName = pin.MappingFunction.Name;
                        lastGroupName = pin.Port;
                    }
                }
            }
        }
    }
}
