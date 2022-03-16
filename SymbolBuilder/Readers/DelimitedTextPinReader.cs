using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SymbolBuilder.Readers
{
    public class DelimitedTextPinReader : TextPinReader
    {
        private char _separator;

        private static char DetectSeparator(string header)
        {
            if (header.Contains("\t"))
            {
                return '\t';
            }

            if (header.Contains(";"))
            {
                return ';';
            }

            if (header.Contains(","))
            {
                return ',';
            }

            return ' ';
        }

        public override string Name => "Delimited text reader";
        public override string Filter => "Comma-separated values (*.csv)|*.csv|Tab-separated values (*.tsv)|*.tsv";
        public override string FileType => "*.csv,*.tsv";

        public override bool CanRead(string fileName)
        {
            var ext = Path.GetExtension(fileName);
            return ext == ".csv" || ext == ".tsv";
        }

        protected override string[] DoGetColumns(StreamReader reader)
        {
            var header = reader.ReadLine() ?? string.Empty;
            _separator = DetectSeparator(header);
            return header.Split(_separator);
        }

        protected override async Task<string[]> DoGetColumnsAsync(StreamReader reader)
        {
            var header = (await reader.ReadLineAsync()) ?? string.Empty;
            _separator = DetectSeparator(header);
            return header.Split(_separator);
        }

        protected override IEnumerable<string[]> DoGetRows(StreamReader reader)
        {
            string row;
            while ((row = reader.ReadLine()) != null)
            {
                yield return row.Split(_separator);
            }
        }

        protected override async Task<IEnumerable<string[]>> DoGetRowsAsync(StreamReader reader)
        {
            List<string[]> rows = new List<string[]>();
            string row;
            while ((row = await reader.ReadLineAsync()) != null)
            {
                rows.Add(row.Split(_separator));
            }

            return rows;
        }
    }
}