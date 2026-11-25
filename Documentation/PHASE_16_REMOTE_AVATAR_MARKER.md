# Phase 16 — Remote avatar marker presentation

## Goal

Add a small, provider-neutral visual component for a remote player's presence. It is deliberately limited to a visible marker and smoothed transform updates; it is not a network transport, player-spawning system, or combat feature.

## New source

- `Assets/Scripts/Online/RemoteAvatarMarkerView.cs`

## Unity setup

1. In a test scene, create a simple capsule or other original marker mesh.
2. Add `RemoteAvatarMarkerView` to the marker root.
3. Optionally add a world-space Canvas with a legacy UI Text element and assign it to **Owner Label**.
4. Keep one marker per remote owner. Do not reuse a marker for a different owner ID; destroy it and create a new view through the future session layer.
5. When a future provider adapter receives a validated remote transform snapshot, call `ApplyRemoteSnapshot(ownerId, position, rotation)` on Unity's main thread.
6. Tune smoothing and snap distance after testing in the Unity Editor and on target Android hardware.

## Behavior

- Stores the first stable owner ID and rejects attempts to change it.
- Ignores empty IDs and non-finite transform values.
- Smoothly interpolates position and rotation between received snapshots.
- Snaps on the first snapshot or when the marker is very far from the new position.
- Does not use a Rigidbody or CharacterController; this is a presentation-only view.

## Acceptance checks

- First valid snapshot places the marker at the supplied transform.
- Later snapshots move and rotate the marker smoothly.
- Empty owner IDs and invalid numeric values are ignored.
- A different owner ID cannot take over an existing marker.
- The marker remains presentation-only and does not affect local player movement.

## Limitations

- No provider adapter currently sends player snapshots.
- No remote marker spawning, despawning, ownership authentication, interpolation buffering, prediction, or server validation is implemented.
- This source has not been compiled in Unity or tested on an Android device.
- A two-client session still requires a selected SDK, transport/session setup, and actual provider callbacks.
