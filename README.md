# Nepal Battle Royale

An original Nepal-inspired 3D battle royale prototype for Android, built with Unity and C#.

## Current status

This repository contains Unity C# starter scripts and implementation notes. It is **not yet a complete Unity project or playable APK**. No Unity compilation or physical-device acceptance test has been run in this environment.

## Requirements

- Unity 2022.3 LTS or a later supported Unity LTS after package compatibility checks
- Android Build Support, Android SDK & NDK Tools, OpenJDK (install through Unity Hub)
- Git

## Quick start

1. Create a new Unity 3D project using the Universal Render Pipeline (URP) template.
2. Copy this repository's `Assets/` folder into the Unity project.
3. Open Unity and resolve any compile errors.
4. Create an empty GameObject named `NepalWorld` and add `NepalMapGenerator`.
5. Use the component context menu **Generate Nepal Map**.
6. Create a player object with a CharacterController, ThirdPersonMotor and MobilePlayerInput.
7. Add a camera with ThirdPersonCameraRig and assign the player as Target.
8. Build a Canvas/EventSystem and wire MobileJoystick, TouchCameraLook, Jump and Sprint controls using the phase notes.
9. Save the scene and test an Android Development Build on a physical device.

## Source milestones

| Milestone | Source status | Editor/device verification |
|---|---|---|
| 1. Foundation and architecture | Documentation committed | Pending |
| 2. Procedural Nepal-inspired terrain and biome fallback | Scripts committed | Pending |
| 3. Third-person movement and camera | Scripts committed | Pending |
| 4. Character animation data/driver | Scripts committed | Pending |
| 5. Mobile joystick and camera drag input | Scripts committed | Pending |
| 6. Main menu and match HUD foundation | Scripts committed | Pending |
| 7. Non-combat NavMesh waypoint test agent | Script committed | Pending |
| 8. Nepal-inspired world decoration scatterer | Script and setup guide committed | Pending |
| 9. Networking benchmark | Not started | Not started |
| 10-13. Online systems, match loop, optimization and release | Not started | Not started |

## Documentation

- [Technical architecture](Documentation/TECHNICAL_ARCHITECTURE.md)
- [Phase 1-2 world prototype](Documentation/PHASE_1_2.md)
- [Phase 2 visual pass](Documentation/PHASE_2_VISUAL_PASS.md)
- [Phase 3 movement](Documentation/PHASE_3_MOVEMENT.md)
- [Phase 4 character animation](Documentation/PHASE_4_CHARACTER_ANIMATION.md)
- [Mobile input integration](Documentation/PHASE_5_MOBILE_INPUT.md)
- [Main menu and HUD foundation](Documentation/PHASE_6_UI_FOUNDATION.md)
- [Phase 7 navigation test agents](Documentation/PHASE_7_NAVIGATION_TEST_AGENTS.md)\n- [Phase 8 world decoration](Documentation/PHASE_8_WORLD_DECORATION.md)

## Design principles

- Original world, branding, models, UI and audio; use only original or properly licensed assets.
- Fictional Nepal-inspired locations rather than claiming a geographically exact replica.
- Server-authoritative multiplayer when online play is implemented.
- Never commit secrets, signing keys or private credentials.
- A source-code milestone is complete only after compilation and acceptance testing.

## Redmi 12 baseline

Use a scalable URP setup and begin with a conservative quality profile. Treat frame-rate and quality settings as hypotheses until measured on the actual phone. Profile CPU, GPU, memory, thermals and battery during real play sessions.

## Important

A GitHub source commit is not a generated Unity scene, a successful build, or a tested Android APK.
