using BxlSharp;
using BxlSharp.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SymbolBuilder.Readers
{
    public class BxlPinReader : PinDataReader
    {

        public override string Name => "BXL reader";
        public override string Filter => "UltraLibrarian File (*.bxl)|*.bxl";
        public override string FileType => "*.bxl";
        public override bool CanRead(string fileName) =>
            Path.GetExtension(fileName).Equals(".bxl", StringComparison.InvariantCultureIgnoreCase);

        public override List<Package> LoadFromStream(Stream stream, string fn = null)
        {
            var fileName = Path.GetTempFileName();
            bool createdTempFile = false;
            if (string.IsNullOrEmpty(fn))
            {
                using (var fs = File.OpenWrite(fileName))
                {
                    createdTempFile = true;
                    stream.CopyTo(fs);
                }
            }
            else
            {
                fileName = fn;
            }

            var data = BxlDocument.ReadFromFile(fileName, BxlFileType.FromExtension, out var logs);

            var result = new List<Package>();

            foreach (var symbol in data.Symbols)
            {
                var package = new Package(symbol.Name);

                package.Pins.AddRange(
                    symbol.Data
                        .Where(d => d is LibPin && 
                            (d as LibPin).Name.Text.ToUpperInvariant() != "NC" &&
                            (d as LibPin).Name.Text.ToUpperInvariant() != "DNC")
                        .Select(d => d as LibPin)
                        .Select(d => new Pin(d.Designator.Text, d.Name.Text))
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
