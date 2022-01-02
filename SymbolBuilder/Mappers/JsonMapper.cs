using SymbolBuilder.Model;
using System;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace SymbolBuilder.Mappers
{
    public class JsonMapper : PinMapper
    {
        private string _fileName;

        public JsonMapper(string fileName)
        {
            _fileName = fileName;
        }

        private static PinPosition GetPinPosition(JsonElement function)
        {
            if (!function.TryGetProperty("position", out var element)) throw new InvalidDataException($"Pin position missing: {function}");

            if (element.ValueKind == JsonValueKind.Number && element.TryGetInt32(out var clockDirection))
            {
                return PinPosition.From(clockDirection);
            }

            var text = element.GetString().Trim();
            var parts = text.Split(new[] { '-', '/', ' ', ':' }, 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) throw new InvalidDataException($"Invalid pin position: {text}");
            if (!Enum.TryParse<PinSide>(parts[0], true, out var side)) throw new InvalidDataException($"Invalid pin position: {text}");
            if (parts.Length == 2 && Enum.TryParse<PinAlignment>(parts[1], true, out var alignment))
            {
                return new PinPosition(side, alignment);
            }
            else
            {
                return new PinPosition(side, PinAlignment.Middle);
            }
        }

        private static int GetPriority(JsonElement function)
        {
            return function.TryGetProperty("priority", out var element) ? element.GetInt32() : 0;
        }

        private static PinElectricalType GetPinElectricalType(JsonElement function)
        {
            if (!function.TryGetProperty("electricalType", out var element) &&
                !function.TryGetProperty("type", out element))
            {
                return PinElectricalType.Passive;
            }

            var text = Regex.Replace(element.GetString(), @"/_-:\s", "");
            if (Enum.TryParse<PinElectricalType>(text, true, out var result))
            {
                return result;
            }

            text = text.ToUpperInvariant();
            if (text == "I" || text.StartsWith("IN")) return PinElectricalType.Input;
            if (text == "O" || text.StartsWith("OUT")) return PinElectricalType.Output;
            if (text == "IO") return PinElectricalType.InputOutput;
            if (text == "OPENDRAIN") return PinElectricalType.OpenCollector;

            return PinElectricalType.Passive;
        }

        private static int GetFunctionSpacing(JsonElement function)
        {
            return (function.TryGetProperty("functionSpacing", out var element) || function.TryGetProperty("spacing", out element)) ? element.GetInt32() : 1;
        }

        private static int GetGroupSpacing(JsonElement function)
        {
            return function.TryGetProperty("groupSpacing", out var element) ? element.GetInt32() : 1;
        }

        private static PinClass GetPinClass(JsonElement function)
        {
            return function.TryGetProperty("pinClass", out var element) ? Enum.TryParse<PinClass>(element.GetString(), out var result) ? result : PinClass.Generic : PinClass.Generic;
        }

        public override void LoadMappings()
        {
            var json = JsonDocument.Parse(File.ReadAllText(_fileName));

            if (json.RootElement.ValueKind != JsonValueKind.Object) throw new InvalidDataException("JSON root element should be an object containing package definitions");
            foreach (var package in json.RootElement.EnumerateObject())
            {
                var packagePattern = package.Name;

                if (package.Value.ValueKind != JsonValueKind.Array) throw new InvalidDataException("Package definition should be an array of function mappings");
                foreach (var function in package.Value.EnumerateArray())
                {
                    if (function.ValueKind != JsonValueKind.Object) throw new InvalidDataException("Function mapping should be an object.");

                    var pinClass = GetPinClass(function);
                    var functionName = function.GetProperty("functionName").GetString();
                    var position = GetPinPosition(function);
                    var priority = GetPriority(function);
                    var electricalType = GetPinElectricalType(function);
                    var functionSpacing = GetFunctionSpacing(function);
                    var groupSpacing = GetGroupSpacing(function);

                    if (string.IsNullOrEmpty(functionName))
                    {
                        SetDefaultFunction(packagePattern, position, priority, electricalType, functionSpacing, groupSpacing);
                    }
                    else
                    {
                        var pinNamePattern = function.GetProperty("pinNamePattern").GetString();
                        AddFunction(packagePattern, pinClass, functionName, pinNamePattern, position, priority, electricalType, functionSpacing, groupSpacing);
                    }
                }
            }
        }
    }
}
