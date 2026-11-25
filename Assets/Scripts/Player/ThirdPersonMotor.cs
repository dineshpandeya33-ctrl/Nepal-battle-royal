using UnityEngine;

namespace NepalBattleRoyale.Player
{
    /// <summary>
    /// Starter third-person locomotion. Requires a CharacterController.
    /// Input is supplied by MobilePlayerInput so the same controller can later
    /// support keyboard/gamepad without changing movement logic.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class ThirdPersonMotor : MonoBehaviour
    {
        [SerializeField] private Transform cameraTransform;
        [SerializeField, Min(0.1f)] private float moveSpeed = 4.5f;
        [SerializeField, Min(0.1f)] private float sprintSpeed = 6.5f;
        [SerializeField, Min(0.1f)] private float rotationSpeed = 12f;
        [SerializeField] private float gravity = -22f;
        [SerializeField] private float jumpHeight = 1.15f;

        private CharacterController controller;
        private Vector2 moveInput;
        private bool sprintHeld;
        private bool jumpQueued;
        private float verticalVelocity;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            if (cameraTransform == null && Camera.main != null)
                cameraTransform = Camera.main.transform;
        }

        public void SetMoveInput(Vector2 value) => moveInput = Vector2.ClampMagnitude(value, 1f);
        public void SetSprint(bool held) => sprintHeld = held;
        public void QueueJump() => jumpQueued = true;

        private void Update()
        {
            if (cameraTransform == null && Camera.main != null)
                cameraTransform = Camera.main.transform;

            Vector3 forward = cameraTransform != null ? cameraTransform.forward : transform.forward;
            Vector3 right = cameraTransform != null ? cameraTransform.right : transform.right;
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            Vector3 direction = forward * moveInput.y + right * moveInput.x;
            float speed = sprintHeld ? sprintSpeed : moveSpeed;

            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
                    rotationSpeed * Time.deltaTime);
            }

            if (controller.isGrounded && verticalVelocity < 0f)
                verticalVelocity = -2f;

            if (jumpQueued && controller.isGrounded)
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpQueued = false;

            verticalVelocity += gravity * Time.deltaTime;
            Vector3 velocity = direction.normalized * (direction.magnitude * speed);
            velocity.y = verticalVelocity;
            controller.Move(velocity * Time.deltaTime);
        }
    }
}
