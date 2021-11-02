using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace SymbolBuilder.Readers
{
    public class IbisPinReader : PinDataReader
    {
        public override string Name => "IBIS";

        public override string Filter => "IBIS (*.ibs)|*.ibs";

        public override string FileType => "*.ibs";

        public override bool CanRead(string fileName)
        {
            return File.Exists(fileName) && Path.GetExtension(fileName) == ".ibs";
        }

        public override List<Package> LoadFromStream(Stream stream, string fileName = null)
        {
            var ret = new List<Package>();

            // super simple, non-robust IBIS paser
            using (StreamReader file = new StreamReader(stream))
            {
                string component = "";
                string manufacturer = "";
                var pins = new List<Pin>();

                bool readingPins = false;

                Regex pinLine = new Regex("(?<Designator>\\w{1,4})\\s+(?<Name>\\S+)\\s+", RegexOptions.Compiled);

                while (file.Peek() >= 0)
                {
                    string line = file.ReadLine();
                    string lowerLine = line.ToLower();
                    if (line.Length == 0 || line[0] == '|')
                        continue;

                    if (!readingPins)
                    {
                        if (lowerLine.StartsWith("[component]"))
                        {
                            component = line.Split(']')[1].Trim();
                            continue;
                        }

                        if (lowerLine.StartsWith("[manufacturer]"))
                        {
                            manufacturer = line.Split(']')[1].Trim();
                            continue;
                        }

                        if (lowerLine.StartsWith("[pin]"))
                        {
                            readingPins = true;
                            continue;
                        }
                    }
                    else // reading pins
                    {
                        if (line[0] == '[')
                            break;

                        var match = pinLine.Match(line);
                        if (!match.Success)
                            continue;

                        if (match.Groups["Name"].Value.ToUpper() == "NC")
                            continue;

                        pins.Add(new Pin(match.Groups["Designator"].Value, match.Groups["Name"].Value));
                    }
                }

                Package dev = new Package(component);
                dev.Pins.AddRange(pins);
                ret.Add(dev);
            }


            return ret;
        }
    }
}
