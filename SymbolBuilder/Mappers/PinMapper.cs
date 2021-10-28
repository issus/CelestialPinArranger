using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AltiumSharp.Records;

namespace SymbolBuilder.Mappers
{
    /// <summary>
    /// Base class for defining pin mappings with a one the defined pin functions.
    /// </summary>
    public abstract class PinMapper
    {
        private readonly List<PinFunction> _pinFunctions = new List<PinFunction>();
        private PinFunction _defaultFunction;

        protected abstract void LoadMappings();

        protected void AddFunction(string packagePattern, string functionName, string pinNamePattern, PinPosition position, int priority = 0, PinElectricalType electricalType = PinElectricalType.Passive, int functionSpacing = 1, int groupSpacing = 1)
        {
            _pinFunctions.Add(new PinFunction(functionName, new Regex(packagePattern, RegexOptions.IgnoreCase), new Regex(pinNamePattern, RegexOptions.IgnoreCase), position, priority, electricalType, functionSpacing, groupSpacing));
        }

        protected void SetDefaultFunction(string packagePattern, PinPosition position, int priority = 0, PinElectricalType electricalType = PinElectricalType.Passive, int functionSpacing = 1, int groupSpacing = 0)
        {
            _defaultFunction = new PinFunction("(Default)", new Regex(packagePattern, RegexOptions.IgnoreCase), new Regex(@"(?<Group>\w+)?(?<Index>\d+)?"), position, priority, electricalType, functionSpacing, groupSpacing);
        }

        private static bool TryMapPin(PinFunction pinFunction, string package, Pin pin)
        {
            if (pinFunction == null)
            {
                return false;
            }

            if (pinFunction.PackageNameRegex.IsMatch(package) &&
                pinFunction.PinNameRegex.IsMatch(pin.NameClean))
            {
                MapPin(pinFunction, pin);
                return true;
            }

            return false;
        }

        private static void MapPin(PinFunction pinFunction, Pin pin)
        {
            if (pinFunction == null) return;

            var match = pinFunction.PinNameRegex.Match(pin.NameClean);
            var groupName = match.Groups["Group"].Success ? match.Groups["Group"].Value : pinFunction.Name;
            int.TryParse(match.Groups["Index"].Value, out var groupIndex);

            pin.Function = pinFunction;
            pin.GroupName = groupName;
            pin.GroupIndex = groupIndex;
            pin.FunctionName = pin.Function.Name;
            pin.ElectricalType ??= pin.Function.ElectricalType;
            pin.Position ??= pin.Function.Position;
        }

        public bool Map(string package, Pin pin)
        {

            if (_pinFunctions.Count == 0) LoadMappings();

            if (pin.FunctionName == null)
            {
                foreach (var pinFunction in _pinFunctions)
                {
                    if (TryMapPin(pinFunction, package, pin))
                    {
                        return true;
                    }
                }

                return TryMapPin(_defaultFunction, package, pin);
            }
            else
            {
                var pinFunction = _pinFunctions.FirstOrDefault(f => f.Name.Equals(pin.FunctionName, StringComparison.InvariantCultureIgnoreCase))
                                  ?? _defaultFunction;
                return TryMapPin(pinFunction, package, pin);
            }
        }
    }
}