# Phase 12 — Network quality metrics foundation

## Included
- `NetworkQualityMonitor.cs` stores recent round-trip-time (RTT) samples supplied by a future networking adapter.
- Reports average, minimum, maximum and average consecutive-sample variation (jitter).
- Counts failed samples and exposes a failure percentage.
- Optional on-screen debug overlay for development builds.
- No networking SDK is required yet. The component does not open sockets, connect to servers, or generate traffic.

## Unity setup
1. Add `NetworkQualityMonitor` to a dedicated GameObject in a test scene.
2. Leave the debug overlay enabled during development, then disable it for normal play.
3. When a networking SDK is integrated, call `RecordRoundTripMilliseconds(value)` after a completed ping/round-trip measurement.
4. Call `RecordFailure()` when a measurement times out or cannot complete.
5. Read the public properties to send metrics to a future test report or proper UI.

## Metric meanings
- **RTT:** elapsed time for a request and its response, in milliseconds.
- **Average/minimum/maximum:** summaries of the most recent valid RTT samples.
- **Jitter:** average absolute difference between consecutive valid RTT samples in the current sample window. It is a simple diagnostic indicator, not a full networking-standard jitter calculation.
- **Failure percentage:** failed measurement count divided by all recorded attempts.

## Benchmark plan
- Run a controlled test with two to eight test clients on a dedicated test environment.
- Record RTT, jitter, failed measurements, server CPU/memory, bandwidth and disconnect/reconnect behavior.
- Repeat under stable Wi-Fi and mobile-network conditions; record region and test duration.
- Compare networking providers only after implementing equivalent test scenes and server configurations.
- Do not infer support for large player counts from a small-client test.

## Mobile notes
- The overlay uses Unity's immediate-mode GUI for development diagnostics. Unity documents that `OnGUI` may be called multiple times per frame, so disable the overlay outside profiling sessions. See the Unity scripting reference: https://docs.unity.com/en-us/engine/6000.7/script-reference/unityengine/monobehaviour/ongui
- This component stores only a small rolling sample window. Keep diagnostic collection lightweight on Android.

## Acceptance checks
- [ ] A test adapter can submit valid RTT values.
- [ ] Average, minimum, maximum and jitter update as expected.
- [ ] Invalid or failed measurements increase the failure count once.
- [ ] Reset clears all statistics.
- [ ] Overlay can be disabled.
- [ ] Real networking SDK integration and device tests are completed before any performance claims.

## Important
This is a metrics foundation, not a multiplayer implementation or a real network benchmark. No provider has been selected, no server is connected, and Unity compilation / Redmi 12 verification remain pending.
