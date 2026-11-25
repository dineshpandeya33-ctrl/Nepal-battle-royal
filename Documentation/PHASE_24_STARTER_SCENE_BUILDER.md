# Phase 24 — Unity Starter Scene Builder

## Goal
Reduce manual Unity Editor setup by generating a basic greybox scene with the existing prototype scripts connected.

## Added editor utility
- `Assets/Editor/NepalStarterSceneBuilder.cs`

## How to run
1. Open the repository's `Assets/` folder inside a Unity 2022.3 LTS (or compatible supported LTS) project.
2. Wait for Unity to compile scripts. Resolve any compile errors before continuing.
3. In the Unity Editor menu, choose **Tools > Nepal Battle Royale > Create Starter Scene**.
4. The utility creates and saves `Assets/Scenes/NepalPrototype.unity`, then adds it to Build Settings.
5. Open the scene and enter Play Mode to inspect the temporary floor, capsule player, follow camera, joystick, camera-look surface, safe-area root, and performance overlay.

## What is generated
- Directional light and ambient lighting.
- NepalWorld object with NepalMapGenerator attached.
- Temporary flat floor for immediate movement testing.
- Capsule player with CharacterController, ThirdPersonMotor, and MobilePlayerInput.
- Main Camera with ThirdPersonCameraRig.
- Screen Space Overlay Canvas, CanvasScaler, EventSystem, movement joystick and camera drag area.
- SafeAreaFitter and PerformanceTelemetryOverlay.

## Manual next steps
- Use the NepalWorld component context menu **Generate Nepal Map** after reviewing terrain settings. The generated terrain replaces the need for the temporary floor; remove or disable the floor when the terrain is ready.
- Replace the capsule with an original rigged character and a compatible Animator Controller.
- Create menu and HUD panels and connect their buttons.
- Configure Android Player Settings and test touch input on a device.
- Check the EventSystem input module against the project's chosen input handling setting.

## Limitations and verification
- This editor utility is not a prebuilt Unity project and cannot run on an Android phone by itself.
- It creates a greybox starter scene, not final art or a complete playable online game.
- It does not install Unity, Android Build Support, SDK, NDK, or JDK.
- Unity compilation, scene execution, Android build, and Redmi 12 testing have not been performed here.
