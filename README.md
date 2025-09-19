# PinballSimulator

This repository now contains a Unity 2022.3 LTS 3D project scaffold for building a pinball simulator. The project includes a starter scene with a camera, directional light, and a simple ground plane so the editor immediately displays 3D content. Open the project in Unity 2022.3.10f1 (or a compatible 2022.3 LTS version) to begin development.

## Unit scale

The project is configured so that **1 Unity unit represents 2 inches** instead of the default 1 meter. Physics gravity has been adjusted accordingly to keep motion consistent with real-world behaviour at this scale.

## Project Structure

- `Assets/` – Application content, currently containing a SampleScene under `Assets/Scenes`.
- `Packages/` – Unity Package Manager manifest and lock files.
- `ProjectSettings/` – Serialized Unity project settings for graphics, quality, physics, input, and more.

To get started, open the project in the Unity Hub, ensure the editor version matches `2022.3.10f1`, and load `Assets/Scenes/SampleScene.unity`.
