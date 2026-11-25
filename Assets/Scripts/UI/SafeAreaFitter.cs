using UnityEngine;

namespace NepalBattleRoyale.UI
{
    /// <summary>
    /// Fits a UI RectTransform inside the device's safe area.
    /// Place this on a full-screen panel under a Canvas.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class SafeAreaFitter : MonoBehaviour
    {
        [SerializeField] private bool refreshContinuously = true;

        private RectTransform target;
        private Rect lastSafeArea;
        private Vector2Int lastScreenSize;
        private ScreenOrientation lastOrientation;

        private void Awake()
        {
            target = GetComponent<RectTransform>();
            ApplySafeArea();
        }

        private void Update()
        {
            if (!refreshContinuously)
                return;

            if (lastSafeArea != Screen.safeArea ||
                lastScreenSize.x != Screen.width ||
                lastScreenSize.y != Screen.height ||
                lastOrientation != Screen.orientation)
            {
                ApplySafeArea();
            }
        }

        /// <summary>May be called after changing panels or screen orientation.</summary>
        public void Refresh()
        {
            ApplySafeArea();
        }

        private void ApplySafeArea()
        {
            if (target == null)
                target = GetComponent<RectTransform>();

            Rect safe = Screen.safeArea;
            Vector2 anchorMin = safe.position;
            Vector2 anchorMax = safe.position + safe.size;

            anchorMin.x /= Mathf.Max(1, Screen.width);
            anchorMin.y /= Mathf.Max(1, Screen.height);
            anchorMax.x /= Mathf.Max(1, Screen.width);
            anchorMax.y /= Mathf.Max(1, Screen.height);

            target.anchorMin = anchorMin;
            target.anchorMax = anchorMax;
            target.offsetMin = Vector2.zero;
            target.offsetMax = Vector2.zero;

            lastSafeArea = safe;
            lastScreenSize = new Vector2Int(Screen.width, Screen.height);
            lastOrientation = Screen.orientation;
        }
    }
}
