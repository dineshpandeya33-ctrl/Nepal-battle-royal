using UnityEngine;

namespace NepalBattleRoyale.Online
{
    /// <summary>
    /// Connects a provider-specific MonoBehaviour implementing
    /// INetworkProviderAdapter to the existing session and quality components.
    /// The provider must dispatch its events on Unity's main thread.
    /// </summary>
    public class NetworkProviderDiagnosticsBridge : MonoBehaviour
    {
        [Header("Provider adapter")]
        [SerializeField] private MonoBehaviour providerBehaviour;

        [Header("Diagnostics receivers")]
        [SerializeField] private NetworkSessionStateController session;
        [SerializeField] private NetworkQualityMonitor quality;

        private INetworkProviderAdapter provider;

        private void Awake()
        {
            provider = providerBehaviour as INetworkProviderAdapter;
            if (providerBehaviour != null && provider == null)
            {
                Debug.LogError(
                    "Assigned provider component does not implement INetworkProviderAdapter.",
                    this);
            }
        }

        private void OnEnable()
        {
            if (provider == null) return;

            provider.SessionStateChanged += HandleSessionState;
            provider.RoundTripTimeMeasured += HandleRoundTripTime;
            provider.MeasurementFailed += HandleMeasurementFailed;
        }

        private void OnDisable()
        {
            if (provider == null) return;

            provider.SessionStateChanged -= HandleSessionState;
            provider.RoundTripTimeMeasured -= HandleRoundTripTime;
            provider.MeasurementFailed -= HandleMeasurementFailed;
        }

        /// <summary>Wire this to a Connect button in the test menu.</summary>
        public void RequestStartClient()
        {
            if (provider == null)
            {
                Debug.LogWarning("No valid network provider adapter is assigned.", this);
                return;
            }

            provider.StartClient();
        }

        /// <summary>Wire this to a Disconnect button in the test menu.</summary>
        public void RequestStopClient()
        {
            if (provider == null)
            {
                Debug.LogWarning("No valid network provider adapter is assigned.", this);
                return;
            }

            provider.StopClient();
        }

        private void HandleSessionState(NetworkSessionState state)
        {
            if (session != null)
                session.TryTransition(state);
        }

        private void HandleRoundTripTime(float milliseconds)
        {
            if (quality != null)
                quality.RecordRoundTripMilliseconds(milliseconds);
        }

        private void HandleMeasurementFailed()
        {
            if (quality != null)
                quality.RecordFailure();
        }
    }
}
