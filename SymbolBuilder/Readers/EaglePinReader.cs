using OriginalCircuit.EagleSharp.Model;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Linq;
using SymbolBuilder.Model;
using System.Threading.Tasks;

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

        public override List<SymbolDefinition> LoadFromStream(Stream stream, string fileName)
        {
            var ret = new List<SymbolDefinition>();

            var xs = new XmlSerializer(typeof(Eagle),"");

            Eagle lbr = (Eagle)xs.Deserialize(stream);

            if (lbr.Drawing.Library == null)
                return ret;

            foreach (var deviceSet in lbr.Drawing.Library.DeviceSets.DeviceSet)
            {
                string name = deviceSet.Name;
                foreach (var device in deviceSet.Devices.Device)
                {
                    SymbolDefinition dev = new SymbolDefinition(name, "", device.Package);

                    dev.SymbolBlocks.FirstOrDefault().Pins.AddRange(device.Connects.Connect.Select(o => new PinDefinition(o.Pad, o.Pin)));
                    
                    dev.CheckPinNames();
                    ret.Add(dev);
                }
            }

            return ret;
        }

        public override async Task<List<SymbolDefinition>> LoadFromStreamAsync(Stream stream, string fileName = null)
        {
            // todo: async
            return LoadFromStream(stream, fileName);
        }
    }
}
