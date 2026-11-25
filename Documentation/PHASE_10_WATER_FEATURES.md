# Phase 10 — River and lake surfaces

## Included
- `WaterFeatureBuilder.cs`: creates a flat river surface mesh along ordered waypoint Transforms and oval lake surfaces at assigned anchor Transforms.
- One shared material can be assigned to all generated water surfaces.
- Optional MeshColliders are available but should remain disabled for ordinary water visuals unless a specific interaction needs them.

## Unity setup
1. Generate the Nepal-inspired terrain.
2. Create an empty GameObject named `WaterFeatures` and add `WaterFeatureBuilder`.
3. Assign a water material that uses a transparent or opaque water shader supported by the project's render pipeline.
4. Create river waypoint objects along the intended river centerline. Their Y positions define the water level. Assign them in order.
5. Set the river width.
6. Create lake anchor objects at the desired lake centers. Set each anchor's Y position to the water level.
7. Set each lake anchor's local Scale X and Z to the desired lake radii. If left near zero, the configured default radii are used.
8. Use **Build Water Features** from the component context menu. Use **Clear Water Features** before rebuilding.

## Design and performance notes
- This is a static greybox water surface, not a fluid simulation.
- River waypoint heights should be adjusted to avoid visible steps between sections.
- The mesh is intentionally simple. Use one shared material, avoid unnecessary colliders, and keep water effects modest on mobile.
- Water surfaces do not automatically carve channels into the terrain or create river banks, bridges, waterfalls, reflections, or animated flow.
- Keep the river and lake meshes within the terrain scene and inspect for z-fighting or intersections.

## Acceptance checks
- [ ] River surface follows the assigned waypoint route.
- [ ] River width remains consistent along straight sections.
- [ ] Lake surfaces appear centered at their anchors with the expected radii.
- [ ] The assigned material renders correctly in the selected URP version.
- [ ] Clear and rebuild leaves no duplicate water objects.
- [ ] Rendering and collision costs are checked on the target Android device.

## Important
The builder creates water geometry only. It does not create a complete river environment, realistic water physics, or a finished game scene. Unity Editor integration and Redmi 12 testing remain pending.
