using SymbolBuilder.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace SymbolBuilder.Readers
{
    public class CubeMXPinReader : PinDataReader
    {
        public override string Name => "STM32CubeMX XML";

        public override string Filter => "STM32CubeMX XML (*.xml)|*.xml";

        public override string FileType => "*.xml";

        public override bool CanRead(string fileName)
        {
            if (File.Exists(fileName))
            {
                // Read file using StreamReader. Reads file line by line  
                using (StreamReader file = new StreamReader(fileName))
                {
                    int counter = 0;
                    string ln;

                    while ((ln = file.ReadLine()) != null && counter < 2)
                    {
                        if (ln.Contains("http://mcd.rou.st.com/modules.php?name=mcu"))
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
            xmlnsManager.AddNamespace("st", "http://mcd.rou.st.com/modules.php?name=mcu");


            string package = mcu.Attributes["Package"]?.Value;
            string refName = mcu.Attributes["RefName"]?.Value;

            SymbolDefinition device = new SymbolDefinition(refName, "ST MICROELECTRONICS", package);
            
            var pins = mcu.SelectNodes("/st:Mcu/st:Pin", xmlnsManager);
            foreach (XmlNode pin in pins)
            {
                string name = pin.Attributes["Name"]?.Value;

                if (name.ToUpper() == "NC")
                    continue;

                string des = pin.Attributes["Position"]?.Value;
                string function = pin.Attributes["Type"]?.Value;

                var signals = pin.SelectNodes("st:Signal", xmlnsManager);

                PinDefinition devicePin = new PinDefinition(des, name);
                devicePin.AlternativeSignals.AddRange(signals.Cast<XmlNode>().Select(n => n.Attributes["Name"]?.Value).Where(n => n != "GPIO").Select(n => new PinSignal(n)));

                device.SymbolBlocks.FirstOrDefault().Pins.Add(devicePin);
                device.CheckPinNames();
            }

            var list = new List<SymbolDefinition> { device };
            return list;
        }

        public override async Task<List<SymbolDefinition>> LoadFromStreamAsync(Stream stream, string fileName = null)
        {
            // todo: async
            return LoadFromStream(stream, fileName);
        }
    }
}
