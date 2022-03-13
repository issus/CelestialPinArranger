# Celestial Library Pin Arranger
 This is highly experimental and not recommended for typical use. The recent rewrite (Dec 2021/Jan 2022) of the arranger and Altium exporter has gone a long way to making it more reliable and have less glitches. Its still under heavy development, though it can ingest many data formats and create a very good starting point for your own symbol, if the output is not perfectly usable as is.
 
 ![Symbol Arranger Jan2021](https://github.com/issus/CelestialPinArranger/blob/main/github-img/pinarranger_02jan2021.jpg?raw=true)
 *ST Microelectronics STM32 Microcontroller generated from pin data ingested from STM32CubeMX data*
 
## What Is It? 
 It will create a "best effort" rectangular schematic symbol from a variety of data sources then save as an Altium SchLib file format. The exported SchLib file name and component name are specifically formatted for my altium library and may not suit your requirements. It will create multi-part symbols for higher pin count components, however this functionality has only been tested on microcontrollers. If the device doesn't have IO ports, it probably won't make a sensible multi-part symbol


This is mostly tested on regulators, microcontrollers, rf, digital sensor ICs, and does reasonably well with devices it has never seen before. 


You can create your own pin mapping definitions as JSON files to help it understand devices it may not currently handle well.


Currently it can only export Altium SchLib files, though the arranger logic and export logic have been separated so there is future scope for adding ECAD exporters for any ECAD application.


## What Isn't It? 
* This does not understand opamps/non-rectangular type symbols (yet). 
* It struggles with application processors like the i.MX7/i.MX8 that have very large numbers of VSS pins - its probably not useful for any device with 400+ pins.



# Use
 You will need to clone this into a directory that also has my AltiumSharp, EagleSharp and BxlSharp repositories cloned into it. I should really put these into NuGet but haven't gotten around to it.
 
 e.g.:
- src/AltiumSharp
- src/BxlSharp
- src/EagleSharp
- src/CelestialPinArranger


You can then load the CelestialPinArranger in Visual Studio (free Community version is fine) to compile/run it. You could also compile it by commandline, it should be cross platform, however it has only been tested on Windows.


## Data Sources
Currently you can ingest pin data from: 
* Altium SchLib files
* Ultra Librarian BXL files
* KiCad Lib files
* Eagle Lbr Files
* STM32CubeMX XML files (C:\Program Files\STMicroelectronics\STM32Cube\STM32CubeMX\db\mcu)
* STM8CubeMX XML files (C:\Program Files\STMicroelectronics\STM8Cube\STM8CubeMX\db\mcu)
* Silicon Labs SimplicityStudio files (C:\SiliconLabs\SimplicityStudio\v5\developer\sdks\gecko_sdk_suite\v3.2\platform\hwconf_data\\`family`\\`device`\\`device`.device)
* NXP MCUXpresso (download from https://mcuxpresso.nxp.com/en/select_config_tools_data - open signal_configuration.xml from `device`\ksdk2_0\\`package` folder)
* TI SysConfig (C:\ti\sysconfig_1.10.0\dist\deviceData) **NOTE: There is no data on fixed function pins (e.g.: power supply)**.
* Infineon/Cypress Pin and Code Wizard files (SPaCWiz\\xml\\mcu\\`series`\\`folder`\\`*`.xml)
* IBIS Models
* TSV/CSV files with very specific column ordering (not recommended)

When importing data from ECAD libraries, it will only use the pin designator/name - it will not take into account any existing pin arrangement/how multi-part symbols are divided - this is intentional.
