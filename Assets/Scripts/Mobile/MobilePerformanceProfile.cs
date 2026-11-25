using UnityEngine;

namespace NepalBattleRoyale.Mobile
{
    public enum MobileGraphicsPreset
    {
        Low,
        Balanced,
        High
    }

    /// <summary>
    /// Applies conservative, scalable Unity quality settings for Android.
    /// Presets are starting points only and must be profiled on real devices.
    /// </summary>
    public class MobilePerformanceProfile : MonoBehaviour
    {
        [Header("Startup")]
        [SerializeField] private MobileGraphicsPreset startupPreset = MobileGraphicsPreset.Balanced;
        [SerializeField, Range(30, 60)] private int targetFrameRate = 30;
        [SerializeField] private bool applyOnAwake = true;

        public MobileGraphicsPreset CurrentPreset { get; private set; }

        private void Awake()
        {
            if (applyOnAwake)
                ApplyPreset(startupPreset);
        }

        public void ApplyLow() => ApplyPreset(MobileGraphicsPreset.Low);
        public void ApplyBalanced() => ApplyPreset(MobileGraphicsPreset.Balanced);
        public void ApplyHigh() => ApplyPreset(MobileGraphicsPreset.High);

        public void ApplyPreset(MobileGraphicsPreset preset)
        {
            CurrentPreset = preset;
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = Mathf.Clamp(targetFrameRate, 30, 60);

            switch (preset)
            {
                case MobileGraphicsPreset.Low:
                    ApplySettings(false, 35f, 0.65f, 2, 0, 2,
                        AnisotropicFiltering.Disable);
                    break;

                case MobileGraphicsPreset.High:
                    ApplySettings(true, 75f, 1.1f, 4, 2, 0,
                        AnisotropicFiltering.Enable);
                    break;

                default:
                    ApplySettings(true, 50f, 0.8f, 2, 0, 1,
                        AnisotropicFiltering.Enable);
                    break;
            }

            Debug.Log("Applied mobile graphics preset: " + preset +
                      " at target " + Application.targetFrameRate + " FPS. " +
                      "Profile on the target device before release.", this);
        }

        private static void ApplySettings(
            bool useShadows,
            float shadowDistance,
            float lodBias,
            int pixelLights,
            int antiAliasing,
            int textureLimit,
            AnisotropicFiltering anisotropic)
        {
            QualitySettings.shadows = useShadows
                ? ShadowQuality.Hard
                : ShadowQuality.Disable;
            QualitySettings.shadowDistance = shadowDistance;
            QualitySettings.shadowCascades = 0;
            QualitySettings.pixelLightCount = pixelLights;
            QualitySettings.lodBias = lodBias;
            QualitySettings.maximumLODLevel = 0;
            QualitySettings.antiAliasing = antiAliasing;
            QualitySettings.masterTextureLimit = textureLimit;
            QualitySettings.anisotropicFiltering = anisotropic;
        }
    }
}
