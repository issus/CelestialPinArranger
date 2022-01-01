using SymbolBuilder.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace SymbolBuilder.Readers
{
    public class SimplicityStudioPinReader : PinDataReader
    {
        class pinModule
        {
            public string Name { get; set; }
            public string Route { get; set; }
            public string PortBankIndex { get; set; }
            public string pinIndex { get; set; }

            public string FullName
            {
                get
                {
                    string fn = Name;
                    if (fn.EndsWith(Route))
                        fn = fn.Replace(Route, "").Trim('.').Trim('_');

                    fn = $"{fn}_{Route}";

                    return fn;
                }
            }
        }

        public override string Name => "SimplicityStudio 5 Device";

        public override string Filter => "SimplicityStudio 5 Device (*.device)|*.device";

        public override string FileType => "*.device";

        public override bool CanRead(string fileName)
        {
            if (File.Exists(fileName))
            {
                if (!File.Exists(Path.Combine(Path.GetDirectoryName(fileName), "PORTIO.portio")))
                {
                    return false;
                }

                // Read file using StreamReader. Reads file line by line  
                using (StreamReader file = new StreamReader(fileName))
                {
                    int counter = 0;
                    string ln;

                    while ((ln = file.ReadLine()) != null && counter < 2)
                    {
                        if (ln.Contains("http://www.silabs.com/ss/hwconfig/device.ecore"))
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
            if (fileName == null)
                throw new ArgumentException("fileName must be specified, this reader needs to read multiple files.");

            if (!File.Exists(Path.Combine(Path.GetDirectoryName(fileName), "PORTIO.portio")))
            {
                throw new FileNotFoundException("Missing PORTIO.portio file in the same directory as the .device file");
            }

            XmlDocument deviceDoc = new XmlDocument();
            XmlDocument portioDoc = new XmlDocument();

            XmlNamespaceManager devicensManager = new XmlNamespaceManager(deviceDoc.NameTable);
            devicensManager.AddNamespace("device", "http://www.silabs.com/ss/hwconfig/device.ecore");

            deviceDoc.Load(stream);


            XmlNamespaceManager portnsManager = new XmlNamespaceManager(portioDoc.NameTable);
            portnsManager.AddNamespace("portio", "http://www.silabs.com/ss/hwconfig/portio.ecore");

            portioDoc.Load(Path.Combine(Path.GetDirectoryName(fileName), "PORTIO.portio"));

            List<pinModule> pinModules = new List<pinModule>();

            // build module/alternative pin function list
            var modules = portioDoc.SelectNodes("//module");
            foreach (XmlNode module in modules)
            {
                var name = module.Attributes["name"]?.Value;

                var routes = module.SelectNodes("selector/route");
                foreach (XmlNode route in routes)
                {
                    var routeName = route.Attributes["name"]?.Value;

                    var locations = route.SelectNodes("location");
                    foreach (XmlNode location in locations)
                    {
                        pinModules.Add(new pinModule
                        {
                            Name = name,
                            Route = routeName,
                            PortBankIndex = location.Attributes["portBankIndex"]?.Value,
                            pinIndex = location.Attributes["pinIndex"]?.Value
                        });
                    }
                }
            }


            var mcu = deviceDoc.DocumentElement.FirstChild;
            var pinOut = mcu.SelectSingleNode("//pinOut");

            string refName = mcu.Attributes["label"]?.Value;
            string package = pinOut.Attributes["description"]?.Value.Replace(",", "");

            SymbolDefinition device = new SymbolDefinition(refName, "SI Labs", package);

            int maxPinNumber = 0;

            var pins = pinOut.ChildNodes;
            foreach (XmlNode pin in pins)
            {
                string name = pin.Attributes["defaultLabel"]?.Value;
                string des = pin.Attributes["number"]?.Value;
                string portBank = pin.Attributes["portBankIndex"]?.Value;
                string pinIndex = pin.Attributes["pinIndex"]?.Value;
                List<PinSignal> altFunctions = new List<PinSignal>();

                int pinNum = 0;
                if (Int32.TryParse(des, out pinNum))
                {
                    if (pinNum > maxPinNumber) maxPinNumber = pinNum;
                }

                if (!string.IsNullOrEmpty(portBank) && !string.IsNullOrEmpty(pinIndex))
                {
                    var signals = pinModules.Where(m => m.PortBankIndex == portBank && m.pinIndex == pinIndex);
                    altFunctions.AddRange(signals.Select(s => new PinSignal(s.FullName)));
                }

                PinDefinition devicePin = new PinDefinition(des, name) { AlternativeSignals = altFunctions };
                device.SymbolBlocks.FirstOrDefault().Pins.Add(devicePin);
            }

            var padDescription = pinOut.Attributes["padDescription"]?.Value;

            if (maxPinNumber != 0 && !string.IsNullOrEmpty(padDescription) && padDescription.Contains("GND"))
            {
                device.SymbolBlocks.FirstOrDefault().Pins.Add(new PinDefinition((maxPinNumber + 1).ToString(), padDescription));
            }

            var list = new List<SymbolDefinition>();

            device.CheckPinNames();

            list.Add(device);
            return list;
        }
    }
}
