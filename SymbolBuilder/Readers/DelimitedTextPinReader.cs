using System.Collections.Generic;
using System.IO;

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

        protected override IEnumerable<string[]> DoGetRows(StreamReader reader)
        {
            string row;
            while ((row = reader.ReadLine()) != null)
            {
                yield return row.Split(_separator);
            }
        }
    }
}