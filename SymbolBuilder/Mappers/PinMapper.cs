using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AltiumSharp.Records;
using SymbolBuilder.Model;

namespace SymbolBuilder.Mappers
{
    /// <summary>
    /// Pin mapper intended for programmatically generating a pin mapper, rather than loading mappings from a source
    /// </summary>
    public class ProgrammaticPinMapper : PinMapper
    {
        public override void LoadMappings()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Base class for defining pin mappings with a one the defined pin functions.
    /// </summary>
    public abstract class PinMapper
    {
        public List<PinFunction> Functions { get; private set; }
        private PinFunction _defaultFunction;

        public abstract void LoadMappings();

        public PinMapper()
        {
            Functions = new List<PinFunction>();
        }

        /// <summary>
        /// Adds a new mapping
        /// </summary>
        /// <param name="packagePattern">Package name mapping regex - only use mapping for this package</param>
        /// <param name="pinClass">Sets the PinClass which the arranger uses to split symbols into multiple blocks</param>
        /// <param name="functionName">Name of the mapping</param>
        /// <param name="pinNamePattern">Regex for matching pin name</param>
        /// <param name="position">Location this pin will be placed in on a rectangular package</param>
        /// <param name="priority">Priority Level</param>
        /// <param name="electricalType">Pin electrical type</param>
        /// <param name="functionSpacing">Spacing on the generated symbol</param>
        /// <param name="groupSpacing">Spacing around the group on the generated symbol</param>
        public void AddFunction(string packagePattern, PinClass pinClass, string functionName, string pinNamePattern, PinPosition position, int priority = 0, Model.PinElectricalType electricalType = Model.PinElectricalType.Passive, int functionSpacing = 1, int groupSpacing = 1)
        {
            Functions.Add(new PinFunction(functionName, new Regex(packagePattern, RegexOptions.IgnoreCase), pinClass, new Regex(pinNamePattern, RegexOptions.IgnoreCase), position, priority, electricalType, functionSpacing, groupSpacing));
        }

        public void SetDefaultFunction(string packagePattern, PinPosition position, int priority = 0, Model.PinElectricalType electricalType = Model.PinElectricalType.Passive, int functionSpacing = 1, int groupSpacing = 0)
        {
            _defaultFunction = new PinFunction("(Default)", new Regex(packagePattern, RegexOptions.IgnoreCase), PinClass.Generic, new Regex(@"(?<Group>\w+)?(?<Index>\d+)?"), position, priority, electricalType, functionSpacing, groupSpacing);
        }

        private static bool TryMapPin(PinFunction pinFunction, string package, PinDefinition pin)
        {
            if (pinFunction == null)
            {
                return false;
            }

            if (pinFunction.PackageNameRegex.IsMatch(package) &&
                pinFunction.PinNameRegex.IsMatch(pin.FullNameSlashed))
            {
                MapPin(pinFunction, pin);
                return true;
            }

            return false;
        }

        private static void MapPin(PinFunction pinFunction, PinDefinition pin)
        {
            if (pinFunction == null) return;

            var match = pinFunction.PinNameRegex.Match(pin.FullNameSlashed);

            if (match.Groups["Keep"].Success)
            {
                // todo: this may bug with alternative functions, since we match on FullNameSlashed
                pin.SignalName.Name = match.Groups["Keep"].Value;
            }

            var groupName = match.Groups["Group"].Success ? match.Groups["Group"].Value : pinFunction.Name;
            int.TryParse(match.Groups["Index"].Value, out var groupIndex);

            pin.MappingFunction = pinFunction;
            pin.PinPosition = pinFunction.Position;
            pin.Port = groupName;
            pin.PortBit = groupIndex;
        }

        public bool Map(string package, PinDefinition pin)
        {
            if (Functions.Count == 0) LoadMappings();

            if (pin.MappingFunction == null)
            {
                foreach (var pinFunction in Functions)
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
                var pinFunction = Functions.FirstOrDefault(f => f.Name.Equals(pin.MappingFunction.Name, StringComparison.InvariantCultureIgnoreCase))
                                  ?? _defaultFunction;
                return TryMapPin(pinFunction, package, pin);
            }
        }
    }
}