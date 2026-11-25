using System;
using UnityEngine;
using UnityEngine.UI;

namespace NepalBattleRoyale.Online
{
    public enum NetworkSessionState
    {
        Disconnected,
        Connecting,
        Connected,
        Reconnecting,
        Disconnecting,
        Failed
    }

    /// <summary>
    /// Provider-neutral session lifecycle state. A networking SDK adapter must
    /// report actual connection events; this component does not connect by itself.
    /// </summary>
    public class NetworkSessionStateController : MonoBehaviour
    {
        [SerializeField] private Text statusLabel;
        [SerializeField] private NetworkSessionState initialState =
            NetworkSessionState.Disconnected;

        public NetworkSessionState CurrentState { get; private set; }
        public event Action<NetworkSessionState> StateChanged;

        private void Awake()
        {
            CurrentState = initialState;
            RefreshLabel();
        }

        public bool ReportConnecting() => TryTransition(NetworkSessionState.Connecting);
        public bool ReportConnected() => TryTransition(NetworkSessionState.Connected);
        public bool ReportReconnecting() => TryTransition(NetworkSessionState.Reconnecting);
        public bool ReportDisconnecting() => TryTransition(NetworkSessionState.Disconnecting);
        public bool ReportDisconnected() => TryTransition(NetworkSessionState.Disconnected);
        public bool ReportFailed() => TryTransition(NetworkSessionState.Failed);

        public bool TryTransition(NetworkSessionState next)
        {
            if (next == CurrentState) return true;
            if (!IsAllowed(CurrentState, next))
            {
                Debug.LogWarning("Rejected network state transition: " +
                    CurrentState + " -> " + next, this);
                return false;
            }

            CurrentState = next;
            RefreshLabel();
            StateChanged?.Invoke(CurrentState);
            return true;
        }

        private static bool IsAllowed(NetworkSessionState current, NetworkSessionState next)
        {
            switch (current)
            {
                case NetworkSessionState.Disconnected:
                    return next == NetworkSessionState.Connecting;
                case NetworkSessionState.Connecting:
                    return next == NetworkSessionState.Connected ||
                           next == NetworkSessionState.Failed ||
                           next == NetworkSessionState.Disconnected;
                case NetworkSessionState.Connected:
                    return next == NetworkSessionState.Reconnecting ||
                           next == NetworkSessionState.Disconnecting ||
                           next == NetworkSessionState.Failed;
                case NetworkSessionState.Reconnecting:
                    return next == NetworkSessionState.Connected ||
                           next == NetworkSessionState.Disconnecting ||
                           next == NetworkSessionState.Disconnected ||
                           next == NetworkSessionState.Failed;
                case NetworkSessionState.Disconnecting:
                    return next == NetworkSessionState.Disconnected ||
                           next == NetworkSessionState.Failed;
                case NetworkSessionState.Failed:
                    return next == NetworkSessionState.Disconnected ||
                           next == NetworkSessionState.Connecting;
                default:
                    return false;
            }
        }

        private void RefreshLabel()
        {
            if (statusLabel != null)
                statusLabel.text = "Network: " + CurrentState;
        }
    }
}
