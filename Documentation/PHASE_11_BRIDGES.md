# Phase 11 — Pedestrian bridge environment

## Included
- `BridgeEnvironmentBuilder.cs`: creates a simple bridge deck mesh along ordered centerline waypoint Transforms.
- Optional side rails are generated as simple box primitives.
- The deck can receive a MeshCollider so a character controller can walk across it.
- The bridge is static environment geometry, not a complete structural simulation.

## Unity setup
1. Create an empty GameObject named `BridgeEnvironment` and add `BridgeEnvironmentBuilder`.
2. Place at least two waypoint Transforms in order along the bridge centerline.
3. Set each waypoint's height to the intended deck level. The configured deck height offset is added above each waypoint.
4. Set the deck width and assign a URP-compatible deck material.
5. Enable the deck collider if characters need to walk across it.
6. Enable side rails and optionally assign a rail material.
7. Use **Build Bridge** from the component context menu. Use **Clear Bridge** before rebuilding.

## Design and mobile notes
- Use a small number of bridge sections and avoid excessively long waypoint lists.
- Keep bridge materials shared and simple.
- Side rails use primitive colliders; remove or disable them if they are not needed for the scene.
- Ensure the bridge approaches meet the terrain and that the deck does not intersect the water surface.
- The deck mesh is a flat surface; it does not generate support pillars, stairs, curved arches, or terrain connections automatically.

## Acceptance checks
- [ ] The deck follows the ordered waypoint path.
- [ ] The deck width is consistent along straight sections.
- [ ] Deck normals face upward and the assigned material renders in URP.
- [ ] The player can walk across the deck when its collider is enabled.
- [ ] Side rails appear on both sides when enabled.
- [ ] Clear and rebuild does not leave duplicate bridge objects.
- [ ] Rendering and collision costs are checked on the target Android device.

## Important
This is a source-code milestone only. Unity Editor compilation, scene integration, and Redmi 12 testing remain pending.
