# Phase 7 — Non-combat navigation test agents

## Included
- `WaypointPatrolAgent.cs`: a NavMeshAgent-based character that walks between assigned waypoint transforms and pauses at each point.

This provides a safe movement and navigation test for the world prototype. It does not pursue, target, or attack the player and is not a complete battle royale opponent AI.

## Unity setup
1. Install the AI Navigation package compatible with the Unity Editor version being used.
2. Add a NavMeshSurface to a scene object and configure the walkable geometry/layers.
3. Bake the NavMesh after the terrain and static walkable surfaces are present.
4. Create a capsule or character prefab and add a NavMeshAgent.
5. Add WaypointPatrolAgent to the same object.
6. Create at least two empty waypoint GameObjects on the baked walkable area.
7. Assign the waypoint Transforms in order in the Inspector.
8. Press Play and confirm the agent walks between points and pauses briefly at each one.

## Inspector starting values
- NavMeshAgent Speed: 2.5
- Acceleration: 8
- Angular Speed: 180
- Stopping Distance: 0.25
- Wait At Waypoint: 1.5 seconds
- Arrival Distance: 0.4

Tune these values after profiling on the target scene. Keep the number of active agents low on the Redmi 12 until CPU usage is measured.

## Acceptance checks
- [ ] A baked NavMesh is visible in the Scene view.
- [ ] The agent reports that it is on the NavMesh.
- [ ] The agent reaches each assigned waypoint.
- [ ] The agent pauses at each point.
- [ ] Looping patrol returns to the first point.
- [ ] With Loop disabled, the agent stops at the final point.

## Important
A NavMesh must be baked before agents can navigate. Unity's NavMeshAgent uses a baked navigation mesh for pathfinding; see the official [NavMeshAgent reference](https://docs.unity.com/en-us/engine/6000.0/script-reference/unityengine/ai/navmeshagent) and [NavMesh Surface documentation](https://github.com/Unity-Technologies/NavMeshComponents/blob/master/Documentation/NavMeshSurface.md).
