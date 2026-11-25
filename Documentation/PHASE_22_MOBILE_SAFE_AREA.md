# Phase 22 — Mobile UI Safe Area

## Goal
Keep important menus and HUD controls inside the usable screen area on Android phones with display cutouts, rounded corners, and changing orientations.

## Added script
- `Assets/Scripts/UI/SafeAreaFitter.cs`

## Unity setup
1. Under the Canvas, create a full-screen UI Panel named `SafeAreaRoot`.
2. Set its anchors to stretch across the Canvas and set all offsets to zero.
3. Add `SafeAreaFitter` to that panel.
4. Move essential UI (menu buttons, status labels, touch controls) under `SafeAreaRoot`.
5. Keep decorative backgrounds outside the safe-area panel if they should extend to the screen edges.
6. Test portrait/landscape changes and devices with different display cutouts.

## Behavior
- Reads Unity's `Screen.safeArea` and converts it to normalized RectTransform anchors.
- Refreshes when the safe area, screen dimensions, or orientation changes.
- Exposes `Refresh()` for a manual update.
- Uses only UnityEngine APIs; no third-party package is required.

## Layout notes
- Use a separate full-screen background behind the safe-area panel.
- Avoid putting every visual element inside the safe-area root if edge-to-edge artwork is desired.
- Check Canvas Scaler settings and touch target spacing on the target phone.

## Verification status
- Script and setup notes committed to GitHub.
- Unity compilation and scene integration have not been run here.
- Redmi 12 notch/gesture-area testing remains pending.
- This phase does not create an APK or certify screen layout on a physical device.
