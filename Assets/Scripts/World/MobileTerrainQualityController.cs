using UnityEngine;
using NepalBattleRoyale.Mobile;

namespace NepalBattleRoyale.World
{
    /// <summary>
    /// Applies mobile-friendly draw distances to authored Terrain objects.
    /// This controls terrain details and trees; it does not delete scene assets.
    /// </summary>
    public sealed class MobileTerrainQualityController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Terrain[] terrains;
        [SerializeField] private MobilePerformanceProfile performanceProfile;
        [SerializeField] private bool applyOnAwake = true;

        [Header("Low preset")]
        [SerializeField, Min(0f)] private float lowTreeDistance = 350f;
        [SerializeField, Min(0f)] private float lowDetailDistance = 45f;
        [SerializeField, Range(0f, 1f)] private float lowDetailDensity = 0.35f;
        [SerializeField, Min(0f)] private float lowBillboardStart = 180f;
        [SerializeField, Min(1f)] private float lowHeightmapPixelError = 12f;

        [Header("Balanced preset")]
        [SerializeField, Min(0f)] private float balancedTreeDistance = 650f;
        [SerializeField, Min(0f)] private float balancedDetailDistance = 70f;
        [SerializeField, Range(0f, 1f)] private float balancedDetailDensity = 0.6f;
        [SerializeField, Min(0f)] private float balancedBillboardStart = 350f;
        [SerializeField, Min(1f)] private float balancedHeightmapPixelError = 8f;

        [Header("High preset")]
        [SerializeField, Min(0f)] private float highTreeDistance = 1000f;
        [SerializeField, Min(0f)] private float highDetailDistance = 100f;
        [SerializeField, Range(0f, 1f)] private float highDetailDensity = 0.85f;
        [SerializeField, Min(0f)] private float highBillboardStart = 600f;
        [SerializeField, Min(1f)] private float highHeightmapPixelError = 5f;

        private void Awake()
        {
            if (!applyOnAwake) return;

            MobileGraphicsPreset preset = performanceProfile != null
                ? performanceProfile.CurrentPreset
                : MobileGraphicsPreset.Balanced;

            ApplyPreset(preset);
        }

        public void ApplyLow() => ApplyPreset(MobileGraphicsPreset.Low);
        public void ApplyBalanced() => ApplyPreset(MobileGraphicsPreset.Balanced);
        public void ApplyHigh() => ApplyPreset(MobileGraphicsPreset.High);

        public void ApplyPreset(MobileGraphicsPreset preset)
        {
            float treeDistance;
            float detailDistance;
            float detailDensity;
            float billboardStart;
            float pixelError;

            switch (preset)
            {
                case MobileGraphicsPreset.Low:
                    treeDistance = lowTreeDistance;
                    detailDistance = lowDetailDistance;
                    detailDensity = lowDetailDensity;
                    billboardStart = lowBillboardStart;
                    pixelError = lowHeightmapPixelError;
                    break;
                case MobileGraphicsPreset.High:
                    treeDistance = highTreeDistance;
                    detailDistance = highDetailDistance;
                    detailDensity = highDetailDensity;
                    billboardStart = highBillboardStart;
                    pixelError = highHeightmapPixelError;
                    break;
                default:
                    treeDistance = balancedTreeDistance;
                    detailDistance = balancedDetailDistance;
                    detailDensity = balancedDetailDensity;
                    billboardStart = balancedBillboardStart;
                    pixelError = balancedHeightmapPixelError;
                    break;
            }

            if (terrains == null) return;

            for (int i = 0; i < terrains.Length; i++)
            {
                Terrain terrain = terrains[i];
                if (terrain == null) continue;

                terrain.treeDistance = treeDistance;
                terrain.detailObjectDistance = detailDistance;
                terrain.detailObjectDensity = detailDensity;
                terrain.treeBillboardDistance = Mathf.Min(billboardStart, treeDistance);
                terrain.heightmapPixelError = pixelError;
                terrain.basemapDistance = Mathf.Max(500f, treeDistance);
            }

            Debug.Log("Applied terrain rendering preset: " + preset, this);
        }
    }
}
