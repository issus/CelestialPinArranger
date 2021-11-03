# Celestial Library Pin Arranger
 This is highly experimental and not recommended for typical use. It's mostly a tool to play with building a .net core library that can make a sensible schematic symbol from a variety of input sources and be integrated into web/automation software in the future once it has had a LOT more development.
 
# What Is It? 
 It will create a "best effort" rectangular schematic symbol from an Altium SchLib, UltraLibrarian BXL or specifically formatted CSV/TSV file then save as an Altium SchLib file format. The exported SchLib file name and component name are specifically formatted for my altium library and may not suit your requirements.

This is mostly tested on regulators, microcontrollers, rf, digital sensor ICs, and does reasonably well with devices it has never seen before.


# What isnt it? 
This does not understand opamps/non-rectangular type symbols. It will not split a large microcontroller into multiple symbols - but it will fix all the pin namings and group ports.



# USE
 You will need to clone this into a directory that also has my AltiumSharp and BxlSharp repositories cloned into it.
 e.g.:
- src/AltiumSharp
- src/BxlSharp
- src/CelestialPinArranger

# Data Sources
Currently you can import 
* Altium SchLib files
* Ultra Librarian BXL files
* KiCad Lib files
* STM32CubeMX XML files (C:\Program Files\STMicroelectronics\STM32Cube\STM32CubeMX\db\mcu)
* NXP MCUXpresso (download from https://mcuxpresso.nxp.com/en/select_config_tools_data - open signal_configuration.xml from `device`\ksdk2_0\\`package` folder)
* IBIS Models
* TSV/CSV files with very specific column ordering
