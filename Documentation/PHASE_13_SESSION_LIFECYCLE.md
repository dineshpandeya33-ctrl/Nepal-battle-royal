# Phase 13 — Network session lifecycle

## Included
- `NetworkSessionStateController.cs` defines a provider-neutral connection lifecycle:
  Disconnected, Connecting, Connected, Reconnecting, Disconnecting, and Failed.
- Only allowed state transitions are accepted; invalid transitions are logged and rejected.
- A Unity UI Text field can display the current state.
- Other scripts can subscribe to `StateChanged` to react to confirmed state changes.
- Public Report methods are intended for a future networking SDK adapter to call after it receives real provider callbacks.

## Unity setup
1. Create a GameObject named `NetworkSession`.
2. Add `NetworkSessionStateController`.
3. Optionally create a UI Text label and assign it to Status Label.
4. Keep the initial state as Disconnected.
5. When a provider is integrated, have its callbacks call the matching Report method. Do not call ReportConnected merely because a user pressed a button; wait for the provider to confirm the connection.
6. Subscribe to `StateChanged` for menu feedback or diagnostic logging.

## State flow
- Disconnected → Connecting
- Connecting → Connected, Failed, or Disconnected
- Connected → Reconnecting, Disconnecting, or Failed
- Reconnecting → Connected, Disconnecting, Disconnected, or Failed
- Disconnecting → Disconnected or Failed
- Failed → Disconnected or Connecting

## Acceptance checks
- [ ] Starts in Disconnected.
- [ ] Valid transitions update the label and raise StateChanged.
- [ ] Invalid transitions are rejected without changing the state.
- [ ] Reconnect success returns to Connected.
- [ ] Disconnect completion returns to Disconnected.
- [ ] Real provider callbacks, timeout handling, and device/network tests are integrated before release.

## Important
This is a lifecycle controller only. It does not authenticate users, create rooms, connect devices, synchronize player objects, or provide a server. No multiplayer session is live yet. Unity compilation and Redmi 12 verification remain pending.

Unity initializes component state in Awake before Start; see the official [MonoBehaviour Awake documentation](https://docs.unity.com/en-us/engine/6000.0/script-reference/unityengine/monobehaviour/awake).
