# Mobile input integration — follow-up implementation

## Added
- `MobileJoystick.cs`: pointer/touch joystick implementing Unity EventSystem interfaces. It normalizes the drag vector and resets on pointer release or disable.
- `TouchCameraLook.cs`: drag-area adapter for `ThirdPersonCameraRig.AddLookInput`.
- `MobilePlayerInput.cs`: movement, sprint and jump events are also forwarded to the optional `PlayerAnimationDriver`.

## Unity scene wiring
1. Add an EventSystem to the scene.
2. Add a Canvas with a GraphicRaycaster.
3. Create a joystick background and handle using RectTransforms; attach `MobileJoystick` to the background and assign its references.
4. Create a separate transparent UI Image on the right side of the screen; keep Raycast Target enabled and attach `TouchCameraLook`.
5. Assign the scene's `ThirdPersonCameraRig` to TouchCameraLook.
6. Add Jump and Sprint UI buttons. Wire Jump to `MobilePlayerInput.OnJumpPressed`; use EventTrigger Pointer Down/Up for Sprint.
7. Assign PlayerAnimationDriver to MobilePlayerInput when an Animator is available.

## Input behavior
- Left touch region: movement joystick.
- Right touch region: camera drag.
- Jump and Sprint: separate UI buttons.
- Releasing the joystick recenters it and sends zero movement.

## Limitations
This is source code only. The UI objects, Canvas layout, Animator Controller, scene references and device testing must be completed in Unity Editor. Test pointer event behavior on a physical Android build before considering the input system complete.
