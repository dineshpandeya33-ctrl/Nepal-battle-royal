# Phase 6 — Main menu and match HUD foundation

## Included
- `GameMode.cs`: Solo, Duo, Squad and Practice mode identifiers.
- `MainMenuController.cs`: inspector-wired main, mode-selection and settings panels; stores the selected mode locally.
- `MatchHudController.cs`: presentation-only mode label, player-count label and elapsed match timer.

## Unity UI setup
1. Create a Canvas with a GraphicRaycaster and an EventSystem.
2. Create Main, Mode Selection and Settings panel GameObjects.
3. Add MainMenuController to a scene object and assign all panel references.
4. Add UI Buttons and wire Play, Settings, Back, Select Solo/Duo/Squad/Practice and Start methods in the Inspector.
5. Add optional Unity UI Text labels for selected mode and status messages.
6. Create a separate match HUD Canvas with Text labels for mode, player count and timer; assign them to MatchHudController.
7. If a prototype gameplay scene exists, enter its exact name in prototypeSceneName and add that scene to Build Settings.

## Scope and limitations
- Mode selection is local UI state only. Duo/Squad do not create online parties or matchmaking.
- The player count is a display value; no player spawning or match authority exists yet.
- Settings panel is a placeholder for later audio, graphics and sensitivity controls.
- Scene loading works only after the scene exists and is included in Build Settings.
- Create the actual Canvas, panels, buttons, fonts and layout in Unity Editor.

## Acceptance checks
- [ ] Main, mode and settings panels switch correctly.
- [ ] Each mode button updates the selected mode label.
- [ ] Start shows a clear message when no scene is configured.
- [ ] Match HUD timer starts and pauses through its public methods.
- [ ] UI remains readable in landscape aspect ratios and safe areas.
- [ ] Android touch testing completed on Redmi 12.
