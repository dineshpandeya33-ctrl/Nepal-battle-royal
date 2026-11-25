using UnityEngine;
using UnityEngine.UI;

namespace NepalBattleRoyale.Online
{
    /// <summary>
    /// Presents session and measured network-quality data in a Unity UI.
    /// It does not create connections or generate ping traffic.
    /// </summary>
    public class NetworkDiagnosticsPanel : MonoBehaviour
    {
        [Header("Data sources")]
        [SerializeField] private NetworkSessionStateController session;
        [SerializeField] private NetworkQualityMonitor quality;

        [Header("Optional UI labels")]
        [SerializeField] private Text sessionLabel;
        [SerializeField] private Text averageRttLabel;
        [SerializeField] private Text jitterLabel;
        [SerializeField] private Text failedSamplesLabel;

        [Header("Refresh")]
        [SerializeField, Min(0.05f)] private float refreshInterval = 0.25f;

        private float nextRefreshTime;

        private void OnEnable()
        {
            if (session != null)
                session.StateChanged += HandleSessionStateChanged;

            RefreshDisplay();
        }

        private void OnDisable()
        {
            if (session != null)
                session.StateChanged -= HandleSessionStateChanged;
        }

        private void Update()
        {
            if (Time.unscaledTime < nextRefreshTime)
                return;

            nextRefreshTime = Time.unscaledTime + Mathf.Max(0.05f, refreshInterval);
            RefreshDisplay();
        }

        private void HandleSessionStateChanged(NetworkSessionState state)
        {
            if (sessionLabel != null)
                sessionLabel.text = "Session: " + state;
        }

        private void RefreshDisplay()
        {
            if (sessionLabel != null)
            {
                string state = session != null
                    ? session.CurrentState.ToString()
                    : "Not assigned";
                sessionLabel.text = "Session: " + state;
            }

            if (quality == null)
            {
                SetText(averageRttLabel, "Average RTT: --");
                SetText(jitterLabel, "Jitter: --");
                SetText(failedSamplesLabel, "Failed samples: --");
                return;
            }

            SetText(averageRttLabel,
                quality.TotalSamples == quality.FailedSamples
                    ? "Average RTT: --"
                    : "Average RTT: " + quality.AverageMilliseconds.ToString("F1") + " ms");

            SetText(jitterLabel,
                "Jitter: " + quality.JitterMilliseconds.ToString("F1") + " ms");

            SetText(failedSamplesLabel,
                "Failed samples: " + quality.FailedSamples + "/" +
                quality.TotalSamples + " (" + quality.FailurePercent.ToString("F1") + "%)");
        }

        private static void SetText(Text label, string value)
        {
            if (label != null)
                label.text = value;
        }
    }
}
