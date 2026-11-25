using UnityEngine;
using UnityEngine.EventSystems;

namespace NepalBattleRoyale.Player
{
    /// <summary>Pointer-drag surface that rotates the third-person camera.</summary>
    public class TouchCameraLook : MonoBehaviour, IDragHandler
    {
        [SerializeField] private ThirdPersonCameraRig cameraRig;
        [SerializeField, Min(0.01f)] private float sensitivity = 0.12f;
        [SerializeField] private bool invertVertical;

        public void OnDrag(PointerEventData eventData)
        {
            if (cameraRig == null) return;
            float vertical = invertVertical ? -eventData.delta.y : eventData.delta.y;
            cameraRig.AddLookInput(new Vector2(
                eventData.delta.x * sensitivity,
                vertical * sensitivity));
        }
    }
}
