using SymbolBuilder.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace SymbolBuilder.Readers
{
    public class NxpMcuXpressoPinReader : PinDataReader
    {
        public override string Name => "MCUXpresso XML";

        public override string Filter => "MCUXpresso XML (signal_configuration.xml)|signal_configuration.xml";

        public override string FileType => "signal_configuration.xml";

        public override bool CanRead(string fileName)
        {
            if (File.Exists(fileName) && fileName.Contains("signal_configuration"))
            {
                // Read file using StreamReader. Reads file line by line  
                using (StreamReader file = new StreamReader(fileName))
                {
                    int counter = 0;
                    string ln;

                    while ((ln = file.ReadLine()) != null && counter < 2)
                    {
                        if (ln.Contains("pinsmodel:signal_configuration"))
                            return true;

                        counter++;
                    }
                    file.Close();
                }
            }

            return false;
        }

        public override List<SymbolDefinition> LoadFromStream(Stream stream, string fileName = null)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(stream);

            var mcu = doc.DocumentElement;

            XmlNamespaceManager xmlnsManager = new XmlNamespaceManager(doc.NameTable);
            xmlnsManager.AddNamespace("ns", "http://swtools.freescale.net/Pins/PinsModel");


            var partInfo = mcu.SelectSingleNode("//part_information/part_number", xmlnsManager);
            string refName = partInfo.Attributes["id"]?.Value;
            string package = partInfo.FirstChild.InnerText;


            SymbolDefinition device = new SymbolDefinition(refName, "NXP", package);

            Regex ncCheck = new Regex("^NC(?:\\d+)?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            var pins = mcu.SelectNodes("//pins/pin", xmlnsManager);
            foreach (XmlNode pin in pins)
            {
                string name = pin.Attributes["name"]?.Value;
                string des = pin.Attributes["coords"]?.Value;

                if (ncCheck.IsMatch(name))
                    continue;

                device.SymbolBlocks.FirstOrDefault().Pins.Add(new PinDefinition(des, name));
            }

            var list = new List<SymbolDefinition>();

            device.CheckPinNames();
            list.Add(device);
            return list;
        }

        public override async Task<List<SymbolDefinition>> LoadFromStreamAsync(Stream stream, string fileName = null)
        {
            // todo: async
            return LoadFromStream(stream, fileName);
        }
    }
}
