using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace InfineonDaveTranslator.Data
{
    public enum Package
    { 
        VQFN,
        LQFP,
        TSSOP
    }

    public class MCU
    {
        private List<Pin> _pins = new List<Pin>();
        private static Regex nameFix = new Regex("^(?<Name>.+?)(?:\\s*/\\s*#\\d+)?$", RegexOptions.Compiled);

        public string? Id { get; set; }
        public string? Name { get; set; }
        public Package Package { get; set; }

        public IOrderedEnumerable<Pin> Pins 
        { 
            get 
            {
                return _pins.OrderBy(p => p.Designator);
            }
        }

        public int PinCount
        {
            get
            {
                return _pins.Count;
            }
        }

        public MCU(string id, string name, Package package)
        {
            Id = id;
            Name = name;
            Package = package;
        }
        public MCU()
        {

        }

        public override string ToString()
        {
            return $"{Name} [{Package}]";
        }

        public Pin? AddMcuPin(string designator, string name)
        {
            name = nameFix.Match(name).Groups["Name"].Value.ToUpper();
            Pin? pin;

            if (!_pins.Any(p => p.Designator.ToUpper() == designator.ToUpper()))
            {
                pin = new Pin(designator, name);
                _pins.Add(pin);
                return pin;
            }

            pin = _pins.FirstOrDefault(p => p.Designator == designator);
            if (pin == null)
                return null;

            if (!pin.AlternativeFunctions.Any(p => p == name) && pin.Name != name)
                pin.AlternativeFunctions.Add(name);

            return pin;
        }

        public void Save(string filePath)
        {
            var options = new JsonSerializerOptions();
            options.WriteIndented = true;
            var json = JsonSerializer.Serialize(this, options);

            using TextWriter writer = new StreamWriter(filePath);
            writer.Write(json);
            writer.Flush();
            writer.Close();
        }
    }

    public class Pin
    {
        public string Designator { get; set; }
        public string Name { get; set; }
        public List<string> AlternativeFunctions { get; } = new List<string>();

        public Pin(string designator, string name)
        {
            Designator = designator;
            Name = name;
        }
        public Pin()
        {

        }
        public override string ToString()
        {
            return $"{Designator}: {Name} {String.Join("/", AlternativeFunctions)}";
        }
    }
}
