using System.Collections.Generic;
using UnityEngine;

namespace NepalBattleRoyale.Online
{
    /// <summary>
    /// Stores recent round-trip-time samples supplied by a future networking adapter.
    /// This component does not create connections or send network traffic.
    /// </summary>
    public class NetworkQualityMonitor : MonoBehaviour
    {
        [SerializeField, Min(4)] private int sampleCapacity = 32;
        [SerializeField] private bool showDebugOverlay = true;

        private readonly Queue<float> samples = new Queue<float>();
        private int failedSamples;
        private int totalSamples;
        private float averageMilliseconds;
        private float minimumMilliseconds;
        private float maximumMilliseconds;
        private float jitterMilliseconds;

        public int TotalSamples => totalSamples;
        public int FailedSamples => failedSamples;
        public float AverageMilliseconds => averageMilliseconds;
        public float MinimumMilliseconds => minimumMilliseconds;
        public float MaximumMilliseconds => maximumMilliseconds;
        public float JitterMilliseconds => jitterMilliseconds;
        public float FailurePercent => totalSamples == 0
            ? 0f : (failedSamples * 100f) / totalSamples;

        /// <summary>Call with a measured round-trip time from the networking SDK.</summary>
        public void RecordRoundTripMilliseconds(float milliseconds)
        {
            if (float.IsNaN(milliseconds) || float.IsInfinity(milliseconds) || milliseconds < 0f)
            {
                RecordFailure();
                return;
            }

            totalSamples++;
            samples.Enqueue(milliseconds);
            int capacity = Mathf.Max(4, sampleCapacity);
            while (samples.Count > capacity)
                samples.Dequeue();

            Recalculate();
        }

        /// <summary>Call when a ping/request times out or cannot be completed.</summary>
        public void RecordFailure()
        {
            totalSamples++;
            failedSamples++;
        }

        public void ResetStatistics()
        {
            samples.Clear();
            totalSamples = 0;
            failedSamples = 0;
            averageMilliseconds = 0f;
            minimumMilliseconds = 0f;
            maximumMilliseconds = 0f;
            jitterMilliseconds = 0f;
        }

        private void Recalculate()
        {
            if (samples.Count == 0) return;

            float sum = 0f;
            float min = float.MaxValue;
            float max = 0f;
            float previous = -1f;
            float variationSum = 0f;
            int variationCount = 0;

            foreach (float value in samples)
            {
                sum += value;
                min = Mathf.Min(min, value);
                max = Mathf.Max(max, value);

                if (previous >= 0f)
                {
                    variationSum += Mathf.Abs(value - previous);
                    variationCount++;
                }
                previous = value;
            }

            averageMilliseconds = sum / samples.Count;
            minimumMilliseconds = min;
            maximumMilliseconds = max;
            jitterMilliseconds = variationCount == 0
                ? 0f : variationSum / variationCount;
        }

        private void OnGUI()
        {
            if (!showDebugOverlay) return;

            GUILayout.BeginArea(new Rect(12f, 12f, 260f, 150f), GUI.skin.box);
            GUILayout.Label("NETWORK QUALITY (DEBUG)");
            GUILayout.Label("RTT average: " + averageMilliseconds.ToString("F1") + " ms");
            GUILayout.Label("RTT min / max: " + minimumMilliseconds.ToString("F1") +
                            " / " + maximumMilliseconds.ToString("F1") + " ms");
            GUILayout.Label("Jitter: " + jitterMilliseconds.ToString("F1") + " ms");
            GUILayout.Label("Failed samples: " + failedSamples + " (" +
                            FailurePercent.ToString("F1") + "%)");
            GUILayout.EndArea();
        }
    }
}
