using System.Collections.Generic;
using UnityEngine;

namespace NepalBattleRoyale.Mobile
{
    /// <summary>
    /// Configures a Unity LODGroup from renderer sets assigned in the Inspector.
    /// Add this to a prefab root and assign progressively simpler mesh renderers.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(LODGroup))]
    public class AuthoredLodGroupSetup : MonoBehaviour
    {
        [Header("Renderer sets — use simpler meshes at each level")]
        [SerializeField] private Renderer[] lod0Renderers;
        [SerializeField] private Renderer[] lod1Renderers;
        [SerializeField] private Renderer[] lod2Renderers;
        [SerializeField] private Renderer[] cullRenderers;

        [Header("Screen-relative transition heights")]
        [SerializeField, Range(0.05f, 1f)] private float lod0Height = 0.6f;
        [SerializeField, Range(0.02f, 0.9f)] private float lod1Height = 0.25f;
        [SerializeField, Range(0.01f, 0.5f)] private float lod2Height = 0.08f;
        [SerializeField] private bool configureOnAwake = true;

        private LODGroup lodGroup;

        private void Awake()
        {
            if (configureOnAwake)
                Configure();
        }

        [ContextMenu("Configure LOD Group")]
        public void Configure()
        {
            if (!TryGetComponent(out lodGroup))
                lodGroup = gameObject.AddComponent<LODGroup>();

            List<LOD> levels = new List<LOD>(4);
            AddLevel(levels, lod0Height, lod0Renderers);
            AddLevel(levels, lod1Height, lod1Renderers);
            AddLevel(levels, lod2Height, lod2Renderers);
            AddLevel(levels, 0.01f, cullRenderers);

            if (levels.Count == 0)
            {
                Debug.LogWarning("Assign at least one LOD renderer set.", this);
                return;
            }

            levels.Sort((a, b) =>
                b.screenRelativeTransitionHeight.CompareTo(a.screenRelativeTransitionHeight));

            lodGroup.SetLODs(levels.ToArray());
            lodGroup.fadeMode = LODFadeMode.None;
            lodGroup.animateCrossFading = false;
            lodGroup.RecalculateBounds();
        }

        private static void AddLevel(List<LOD> levels, float height, Renderer[] source)
        {
            if (source == null || source.Length == 0)
                return;

            List<Renderer> valid = new List<Renderer>(source.Length);
            for (int i = 0; i < source.Length; i++)
            {
                Renderer renderer = source[i];
                if (renderer != null && !valid.Contains(renderer))
                    valid.Add(renderer);
            }

            if (valid.Count > 0)
                levels.Add(new LOD(Mathf.Clamp(height, 0.01f, 1f), valid.ToArray()));
        }
    }
}
