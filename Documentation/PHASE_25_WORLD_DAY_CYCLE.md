# Phase 25 — World Day Cycle

## Purpose
Adds an optional lightweight day/night lighting controller for the Nepal-inspired prototype. It changes the directional light, flat ambient light, and optional exponential fog. It does not require an external package.

## Unity setup
1. Open the prototype scene.
2. Create an empty GameObject named `WorldAtmosphere`.
3. Add the `WorldDayCycle` component.
4. Drag the scene's Directional Light into the Sun field.
5. Set Full Day Length Minutes and Starting Time Of Day.
6. Enable Control Fog only if the scene should use the controller's fog settings.
7. Enter Play mode and check sunrise, daylight, sunset, and night.

## Controls
- `SetTimeOfDay(0..1)` sets the normalized time.
- `SetCycleRunning(false)` pauses the cycle.
- Disable the component to stop it controlling scene lighting.

## Mobile notes
- The controller updates a small number of lighting properties and creates no per-frame objects.
- Dynamic shadows and fog can be expensive on mobile GPUs. Test with shadows disabled or reduced on the target Android device.
- The values are starting points, not Redmi 12 benchmark results.
- URP volume settings and baked lighting may need separate scene-specific configuration.

## Verification status
Source and documentation committed. Unity compilation, visual review, performance measurement, and Redmi 12 testing remain pending.
