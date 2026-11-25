# Phase 18 — Mobile performance profile

## Goal

Provide runtime-selectable graphics presets as a starting point for profiling Android builds, including the Redmi 12 target device.

## New source

- `Assets/Scripts/Mobile/MobilePerformanceProfile.cs`

## Presets

| Preset | Intended use | Initial settings |
|---|---|---|
| Low | Reduce GPU load on slower or hot devices | Shadows disabled, shorter shadow distance, lower LOD bias, reduced texture resolution |
| Balanced | Starting point for Redmi 12 testing | Short shadow distance, moderate LOD bias, reduced texture resolution |
| High | Visual comparison on capable devices | Shadows enabled, longer shadow distance, higher LOD bias, full texture resolution |

The component also sets a configurable target frame rate (30–60 FPS) and disables VSync so the target frame rate can be applied consistently on mobile.

## Unity setup

1. Copy the repository's `Assets/` folder into a Unity URP project.
2. Create an empty GameObject named `MobilePerformance`.
3. Add `MobilePerformanceProfile`.
4. Select the startup preset in the Inspector. Balanced is only a starting hypothesis for Redmi 12, not a measured guarantee.
5. Set the target frame rate. Begin with 30 FPS, then compare 45 or 60 only after thermal and frame-time testing.
6. Optional: connect settings menu buttons to `ApplyLow`, `ApplyBalanced`, and `ApplyHigh`.
7. Compare the same scene and camera path for each preset on the same device.

## Additional scene optimization checklist

- Use LOD Groups for buildings, trees and large props.
- Enable GPU instancing on compatible repeated materials.
- Use baked lighting where practical; limit realtime lights and shadow casters.
- Use occlusion culling only after baking and verifying the scene.
- Use texture atlases and appropriate Android texture compression.
- Add colliders only where interaction or navigation requires them.
- Keep decoration counts bounded; test scatter density at multiple quality levels.
- Profile CPU, GPU, memory, frame time, battery and device temperature over repeatable sessions.

## Acceptance test

Record device model, Unity version, render pipeline, preset, target FPS, average and worst frame time, memory use, temperature trend and test duration. Repeat the same route for each preset. Choose release defaults from measured results rather than appearance alone.

## Limitations

- This component changes Unity QualitySettings at runtime; it does not edit URP asset settings, shader complexity, mesh density, or prefab LODs automatically.
- Preset values are initial tuning suggestions and are not certified Redmi 12 settings.
- No Unity compilation, Android build, profiling run, or physical Redmi 12 test has been performed.
