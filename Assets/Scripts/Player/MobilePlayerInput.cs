using UnityEngine;

namespace NepalBattleRoyale.Player
{
    /// <summary>Connects on-screen controls to movement and optional animation.</summary>
    public class MobilePlayerInput : MonoBehaviour
    {
        [SerializeField] private ThirdPersonMotor motor;
        [SerializeField] private PlayerAnimationDriver animationDriver;

        private Vector2 moveValue;
        private bool sprintHeld;

        private void Awake()
        {
            if (motor == null) motor = GetComponent<ThirdPersonMotor>();
            if (animationDriver == null) animationDriver = GetComponentInChildren<PlayerAnimationDriver>();
        }

        public void OnMove(Vector2 value)
        {
            moveValue = Vector2.ClampMagnitude(value, 1f);
            if (motor != null) motor.SetMoveInput(moveValue);
            UpdateAnimation();
        }

        public void OnSprintPressed()
        {
            sprintHeld = true;
            if (motor != null) motor.SetSprint(true);
            UpdateAnimation();
        }

        public void OnSprintReleased()
        {
            sprintHeld = false;
            if (motor != null) motor.SetSprint(false);
            UpdateAnimation();
        }

        public void OnJumpPressed()
        {
            if (motor != null) motor.QueueJump();
            if (animationDriver != null) animationDriver.TriggerJump();
        }

        private void UpdateAnimation()
        {
            if (animationDriver != null)
                animationDriver.SetMovement(moveValue, sprintHeld);
        }
    }
}
