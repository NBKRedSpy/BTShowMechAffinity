# Battletech Show Mech Affinity

An extension for the Mech Affinity mod.  This mod shows which pilots have an affinity with a specific chassis.
Rather than searching pilots for chassis affinity via the tool tips, the pilot list will be colored and sorted by level of affinity for the selected mech.

![image](https://user-images.githubusercontent.com/54865934/168877225-4e1c38bc-c505-4dfa-a907-c60781d66cc6.png)

## Usage
When in the contract loadout screen, click on a mech and all the pilots will have their rank icon changed to a color based on the pilot's affinity.

The pilots on the right of the screen will also be sorted by the number of drops with the selected mech and then by name.


## Default Affinity Colors:

|Affinity level| color|
|--|--|
|No Affinity (0) | Black|
| < 10| Grey|
| < 20| Green|
| < 30| Blue|
| < 50| Purple|
| >= 50| Orange|

## User Custom Colors

The colors can be set by editing the BATTLETECH\Mods\BTShowMechAffinity\mod.json file.

```json
  "Settings": {
    "AffinityColors": [
      {"deploymentCountMax": 0, "Color":  "#000000"},
      {"deploymentCountMax": 9, "Color":  "#BABABA"},
      {"deploymentCountMax": 19, "Color":  "#309231"},
      {"deploymentCountMax": 29, "Color":  "#4d51f8"},
      {"deploymentCountMax": 49, "Color":  "#A335EE"},
      {"deploymentCountMax": 9999999, "Color":  "#FF8000"}
```

The mech affinity is based on the number of deployments a pilot has in a chassis.  The color selection is based on the deployment count vs the deploymentCountMax value in numeric order.

In the example above, if the pilot has a deployment count of 23, the color will be #4d51f8 since it is greater than 19 and less than or equal to 29.  

The Color is an HTML color.  The website https://htmlcolorcodes.com/color-picker/ has a useful color picker.

The last entry uses 9999999 to cover an affinity of 50 or more.

Note that the default list does not have a 40 entry (30-39) since the MechAffinity mod data does not have a bonus at that level.


# Requirements
Requires the Mech Affinity mod to be installed.  Some Battletech projects will already have it installed.  For example, Battletech Extended Comander Edition has Mech Affinity installed.

Mech Affinity on nexusmods.com:
https://www.nexusmods.com/battletech/mods/587


# Installation
To install, download the zip file from the releases and extract to the Battletech Mods folder.

This assumes ModTek has been installed and injected.

# Compatibility
This should be compatible with all mods.
Safe to add to and remove from existing saves.

# Change Log

## 1.1.0
* Added user defined colors.
* Corrected the purple color showing as red.

Thanks to the NexusMod users fys and synopse8 for reporting the color issues as well as requesting color customization.