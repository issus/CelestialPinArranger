using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SymbolBuilder.Model
{
    /// <summary>
    /// Electrical type of the pin
    /// </summary>
    public enum PinElectricalType
    {
        Input = 0, InputOutput, Output, OpenCollector, Passive, HiZ, OpenEmitter, Power
    }

    /// <summary>
    /// Adornment of the pin
    /// </summary>
    public enum PinSymbol
    {
        None = 0,
        Dot = 1,
        RightLeftSignalFlow = 2,
        Clock = 3,
        ActiveLowInput = 4,
        AnalogSignalIn = 5,
        NotLogicConnection = 6,
        PostponedOutput = 8,
        OpenCollector = 9,
        HiZ = 10,
        HighCurrent = 11,
        Pulse = 12,
        Schmitt = 13,
        ActiveLowOutput = 17,
        OpenCollectorPullUp = 22,
        OpenEmitter = 23,
        OpenEmitterPullUp = 24,
        DigitalSignalIn = 25,
        ShiftLeft = 30,
        OpenOutput = 32,
        LeftRightSignalFlow = 33,
        BidirectionalSignalFlow = 34,
    }

    /// <summary>
    /// Class of pin, used for splitting symbols into multiple blocks
    /// </summary>
    public enum PinClass
    {
        /// <summary>
        /// Generic pin with no special context in the scope of the pin arranger
        /// </summary>
        Generic,
        /// <summary>
        /// IO Port - prioritise moving these to additional schematic blocks
        /// </summary>
        IOPort,
        /// <summary>
        /// Device power supply, prioritise keeping these on first schematic block
        /// </summary>
        PowerSupply,
        /// <summary>
        /// Chip setup/management/static configuration pins, prioritise keeping these on first schematic block
        /// </summary>
        ChipConfiguration
    }

    public class PinSignal
    {
        public string Name { get; set; }
        public bool ActiveLow { get; set; }

        public PinSignal(string name, bool activeLow)
        {
            Name = name;
            ActiveLow = activeLow;
        }

        public PinSignal(string name)
        {
            Name = name;
            ActiveLow = false;

            string lowerName = name.ToLower();

            if (lowerName.Contains("_n") || lowerName.Contains("_b") || lowerName.Contains("#") || lowerName.EndsWith("*") || lowerName.StartsWith("*") || lowerName.StartsWith("!"))
            {
                Name
                    .Replace("_n", "")
                    .Replace("_N", "")
                    .Replace("_b", "")
                    .Replace("_B", "")
                    .Replace("#", "")
                    .Trim('*').TrimStart('!').Trim();

                ActiveLow = true;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// Component pin information
    /// </summary>
    public class PinDefinition
    {
        /// <summary>
        /// Primary logical name for the pin, that will be displayed on the symbol (e.g.: VCC or SDIO)
        /// </summary>
        public PinSignal SignalName { get; set; }

        /// <summary>
        /// Alternative functions available for a pin (e.g.: On PB4 you might have USART1_TXD, PWM0, SPI1_SCK)
        /// </summary>
        public List<PinSignal> AlternativeSignals { get; set; }

        public string FullNameSlashed
        {
            get
            {
                if (AlternativeSignals.Any())
                {
                    return SignalName.Name + "/" + string.Join("/", AlternativeSignals.Select(n => n.Name));
                }
                else
                {
                    return SignalName.Name;
                }
            }
        }

        /// <summary>
        /// Designator on the package that the signal connects to (e.g.: 4 or B4)
        /// </summary>
        public string Designator { get; set; }

        /// <summary>
        /// If the pin is for a port, the port's designator (e.g.: PortB)
        /// </summary>
        public string Port { get; set; }
        /// <summary>
        /// If the pin is a port, the port's bit (e.g.: 12 for PB12)
        /// </summary>
        public int PortBit { get; set; }

        /// <summary>
        /// Mapping function for this pin
        /// </summary>
        public PinFunction MappingFunction { get; set; }

        private PinPosition? pinPosition;
        /// <summary>
        /// Pin position on the schematic
        /// </summary>
        public PinPosition? PinPosition
        {
            get
            {
                if (!pinPosition.HasValue)
                    return MappingFunction?.Position;

                return pinPosition;
            }
            set
            {
                pinPosition = value;
            }
        }

        /// <summary>
        /// Electrical type of the pin
        /// </summary>
        public PinElectricalType? ElectricalType 
        {
            get
            {
                return MappingFunction?.ElectricalType ?? PinElectricalType.Passive;
            }
        }

        /// <summary>
        /// Adornment of the pin symbol on the schematic symbol
        /// </summary>
        public PinSymbol? Symbol { get; set; }

        /// <summary>
        /// Define a new component pin
        /// </summary>
        /// <param name="signalName">Logical/signal name for the pin</param>
        /// <param name="designator">Designator on the device's package</param>
        public PinDefinition(string designator, string signalName)
        {
            SignalName = new PinSignal(signalName);
            AlternativeSignals = new List<PinSignal>();
            
            Designator = designator;

            PinPosition = Model.PinPosition.From(PinSide.Right, PinAlignment.Middle);
            Symbol = PinSymbol.None;
        }

        /// <summary>
        /// Define a new component pin
        /// </summary>
        /// <param name="signalName">Logical/signal name for the pin</param>
        /// <param name="designator">Designator on the device's package</param>
        /// <param name="pinPosition">Location of the pin on the schematic symbol, assuming it will be rectangular</param>
        /// <param name="electricalType">Electrical type of the pin</param>
        /// <param name="symbol">Adornment of the pin symbol on the schematic symbol</param>
        public PinDefinition(string designator, string signalName, PinPosition? pinPosition, PinElectricalType? electricalType = PinElectricalType.Passive, PinSymbol? symbol = PinSymbol.None)
        {
            SignalName = new PinSignal(signalName);
            AlternativeSignals = new List<PinSignal>();

            Designator = designator;
            
            PinPosition = pinPosition;
            Symbol = symbol;
        }

        public override string ToString()
        {
            return string.Format($"[{Designator}] {SignalName}");
        }
    }
}
