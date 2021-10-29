using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AltiumSharp.Records;

namespace SymbolBuilder.Readers
{
    public abstract class TextPinReader : PinDataReader
    {
        protected abstract string[] DoGetColumns(StreamReader reader);

        protected abstract IEnumerable<string[]> DoGetRows(StreamReader reader);

        private bool IsValueEmpty(string value)
        {
            return string.IsNullOrWhiteSpace(value) || !Regex.IsMatch(value, @"^\s*\w+\s*$");
        }

        private static PinPosition? GetPinPosition(string[] columns, int index)
        {
            if (index < 0 || index >= columns.Length) return null;

            var text = columns[index].Trim();
            if (int.TryParse(text, out var clockDirection))
            {
                return PinPosition.From(clockDirection);
            }
            else
            {
                var parts = text.Split(new[] { '-', '/', ' ', ':' }, 2, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 0) return null;
                if (!Enum.TryParse<PinSide>(parts[0], true, out var side)) return null;
                if (parts.Length == 2 && Enum.TryParse<PinAlignment>(parts[1], true, out var alignment))
                {
                    return new PinPosition(side, alignment);
                }
                else
                {
                    return new PinPosition(side, PinAlignment.Middle);
                }
            }
        }

        private static PinElectricalType? GetPinType(string[] columns, int index)
        {
            if (index < 0 || index >= columns.Length) return null;

            var text = Regex.Replace(columns[index], @"/_-:\s", "");
            if (Enum.TryParse<PinElectricalType>(text, true, out var result))
            {
                return result;
            }

            text = text.ToUpperInvariant();
            if (text == "I" || text.StartsWith("IN")) return PinElectricalType.Input;
            if (text == "O" || text.StartsWith("OUT")) return PinElectricalType.Output;
            if (text == "IO") return PinElectricalType.InputOutput;
            if (text == "OPENDRAIN") return PinElectricalType.OpenCollector;

            return null;
        }

        public override List<Package> LoadFromStream(Stream stream, string fileName = null)
        {
            using var reader = new StreamReader(stream, Encoding.UTF8, true);

            var columns = DoGetColumns(reader).ToList();

            var ndxName = columns.FindIndex(c => c.StartsWith("pin", StringComparison.InvariantCultureIgnoreCase) || c.StartsWith("name", StringComparison.InvariantCultureIgnoreCase));
            if (ndxName == -1) ndxName = columns.Count - 1;
            var ndxFunction = columns.FindIndex(c => c.StartsWith("fun", StringComparison.InvariantCultureIgnoreCase));
            var ndxPosition = columns.FindIndex(c => c.StartsWith("pos", StringComparison.InvariantCultureIgnoreCase));
            var ndxType = columns.FindIndex(c => c.StartsWith("type", StringComparison.InvariantCultureIgnoreCase));

            bool IsPackageDesignator(int index) =>
                index != ndxName && index != ndxFunction && index != ndxPosition && index != ndxType;

            var packages = new List<Package>();
            for (int i = 0; i < columns.Count; i++)
            {
                if (!IsPackageDesignator(i)) continue;
                packages.Add(new Package(columns[i].Trim()));
            }

            foreach (var row in DoGetRows(reader))
            {
                var pinName = row[ndxName];
                var pinFunction = ndxFunction != -1 ? row[ndxFunction] : null;
                var pinPosition = GetPinPosition(row, ndxPosition);
                var pinType = GetPinType(row, ndxType);

                for (int i = 0; i < row.Length - 1; i++)
                {
                    var designator = row[i].Trim();
                    if (IsValueEmpty(designator)) continue;

                    packages[i].Pins.Add(new Pin(designator, pinName, pinFunction, pinPosition, pinType));
                }
            }

            return packages;
        }
    }
}