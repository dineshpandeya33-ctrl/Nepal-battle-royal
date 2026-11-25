# Phase 1-2: Foundation and Nepal-inspired world prototype

## What is included in this commit

- Unity C# scripts for a procedural heightmap terrain.
- Seven fictional region definitions and scene markers.
- A route/landmark scaffold for later art and gameplay passes.
- Setup instructions and technical architecture.

## What is not included yet

- A Unity-generated scene or binary terrain asset.
- Finished buildings, vegetation, roads, bridges, water shaders, textures, character models, or audio.
- Player controller, combat, AI, multiplayer, or an Android APK.

These require Unity Editor generation, licensed/original art assets, and device testing.

## Unity setup

1. Install Unity Hub and a supported Unity LTS editor.
2. Add Android Build Support, Android SDK & NDK Tools, and OpenJDK.
3. Create a new 3D Core project.
4. Copy this repository's `Assets` directory into the project.
5. Open the project and wait for compilation.
6. Create a scene named `NepalPrototype`.
7. Create an empty GameObject named `NepalWorld` and attach `NepalMapGenerator`.
8. Use the component context menu **Generate Nepal Map**.
9. Add a directional light and a simple camera looking at the terrain.
10. Save the scene. Commit the scene and Unity-generated metadata from your local Unity project.

## Terrain prototype

The generator creates a square terrain with broad height variation and seven labelled region marker objects. It is deliberately a greybox: the next art pass should replace markers with optimized modular buildings, vegetation, rock formations, roads, and water features.

## Verification checklist

- [ ] Scripts compile without errors in the selected Unity LTS.
- [ ] Generator creates one Terrain object.
- [ ] Seven region markers appear and have the correct labels.
- [ ] Terrain is traversable and has no extreme spikes.
- [ ] Scene can be saved and reopened.
- [ ] Android Development Build launches on a physical test device.
- [ ] Performance and memory are recorded on the lowest target device.

## Next implementation task

Add a region-aware material/texture pass, terrain chunking/LOD plan, and original low-poly greybox settlements. Then implement a third-person controller in a separate milestone.
