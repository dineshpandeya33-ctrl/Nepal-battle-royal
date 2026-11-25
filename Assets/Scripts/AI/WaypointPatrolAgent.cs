using UnityEngine;
using UnityEngine.AI;

namespace NepalBattleRoyale.AI
{
    /// <summary>
    /// Non-combat navigation test agent. Walks between scene waypoint transforms
    /// using Unity's NavMeshAgent. This is a movement prototype, not a full opponent AI.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class WaypointPatrolAgent : MonoBehaviour
    {
        [SerializeField] private Transform[] waypoints;
        [SerializeField, Min(0f)] private float waitAtWaypoint = 1.5f;
        [SerializeField, Min(0.05f)] private float arrivalDistance = 0.4f;
        [SerializeField] private bool loop = true;
        [SerializeField] private bool startAutomatically = true;

        private NavMeshAgent agent;
        private int waypointIndex;
        private float waitUntil;
        private bool waiting;
        private bool patrolStarted;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            if (startAutomatically) BeginPatrol();
        }

        public void BeginPatrol()
        {
            if (waypoints == null || waypoints.Length == 0 || agent == null)
                return;

            if (!agent.isOnNavMesh)
            {
                Debug.LogWarning("Patrol agent is not placed on a baked NavMesh.", this);
                return;
            }

            patrolStarted = true;
            waiting = false;
            waypointIndex = Mathf.Clamp(waypointIndex, 0, waypoints.Length - 1);
            SetCurrentDestination();
        }

        public void StopPatrol()
        {
            patrolStarted = false;
            waiting = false;
            if (agent != null && agent.isOnNavMesh)
                agent.isStopped = true;
        }

        private void Update()
        {
            if (!patrolStarted || agent == null || !agent.isOnNavMesh)
                return;

            if (waiting)
            {
                if (Time.time >= waitUntil)
                {
                    waiting = false;
                    AdvanceWaypoint();
                }
                return;
            }

            if (agent.pathPending || agent.pathStatus == NavMeshPathStatus.PathInvalid)
                return;

            if (agent.remainingDistance <= Mathf.Max(arrivalDistance, agent.stoppingDistance))
            {
                agent.isStopped = true;
                waiting = true;
                waitUntil = Time.time + waitAtWaypoint;
            }
        }

        private void AdvanceWaypoint()
        {
            if (waypoints.Length == 0) return;

            int next = waypointIndex + 1;
            if (next >= waypoints.Length)
            {
                if (!loop)
                {
                    StopPatrol();
                    return;
                }
                next = 0;
            }

            waypointIndex = next;
            SetCurrentDestination();
        }

        private void SetCurrentDestination()
        {
            if (waypoints == null || waypoints.Length == 0) return;

            Transform point = waypoints[waypointIndex];
            if (point == null)
            {
                AdvanceWaypoint();
                return;
            }

            agent.isStopped = false;
            agent.SetDestination(point.position);
        }
    }
}
