using System;
using System.Collections.Generic;
using System.Linq;

namespace SymbolBuilder.Model
{
    /// <summary>
    /// Symbol part that would typically define pins arranged around a rectangle that can be placed in EDA software. A single symbol my contain multiple parts, e.g.: high pin count microcontrollers
    /// </summary>
    public class SymbolBlock
    {
        public List<PinDefinition> Pins { get; set; }
        public SymbolBlock()
        {
            Pins = new List<PinDefinition>();
        }
        public override string ToString()
        {
            return $"{Pins?.Count} Pins";
        }
    }
}
