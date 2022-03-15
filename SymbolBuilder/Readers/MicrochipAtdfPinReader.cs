using SymbolBuilder.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace SymbolBuilder.Readers
{
    public class MicrochipAtdfPinReader : PinDataReader
    {
        public override string Name => "Microchip Atdf";

        public override string Filter => "Microchip Atdf (*.atdf)|*.atdf";

        public override string FileType => "*.atdf";

        public override bool CanRead(string fileName)
        {
            if (File.Exists(fileName))
            {
                // Read file using StreamReader. Reads file line by line  
                using (StreamReader file = new StreamReader(fileName))
                {
                    int counter = 0;
                    string ln;

                    while ((ln = file.ReadLine()) != null && counter < 40)
                    {
                        if (ln.Contains("avr-tools-device-file"))
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
            var list = new List<SymbolDefinition>();

            XmlDocument doc = new XmlDocument();
            doc.Load(stream);

            var mcu = doc.DocumentElement;

            var variantNodes = mcu.SelectNodes("//variants/variant");
            foreach (XmlNode variant in variantNodes)
            {
                string ordercode = variant.Attributes["ordercode"]?.Value;
                string pack = variant.Attributes["package"]?.Value;
                string pinoutKey = variant.Attributes["pinout"]?.Value;

                SymbolDefinition device = new SymbolDefinition(ordercode, "Microchip", pack);

                var pinNodes = mcu.SelectNodes($"//pinouts/pinout[@name='{pinoutKey}']/pin");
                if (pinNodes.Count == 0)
                {
                    continue;
                }

                foreach (XmlNode pinNode in pinNodes)
                {
                    string name = pinNode.Attributes["pad"]?.Value;
                    string des = pinNode.Attributes["position"]?.Value;

                    if (name == "NC")
                        continue;

                    var signals = mcu.SelectNodes($"//signal[@pad='{name}']");

                    PinDefinition devicePin = new PinDefinition(des, name);

                    var pinList = new List<string>();

                    foreach (XmlNode signalNode in signals)
                    {
                        string function = signalNode.Attributes["function"]?.Value;
                        string group = signalNode.Attributes["group"]?.Value;
                        string index = signalNode.Attributes["index"]?.Value;

                        if (function == null)
                            function = group;

                        var instance = signalNode.ParentNode.ParentNode;
                        var instName = instance.Attributes["name"]?.Value;

                        if (instName.StartsWith("PORT"))
                            continue;

                        
                        pinList.Add($"{instName}_{group.Replace(instName, "")}{index}".Replace("__", "_"));
                    }

                    devicePin.AlternativeSignals.AddRange(pinList.Select(n => new PinSignal(n)));
                    device.SymbolBlocks.FirstOrDefault().Pins.Add(devicePin);
                }

                device.CheckPinNames();
                list.Add(device);
            }

            return list;
        }
    }
}
