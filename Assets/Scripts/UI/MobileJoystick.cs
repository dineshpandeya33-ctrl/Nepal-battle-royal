using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NepalBattleRoyale.UI
{
    /// <summary>
    /// UI joystick for mobile movement. Put this on the joystick background Image
    /// and assign the handle RectTransform and MobilePlayerInput target.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class MobileJoystick : MonoBehaviour, IPointerDownHandler,
        IPointerUpHandler, IDragHandler
    {
        [SerializeField] private RectTransform handle;
        [SerializeField] private NepalBattleRoyale.Player.MobilePlayerInput playerInput;
        [SerializeField, Range(0.1f, 1f)] private float handleRange = 0.65f;

        private RectTransform rect;
        private Vector2 input;

        private void Awake()
        {
            rect = transform as RectTransform;
            if (handle != null) handle.anchoredPosition = Vector2.zero;
        }

        public void OnPointerDown(PointerEventData eventData) => OnDrag(eventData);

        public void OnDrag(PointerEventData eventData)
        {
            if (rect == null || handle == null) return;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rect, eventData.position, eventData.pressEventCamera, out Vector2 local))
                return;

            Vector2 half = rect.rect.size * 0.5f;
            input = new Vector2(
                half.x > 0f ? local.x / half.x : 0f,
                half.y > 0f ? local.y / half.y : 0f);
            input = Vector2.ClampMagnitude(input, 1f);
            handle.anchoredPosition = input * (Mathf.Min(half.x, half.y) * handleRange);
            if (playerInput != null) playerInput.OnMove(input);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            input = Vector2.zero;
            if (handle != null) handle.anchoredPosition = Vector2.zero;
            if (playerInput != null) playerInput.OnMove(Vector2.zero);
        }
    }
}
