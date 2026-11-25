# Phase 8 — Nepal-inspired world decoration

## Included
- `WorldDecorationScatterer.cs`: places a limited number of assigned tree, rock, bush, or other decorative prefabs on a referenced Unity Terrain.
- Uses a fixed seed for repeatable placement, terrain slope checks, minimum spacing, edge margins, and optional keep-clear radii around assigned Transforms.
- Exposes a maximum attempt count and a conservative default instance count for early mobile profiling.

## Unity setup
1. Generate the terrain with `NepalMapGenerator` first.
2. Create a separate empty GameObject named `WorldDecoration` (do not put this component on the NepalMapGenerator object).
3. Add `WorldDecorationScatterer` and assign the generated Terrain.
4. Assign original or properly licensed low-poly/optimized prefabs, such as trees, rocks, bushes, and small environment props.
5. Optionally assign spawn or practice-area Transforms to `Keep Clear Around`.
6. Use the component context menu **Scatter Decorations**.
7. Use **Clear Decorations** to remove the generated objects, then scatter again.

## Starting values
- Instance Count: 250
- Minimum Spacing: 8
- Maximum Slope: 28 degrees
- Keep Clear Radius: 18
- Maximum Attempts Per Instance: 30

These are prototype starting values, not measured Redmi 12 performance limits. Start with a small number of simple meshes, use shared materials, enable GPU instancing where appropriate, and profile CPU, GPU, memory, thermals, and frame rate on the actual device.

## Acceptance checks
- [ ] Decoration instances appear on the terrain surface.
- [ ] Instances are not placed on slopes steeper than the configured limit.
- [ ] Instances maintain the configured spacing.
- [ ] Assigned keep-clear Transforms have no decoration within the configured radius.
- [ ] Repeating the same seed produces the same placement when terrain and prefab assignments are unchanged.
- [ ] Clear Decorations removes the generated set.
- [ ] Performance is measured on the target Android device before increasing density.

## Important
This script places decorative prefabs only. It does not create a finished village, roads, buildings, a complete playable scene, or an APK. Unity Editor integration and Android device testing remain pending.
