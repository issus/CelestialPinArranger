using System.Text.RegularExpressions;
using AltiumSharp.Records;

namespace SymbolBuilder
{
    /// <summary>
    /// Class that represents a pin function configuration for a given IC package type
    /// and a given pin name.
    /// This allows mapping a pin with a pair of (package, name) to a pin function that defines
    /// a position in the schematic symbol for the pin and an electrical type of the pin.
    /// Also defines some other characteristics for each pin function block to account for
    /// possible layout options, including spacing between blocks and the ordering of the
    /// blocks themselves.
    /// </summary>
    public class PinFunction
    {
        public string Name { get; }
        public Regex PackageNameRegex { get; }
        public Regex PinNameRegex { get; }
        public PinPosition Position { get; }
        public int FunctionIndex { get; }
        public PinElectricalType ElectricalType { get; }
        public int FunctionSpacing { get; }
        public int GroupSpacing { get; }

        internal object Ordering => (Position.Side, Position.Alignment, FunctionIndex);

        public PinFunction(string name, Regex packageName, Regex pinNameRegex, PinPosition position, int functionIndex, PinElectricalType electricalType, int functionSpacing = 1, int groupSpacing = 1)
        {
            Name = name;
            PackageNameRegex = packageName;
            PinNameRegex = pinNameRegex;
            Position = position;
            FunctionIndex = functionIndex;
            ElectricalType = electricalType;
            FunctionSpacing = functionSpacing;
            GroupSpacing = groupSpacing;
        }
    }
}