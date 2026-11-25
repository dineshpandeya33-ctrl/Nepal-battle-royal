# Phase 3 — Third-person movement starter

## Included source
- `ThirdPersonMotor.cs`: camera-relative CharacterController movement, sprint, jump and gravity.
- `MobilePlayerInput.cs`: bridge between UI events and the motor.
- `MobileJoystick.cs`: pointer/touch joystick component.
- `ThirdPersonCameraRig.cs`: smooth follow camera with a look-input API.

## Unity Editor setup (required)
1. Create a player GameObject with a Capsule and CharacterController.
2. Add `ThirdPersonMotor` and `MobilePlayerInput`.
3. Create a camera and add `ThirdPersonCameraRig`; assign the player as Target.
4. Create a Canvas with a joystick background Image and a child handle Image.
5. Add `MobileJoystick` to the background, assign its handle and player input.
6. Add UI Buttons for Jump and Sprint. Wire Jump OnClick to `MobilePlayerInput.OnJumpPressed`; wire Sprint Pointer Down/Up using an EventTrigger to the pressed/released methods.
7. Ensure an EventSystem exists in the scene and the Canvas has a GraphicRaycaster.
8. Test movement in the Unity Editor and on an Android Development Build.

## Notes and limitations
- This is a controller source-code milestone, not a compiled or tested Unity scene.
- The camera's look API is ready for a touch-drag UI handler; that handler is not included yet.
- Animation blending, crouch, swimming, slope tuning, footstep audio, and device-specific sensitivity remain future tasks.
- Use Unity's Input System package for a production input abstraction if the project later standardizes on it.
- No multiplayer synchronization is included in this milestone.
