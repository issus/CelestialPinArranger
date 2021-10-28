using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SymbolBuilder.Mappers
{
    public class DefaultMapper : PinMapper
    {
        protected override void LoadMappings()
        {
            AddFunction(string.Empty, "Port", "(?:P(?<Group>\\d)\\[(?<Index>\\d+)\\])|(?:RP?(?<Group>[A-Z])(?<Index>\\d+))|^(?:G?PI?(?<Group>[A-Z])_?(?<Index>\\d+))|(?:(?<Group>D?IO)_(?<Index>\\d+))|(?:PT(?<Group>[A-Z])(?<Index>\\d+))", PinPosition.From(PinSide.Right, PinAlignment.Middle));
            AddFunction(string.Empty, "Osc", "^X(?:\\d\\d)?.+?_?|^OSC\\d+", PinPosition.From(PinSide.Left, PinAlignment.Middle));
            AddFunction(string.Empty, "AnalogueGround", "^GNDA$|^VSSA|^AGND|^AVSS|^AVEE|^VIN-", PinPosition.From(PinSide.Left, PinAlignment.Lower), priority: 1);
            AddFunction(string.Empty, "Ground", "^GND$|^VSS|^PGND|^EP$|^EPAD$|^DGND|^VEE|^DVEE|^PVEE|^PAD|^GND\\d|^DRVSS", PinPosition.From(PinSide.Left, PinAlignment.Lower));
            AddFunction(string.Empty, "AnaloguePowerIn", "^VDDA|^VCCA|^AVIN|^PVIN|^AVCC|^AVDD|^VDDA|^DRVDD|^DRVCC", PinPosition.From(PinSide.Left, PinAlignment.Upper), priority: 1, groupSpacing: 2);
            AddFunction(string.Empty, "PowerIn", "^VDD|^VCC|^VIN|^DVDD|^PVDD|^VLOGIC|^DVCC|^V\\+|^IN|^VIN|^VCC\\d\\+$", PinPosition.From(PinSide.Left, PinAlignment.Upper));
            AddFunction(string.Empty, "PowerInSecondary", "^VBAT", PinPosition.From(PinSide.Left, PinAlignment.Upper), priority: 2, groupSpacing: 2);
            AddFunction(string.Empty, "PowerRef", "^VREF", PinPosition.From(PinSide.Left, PinAlignment.Upper), priority: 2);
            AddFunction(string.Empty, "PowerDecoupling", "^VCAP|^DCOUPL", PinPosition.From(PinSide.Left, PinAlignment.Middle));
            AddFunction(string.Empty, "Enable", "^EN$|^EN1$|^EN2$|^EN_SYNC|^EN_LDO\\d|^EN_DCDC$", PinPosition.From(PinSide.Left, PinAlignment.Upper), 3);
            AddFunction(string.Empty, "Uvlo", "UVLO", PinPosition.From(PinSide.Left, PinAlignment.Upper), 4);
            AddFunction(string.Empty, "Reset", "^RESET|RESET$|^RST|RST$|^MCLR$", PinPosition.From(PinSide.Left, PinAlignment.Upper), 3);
            AddFunction(string.Empty, "RegulatorSwitch", "^SW$|^DCDC_SW$|^SW\\d$", PinPosition.From(PinSide.Right, PinAlignment.Upper), 1);
            AddFunction(string.Empty, "ISP", "^JTAG|^SWD", PinPosition.From(PinSide.Left, PinAlignment.Middle));
            AddFunction(string.Empty, "RF", "^RF", PinPosition.From(PinSide.Right, PinAlignment.Upper));
            AddFunction(string.Empty, "PowerOut", "^VOUT|^VO|^VOUT\\+$", PinPosition.From(PinSide.Right, PinAlignment.Upper), groupSpacing: 2);
            AddFunction(string.Empty, "SwitchmodeSetup", "^SS|^COMP|^SYNC|^RT$", PinPosition.From(PinSide.Left, PinAlignment.Lower), priority: 5, groupSpacing: 2);
            AddFunction(string.Empty, "PowerGood", "^PG$|^PWRGD$|^PGOOD$", PinPosition.From(PinSide.Right, PinAlignment.Lower), groupSpacing: 2);
            AddFunction(string.Empty, "Feedback", "^FB|^FB_LDO\\d|^FB_DCDC|^VFB|^FB\\d$", PinPosition.From(PinSide.Right, PinAlignment.Middle), functionSpacing: 2);
            AddFunction(string.Empty, "SPI", @"^CS$|^C\S\$|^SDI$|^SDO$|^MOSI$|^MISO$|^SCLK$|^SDIN$|^MISO(SDO)$|^MOSI(SDI)$", PinPosition.From(PinSide.Right, PinAlignment.Lower));
            AddFunction(string.Empty, "I2C", "^SCL$|^SDA$", PinPosition.From(PinSide.Right, PinAlignment.Lower));

            SetDefaultFunction(string.Empty, PinPosition.From(PinSide.Right, PinAlignment.Middle));
        }
    }
}
