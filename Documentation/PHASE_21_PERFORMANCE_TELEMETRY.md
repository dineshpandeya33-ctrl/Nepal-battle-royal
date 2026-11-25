# Phase 21 — Performance Telemetry Overlay

## Goal
Add a lightweight development overlay so the team can observe frame rate, frame time, Unity memory allocation/reservation, active quality level, and configured target frame rate while testing the Nepal-inspired environment.

## Added script
- `Assets/Scripts/Mobile/PerformanceTelemetryOverlay.cs`

## Unity setup
1. In the Unity scene, create or select a Screen Space - Overlay Canvas.
2. Add a legacy UI **Text** element as a child of the Canvas. Place it in a screen corner and use a readable font size.
3. Add `PerformanceTelemetryOverlay` to a scene object that remains active during the test.
4. Drag the Text component into the **Telemetry Text** field.
5. Enable **Start Visible** for development, or leave it off and connect a UI Button's OnClick event to `ToggleOverlay()`.
6. Build a Development Build and compare the readings in the same scene at the same camera position across Low, Balanced, and High quality presets.

## What the overlay reports
- Smoothed frames per second (FPS).
- Average frame time for the latest sample window, in milliseconds.
- Unity total allocated and reserved memory, in MB.
- Current Unity quality level.
- Configured application target frame rate.

## Important interpretation
- Allocated/reserved memory values are Unity profiler memory counters, not a complete measurement of all Android system or GPU memory.
- The overlay does not report GPU frame time, thermal throttling, battery use, network latency, or crash risk.
- FPS and frame time are sampled and smoothed; short spikes may not appear immediately.
- Use Unity Profiler / Android profiling tools for deeper CPU, GPU, rendering, and memory investigation.
- Do not treat any preset or target FPS as verified for Redmi 12 until measured on the actual device in a Development Build.

## Current verification status
- Source and setup documentation committed to GitHub.
- Unity compilation: not run in this environment.
- Android APK: not built.
- Redmi 12 profiling: not performed.

This phase adds observability only. It does not change gameplay, networking, or scene content.
