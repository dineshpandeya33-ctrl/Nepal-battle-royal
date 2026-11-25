# Phase 17 — Provider diagnostics bridge

## Goal

Connect a future provider-specific component to the session lifecycle and network quality systems without coupling those systems to a particular SDK.

## New source

- `Assets/Scripts/Online/NetworkProviderDiagnosticsBridge.cs`

## Setup

1. Implement `INetworkProviderAdapter` in a provider-specific MonoBehaviour after selecting and installing a supported networking SDK.
2. Add `NetworkProviderDiagnosticsBridge` to a scene GameObject.
3. Assign the provider component to **Provider Behaviour**.
4. Assign the existing `NetworkSessionStateController` and `NetworkQualityMonitor`.
5. Connect the menu's Connect and Disconnect buttons to `RequestStartClient` and `RequestStopClient`.
6. Ensure provider callbacks are dispatched on Unity's main thread before raising the interface events.

## Data flow

- Provider session callbacks -> `SessionStateChanged` -> session state controller.
- Completed RTT measurements -> `RoundTripTimeMeasured` -> quality monitor.
- Failed measurements -> `MeasurementFailed` -> quality monitor.
- UI reads the existing session and quality components through Phase 14 diagnostics panel.

## Safety and correctness notes

- The bridge does not invent measurements or simulate successful connections.
- It does not implement transport, authentication, matchmaking, server hosting, or remote player spawning.
- The provider adapter must validate incoming provider data and report only real state changes.
- UI and Unity scene objects must only be touched on Unity's main thread.
- Never store service secrets in the client project or commit credentials to Git.

## Verification checklist

- With no provider assigned, button requests produce a clear warning and do not claim connection success.
- With a valid test adapter, start/stop requests reach the adapter.
- Session events update the lifecycle controller.
- RTT and failed measurement events update the quality monitor.
- Disable/enable the bridge repeatedly and confirm event handlers are not duplicated.

## Current status

This is a provider integration bridge, not a working SDK implementation. A real SDK, compatible package version, credentials/configuration where applicable, Unity Editor compilation, and two-client testing are still required. No provider has been selected yet.
