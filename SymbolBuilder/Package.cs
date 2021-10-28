using System;
using System.Collections.Generic;
using System.Text;

namespace SymbolBuilder
{
    public class Package
    {
        public string Name { get; }
        public List<Pin> Pins { get; } = new List<Pin>();

        public Package(string name)
        {
            Name = name;
        }
    }
}
