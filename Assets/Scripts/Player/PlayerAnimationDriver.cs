using UnityEngine;

namespace NepalBattleRoyale.Player
{
    /// <summary>
    /// Drives a Mecanim Animator. Expected parameters: Speed (float),
    /// Grounded (bool), and Jump (trigger).
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimationDriver : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private CharacterController characterController;
        [SerializeField, Min(0.01f)] private float dampTime = 0.12f;

        private static readonly int SpeedHash = Animator.StringToHash("Speed");
        private static readonly int GroundedHash = Animator.StringToHash("Grounded");
        private static readonly int JumpHash = Animator.StringToHash("Jump");

        private float movementAmount;
        private bool sprinting;

        private void Awake()
        {
            if (animator == null) animator = GetComponent<Animator>();
            if (characterController == null)
                characterController = GetComponentInParent<CharacterController>();
        }

        public void SetMovement(Vector2 input, bool isSprinting)
        {
            movementAmount = Mathf.Clamp01(input.magnitude);
            sprinting = isSprinting;
        }

        public void TriggerJump()
        {
            if (animator != null) animator.SetTrigger(JumpHash);
        }

        private void Update()
        {
            if (animator == null) return;
            float targetSpeed = movementAmount * (sprinting ? 1f : 0.65f);
            animator.SetFloat(SpeedHash, targetSpeed, dampTime, Time.deltaTime);
            if (characterController != null)
                animator.SetBool(GroundedHash, characterController.isGrounded);
        }
    }
}
