# Phase 26 — Mobile Terrain Quality Controller

## Purpose
Adds one component to tune Unity Terrain tree distance, detail distance/density, billboard transition, heightmap pixel error, and basemap distance for Low, Balanced, and High presets.

## Setup
1. Add `MobileTerrainQualityController` to a scene manager GameObject.
2. Assign every generated or authored Terrain to the Terrains array.
3. Optionally assign the `MobilePerformanceProfile` component.
4. Keep Apply On Awake enabled to apply the selected startup preset.
5. Connect settings buttons to `ApplyLow`, `ApplyBalanced`, or `ApplyHigh`.

## Important integration note
The terrain controller reads the profile's current preset in Awake. Ensure the performance profile applies its startup preset before this component, or call the terrain controller's Apply methods after changing graphics settings from the settings UI.

## Performance and limitations
- Lower detail density and draw distances can reduce mobile rendering workload.
- Values are editable starting points, not measured Redmi 12 recommendations.
- This component only changes Unity Terrain rendering properties. It does not change mesh renderers, colliders, NavMesh, or terrain height data.
- Test foliage pop-in, visibility, frame pacing, and memory on the target device.

## Verification status
Source and documentation committed. Unity compilation and Android device profiling remain pending.
