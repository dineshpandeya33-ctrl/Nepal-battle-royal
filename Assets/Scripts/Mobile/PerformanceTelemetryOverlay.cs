using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

namespace NepalBattleRoyale.Mobile
{
    /// <summary>
    /// Lightweight on-screen performance telemetry for development builds.
    /// Uses Unity's managed UI Text component and samples at a configurable interval.
    /// This is a diagnostic aid, not a substitute for the Unity Profiler.
    /// </summary>
    public class PerformanceTelemetryOverlay : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Text telemetryText;
        [SerializeField] private bool startVisible = true;

        [Header("Sampling")]
        [SerializeField, Min(0.1f)] private float sampleInterval = 0.5f;
        [SerializeField, Range(0.01f, 1f)] private float smoothing = 0.25f;

        private float elapsedSampleTime;
        private float elapsedSessionTime;
        private int framesInSample;
        private float smoothedFps;
        private float smoothedFrameTimeMs;
        private bool hasSample;

        public float FramesPerSecond => smoothedFps;
        public float FrameTimeMilliseconds => smoothedFrameTimeMs;

        private void Awake()
        {
            if (telemetryText == null)
            {
                Debug.LogWarning(
                    "PerformanceTelemetryOverlay needs a UI Text reference.",
                    this);
                enabled = false;
                return;
            }

            telemetryText.enabled = startVisible;
            RefreshDisplay();
        }

        private void LateUpdate()
        {
            float delta = Time.unscaledDeltaTime;
            if (delta <= 0f)
                return;

            elapsedSessionTime += delta;
            elapsedSampleTime += delta;
            framesInSample++;

            if (elapsedSampleTime < Mathf.Max(0.1f, sampleInterval))
                return;

            float fps = framesInSample / elapsedSampleTime;
            float frameTimeMs = (elapsedSampleTime / framesInSample) * 1000f;

            if (!hasSample)
            {
                smoothedFps = fps;
                smoothedFrameTimeMs = frameTimeMs;
                hasSample = true;
            }
            else
            {
                smoothedFps = Mathf.Lerp(smoothedFps, fps, smoothing);
                smoothedFrameTimeMs = Mathf.Lerp(
                    smoothedFrameTimeMs, frameTimeMs, smoothing);
            }

            elapsedSampleTime = 0f;
            framesInSample = 0;
            RefreshDisplay();
        }

        /// <summary>Can be connected to a UI Button to show or hide the overlay.</summary>
        public void ToggleOverlay()
        {
            SetOverlayVisible(!IsOverlayVisible);
        }

        public void SetOverlayVisible(bool visible)
        {
            if (telemetryText != null)
                telemetryText.enabled = visible;
        }

        public bool IsOverlayVisible =>
            telemetryText != null && telemetryText.enabled;

        /// <summary>Refreshes the text using the latest sampled values.</summary>
        public void RefreshNow()
        {
            RefreshDisplay();
        }

        private void RefreshDisplay()
        {
            if (telemetryText == null)
                return;

            long allocatedMb = Profiler.GetTotalAllocatedMemoryLong() / (1024L * 1024L);
            long reservedMb = Profiler.GetTotalReservedMemoryLong() / (1024L * 1024L);

            telemetryText.text =
                "PERFORMANCE" +
                "\nFPS: " + (hasSample ? smoothedFps.ToString("0") : "--") +
                "\nFrame: " + (hasSample ? smoothedFrameTimeMs.ToString("0.0") : "--") + " ms" +
                "\nUnity memory: " + allocatedMb + " / " + reservedMb + " MB" +
                "\nQuality: " + QualitySettings.names[QualitySettings.GetQualityLevel()] +
                "\nTarget: " + Application.targetFrameRate + " FPS";
        }

        private void OnValidate()
        {
            sampleInterval = Mathf.Max(0.1f, sampleInterval);
            smoothing = Mathf.Clamp(smoothing, 0.01f, 1f);
        }
    }
}
