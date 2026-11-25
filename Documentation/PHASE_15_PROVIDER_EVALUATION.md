# Phase 15 — Provider evaluation and adapter contract

## Goal

Keep the game independent of a specific networking SDK until an equivalent small test has been completed. This phase adds a C# contract for a future provider adapter and records the evaluation plan.

## New source

- `Assets/Scripts/Online/INetworkProviderAdapter.cs`

The interface describes the minimum boundary expected from a provider-specific implementation:

- Report whether its client has started.
- Request client start and stop.
- Publish confirmed session lifecycle changes.
- Publish measured round-trip times and failed measurement events.

The interface does not connect to a service, open sockets, authenticate, create rooms, or synchronize game objects. A real adapter must be written against the selected SDK and must raise events only from actual provider callbacks or completed measurements.

## Candidate comparison

| Candidate | Useful fit | Main trade-off to evaluate |
|---|---|---|
| Unity Netcode for GameObjects (NGO) | High-level networking for Unity GameObject and MonoBehaviour projects. | The team must select and configure a transport, session services, hosting, authentication and server operations. NGO is not a hosted game-server service by itself. |
| Photon Fusion | Integrated real-time networking with Shared, Host and Dedicated Server topologies. | Topology changes the authority model, hosting needs, operational cost and implementation approach. Check current plan limits and deployment costs before choosing. |

Official references:
- Unity netcode package overview: https://docs.unity.com/multiplayer/netcode/netcode
- Photon Fusion topology choices: https://doc.photonengine.com/fusion/v2/fusion-choose

This is an initial architecture comparison, not a provider selection. Exact package versions and compatibility must be checked against the Unity Editor version chosen for the actual project.

## Two-client connectivity test plan

Run this only after a provider adapter and a test session service have been implemented.

1. Create a small empty test scene with two visible, non-combat player markers.
2. Start one client as the session host/server where supported.
3. Join from a second client using a separate device or editor instance.
4. Confirm both clients see both markers and that each marker has a stable owner identifier.
5. Move one marker using a harmless test input and confirm the other client receives the updated position.
6. Record RTT, jitter and failed measurements through the Phase 12 monitor.
7. Stop one client normally, then test an unexpected disconnect and reconnect.
8. Repeat on Wi-Fi and mobile data; record device, region, duration and build version.

## Pass criteria

- Both clients join the same test session.
- Each client sees one local marker and one remote marker.
- Remote state updates arrive without duplicate marker creation.
- A disconnect is reflected in the session lifecycle UI.
- Rejoining restores the expected session state.
- Diagnostics show measured values from the provider adapter, not fabricated test values.
- No credentials or service secrets are committed to Git.

## Current limitations

- No provider has been selected or installed.
- No adapter implementation exists yet; only the interface contract is committed.
- No live session or two-client test has been run.
- The repository is still source scripts and documentation, not a complete Unity project or APK.
- Unity compilation and Redmi 12 testing remain pending.

Because development is currently being done from an Android phone without a computer, this phase prepares the code boundary and test procedure. Running Unity Editor tests will still require access to a compatible computer or a trusted remote Unity build environment.
