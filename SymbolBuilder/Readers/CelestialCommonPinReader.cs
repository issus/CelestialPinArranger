using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using SymbolBuilder.Model;
using System.Threading.Tasks;
using System.Linq;

namespace SymbolBuilder.Readers
{
    public class CelestialCommonPinReader : PinDataReader
    {
        public class MCU
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public int Package { get; set; }
            public List<Pin> Pins { get; set; } = new List<Pin>();
        }
        public class Pin
        {
            public string Designator { get; set; }
            public string Name { get; set; }
            public List<string> AlternativeFunctions { get; set;  } = new List<string>();
        }

        public override string Name => "Celestial Common";

        public override string Filter => "Celestial Common (*.cljson)|*.cljson";

        public override string FileType => "*.cljson";

        public override bool CanRead(string fileName)
        {
            return File.Exists(fileName) && Path.GetExtension(fileName) == ".cljson";
        }

        public override List<SymbolDefinition> LoadFromStream(Stream stream, string fileName)
        {
            using TextReader reader = new StreamReader(stream);
            var jsonIn = reader.ReadToEnd();

            var json = JsonSerializer.Deserialize<MCU>(jsonIn);

            return ProcessJson(json);
        }

        public override async Task<List<SymbolDefinition>> LoadFromStreamAsync(Stream stream, string fileName = "")
        {
            var json = await JsonSerializer.DeserializeAsync<MCU>(stream);

            return ProcessJson(json);
        }

        private List<SymbolDefinition> ProcessJson(MCU json)
        {
            var ret = new List<SymbolDefinition>();

            var symbol = new SymbolDefinition(json.Name);
            symbol.SymbolBlocks.FirstOrDefault().Pins.AddRange(json.Pins.Select(p => new PinDefinition(p.Designator, p.Name) { AlternativeSignals = p.AlternativeFunctions.Select(s => new PinSignal(s)).ToList() }));

            ret.Add(symbol);
            return ret;
        }
    }
}
