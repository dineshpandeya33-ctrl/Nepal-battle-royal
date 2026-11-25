# Phase 19 — Distance-based world visibility

## Goal

Reduce unnecessary world rendering on Android by hiding distant decorative renderers at a configurable distance. This is a visibility optimization only; it does not remove or disable gameplay objects.

## New source

- `Assets/Scripts/Mobile/DistanceRendererCulling.cs`

## Unity setup

1. Add an empty GameObject named `WorldVisibilityOptimizer`.
2. Add `DistanceRendererCulling`.
3. Assign the player camera, or leave camera discovery enabled and tag the gameplay camera as MainCamera.
4. Add entries to **World Groups**. Assign each group's root Transform and a visible distance.
5. Start with separate groups for distant trees, decorative rocks and non-interactive scenery. Keep important landmarks and nearby village details visible.
6. Use a check interval around 0.25 seconds as a starting point. Tune distances by comparing pop-in and frame time on the target phone.
7. If children are added or removed after startup, call `RefreshGroups()` to rebuild the cached renderer list.

## Safety and behavior

- Only Renderer.enabled is changed.
- Colliders, scripts, NavMesh agents, transforms and network state are not disabled.
- The component restores each renderer's original enabled state when disabled.
- A small hysteresis distance reduces rapid visibility flicker near the cutoff.
- The camera position is checked at a configurable interval, not every frame.

## Recommended grouping

- Group distant foliage by region rather than putting every tree in its own group.
- Keep groups spatially compact; one huge group can remain visible because its root is near the camera.
- Do not place player characters, UI, essential navigation markers or interactive objects in decorative groups.
- Continue using LOD Groups for mesh detail reduction; distance culling complements LOD rather than replacing it.

## Acceptance test

Use a repeatable route and record average/worst frame time, visible renderer count, memory, temperature trend and visual pop-in. Compare culling enabled/disabled on the same Android device and scene. Confirm all important landmarks remain visible at intended distances.

## Limitations

- This is a source component, not automatic scene setup.
- It caches child renderers at startup; call `RefreshGroups()` after runtime hierarchy changes.
- Visibility distances are project-specific and require tuning.
- No Unity compilation, Android build or Redmi 12 test has been performed.
