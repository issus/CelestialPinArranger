using AltiumSharp;
using SymbolBuilder.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SymbolBuilder.Readers
{
    public class SchLibPinReader : PinDataReader
    {
        public override string Name => "SchLib reader";
        public override string Filter => "Altium Schematic Symbol Library (*.SchLib)|*.schlib";
        public override string FileType => "*.schlib";
        public override bool CanRead(string fileName) =>
            Path.GetExtension(fileName).Equals(".schlib", StringComparison.InvariantCultureIgnoreCase);

        public override List<SymbolDefinition> LoadFromStream(Stream stream, string fn = null)
        {
            var reader = new SchLibReader();
            SchLib schLib;

            if (string.IsNullOrEmpty(fn))
                 schLib = reader.Read(stream);
            else
                schLib = reader.Read(fn);

            var result = new List<SymbolDefinition>();
            foreach (var component in schLib.Items)
            {
                var package = new SymbolDefinition(component.LibReference);
                foreach (var pin in
                    component.GetPrimitivesOfType<AltiumSharp.Records.SchPin>()
                        .Where(p => p.Name.ToUpperInvariant() != "NC")
                        .Select(p => new  { Designator = p.Designator, Name = p.Name, ElectricalType = (Model.PinElectricalType)p.Electrical })
                        .Distinct())
                {
                    string name = pin.Name;
                    List<PinSignal> altSignals = new List<PinSignal>();

                    if (pin.Name.Contains("/"))
                    {
                        var names = pin.Name.Split('/');
                        name = names[0];
                        altSignals.AddRange(names.Skip(1).Select(n => new PinSignal(n.Replace("\\", ""), n.Contains("\\"))));
                    }

                    package.SymbolBlocks.FirstOrDefault().Pins.Add(new PinDefinition(pin.Designator, name) { AlternativeSignals = altSignals });
                }

                package.CheckPinNames();
                result.Add(package);
            }

            return result;
        }
    }
}
