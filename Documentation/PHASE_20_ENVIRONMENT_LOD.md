# Phase 20 — Authored environment LOD setup

## Goal

Allow environment prefabs to switch between artist-authored mesh detail levels as they occupy less of the camera view. This complements Phase 19 distance visibility and Phase 18 graphics presets.

## New source

- `Assets/Scripts/Mobile/AuthoredLodGroupSetup.cs`

## Unity setup

1. Prepare simplified mesh variants for a tree, house, rock or other environment prop.
2. Put all variants under one prefab root. Keep each variant's renderer disabled in the prefab if it should only appear at its assigned LOD.
3. Add `AuthoredLodGroupSetup` to the prefab root.
4. Assign the detailed renderers to LOD 0, simpler renderers to LOD 1, and very simple renderers to LOD 2.
5. Optionally assign a final low-cost renderer set to the cull level. Leave it empty to let Unity cull the object after the last LOD threshold.
6. Keep the transition heights in descending order. The defaults are starting values, not universal settings.
7. Use the component context menu **Configure LOD Group** or leave Configure On Awake enabled.
8. Test from several camera distances and angles to check for gaps, overlapping variants and distracting transitions.

## Suggested starting content

- Trees: full branch/leaf mesh → reduced canopy mesh → simple silhouette.
- Houses: full roof and wall detail → simplified walls and roof → low-detail shape.
- Rocks: detailed mesh → reduced polygon mesh → optional simple silhouette.

Use original or properly licensed assets. Keep colliders on a stable root or dedicated collision child; do not put gameplay collision on render-only LOD variants.

## Mobile guidance

- LOD reduces vertex and pixel work only when simpler meshes/materials are actually assigned.
- Prefer fewer material slots and shared materials across variants.
- Avoid expensive cross-fades on mobile until measured; this setup uses standard non-fading transitions.
- Use Phase 19 distance culling for groups of distant decorative objects, not as a replacement for per-object LOD.
- Do not add LOD components to UI, player controls or essential navigation markers.

## Acceptance test

On the same scene and Android device, compare LOD enabled/disabled while moving along a repeatable route. Record frame time, visible mesh complexity, memory and visual transition quality. Verify all LOD renderer sets belong to the intended prefab and that no visible gaps appear.

## Limitations

- This component configures renderer references supplied manually; it does not generate simplified meshes.
- Unity LODGroup behavior and transition thresholds must be verified in the target Unity version and scene.
- No Unity compilation, Android build or Redmi 12 performance test has been performed.
