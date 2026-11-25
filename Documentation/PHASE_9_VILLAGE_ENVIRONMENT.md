# Phase 9 — Village roads and building layout

## Included
- `VillageEnvironmentBuilder.cs`: creates a basic road mesh between hand-placed waypoint Transforms, sampling the terrain under both road edges.
- Places assigned building prefabs at hand-authored anchors and skips anchors outside the terrain or above the configured slope limit.
- Supports an optional road material and MeshCollider.
- Uses assigned original or properly licensed building assets; it does not generate detailed building models.

## Unity setup
1. Generate the terrain with `NepalMapGenerator`.
2. Create a separate GameObject named `VillageLayout` and add `VillageEnvironmentBuilder`.
3. Assign the generated Terrain.
4. Create at least two empty waypoint objects along the desired road centerline. Keep the road inside the terrain boundaries and assign them in order.
5. Set the road width and assign a simple road material.
6. Create empty building anchor objects where houses should stand. Set each anchor's rotation to the desired building facing direction.
7. Assign optimized building prefabs and the anchors.
8. Use the component context menu **Build Village Environment**. Use **Clear Village Environment** before rebuilding.

## Starter layout guidance
- Begin with a short village street and 4–8 building anchors.
- Use simple meshes, shared materials, baked lighting where appropriate, and modest texture sizes.
- Keep the road waypoints reasonably close to the terrain surface; the builder samples terrain height for the road mesh.
- Ensure building pivots are at the base of each model for correct placement.
- Use low-detail collision meshes on buildings; avoid unnecessary moving objects.

## Acceptance checks
- [ ] Road mesh follows the terrain between assigned waypoints.
- [ ] Road material is visible and road collider works if enabled.
- [ ] Buildings appear at valid anchors and align to anchor rotation.
- [ ] Steep or out-of-terrain anchors are skipped.
- [ ] Clear and rebuild does not leave duplicate generated objects.
- [ ] Profile rendering, physics, memory, and frame rate on the target Android phone.

## Important
This is a layout helper, not a finished village asset pack or a playable scene. Road junction smoothing, bridges, interior design, detailed Nepalese architecture, lighting polish, and device testing remain future work.
