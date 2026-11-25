using UnityEngine;

namespace NepalBattleRoyale.World
{
    /// <summary>
    /// Optional, lightweight day-cycle controller for the Nepal prototype.
    /// Assign a directional light in the Inspector. Disable the component
    /// to keep the scene's authored lighting unchanged.
    /// </summary>
    public sealed class WorldDayCycle : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Light sun;

        [Header("Cycle")]
        [SerializeField, Min(1f)] private float fullDayLengthMinutes = 12f;
        [SerializeField, Range(0f, 1f)] private float startingTimeOfDay = 0.32f;
        [SerializeField] private bool runCycle = true;

        [Header("Lighting")]
        [SerializeField, Min(0f)] private float maximumSunIntensity = 1.1f;
        [SerializeField, Range(0f, 1f)] private float nightAmbientIntensity = 0.18f;
        [SerializeField, Range(0f, 2f)] private float dayAmbientIntensity = 1f;
        [SerializeField] private Color dayAmbient = new Color(0.62f, 0.68f, 0.78f);
        [SerializeField] private Color nightAmbient = new Color(0.12f, 0.16f, 0.25f);

        [Header("Fog")]
        [SerializeField] private bool controlFog;
        [SerializeField] private Color dayFog = new Color(0.68f, 0.76f, 0.82f);
        [SerializeField] private Color nightFog = new Color(0.10f, 0.14f, 0.22f);
        [SerializeField, Min(0f)] private float fogDensity = 0.0012f;

        public float TimeOfDay => currentTimeOfDay;
        private float currentTimeOfDay;

        private void OnEnable()
        {
            currentTimeOfDay = startingTimeOfDay;
            ApplyLighting();
        }

        private void Update()
        {
            if (runCycle)
            {
                currentTimeOfDay = Mathf.Repeat(
                    currentTimeOfDay + Time.deltaTime / (fullDayLengthMinutes * 60f), 1f);
            }

            ApplyLighting();
        }

        public void SetTimeOfDay(float normalizedTime)
        {
            currentTimeOfDay = Mathf.Repeat(normalizedTime, 1f);
            ApplyLighting();
        }

        public void SetCycleRunning(bool value)
        {
            runCycle = value;
        }

        private void ApplyLighting()
        {
            // Noon is 0.5. Smoothly fade daylight at sunrise and sunset.
            float daylight = Mathf.Clamp01(
                Mathf.Sin((currentTimeOfDay - 0.25f) * Mathf.PI * 2f) * 2.2f + 0.5f);
            daylight = Mathf.SmoothStep(0f, 1f, daylight);

            if (sun != null)
            {
                sun.transform.rotation = Quaternion.Euler(
                    currentTimeOfDay * 360f - 90f, 170f, 0f);
                sun.intensity = maximumSunIntensity * daylight;
                sun.enabled = sun.intensity > 0.01f;
            }

            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = Color.Lerp(nightAmbient, dayAmbient, daylight);
            RenderSettings.ambientIntensity = Mathf.Lerp(
                nightAmbientIntensity, dayAmbientIntensity, daylight);

            if (controlFog)
            {
                RenderSettings.fog = true;
                RenderSettings.fogColor = Color.Lerp(nightFog, dayFog, daylight);
                RenderSettings.fogMode = FogMode.Exponential;
                RenderSettings.fogDensity = fogDensity;
            }
        }
    }
}
