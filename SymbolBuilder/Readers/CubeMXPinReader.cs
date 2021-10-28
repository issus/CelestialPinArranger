using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public override List<Package> LoadFromStream(Stream stream)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(stream);

            var mcu = doc.DocumentElement;

            XmlNamespaceManager xmlnsManager = new XmlNamespaceManager(doc.NameTable);
            xmlnsManager.AddNamespace("st", "http://mcd.rou.st.com/modules.php?name=mcu");


            string package = mcu.Attributes["Package"]?.Value;
            string refName = mcu.Attributes["RefName"]?.Value;

            Package device = new Package(package);
            

            var pins = mcu.SelectNodes("/st:Mcu/st:Pin", xmlnsManager);
            foreach (XmlNode pin in pins)
            {
                string name = pin.Attributes["Name"]?.Value;
                string des = pin.Attributes["Position"]?.Value;
                string function = pin.Attributes["Type"]?.Value;

                var signals = pin.SelectNodes("st:Signal", xmlnsManager);
                var altFunctions = string.Join("/", signals.Cast<XmlNode>().Select(n => n.Attributes["Name"]?.Value));

                Pin devicePin = new Pin(des, $"{name}/{altFunctions}".Trim('/'));
                device.Pins.Add(devicePin);
            }

            var list = new List<Package>();
            list.Add(device);
            return list;
        }
    }
}
