# Phase 2 visual pass — realistic graphics target

## What this increment adds
- Five procedural terrain albedo layers: meadow, forest, rock, snow, and dry earth.
- Terrain splat weights driven by elevation and slope.
- Region marker placement sampled from the terrain surface.
- Reuse of the existing TerrainData on regeneration to avoid creating a new terrain asset each time.

## Important quality note
This is a visual greybox improvement, not final AAA realism. Procedural color textures do not replace high-resolution scanned PBR materials, authored terrain masks, detailed rock meshes, foliage, buildings, water, atmosphere, post-processing, character art, or animation. Final quality requires original/licensed assets and testing in the Unity Editor on target Android hardware.

## Setup
1. Copy the updated `Assets/Scripts/World` folder into the Unity project.
2. Reopen Unity and wait for C# compilation.
3. Select the `NepalWorld` object and run **Generate Nepal Map**.
4. Inspect the five terrain layers in Terrain settings.
5. Check marker bases sit on the terrain surface.
6. Save the scene and run an Android Development Build.

## Next art tasks
- Replace generated albedo textures with tileable PBR sets (albedo, normal, mask).
- Author region-specific terrain masks and a snowline profile.
- Add a lake mesh/material, river spline, road spline, and modular settlement kits.
- Add terrain chunking/LOD and vegetation density controls.
- Add URP lighting, fog, sky, and post-processing profiles.
- Profile draw calls, GPU time, memory, and thermal behavior on low/mid/high Android devices.

## Verification status
The source has been committed to GitHub. It has not been compiled in Unity or visually tested on an Android device in this environment.
