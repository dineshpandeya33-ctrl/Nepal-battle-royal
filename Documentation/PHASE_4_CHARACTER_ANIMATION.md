# Phase 4 — Character animation foundation

## Included
- `PlayerAnimationDriver.cs`: drives a Mecanim Animator from mobile movement values.
- `PlayerCharacterDefinition.cs`: ScriptableObject data for future character selection.

## Animator parameters
Create these exact parameters in an Animator Controller:
- `Speed` — Float: 0 idle, 0.65 walk, 1 sprint.
- `Grounded` — Bool.
- `Jump` — Trigger.

Create Idle/Locomotion and Jump states and configure transitions. Assign the Animator and CharacterController references in the Inspector. Call `SetMovement(joystickVector, sprintHeld)` from the mobile input script and `TriggerJump()` when the jump button is pressed.

Use animation clips and character models that you created or have permission to use. Keep root motion disabled because the CharacterController owns movement.

## Character data
In Unity, choose Assets > Create > Nepal Battle Royale > Character Definition. Enter a unique ID, display name, prefab, portrait and description. This creates character data only; the selection menu and prefab spawner are later tasks.

## Redmi 12 starting profile
Start with one skinned character, one Animator Controller, a modest bone count, and limited real-time shadows. These are starting settings, not measured performance results. Profile on the actual device before raising quality.

## Status
Source files only. Not compiled or tested in Unity. Animation clips, transitions, UI wiring and Android build remain to be completed in the Unity Editor.
