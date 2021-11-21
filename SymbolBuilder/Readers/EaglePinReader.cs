using EagleSharp.Model;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Linq;

namespace SymbolBuilder.Readers
{
    public class EaglePinReader : PinDataReader
    {
        public override string Name => "Eagle Library";

        public override string Filter => "Eagle Library (*.lbr)|*.lbr";

        public override string FileType => "*.lbr";

        public override bool CanRead(string fileName)
        {
            return File.Exists(fileName) && Path.GetExtension(fileName) == ".lbr";
        }

        public override List<Package> LoadFromStream(Stream stream, string fileName)
        {
            var ret = new List<Package>();

            var xs = new XmlSerializer(typeof(Eagle),"");

            Eagle lbr = (Eagle)xs.Deserialize(stream);

            if (lbr.Drawing.Library == null)
                return ret;

            foreach (var deviceSet in lbr.Drawing.Library.DeviceSets.DeviceSet)
            {
                string name = deviceSet.Name;
                foreach (var device in deviceSet.Devices.Device)
                {
                    Package dev = new Package($"{name} {device.Package}");

                    dev.Pins.AddRange(device.Connects.Connect.Select(o => new Pin(o.Pad, o.Pin)));
                    ret.Add(dev);
                }
            }

            return ret;
        }
    }
}
