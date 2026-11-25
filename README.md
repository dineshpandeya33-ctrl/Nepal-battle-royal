# Nepal Battle Royale

An original Nepal-inspired 3D battle royale for Android, built with Unity and C#.

## Current milestone
Phase 1-2 foundation and procedural map starter. This repository contains technical documentation and Unity C# starter scripts. It is not yet a complete Unity project or playable APK.

## Requirements
- Unity 2022.3 LTS or newer supported Unity LTS
- Android Build Support, Android SDK & NDK Tools, OpenJDK (install through Unity Hub)
- Git

## Quick start
1. Create a new Unity **3D (Core)** project.
2. Copy this repository's `Assets/` folder into the Unity project.
3. Open Unity and wait for scripts to compile.
4. Create an empty GameObject named `NepalWorld`.
5. Add the `NepalMapGenerator` component.
6. Use the component context menu **Generate Nepal Map**.
7. Save the scene as `Assets/Scenes/NepalPrototype.unity`.

The generator creates a fictional Nepal-inspired greybox terrain and region markers. Final art, buildings, networking, combat, and Android packaging are not included yet.

## Documentation
- [Technical architecture and roadmap](Documentation/TECHNICAL_ARCHITECTURE.md)
- [Phase 1-2 implementation notes](Documentation/PHASE_1_2.md)

## Principles
- Original world, branding, models, UI, and audio.
- Server-authoritative multiplayer when online play is implemented.
- Never commit secrets or private keys.
- A feature is complete only after compilation and acceptance testing.

## Status
| Phase | Status |
|---|---|
| 1. Foundation | Starter docs committed; local Unity project setup remains |
| 2. World prototype | Procedural terrain scripts committed; scene generation and visual QA remain |
| 3. Player movement | Planned |
| 4-5. Gameplay and AI | Planned |
| 6-8. Online multiplayer | Provider evaluation pending |
| 9-12. Battle royale systems and Android release | Planned |
