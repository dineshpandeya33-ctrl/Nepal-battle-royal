using UnityEngine;
using UnityEngine.UI;

namespace NepalBattleRoyale.Online
{
    /// <summary>
    /// Visual-only representation of a remote player marker.
    /// A future trusted networking adapter may feed snapshots into this view.
    /// This component does not create connections, spawn players, or handle combat.
    /// </summary>
    public class RemoteAvatarMarkerView : MonoBehaviour
    {
        [Header("Presentation")]
        [SerializeField] private Text ownerLabel;
        [SerializeField, Min(0.01f)] private float positionSmoothTime = 0.12f;
        [SerializeField, Min(0f)] private float rotationLerpSpeed = 12f;
        [SerializeField, Min(0.1f)] private float snapDistance = 8f;

        private string ownerId;
        private Vector3 targetPosition;
        private Quaternion targetRotation;
        private Vector3 positionVelocity;
        private bool hasSnapshot;

        public string OwnerId => ownerId;
        public bool HasSnapshot => hasSnapshot;

        /// <summary>
        /// Apply a validated snapshot received from the future provider adapter.
        /// Call on Unity's main thread. This only updates presentation.
        /// </summary>
        public void ApplyRemoteSnapshot(string stableOwnerId, Vector3 position, Quaternion rotation)
        {
            if (string.IsNullOrWhiteSpace(stableOwnerId))
            {
                Debug.LogWarning("Remote marker snapshot ignored: owner ID is empty.", this);
                return;
            }

            if (!IsFinite(position) || !IsFinite(rotation))
            {
                Debug.LogWarning("Remote marker snapshot ignored: transform is not finite.", this);
                return;
            }

            if (hasSnapshot && ownerId != stableOwnerId)
            {
                Debug.LogWarning("Remote marker cannot be reassigned to a different owner.", this);
                return;
            }

            ownerId = stableOwnerId;
            targetPosition = position;
            targetRotation = rotation.normalized;

            if (!hasSnapshot || Vector3.Distance(transform.position, targetPosition) > snapDistance)
            {
                transform.SetPositionAndRotation(targetPosition, targetRotation);
                positionVelocity = Vector3.zero;
            }

            hasSnapshot = true;
            if (ownerLabel != null)
                ownerLabel.text = "Player " + ownerId;
        }

        private void Update()
        {
            if (!hasSnapshot) return;

            transform.position = Vector3.SmoothDamp(
                transform.position, targetPosition, ref positionVelocity,
                Mathf.Max(0.01f, positionSmoothTime));

            float rotationStep = Mathf.Max(0f, rotationLerpSpeed) * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(
                transform.rotation, targetRotation, rotationStep);
        }

        private static bool IsFinite(Vector3 value)
        {
            return IsFinite(value.x) && IsFinite(value.y) && IsFinite(value.z);
        }

        private static bool IsFinite(Quaternion value)
        {
            return IsFinite(value.x) && IsFinite(value.y) &&
                   IsFinite(value.z) && IsFinite(value.w) &&
                   value.sqrMagnitude > 0.000001f;
        }

        private static bool IsFinite(float value)
        {
            return !float.IsNaN(value) && !float.IsInfinity(value);
        }
    }
}
