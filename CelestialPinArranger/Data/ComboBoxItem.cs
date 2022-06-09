using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelestialPinArranger.Data
{
    internal class ComboBoxItem
    {
        public ComboBoxItem(string display, object value)
        {
            Display = display;
            Value = value;
        }

        public string Display { get; set; }
        public Object Value { get; set; }
    }
}
