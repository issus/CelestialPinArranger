
# Celestial Library Pin Arranger

The Celestial Library Pin Arranger is a tool for creating "best effort" rectangular schematic symbols from a variety of data sources, including Altium SchLib files, Ultra Librarian BXL files, and KiCad Lib files. The tool can create multi-part symbols for higher pin count components, such as microcontrollers, but has only been tested on this type of device. It can export Altium SchLib files, but there is scope for adding ECAD exporters for other applications in the future.

![Symbol Arranger Jan 2021](https://github.com/issus/CelestialPinArranger/blob/main/github-img/pinarranger_02jan2021.jpg?raw=true) _ST Microelectronics STM32 Microcontroller generated from pin data ingested from STM32CubeMX data_

## Key Features

-   Ingests pin data from a variety of sources
-   Creates "best effort" rectangular schematic symbols
-   Can create multi-part symbols for higher pin count components
-   Exports Altium SchLib files

## Limitations

-   Does not understand opamps or non-rectangular symbols (yet)
-   Struggles with application processors with 400+ pins, such as the i.MX7/i.MX8

## Data Sources

The Celestial Library Pin Arranger can ingest pin data from the following sources:

-   Altium SchLib files
-   Ultra Librarian BXL files
-   KiCad Lib files
-   Eagle Lbr Files
-   STM32CubeMX XML files
-   STM8CubeMX XML files
-   Silicon Labs SimplicityStudio files
-   NXP MCUXpresso
-   TI SysConfig (note: no data on fixed function pins)
-   Infineon/Cypress Pin and Code Wizard files
-   Microchip AVR Tools Device File (atdf)
-   IBIS Models
-   TSV/CSV files with specific column ordering (not recommended)

## Usage

To use the Celestial Library Pin Arranger, open it in Visual Studio (the free Community version is fine) or compile it by command line. The tool is cross-platform, but has only been tested on Windows.

## Note

When importing data from ECAD libraries, the Celestial Library Pin Arranger only uses the pin designator/name and does not take into account any existing pin arrangement or how multi-part symbols are divided. This is intentional.
