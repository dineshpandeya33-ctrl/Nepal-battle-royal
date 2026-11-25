using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NepalBattleRoyale.UI
{
    /// <summary>Inspector-wired menu panel controller for the first UI prototype.</summary>
    public class MainMenuController : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject mainPanel;
        [SerializeField] private GameObject modePanel;
        [SerializeField] private GameObject settingsPanel;

        [Header("Optional labels")]
        [SerializeField] private Text selectedModeLabel;
        [SerializeField] private Text statusLabel;

        [Header("Scene")]
        [SerializeField] private string prototypeSceneName = "";

        public GameMode SelectedMode { get; private set; } = GameMode.Solo;

        private void Start()
        {
            ShowMain();
            UpdateModeLabel();
        }

        public void OnPlayClicked()
        {
            SetPanels(false, true, false);
            SetStatus("Choose a game mode");
        }

        public void OnSettingsClicked()
        {
            SetPanels(false, false, true);
            SetStatus("Settings");
        }

        public void OnBackClicked() => ShowMain();

        public void SelectSolo() => SelectMode(GameMode.Solo);
        public void SelectDuo() => SelectMode(GameMode.Duo);
        public void SelectSquad() => SelectMode(GameMode.Squad);
        public void SelectPractice() => SelectMode(GameMode.Practice);

        public void SelectMode(GameMode mode)
        {
            SelectedMode = mode;
            UpdateModeLabel();
            SetStatus(mode == GameMode.Practice
                ? "Practice mode selected"
                : mode + " mode selected — online matchmaking is not connected yet");
        }

        public void OnStartClicked()
        {
            if (string.IsNullOrWhiteSpace(prototypeSceneName))
            {
                SetStatus("Prototype scene is not assigned yet. Create and add it to Build Settings.");
                return;
            }

            if (!Application.CanStreamedLevelBeLoaded(prototypeSceneName))
            {
                SetStatus("The configured scene is not in Build Settings.");
                return;
            }

            PlayerPrefs.SetInt("SelectedGameMode", (int)SelectedMode);
            SceneManager.LoadScene(prototypeSceneName);
        }

        private void ShowMain()
        {
            SetPanels(true, false, false);
            SetStatus("Welcome to Nepal Battle Royale");
        }

        private void SetPanels(bool showMain, bool showModes, bool showSettings)
        {
            if (mainPanel != null) mainPanel.SetActive(showMain);
            if (modePanel != null) modePanel.SetActive(showModes);
            if (settingsPanel != null) settingsPanel.SetActive(showSettings);
        }

        private void UpdateModeLabel()
        {
            if (selectedModeLabel != null)
                selectedModeLabel.text = "Mode: " + SelectedMode;
        }

        private void SetStatus(string message)
        {
            if (statusLabel != null) statusLabel.text = message;
        }
    }
}
