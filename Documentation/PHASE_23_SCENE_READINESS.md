# Phase 23 — Scene Readiness Validation

## Goal
Provide a simple in-scene checklist that reports missing references before attempting an Android Development Build.

## Added script
- `Assets/Scripts/Diagnostics/SceneReadinessValidator.cs`

## Setup
1. In the prototype scene, create an empty GameObject named `SceneReadiness`.
2. Add the `SceneReadinessValidator` component.
3. Assign the existing NepalMapGenerator, player root, ThirdPersonMotor, MobilePlayerInput, gameplay Camera, ThirdPersonCameraRig, Canvas, EventSystem, MainMenuController, MobilePerformanceProfile, and SafeAreaFitter references.
4. Leave **Validate On Start** enabled during development, or disable it and run the component context menu **Validate Scene Readiness** when needed.
5. Review the Console. Assign missing references and validate again.
6. Remove or disable the validator in release scenes if you do not want development diagnostics in the build.

## What it checks
- Required scene references are assigned.
- Player root and motor belong to the same hierarchy.
- Assigned camera and camera rig use the same Transform.

## What it does not check
- It does not generate a Unity scene or automatically create missing objects.
- It does not verify that joystick pointer events are wired correctly.
- It does not compile scripts, validate imported assets, test NavMesh baking, connect multiplayer, or build an APK.
- A passing checklist is not a substitute for Play Mode and physical-device testing.

## Verification status
- Source and documentation committed to GitHub.
- Unity compilation and Play Mode validation remain pending.
- Android APK and Redmi 12 testing remain pending.
