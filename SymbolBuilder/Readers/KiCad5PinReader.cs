using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace SymbolBuilder.Readers
{
    public class KiCad5PinReader : PinDataReader
    {
        public override string Name => "KiCAD Library";

        public override string Filter => "KiCAD Library (*.lib)|*.lib";

        public override string FileType => "*.lib";

        public override bool CanRead(string fileName)
        {
            return File.Exists(fileName) && Path.GetExtension(fileName) == ".lib";
        }

        public override List<Package> LoadFromStream(Stream stream, string fileName = null)
        {
            var ret = new List<Package>();

            // super simple, kicad lib parser
            using (StreamReader file = new StreamReader(stream))
            {
                string component = "";
                var pins = new List<Pin>();

                bool readingDef = false;
                bool readingPins = false;

                Regex defLine = new Regex("^DEF\\s+(?<Name>\\w+)\\s+", RegexOptions.Compiled);
                Regex pinLine = new Regex("^X\\s+(?<Name>\\S+)\\s+(?<Designator>\\w{1,4})\\s+", RegexOptions.Compiled);

                while (file.Peek() >= 0)
                {
                    string line = file.ReadLine();
                    string lowerLine = line.ToLower();
                    if (line.Length == 0 || line[0] == '#')
                        continue;

                    if (!readingDef)
                    {
                        if (lowerLine.StartsWith("def") && defLine.IsMatch(line))
                        {
                            var match = defLine.Match(line);

                            component = match.Groups["Name"].Value;
                            readingDef = true;

                            continue;
                        }
                    }
                    else // reading definition
                    {
                        if (lowerLine.StartsWith("enddraw"))
                        {
                            readingDef = false;
                            readingPins = false;


                            Package dev = new Package(component);
                            dev.Pins.AddRange(pins);
                            ret.Add(dev);
                            continue;
                        }

                        if (!readingPins)
                        {
                            if (lowerLine.StartsWith("draw"))
                            {
                                readingPins = true;
                            }
                        }
                        else
                        {
                            var match = pinLine.Match(line);
                            if (!match.Success)
                                continue;

                            if (match.Groups["Name"].Value.ToUpper() == "NC")
                                continue;

                            pins.Add(new Pin(match.Groups["Designator"].Value, match.Groups["Name"].Value));
                        }
                    }
                }

            }

            return ret;
        }
    }
}
