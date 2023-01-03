using OriginalCircuit.BxlSharp;
using OriginalCircuit.BxlSharp.Types;
using SymbolBuilder.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SymbolBuilder.Readers
{
    public class BxlPinReader : PinDataReader
    {
        public override string Name => "BXL reader";
        public override string Filter => "UltraLibrarian File (*.bxl)|*.bxl";
        public override string FileType => "*.bxl";
        public override bool CanRead(string fileName) =>
            Path.GetExtension(fileName).Equals(".bxl", StringComparison.InvariantCultureIgnoreCase);

        public override List<SymbolDefinition> LoadFromStream(Stream stream, string fn = null)
        {
            string fileName;
            bool createdTempFile = false;
            if (string.IsNullOrEmpty(fn))
            {
                fileName = Path.GetTempFileName();
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
            List<SymbolDefinition> result = ProcessBxl(data);

            if (createdTempFile)
            {
                File.Delete(fileName);
            }

            return result;
        }

        public override async Task<List<SymbolDefinition>> LoadFromStreamAsync(Stream stream, string fn = null)
        {
            string fileName;
            bool createdTempFile = false;
            if (string.IsNullOrEmpty(fn))
            {
                fileName = Path.GetTempFileName();
                using (var fs = File.OpenWrite(fileName))
                {
                    createdTempFile = true;
                    await stream.CopyToAsync(fs);
                }
            }
            else
            {
                fileName = fn;
            }

            var data = await BxlDocument.ReadFromFileAsync(fileName, BxlFileType.FromExtension);
            List<SymbolDefinition> result = ProcessBxl(data);

            if (createdTempFile)
            {
                File.Delete(fileName);
            }

            return result;
        }

        private static List<SymbolDefinition> ProcessBxl(BxlDocument data)
        {
            var result = new List<SymbolDefinition>();

            foreach (var symbol in data.Symbols)
            {
                var package = new SymbolDefinition(symbol.Name);

                package.SymbolBlocks.FirstOrDefault().Pins.AddRange(
                    symbol.Data
                        .Where(d => d is LibPin &&
                            (d as LibPin).Name.Text.ToUpperInvariant() != "NC" &&
                            (d as LibPin).Name.Text.ToUpperInvariant() != "N/C" &&
                            (d as LibPin).Name.Text.ToUpperInvariant() != "DNC")
                        .Select(d => d as LibPin)
                        .Select(d => new PinDefinition(d.Designator.Text, d.Name.Text))
                    );

                package.CheckPinNames();
                result.Add(package);
            }

            return result;
        }
    }
}
