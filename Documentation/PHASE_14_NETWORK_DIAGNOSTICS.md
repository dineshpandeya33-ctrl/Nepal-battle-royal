# Phase 14 — Network diagnostics UI

## Goal

Show the session lifecycle and quality measurements together in a small Unity UI panel. This is a presentation layer only: it does not connect to a provider, send ping traffic, create rooms, or synchronize player objects.

## Files

- `Assets/Scripts/Online/NetworkDiagnosticsPanel.cs`
- Uses `NetworkSessionStateController` from Phase 13.
- Uses `NetworkQualityMonitor` from Phase 12.

## Unity setup

1. Create a Canvas and add a small Panel.
2. Add four legacy UI Text labels to the panel: Session, Average RTT, Jitter, and Failed Samples.
3. Add `NetworkDiagnosticsPanel` to an empty GameObject.
4. Assign the Phase 13 session controller and Phase 12 quality monitor.
5. Drag the four Text components into their matching optional label fields.
6. Keep the panel hidden in normal player-facing builds if it is only needed for development diagnostics.

Any label can be left unassigned. If the quality monitor is not assigned, the panel displays placeholders. The panel refreshes at a configurable interval using unscaled time, so it remains readable when gameplay time is paused.

## Data flow

A future networking-provider adapter should report actual lifecycle callbacks to `NetworkSessionStateController` and pass measured round-trip samples or failed requests to `NetworkQualityMonitor`. The diagnostics panel reads those components; it does not invent measurements.

## Test checklist

- [ ] Panel displays Disconnected when the session controller starts in that state.
- [ ] A reported valid state transition updates the session label.
- [ ] Recorded RTT samples update average RTT and jitter.
- [ ] Failed samples update the failed count and percentage.
- [ ] Missing optional references do not cause null-reference errors.
- [ ] Disable and re-enable the panel; verify it still receives state changes once.
- [ ] Verify the panel does not send network traffic by itself.

## Limitations

This source has not been compiled in the Unity Editor or tested on a device. It uses Unity's legacy `UnityEngine.UI.Text`; if the project uses TextMeshPro, adapt the label fields to TMP_Text and add the TextMeshPro package. Real measurements require a separately implemented and tested networking adapter.
