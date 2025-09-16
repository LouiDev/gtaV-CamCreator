# GTA V CamCreator - An open source tool

A tool for GTA V modders that allows you to detach the gameplay camera from the player and move it freely in the world.  
With a hotkey, the current camera position, rotation, and field of view (FOV) can be exported automatically into a text file as **ready-to-use C# code**.  
This code can then be directly integrated into your own mods.

## Features
- Detach the camera from the player (FreeCam).
- Adjust FOV dynamically ingame
- Move freely around the GTA V world.
- Full control over controls and default values
- Controller support
- Hotkey to export:
  - Position
  - Rotation
  - Field of View (FOV)
- Export directly as **ready-to-use C# code**.

## Installation
1. Download the latest [release](https://github.com/yourname/gta-v-freecam-tool/releases).
2. Follow included installation instructions.
3. Activate the tool with the defined hotkey.

## Usage
- Toggle camera: `G` (`DPad-Left`) 
- Move camera: `WASD` + mouse (`LS` + `RS`)
- Export camera data: `Tab` (`X`)
- Exported code will be saved to `GTA V/scripts/{filename}.cs`.

## Requirements
- GTA V Legacy
- [ScriptHookV](http://www.dev-c.com/gtav/scripthookv/)
- [ScriptHookVDotNet nightly](https://github.com/scripthookvdotnet/scripthookvdotnet-nightly/releases) v45 (!) or higher

## Contact me
If you have questions, feedback or suggestions, hit me up on [Discord](https://discord.gg/U2KGVbj3uh). 
This is the fastest way to get a response.

## License
This project is licensed under the [MIT License](LICENSE).
