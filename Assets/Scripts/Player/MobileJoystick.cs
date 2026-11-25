using UnityEngine;
using UnityEngine.EventSystems;

namespace NepalBattleRoyale.Player
{
    /// <summary>Virtual joystick that sends normalized movement to MobilePlayerInput.</summary>
    public class MobileJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform background;
        [SerializeField] private RectTransform handle;
        [SerializeField] private MobilePlayerInput playerInput;
        [SerializeField, Range(0.1f, 1f)] private float handleRange = 0.65f;

        private Vector2 input;

        private void Awake()
        {
            if (background == null) background = transform as RectTransform;
            if (handle != null) handle.anchoredPosition = Vector2.zero;
        }

        public void OnPointerDown(PointerEventData eventData) => UpdateInput(eventData);
        public void OnDrag(PointerEventData eventData) => UpdateInput(eventData);
        public void OnPointerUp(PointerEventData eventData) => ResetInput();

        private void UpdateInput(PointerEventData eventData)
        {
            if (background == null) return;

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                background, eventData.position, eventData.pressEventCamera, out Vector2 localPoint))
                return;

            Vector2 halfSize = background.rect.size * 0.5f;
            float radius = Mathf.Min(halfSize.x, halfSize.y);
            if (radius <= 0f) return;

            input = Vector2.ClampMagnitude(localPoint / radius, 1f);
            if (handle != null)
                handle.anchoredPosition = input * radius * handleRange;
            if (playerInput != null)
                playerInput.OnMove(input);
        }

        private void ResetInput()
        {
            input = Vector2.zero;
            if (handle != null) handle.anchoredPosition = Vector2.zero;
            if (playerInput != null) playerInput.OnMove(Vector2.zero);
        }

        private void OnDisable() => ResetInput();
    }
}
