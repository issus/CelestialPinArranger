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

        public override List<Package> LoadFromStream(Stream stream, string fn = null)
        {
            var fileName = Path.GetTempFileName();
            bool createdTempFile = false;

            if (string.IsNullOrEmpty(fn))
            {
                using (var fs = File.OpenWrite(fileName))
                {
                    stream.CopyTo(fs);
                    createdTempFile = true;
                }
            }
            else
            {
                fileName = fn;
            }

            var reader = new AltiumSharp.SchLibReader();
            var schLib = reader.Read(fileName);
            var result = new List<Package>();
            foreach (var component in schLib.Items)
            {
                var package = new Package(component.LibReference);
                package.Pins.AddRange(
                    component.GetPrimitivesOfType<AltiumSharp.Records.SchPin>()
                        .Where(p => p.Name.ToUpperInvariant() != "NC")
                        .Select(p => new Pin(p.Designator, p.Name, pinElectricalType: p.Electrical))
                );
                result.Add(package);
            }

            if (createdTempFile)
            {
                File.Delete(fileName);
            }

            return result;
        }
    }
}
