using System;
using UnityEngine;

namespace NepalBattleRoyale.Mobile
{
    /// <summary>
    /// Distance-culls renderers in authored world groups.
    /// It never disables colliders, scripts, navigation or whole GameObjects.
    /// </summary>
    public class DistanceRendererCulling : MonoBehaviour
    {
        [Serializable]
        private class VisibilityGroup
        {
            public string label = "World group";
            public Transform root;
            [Min(1f)] public float visibleDistance = 180f;

            [NonSerialized] public Renderer[] renderers;
            [NonSerialized] public bool[] originallyEnabled;
            [NonSerialized] public bool isVisible = true;
            [NonSerialized] public bool initialized;
        }

        [Header("Camera")]
        [SerializeField] private Camera targetCamera;
        [SerializeField] private bool findMainCameraIfMissing = true;

        [Header("Update budget")]
        [SerializeField, Min(0.05f)] private float checkInterval = 0.25f;
        [SerializeField, Min(0f)] private float hysteresisDistance = 12f;

        [Header("World groups")]
        [SerializeField] private VisibilityGroup[] groups;

        private float nextCheckTime;

        private void Awake()
        {
            CacheGroups();
            ResolveCamera();
        }

        private void OnEnable()
        {
            nextCheckTime = 0f;
        }

        private void Update()
        {
            if (Time.unscaledTime < nextCheckTime)
                return;

            nextCheckTime = Time.unscaledTime + Mathf.Max(0.05f, checkInterval);
            if (targetCamera == null && findMainCameraIfMissing)
                ResolveCamera();

            if (targetCamera == null)
                return;

            Vector3 cameraPosition = targetCamera.transform.position;
            if (groups == null) return;

            for (int i = 0; i < groups.Length; i++)
            {
                VisibilityGroup group = groups[i];
                if (group == null || group.root == null || !group.initialized)
                    continue;

                float distanceSqr = (group.root.position - cameraPosition).sqrMagnitude;
                float limit = Mathf.Max(1f, group.visibleDistance);
                float threshold = group.isVisible
                    ? limit
                    : Mathf.Max(0f, limit - hysteresisDistance);

                bool shouldBeVisible = distanceSqr <= threshold * threshold;
                if (shouldBeVisible != group.isVisible)
                    SetGroupVisible(group, shouldBeVisible);
            }
        }

        private void CacheGroups()
        {
            if (groups == null) return;

            for (int i = 0; i < groups.Length; i++)
            {
                VisibilityGroup group = groups[i];
                if (group == null || group.root == null) continue;

                group.renderers = group.root.GetComponentsInChildren<Renderer>(true);
                group.originallyEnabled = new bool[group.renderers.Length];

                for (int r = 0; r < group.renderers.Length; r++)
                    group.originallyEnabled[r] = group.renderers[r] != null &&
                                                 group.renderers[r].enabled;

                group.isVisible = true;
                group.initialized = true;
            }
        }

        private void SetGroupVisible(VisibilityGroup group, bool visible)
        {
            group.isVisible = visible;

            for (int i = 0; i < group.renderers.Length; i++)
            {
                Renderer renderer = group.renderers[i];
                if (renderer == null) continue;

                renderer.enabled = visible && group.originallyEnabled[i];
            }
        }

        private void ResolveCamera()
        {
            if (targetCamera == null && findMainCameraIfMissing)
                targetCamera = Camera.main;
        }

        /// <summary>Rebuild cached renderer lists after changing group contents.</summary>
        public void RefreshGroups()
        {
            CacheGroups();
            nextCheckTime = 0f;
        }

        private void OnDisable()
        {
            if (groups == null) return;

            for (int i = 0; i < groups.Length; i++)
            {
                VisibilityGroup group = groups[i];
                if (group == null || !group.initialized) continue;

                for (int r = 0; r < group.renderers.Length; r++)
                {
                    Renderer renderer = group.renderers[r];
                    if (renderer != null)
                        renderer.enabled = group.originallyEnabled[r];
                }

                group.isVisible = true;
            }
        }
    }
}
