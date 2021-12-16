using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace SymbolBuilder.Readers
{
    public class IbisPinReader : PinDataReader
    {
        static Regex ibisReader = new Regex("\\[[Cc]omponent\\]\\s+(?<Component>[^\\r\\n]+)[ \\t]*(?:[\\r\\n]{1,2}[ \\t]*(?:[^\\[\\r\\n][^\\r\\n]*)?)*(?:[\\r\\n]{1,2}\\[Package Model\\][^\\r\\n]*)?(?:[\\r\\n]{1,2}\\[[Mm]anufacturer\\]\\s+(?<Manufacturer>[^\\r\\n]+)[ \\t]*)(?:[\\r\\n]{1,2}[ \\t]*(?:[^\\[\\r\\n][^\\r\\n]*)?)*(?:[\\r\\n]{1,2}\\[Package Model\\][^\\r\\n]*)?(?:[\\r\\n]{1,2}\\[[Pp]ackage\\][^\\r\\n]*(?:[\\r\\n]{1,2}[ \\t]*(?:[^\\[\\r\\n][^\\r\\n]*)?)*)+(?:[\\r\\n]{1,2}\\[Package Model\\][^\\r\\n]*)?[\\r\\n]{1,2}\\[[Pp][Ii][Nn]]\\s*signal_name\\s+model_name[^\\r\\n]*(?:[\\r\\n]{1,2}[ \\t]*(?:[^\\w\\r\\n][^\\r\\n]*)?)*(?:[\\r\\n]{1,2}[ \\t]*(?:\\|[^\\r\\n]*)?|(?<PinDef>(?<Pin>\\w+)\\s+(?<Signal>\\S+)[^\\r\\n]*))*(?:[\\r\\n]{1,2}[ \\t]*(?:[^\\[\\r\\n][^\\r\\n]*)?)*(?:[\\r\\n]{1,2}[\\t ]*\\[)", RegexOptions.Compiled);

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

            // regex has been tested on every microchip, onsemi, analog devices IBIS file I could find... seems to work.
            using (StreamReader file = new StreamReader(stream))
            {
                var pins = new List<Pin>();

                string fileContents = file.ReadToEnd();
                var matches = ibisReader.Matches(fileContents);

                foreach (Match match in matches)
                {
                    Package pack = new Package(match.Groups["Component"].Value);
                    int pinCount = match.Groups["Pin"].Captures.Count;
                    for (int i = 0; i < pinCount; i++)
                    {
                        if (match.Groups["Signal"].Captures[i].Value.ToLower() == "nc")
                            continue;

                        pack.Pins.Add(new Pin(match.Groups["Pin"].Captures[i].Value, match.Groups["Signal"].Captures[i].Value));
                    }
                    ret.Add(pack);
                }
            }

            return ret;
        }
    }
}
