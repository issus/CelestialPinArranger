using SymbolBuilder.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace SymbolBuilder.Readers
{
    public class RenesasSspPackPinReader : PinDataReader
    {
        public override string Name => "Renesas PinCfg XML";

        public override string Filter => "Renesas PinCfg XML (*.xml)|*.xml";

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

                    while ((ln = file.ReadLine()) != null && counter < 3)
                    {
                        if (ln.Contains("http://www.tasking.com/schema/pinmappings/v1.1"))
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

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("r", "http://www.tasking.com/schema/pinmappings/v1.1");
            

            string package = mcu.SelectSingleNode("/r:pinMappings/r:device/r:package", nsmgr).Attributes["name"]?.Value;
            string refName = mcu.SelectSingleNode("/r:pinMappings/r:device", nsmgr).Attributes["name"]?.Value;

            SymbolDefinition device = new SymbolDefinition(refName, "Renesas", package);

            var pins = mcu.SelectNodes("/r:pinMappings/r:device/r:package/r:pinLayout/r:pin", nsmgr);
            foreach (XmlNode pin in pins)
            {
                string name = pin.Attributes["ref"]?.Value.ToUpper();
                string des = pin.Attributes["name"]?.Value;

                var signals = mcu.SelectNodes($"//r:alt[@name='{name}']", nsmgr)
                    .Cast<XmlElement>()
                    .Where(p =>
                        !p.Attributes["id"].Value.ToUpper().StartsWith(name) &&
                        !p.Attributes["id"].Value.ToUpper().EndsWith($"{name}.{name}"))
                    .Select(p => 
                        p.Attributes["id"].Value
                        .ToUpper()
                        .Replace('.', '_')
                        .Replace($"_{name}", "")
                        );

                PinDefinition devicePin = new PinDefinition(des, name);
                devicePin.AlternativeSignals.AddRange(signals.Select(n => new PinSignal(n)));

                device.SymbolBlocks.FirstOrDefault().Pins.Add(devicePin);
                device.CheckPinNames();
            }

            var list = new List<SymbolDefinition>();
            list.Add(device);
            return list;
        }
    }
}
