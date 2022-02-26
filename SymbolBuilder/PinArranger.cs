using AltiumSharp;
using AltiumSharp.BasicTypes;
using AltiumSharp.Records;
using SymbolBuilder.Mappers;
using SymbolBuilder.Model;
using SymbolBuilder.Readers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SymbolBuilder.Translators;

namespace SymbolBuilder
{
    public class PinArranger
    {
        private PinMapper _pinMapper;
        private List<SymbolDefinition> _symbols = new List<SymbolDefinition>();
        public IEnumerable<SymbolDefinition> Symbols => _symbols;

        public PinArranger(PinMapper pinMapper)
        {
            _pinMapper = pinMapper;
        }

        public void LoadFromFile(string fileName)
        {
            _symbols.Clear();
            _symbols.AddRange(PinDataReader.Load(fileName));
        }

        public void LoadFromText(string text)
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(text));
            LoadFromStream(stream);
        }

        public void LoadFromStream(Stream stream)
        {
            _symbols.Clear();

            var reader = new DelimitedTextPinReader();
            _symbols.AddRange(reader.LoadFromStream(stream));
        }

        /// <summary>
        /// Executes the pin mapping and arranges them in the schematic symbol for each
        /// type of packaging in the input.
        /// </summary>
        public List<SymbolDefinition> Execute()
        {
            _symbols.ForEach(a => ArrangeSymbol(a));

            return _symbols;
        }

        private SymbolDefinition ArrangeSymbol(SymbolDefinition symbol)
        {
            foreach (var pin in symbol.Pins)
            {
                _pinMapper.Map(symbol.DevicePackage, pin);
            }

            // debug point
            var nullMapping = symbol.Pins.Where(p => p.MappingFunction == null);
            if (nullMapping.Any())
            {
                int i = 0;
            }

            var nonIoPins = symbol.Pins.Where(p => p.MappingFunction != null && p.MappingFunction.PinClass != PinClass.IOPort);

            if (nonIoPins.Any())
            {
                // Order non-port pins to make it less of a jumble
                foreach (var alignmentPins in nonIoPins.GroupBy(p => p.MappingFunction.Position.Alignment))
                {
                    int order = 0;
                    foreach (var fn in alignmentPins.OrderByDescending(p => p.SignalName.Name).GroupBy(p => p.MappingFunction.Name))
                    {
                        foreach (var pin in fn)
                        {
                            pin.PortBit = order++;
                        }

                        order = 0;
                    }
                }
            }
            else
            {
                // debug point
                int i = 0;
            }

            // Break symbol into blocks if needed
            if (symbol.Pins.Where(p => p.MappingFunction.PinClass == PinClass.IOPort).Count() > 16 && symbol.Pins.Where(p => p.PinPosition?.Side == PinSide.Right).Count() > 24)
            {
                // in Altium, 32 grid squares (3200mil) seems to be a reasonable maximum height for a rectangle if fitting on an A4/Letter schematic sheet.
                // Try to match blocks to be less than 30 or so pins to allow for spaces between pin groups/functions

                int ioPortPinCount = symbol.Pins.Where(p => p.MappingFunction.PinClass == PinClass.IOPort).Count();
                int powerPinCount = symbol.Pins.Where(p => p.MappingFunction.PinClass == PinClass.PowerSupply).Count();
                int configPinCount = symbol.Pins.Where(p => p.MappingFunction.PinClass == PinClass.ChipConfiguration).Count();

                // work on the principle of cascading schematic blocks

                var firstBlock = symbol.SymbolBlocks.FirstOrDefault();

                // should first block be just power/config?
                if (powerPinCount >= 24)
                {
                    var newBlock = new SymbolBlock();

                    // move all config pins to right side of first schematic block.
                    foreach (var pin in symbol.Pins.Where(p => p.MappingFunction.PinClass == PinClass.ChipConfiguration))
                    {
                        pin.PinPosition = PinPosition.From(PinSide.Right, pin.PinPosition.Value.Alignment);
                    }

                    // move all other pins to a new block = first block will be just power/config
                    var pinsToMove = symbol.Pins.Where(p => p.MappingFunction.PinClass != PinClass.ChipConfiguration && p.MappingFunction.PinClass != PinClass.PowerSupply).ToList();
                    foreach (var pin in pinsToMove)
                    {
                        firstBlock.Pins.Remove(pin);
                        newBlock.Pins.Add(pin);
                    }

                    symbol.SymbolBlocks.Add(newBlock);
                }

                /// break ports out to new schematic symbol blocks
                while (symbol.SymbolBlocks.Last().Pins.Where(p => p.PinPosition.Value.Side == PinSide.Right).Count() > 30 &&
                       symbol.SymbolBlocks.Last().Pins.Where(p => p.MappingFunction.PinClass == PinClass.IOPort).GroupBy(p => p.Port).Count() > 1)
                {
                    var newBlock = new SymbolBlock();
                    var lastBlock = symbol.SymbolBlocks.Last();

                    int blockPinCount = symbol.SymbolBlocks.Last().Pins.Where(p => p.MappingFunction.PinClass != PinClass.IOPort && p.PinPosition.Value.Side == PinSide.Right).Count();

                    var pinGroups = symbol.SymbolBlocks.Last().Pins.Where(p => p.MappingFunction.PinClass == PinClass.IOPort).GroupBy(p => p.Port).OrderBy(g => g.Key).ToList();

                    int pinCount = blockPinCount;
                    bool maxPinsReached = false;

                    foreach (var group in pinGroups)
                    {
                        if (!maxPinsReached)
                        {
                            if (pinCount + group.Count() > 30)
                            {
                                maxPinsReached = true;
                            }
                            else
                            {
                                pinCount += group.Count();
                                continue;
                            }
                        }

                        if (maxPinsReached && pinCount == 0)
                        {
                            // add this port, even if it blows the symbol size limit. Ports can be larger than max count (especially on 32/64bit devices)
                            // if all ports on device are > 30 pin, this would just generate empty blocks without this
                            pinCount += group.Count();
                            continue;
                        }
                        else if (maxPinsReached)
                        { 
                            foreach (var pin in group)
                            {
                                lastBlock.Pins.Remove(pin);
                                newBlock.Pins.Add(pin);
                            }
                        }
                    }

                    if (newBlock.Pins.Count > 0)
                        symbol.SymbolBlocks.Add(newBlock);
                }
            }

            return symbol;
        }
    }
}
