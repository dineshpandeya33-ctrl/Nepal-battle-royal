# Nepal Battle Royale — Technical Architecture

## Product goal

Build an original Android third-person battle royale set in a fictional map inspired by Nepal's geography. The first deliverable is a small, locally playable vertical slice. A 50-player online release is a later milestone and must not be represented as complete until tested on real devices and hosted servers.

## Proposed client architecture

- **Engine:** Unity 2022.3 LTS (or a later supported Unity LTS after package compatibility checks).
- **Language:** C#.
- **Rendering:** Start with URP for mobile scalability; profile before selecting quality tiers.
- **World:** Unity Terrain, terrain tiles/chunks, baked lighting where possible, LOD groups, occlusion culling, GPU instancing, and pooled props.
- **Input:** Unity Input System with touch controls and optional gamepad support.
- **Code organization:** feature-oriented assemblies/namespaces: Core, World, Player, Gameplay, UI, Online, Audio.
- **Content:** original or properly licensed assets only. Do not redistribute commercial assets or copyrighted game assets.

## Multiplayer options evaluation

Do not select a networking provider until a small benchmark prototype has been tested.

### Photon Fusion

Potential strengths:
- Purpose-built real-time networking and room/session workflows.
- Supports server-authoritative topologies and dedicated-server deployment patterns.
- Managed Photon services can reduce initial infrastructure work.

Cost considerations:
- Commercial plans and concurrent-user/traffic limits depend on the current Photon plan.
- Dedicated server compute, bandwidth, regional deployment, and monitoring may be additional costs.
- Obtain current pricing from Photon before launch; do not assume free tiers cover a production 50-player game.

### Unity Netcode for GameObjects (NGO)

Potential strengths:
- Unity-native networking package and integration with Unity services.
- Useful for a Unity-first stack and custom server hosting.
- Can be paired with Unity Transport and a dedicated server build.

Cost considerations:
- NGO itself is a package, not a free hosted 50-player backend.
- Hosting, orchestration, bandwidth, authentication, matchmaking, database, observability, and DDoS protection are separate operational costs.
- Unity Gaming Services pricing and limits must be checked for the target regions and expected traffic.

### Decision gate

Build the same small network benchmark in both candidates:
1. 2-8 clients moving in a test arena.
2. Server-authoritative movement and state.
3. Replicated interactions and latency simulation.
4. Disconnect/reconnect test.
5. Load test with bots or simulated clients up to the target concurrency.
6. Measure bandwidth, CPU, server tick stability, and mobile battery use.

Choose the provider based on measured performance, regional availability, developer complexity, and a written monthly cost estimate. Do not promise 50-player matches until the load test passes.

## Proposed online topology

Android client -> TLS authentication/API -> matchmaking/session service -> dedicated authoritative match server -> persistent backend.

The match server owns authoritative player state, movement validation, item state, match phase, and hit/damage outcomes. Clients send input/intent; they do not decide authoritative results. Persistent services store account profiles, friends, progression, and match summaries. Voice should use a dedicated voice provider or a separately evaluated voice service; do not relay raw voice through the gameplay server.

## Security

- Never put service-role keys, database passwords, signing keys, or private server credentials in the Android client or Git repository.
- Use short-lived access tokens and server-side authorization.
- Validate movement, fire cadence, inventory changes, and match results on the server.
- Rate-limit account and room APIs.
- Use server logs and anomaly detection; anti-cheat is layered and cannot be guaranteed by client-side checks alone.
- Keep secrets in environment variables or a managed secret store.
- Protect release signing keys outside GitHub.

## Hosting cost model

A trustworthy estimate requires region, expected concurrent players, average match duration, traffic per player, server tick rate, database usage, and provider plan. These are not yet known, so no exact monthly price is asserted.

Budget categories:
- Dedicated game-server CPU/RAM instances and autoscaling.
- Egress bandwidth and regional networking.
- Authentication, matchmaking, relay or session services.
- Database and backups.
- Logs, metrics, crash reporting, and alerting.
- Voice service, if enabled.
- DDoS protection and operational reserve.

Before multiplayer implementation, prepare low/medium/high concurrency scenarios and calculate: (active server instances × hourly instance price × hours) + bandwidth + managed services + storage + monitoring. Confirm current vendor quotes and free-tier limits.

## World design

The map is fictional rather than a literal geographic replica. Seven broad biomes/landmarks are planned:
- Kathmandu-inspired urban basin.
- Pokhara-inspired lake and foothills.
- Mustang-inspired arid highlands.
- Everest-inspired snowy high mountains.
- Chitwan-inspired subtropical forest.
- Terai-inspired agricultural plains.
- Lumbini-inspired heritage settlement.

Use an original layout to make traversal and gameplay readable. Avoid claiming exact geographic accuracy.

## Development roadmap and acceptance criteria

1. **Foundation:** repository conventions, Unity project setup, package manifest, architecture docs, Android build settings.
2. **World prototype:** generated terrain, seven region markers, traversal routes, one test scene; verify on a low-end Android device.
3. **Player:** third-person controller, camera, input remapping, mobile touch UI.
4. **Gameplay foundation:** abstract game interactions, inventory state, health state, animation hooks, UI.
5. **AI test actors:** navigation and test behavior for local development.
6. **Networking spike:** compare Fusion and NGO with a small authoritative session.
7. **Social:** profiles, friends, invitations, private rooms, squads.
8. **Scale test:** dedicated server deployment and measured 50-player load test.
9. **Match loop:** loot, safe area, vehicles, spectator/result flow.
10. **Presentation:** original UI, audio, animations, accessibility.
11. **Optimization:** device matrix, thermal/battery, memory, network profiling.
12. **Release:** signing, privacy policy, store listing, AAB and staged rollout.

## Definition of done

A feature is complete only when its code is committed, setup is documented, it compiles in the declared Unity version, and its acceptance test passes. A GitHub source commit is not the same as a built or tested Android APK.
