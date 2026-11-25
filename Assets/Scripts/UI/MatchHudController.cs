using UnityEngine;
using UnityEngine.UI;

namespace NepalBattleRoyale.UI
{
    /// <summary>Simple presentation-only match HUD; it does not implement gameplay rules.</summary>
    public class MatchHudController : MonoBehaviour
    {
        [SerializeField] private Text modeLabel;
        [SerializeField] private Text alivePlayersLabel;
        [SerializeField] private Text timerLabel;
        [SerializeField] private int startingPlayerCount = 1;

        private float elapsedSeconds;
        private bool timerRunning;

        private void Start()
        {
            GameMode mode = (GameMode)Mathf.Clamp(
                PlayerPrefs.GetInt("SelectedGameMode", (int)GameMode.Solo),
                (int)GameMode.Solo, (int)GameMode.Practice);

            if (modeLabel != null) modeLabel.text = mode.ToString();
            SetAlivePlayers(startingPlayerCount);
            UpdateTimerLabel();
        }

        private void Update()
        {
            if (!timerRunning) return;
            elapsedSeconds += Time.deltaTime;
            UpdateTimerLabel();
        }

        public void BeginMatchTimer() => timerRunning = true;
        public void PauseMatchTimer() => timerRunning = false;
        public void ResetMatchTimer()
        {
            elapsedSeconds = 0f;
            UpdateTimerLabel();
        }

        public void SetAlivePlayers(int count)
        {
            if (alivePlayersLabel != null)
                alivePlayersLabel.text = "Players: " + Mathf.Max(0, count);
        }

        private void UpdateTimerLabel()
        {
            if (timerLabel == null) return;
            int totalSeconds = Mathf.FloorToInt(elapsedSeconds);
            timerLabel.text = string.Format("{0:00}:{1:00}",
                totalSeconds / 60, totalSeconds % 60);
        }
    }
}
