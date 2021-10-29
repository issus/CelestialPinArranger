using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AltiumSharp.Records;

namespace SymbolBuilder
{
    public class Pin
    {
        public string Designator { get; }
        public string Name { get; private set;  }
        public string FunctionName { get; internal set; }
        public PinPosition? Position { get; internal set; }
        public PinElectricalType? ElectricalType { get; internal set; }
        
        internal PinFunction Function { get; set; }
        internal string GroupName { get; set; }
        internal int GroupIndex { get; set; }

        internal string NameOriginal { get; }
        internal string NameClean => Name?.Replace(@"\", "") ?? string.Empty;
        internal bool IsArranged { get; set; }

        internal object Ordering => (Function?.Ordering, GroupName, GroupIndex);

        public Pin(string designator, string name, string functionName = null, PinPosition? position = null, PinElectricalType? pinElectricalType = null)
        {
            Designator = designator;
            NameOriginal = name;
            FunctionName = functionName;
            Position = position;
            ElectricalType = pinElectricalType;

            Name = CleanupName(name);
        }

        public void UpdateName(string newName)
        {
            Name = newName;
        }

        internal static string CleanupName(string name)
        {
            name = name.Replace(", ", "/").Replace(',', '/').Replace(" ", "").Trim();

            // fix STM32 port-osc notation "PH0- OSC_IN(PH0)"
            var portOscFix = new Regex(@"(?<Port>P[A-Z]\d+)-\s?(?<Osc>OSC\d*_\w+)\(.+?\)");
            if (portOscFix.IsMatch(name))
            {
                name = portOscFix.Replace(name, "${Port}/${Osc}");
            }

            // replace superscript notes like "(4)"
            var notationFix = new Regex(@"\(\d+\)");
            if (notationFix.IsMatch(name))
            {
                name = notationFix.Replace(name, "");
            }

            // replace Exposed Pad notes like "EP(GND)"
            var epFix = new Regex(@"EP\((?<Net>\w+)\)");
            if (epFix.IsMatch(name))
            {
                name = epFix.Replace(name, "${Net}");
            }

            // fix port(func) such as "PA15(JTDI)"
            var portOtherFuncFix = new Regex(@"(?<Port>P[A-Z]\d+)\((?<Func>[^)]+)\)");
            if (portOtherFuncFix.IsMatch(name))
            {
                name = portOtherFuncFix.Replace(name, "${Port}/${Func}");
            }

            var segments = name.Split('/');
            var uniqueSegments = segments.Distinct();

            var nameBuild = new StringBuilder();
            foreach (var item in uniqueSegments)
            {
                if (item.ToUpper() == "EVENTOUT" || item.Trim() == "-") continue;

                // handle active low bar
                if (item.ToLower().Contains("_n") || item.ToLower().Contains("_b") || item.Contains("#") || item.EndsWith("*") || item.StartsWith("*"))
                {
                    var append = new StringBuilder();
                    var segs = item.Replace("_n", "").Replace("_b", "").Replace("_N", "").Replace("_B", "").Replace("#", "").Trim('*').Trim();
                    foreach (var s in segs)
                    {
                        append.Append(s);
                        append.Append("\\");
                    }

                    nameBuild.Append(append.ToString().Trim());
                }
                else
                {
                    nameBuild.Append(item.Trim());
                }

                nameBuild.Append("/");
            }

            return nameBuild.ToString().Trim('/').Trim();
        }
    }
}
