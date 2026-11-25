using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using NepalBattleRoyale.Mobile;
using NepalBattleRoyale.Player;
using NepalBattleRoyale.UI;
using NepalBattleRoyale.World;

namespace NepalBattleRoyale.Diagnostics
{
    /// <summary>
    /// Inspector-wired scene checklist. Reports missing prototype references
    /// before a Development Build; it does not create or repair scene objects.
    /// </summary>
    public class SceneReadinessValidator : MonoBehaviour
    {
        [Header("World")]
        [SerializeField] private NepalMapGenerator mapGenerator;

        [Header("Player and camera")]
        [SerializeField] private Transform playerRoot;
        [SerializeField] private ThirdPersonMotor playerMotor;
        [SerializeField] private MobilePlayerInput mobilePlayerInput;
        [SerializeField] private Camera gameplayCamera;
        [SerializeField] private ThirdPersonCameraRig cameraRig;

        [Header("UI")]
        [SerializeField] private Canvas mainCanvas;
        [SerializeField] private EventSystem eventSystem;
        [SerializeField] private MainMenuController mainMenu;

        [Header("Mobile diagnostics")]
        [SerializeField] private MobilePerformanceProfile performanceProfile;
        [SerializeField] private SafeAreaFitter safeAreaFitter;

        [Header("Validation")]
        [SerializeField] private bool validateOnStart = true;
        [SerializeField] private bool logSuccessfulChecks = true;

        private void Start()
        {
            if (validateOnStart)
                ValidateNow();
        }

        /// <summary>Connect this method to a development-only Validate button.</summary>
        [ContextMenu("Validate Scene Readiness")]
        public void ValidateNow()
        {
            int passed = 0;
            int failed = 0;

            Check("Nepal map generator", mapGenerator != null, ref passed, ref failed);
            Check("Player root", playerRoot != null, ref passed, ref failed);
            Check("Third-person motor", playerMotor != null, ref passed, ref failed);
            Check("Mobile player input", mobilePlayerInput != null, ref passed, ref failed);
            Check("Gameplay camera", gameplayCamera != null, ref passed, ref failed);
            Check("Third-person camera rig", cameraRig != null, ref passed, ref failed);
            Check("Main Canvas", mainCanvas != null, ref passed, ref failed);
            Check("EventSystem", eventSystem != null, ref passed, ref failed);
            Check("Main menu controller", mainMenu != null, ref passed, ref failed);
            Check("Mobile performance profile", performanceProfile != null, ref passed, ref failed);
            Check("Safe area fitter", safeAreaFitter != null, ref passed, ref failed);

            if (playerRoot != null && playerMotor != null &&
                playerMotor.transform.root != playerRoot.root)
            {
                failed++;
                Debug.LogWarning(
                    "[Scene Readiness] Player root and motor do not belong to the same hierarchy.",
                    this);
            }

            if (gameplayCamera != null && cameraRig != null &&
                cameraRig.transform != gameplayCamera.transform)
            {
                failed++;
                Debug.LogWarning(
                    "[Scene Readiness] Camera rig is not on the assigned gameplay camera.",
                    this);
            }

            string summary = "Scene readiness checks: " + passed +
                             " passed, " + failed + " failed.";
            if (failed == 0)
                Debug.Log("[Scene Readiness] " + summary, this);
            else
                Debug.LogWarning("[Scene Readiness] " + summary +
                    " Assign missing references in the Inspector, then validate again.", this);
        }

        private void Check(string label, bool condition, ref int passed, ref int failed)
        {
            if (condition)
            {
                passed++;
                if (logSuccessfulChecks)
                    Debug.Log("[Scene Readiness] OK: " + label, this);
            }
            else
            {
                failed++;
                Debug.LogWarning("[Scene Readiness] Missing: " + label, this);
            }
        }
    }
}
