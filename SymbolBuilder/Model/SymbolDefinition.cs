using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SymbolBuilder.Mappers;
using System.Text.RegularExpressions;

namespace SymbolBuilder.Model
{
    // todo: ISerializable to save generic XML file out/read XML file in
    // todo: save as JSON/read from JSON

    /// <summary>
    /// Generic symbol description
    /// </summary>
    public class SymbolDefinition
    {
        static Regex findDiffPairP = new Regex("(?<PairName>[\\w-]+?)_P$", RegexOptions.IgnoreCase | RegexOptions.Compiled);


        /// <summary>
        /// Pin Mapper used to arrange pins, for use by final EDA output translator
        /// </summary>
        public PinMapper PinMapper { get; set; }

        /// <summary>
        /// Manufacturer of the part (e.g.: Analog Devices)
        /// </summary>
        public string Manufacturer { get; set; }
        /// <summary>
        /// Device package (e.g.: TSSOP-10)
        /// </summary>
        public string DevicePackage { get; set; }
        /// <summary>
        /// Manufacturer's part number
        /// </summary>
        public string PartNumber { get; set; }

        public List<SymbolBlock> SymbolBlocks { get; set; }

        public SymbolDefinition(string partNumber, string manufacturer = "", string package = "")
        {
            Manufacturer = manufacturer;
            DevicePackage = package;
            PartNumber = partNumber;

            SymbolBlocks = new List<SymbolBlock>();
            SymbolBlocks.Add(new SymbolBlock());
        }

        public SymbolDefinition()
        {
            SymbolBlocks = new List<SymbolBlock>();
            SymbolBlocks.Add(new SymbolBlock());
        }


        public IEnumerable<PinDefinition> Pins
        {
            get
            {
                return SymbolBlocks.SelectMany(s => s.Pins);
            }
        }

        /// <summary>
        /// Cleanup pin names, correct issues from importing pin data
        /// </summary>
        public void CheckPinNames()
        {
            // look for pins which might have alternative signal definitions within it's name
            foreach (var pin in Pins.Where(n => n.SignalName.Name.Contains('/')))
            {
                var alternatives = pin.SignalName.Name.Split('/');
                pin.SignalName.Name = alternatives[0];

                pin.AlternativeSignals.AddRange(alternatives.Skip(1).Select(n => new PinSignal(n)));
            }

            // change a diff pair (e.g.: net_p/net_n) from being active low on _n back
            ActiveLowToDiffPair(Pins.Select(p => p.SignalName));
            ActiveLowToDiffPair(Pins.SelectMany(p => p.AlternativeSignals));
        }

        private void ActiveLowToDiffPair(IEnumerable<PinSignal> source)
        {
            if (source == null)
                return;

            if (!source.Any())
                return;

            foreach (var pin in source.Where(p => p.Name.ToUpper().EndsWith("_P")))
            {
                var name = findDiffPairP.Match(pin.Name).Groups["PairName"].Value;

                // could be multiple pins which match, in the case of a muxed MCU for example
                ChangeActiveLowNameToDiffPair(name, Pins.Select(p => p.SignalName));
                ChangeActiveLowNameToDiffPair(name, Pins.SelectMany(p => p.AlternativeSignals));
            }
        }

        private static void ChangeActiveLowNameToDiffPair(string name, IEnumerable<PinSignal> source)
        {
            if (source == null)
                return;

            if (!source.Any())
                return;

            foreach (var match in source.Where(n => n.ActiveLow && n.Name == name))
            {
                match.Name = $"{match.Name}_n";
                match.ActiveLow = false;
            }
        }

        public override string ToString()
        {
            string ret = "";
            if (!string.IsNullOrEmpty(Manufacturer))
                ret = Manufacturer;

            if (!string.IsNullOrEmpty(PartNumber))
            {
                if (!string.IsNullOrEmpty(ret))
                    ret = ret + " " + PartNumber;
                else
                    ret = PartNumber;
            }

            if (!String.IsNullOrEmpty(DevicePackage))
            {
                if (!string.IsNullOrEmpty(ret))
                    ret = ret + $" [{DevicePackage}]";
                else
                    ret = DevicePackage;
            }

            return ret;
        }
    }
}
