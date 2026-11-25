using System;

namespace NepalBattleRoyale.Online
{
    /// <summary>
    /// Provider-neutral contract for a future networking SDK adapter.
    /// Implementations translate real SDK callbacks into these events.
    /// This interface contains no transport or connection implementation.
    /// </summary>
    public interface INetworkProviderAdapter
    {
        bool IsClientStarted { get; }

        event Action<NetworkSessionState> SessionStateChanged;
        event Action<float> RoundTripTimeMeasured;
        event Action MeasurementFailed;

        /// <summary>Request that the provider starts a client connection.</summary>
        void StartClient();

        /// <summary>Request that the provider stops the client connection.</summary>
        void StopClient();
    }
}
