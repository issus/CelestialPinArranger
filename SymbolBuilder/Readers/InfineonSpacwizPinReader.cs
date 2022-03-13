using SymbolBuilder.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace SymbolBuilder.Readers
{
    public class ShiftJisFakeEecoding : EncodingProvider
    {
        public override Encoding GetEncoding(int codepage)
        {
            return null;
        }

        public override Encoding GetEncoding(string name)
        {
            if (name == null)
                return null;
            
            if (name.ToLower() == "shift-jis")
            {
                return Encoding.UTF8;
            }

            return null;
        }
    }

    public class InfineonSpacwizPinReader : PinDataReader
    {
        public override string Name => "Infineon Spacwiz XML";

        public override string Filter => "Infineon Spacwiz XML (*.xml)|*.xml";

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

                    while ((ln = file.ReadLine()) != null && counter < 6)
                    {
                        if (ln.Contains("MCU-Info</library>"))
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
            Encoding.RegisterProvider(new ShiftJisFakeEecoding());

            XmlDocument doc = new XmlDocument();
            doc.Load(stream);

            var mcu = doc.DocumentElement;

            string package = mcu.SelectSingleNode("/device/parameters/parameter[@name='packageType']/values/value").InnerText;
            string pinCount = mcu.SelectSingleNode("/device/parameters/parameter[@name='numberOfPins']/values/value").InnerText;
            string refName = mcu.SelectSingleNode("/device/displayName").InnerText;

            SymbolDefinition device = new SymbolDefinition(refName, "Infineon", $"{package}-{pinCount}");

            var pins = mcu.SelectNodes("//functionGroup[@name='PinAssignments']/functions/function");
            foreach (XmlNode pin in pins)
            {
                var signalsNodes = pin.SelectNodes("choices/choice[@name='SelectedChoice']/values/value");
                var signals = signalsNodes.Cast<XmlElement>().Select(n => n.InnerText).Where(n => n != "false" && n.ToLower() != "nc");
                string des = pin.Attributes["name"]?.Value;

                PinDefinition devicePin = new PinDefinition(des, signals.First());
                devicePin.AlternativeSignals.AddRange(signals.Skip(1).Select(n => new PinSignal(n)));

                device.SymbolBlocks.FirstOrDefault().Pins.Add(devicePin);
                device.CheckPinNames();
            }

            var list = new List<SymbolDefinition>();
            list.Add(device);
            return list;
        }
    }
}
