using UnityEngine;

namespace NepalBattleRoyale.Player
{
    /// <summary>
    /// Smooth follow camera for the greybox prototype.
    /// Drag input can be connected from a UI pointer-drag handler later.
    /// </summary>
    public class ThirdPersonCameraRig : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0f, 3.2f, -5.5f);
        [SerializeField, Min(0.01f)] private float followSmooth = 0.12f;
        [SerializeField, Min(0.1f)] private float lookHeight = 1.4f;
        [SerializeField] private float yaw;
        [SerializeField, Range(-20f, 75f)] private float pitch = 12f;

        private Vector3 velocity;

        public void SetTarget(Transform value) => target = value;

        public void AddLookInput(Vector2 delta)
        {
            yaw += delta.x;
            pitch = Mathf.Clamp(pitch - delta.y, -20f, 75f);
        }

        private void LateUpdate()
        {
            if (target == null) return;
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
            Vector3 desiredPosition = target.position + rotation * offset;
            transform.position = Vector3.SmoothDamp(transform.position,
                desiredPosition, ref velocity, followSmooth);
            transform.rotation = Quaternion.LookRotation(
                target.position + Vector3.up * lookHeight - transform.position,
                Vector3.up);
        }
    }
}
