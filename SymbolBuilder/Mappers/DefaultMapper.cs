using SymbolBuilder.Model;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SymbolBuilder.Mappers
{
    public class DefaultMapper : PinMapper
    {
        protected override void LoadMappings()
        {
            AddFunction(string.Empty, PinClass.ChipConfiguration,   "Osc",              "^X(?:\\d\\d)?.+?_?|^OSC\\d+|^E?XTAL(?:\\d{1,2})?", PinPosition.From(PinSide.Left, PinAlignment.Middle));
            AddFunction(string.Empty, PinClass.PowerSupply,         "AnalogueGround",   "^GNDA$|^VSSA|^AGND|^AVSS|^AVEE|^VIN-", PinPosition.From(PinSide.Left, PinAlignment.Lower), priority: 1);
            AddFunction(string.Empty, PinClass.PowerSupply,         "Ground",           "^GND(?:\\d+)?$|^VSS|^IOVSS|^PGND|^EP$|^EPAD$|^DGND|^VEE|^DVEE|^PVEE|^PAD$|^DRVSS|^VSS_RF(?:\\d+)?$", PinPosition.From(PinSide.Left, PinAlignment.Lower));
            AddFunction(string.Empty, PinClass.PowerSupply,         "AnaloguePowerIn",  "^VDDA|^VCCA|^AVIN|^PVIN|^AVCC|^AVDD|^VDDA|^DRVDD|^DRVCC", PinPosition.From(PinSide.Left, PinAlignment.Upper), priority: 1);
            AddFunction(string.Empty, PinClass.PowerSupply,         "PowerIn",          "^VDD|^VCC\\d*|^VIN|^DVDD|^PVDD|^IOVDD|^VLOGIC|^DVCC|^V\\+|^IN|^VIN|^VCC|^VCC_RF(?:\\d+)?$", PinPosition.From(PinSide.Left, PinAlignment.Upper));
            AddFunction(string.Empty, PinClass.PowerSupply,         "PowerInSecondary", "^VBAT", PinPosition.From(PinSide.Left, PinAlignment.Upper), priority: 2);
            AddFunction(string.Empty, PinClass.PowerSupply,         "PowerRef",         "^VREF", PinPosition.From(PinSide.Left, PinAlignment.Upper), priority: 2);
            AddFunction(string.Empty, PinClass.PowerSupply,         "PowerDecoupling",  "^VCAP|^DCOUPL", PinPosition.From(PinSide.Left, PinAlignment.Middle));
            AddFunction(string.Empty, PinClass.ChipConfiguration,   "Enable",           "^EN$|^EN1$|^EN2$|^EN_SYNC|^EN_LDO\\d|^EN_DCDC$", PinPosition.From(PinSide.Left, PinAlignment.Upper), 3);
            AddFunction(string.Empty, PinClass.ChipConfiguration,   "Uvlo",             "UVLO", PinPosition.From(PinSide.Left, PinAlignment.Upper), 4);
            AddFunction(string.Empty, PinClass.ChipConfiguration,   "Reset",            "^RESET|RESET$|^RST|RST$|^MCLR$|^PAD_RESET$", PinPosition.From(PinSide.Left, PinAlignment.Upper), 3);
            AddFunction(string.Empty, PinClass.PowerSupply,         "RegulatorSwitch",  "^SW$|^DCDC_SW$|^SW\\d$", PinPosition.From(PinSide.Right, PinAlignment.Upper), 1);
            AddFunction(string.Empty, PinClass.ChipConfiguration,   "ISP",              "^JTAG|^SWD", PinPosition.From(PinSide.Left, PinAlignment.Middle));
            AddFunction(string.Empty, PinClass.Generic,             "USB",              "^USB\\d{0,2}_D[PM]$|USB_VBUS(?:_\\d{0,2})?", PinPosition.From(PinSide.Left, PinAlignment.Middle));
            AddFunction(string.Empty, PinClass.Generic,             "USBRegulator",     "^USB_VREG[IO]?(?:_\\d{1,2})?$", PinPosition.From(PinSide.Left, PinAlignment.Middle));
            AddFunction(string.Empty, PinClass.Generic,             "RF",               "^RF", PinPosition.From(PinSide.Right, PinAlignment.Upper));
            AddFunction(string.Empty, PinClass.PowerSupply,         "PowerOut",         "^VOUT|^VO|^VOUT\\+$", PinPosition.From(PinSide.Right, PinAlignment.Upper), groupSpacing: 2);
            AddFunction(string.Empty, PinClass.ChipConfiguration,   "SwitchmodeSetup",  "^SS|^COMP|^SYNC|^RT$", PinPosition.From(PinSide.Left, PinAlignment.Lower), priority: 5, groupSpacing: 2);
            AddFunction(string.Empty, PinClass.Generic,             "PowerGood",        "^PG$|^PWRGD$|^PGOOD$", PinPosition.From(PinSide.Right, PinAlignment.Lower), groupSpacing: 2);
            AddFunction(string.Empty, PinClass.ChipConfiguration,   "Feedback",         "^FB|^FB_LDO\\d|^FB_DCDC|^VFB|^FB\\d$", PinPosition.From(PinSide.Right, PinAlignment.Middle), functionSpacing: 2);
            AddFunction(string.Empty, PinClass.Generic,             "SPI",             @"^CS$|^C\S\$|^SDI$|^SDO$|^MOSI$|^MISO$|^SCLK$|^SDIN$|^MISO(SDO)$|^MOSI(SDI)$", PinPosition.From(PinSide.Right, PinAlignment.Lower));
            AddFunction(string.Empty, PinClass.Generic,             "I2C",              "^SCL$|^SDA$", PinPosition.From(PinSide.Right, PinAlignment.Lower));

            AddFunction(string.Empty, PinClass.IOPort, "Port", "(?:P(?<Group>\\d)\\[(?<Index>\\d+)\\])|(?:RP?(?<Group>[A-Z])(?<Index>\\d+))|^(?:G?PI?O?(?<Group>[A-Z0-9])_?(?<Index>\\d+))|(?:(?<Group>D?IO)_(?<Index>\\d+))|(?:PT(?<Group>[A-Z])(?<Index>\\d+))|^(?<Group>[a-zA-Z]{1,3})(?<Index>\\d+)$", PinPosition.From(PinSide.Right, PinAlignment.Middle));


            SetDefaultFunction(string.Empty, PinPosition.From(PinSide.Right, PinAlignment.Middle));
        }
    }
}
