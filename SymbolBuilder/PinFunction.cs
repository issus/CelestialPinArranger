using System;
using System.Text.RegularExpressions;
using AltiumSharp.Records;
using SymbolBuilder.Model;

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
    public class PinFunction : IComparable<PinFunction>
    {
        public string Name { get; }
        public Regex PackageNameRegex { get; }
        public Regex PinNameRegex { get; }
        public PinPosition Position { get; }
        public int OrderPriority { get; }
        public Model.PinElectricalType ElectricalType { get; }
        public int FunctionSpacing { get; }
        public int GroupSpacing { get; }
        public PinClass PinClass { get; set; }

        internal object Ordering => (Position.Side, Position.Alignment, OrderPriority);

        public PinFunction(string name, Regex packageName, PinClass pinClass, Regex pinNameRegex, PinPosition position, int orderPriority, Model.PinElectricalType electricalType, int functionSpacing = 1, int groupSpacing = 1)
        {
            Name = name;
            PackageNameRegex = packageName;
            PinNameRegex = pinNameRegex;
            Position = position;
            OrderPriority = orderPriority;
            ElectricalType = electricalType;
            FunctionSpacing = functionSpacing;
            GroupSpacing = groupSpacing;
            PinClass = pinClass;
        }

        public int CompareTo(PinFunction other)
        {
            if (other == null) return 1;

            return Name.CompareTo(other.Name);
        }

        public static bool operator >(PinFunction operand1, PinFunction operand2)
        {
            return operand1.CompareTo(operand2) > 0;
        }

        // Define the is less than operator.
        public static bool operator <(PinFunction operand1, PinFunction operand2)
        {
            return operand1.CompareTo(operand2) < 0;
        }

        // Define the is greater than or equal to operator.
        public static bool operator >=(PinFunction operand1, PinFunction operand2)
        {
            return operand1.CompareTo(operand2) >= 0;
        }

        // Define the is less than or equal to operator.
        public static bool operator <=(PinFunction operand1, PinFunction operand2)
        {
            return operand1.CompareTo(operand2) <= 0;
        }

        public override string ToString()
        {
            return $"[{PinClass}] {Name} :: {PinNameRegex}";
        }
    }
}