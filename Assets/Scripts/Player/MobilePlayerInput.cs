using UnityEngine;

namespace NepalBattleRoyale.Player
{
    /// <summary>Connects the on-screen joystick and action buttons to the motor.</summary>
    public class MobilePlayerInput : MonoBehaviour
    {
        [SerializeField] private ThirdPersonMotor motor;

        private void Awake()
        {
            if (motor == null) motor = GetComponent<ThirdPersonMotor>();
        }

        public void OnMove(Vector2 value)
        {
            if (motor != null) motor.SetMoveInput(value);
        }

        public void OnSprintPressed()
        {
            if (motor != null) motor.SetSprint(true);
        }

        public void OnSprintReleased()
        {
            if (motor != null) motor.SetSprint(false);
        }

        public void OnJumpPressed()
        {
            if (motor != null) motor.QueueJump();
        }
    }
}
