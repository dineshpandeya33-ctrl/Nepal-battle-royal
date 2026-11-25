#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using NepalBattleRoyale.Mobile;
using NepalBattleRoyale.Player;
using NepalBattleRoyale.UI;
using NepalBattleRoyale.World;

namespace NepalBattleRoyale.EditorTools
{
    /// <summary>
    /// Creates a starter greybox scene with a flat test floor, player,
    /// follow camera, Canvas, joystick and development diagnostics.
    /// Run from Tools > Nepal Battle Royale > Create Starter Scene.
    /// </summary>
    public static class NepalStarterSceneBuilder
    {
        private const string SceneFolder = "Assets/Scenes";
        private const string ScenePath = SceneFolder + "/NepalPrototype.unity";

        [MenuItem("Tools/Nepal Battle Royale/Create Starter Scene")]
        public static void CreateStarterScene()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return;

            EnsureFolder("Assets", "Scenes");

            Scene scene = EditorSceneManager.NewScene(
                NewSceneSetup.EmptyScene, NewSceneMode.Single);

            CreateLighting();
            CreateWorld();
            GameObject player = CreatePlayer();
            Camera camera = CreateCamera(player.transform);
            CreateUI(player.GetComponent<MobilePlayerInput>(), camera.GetComponent<ThirdPersonCameraRig>());

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene, ScenePath);
            AddSceneToBuildSettings(ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog(
                "Nepal Battle Royale",
                "Starter scene created at " + ScenePath +
                "\n\nThis is a greybox setup. Replace the flat floor and capsule with generated terrain and original art assets.",
                "OK");
        }

        private static void CreateLighting()
        {
            GameObject lightObject = new GameObject("Directional Light");
            Light light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1f;
            lightObject.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.55f, 0.58f, 0.62f);
        }

        private static void CreateWorld()
        {
            GameObject world = new GameObject("NepalWorld");
            world.AddComponent<NepalMapGenerator>();

            GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
            floor.name = "Temporary Greybox Floor - Replace With Nepal Terrain";
            floor.transform.position = new Vector3(0f, -0.1f, 0f);
            floor.transform.localScale = new Vector3(400f, 1f, 400f);
        }

        private static GameObject CreatePlayer()
        {
            GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = "Player";
            player.tag = "Player";
            player.transform.position = new Vector3(0f, 1.1f, 0f);

            CapsuleCollider oldCollider = player.GetComponent<CapsuleCollider>();
            if (oldCollider != null)
                Object.DestroyImmediate(oldCollider);

            CharacterController controller = player.AddComponent<CharacterController>();
            controller.height = 2f;
            controller.radius = 0.42f;
            controller.center = new Vector3(0f, 1f, 0f);

            player.AddComponent<ThirdPersonMotor>();
            player.AddComponent<MobilePlayerInput>();
            return player;
        }

        private static Camera CreateCamera(Transform player)
        {
            GameObject cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            cameraObject.transform.position = player.position + new Vector3(0f, 3f, -6f);
            Camera camera = cameraObject.AddComponent<Camera>();
            camera.fieldOfView = 60f;
            cameraObject.AddComponent<AudioListener>();
            ThirdPersonCameraRig rig = cameraObject.AddComponent<ThirdPersonCameraRig>();
            rig.SetTarget(player);
            return camera;
        }

        private static void CreateUI(MobilePlayerInput playerInput, ThirdPersonCameraRig cameraRig)
        {
            GameObject canvasObject = new GameObject(
                "Mobile Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;

            GameObject safeRoot = CreateUIObject("SafeAreaRoot", canvas.transform, typeof(RectTransform));
            RectTransform safeRect = safeRoot.GetComponent<RectTransform>();
            Stretch(safeRect);
            safeRoot.AddComponent<SafeAreaFitter>();

            GameObject joystick = CreateUIObject("Movement Joystick", safeRoot.transform, typeof(Image));
            RectTransform joystickRect = joystick.GetComponent<RectTransform>();
            joystickRect.anchorMin = new Vector2(0f, 0f);
            joystickRect.anchorMax = new Vector2(0f, 0f);
            joystickRect.pivot = new Vector2(0.5f, 0.5f);
            joystickRect.anchoredPosition = new Vector2(170f, 170f);
            joystickRect.sizeDelta = new Vector2(250f, 250f);
            joystick.GetComponent<Image>().color = new Color(0.1f, 0.1f, 0.1f, 0.45f);

            GameObject handle = CreateUIObject("Joystick Handle", joystick.transform, typeof(Image));
            RectTransform handleRect = handle.GetComponent<RectTransform>();
            handleRect.anchorMin = new Vector2(0.5f, 0.5f);
            handleRect.anchorMax = new Vector2(0.5f, 0.5f);
            handleRect.pivot = new Vector2(0.5f, 0.5f);
            handleRect.sizeDelta = new Vector2(95f, 95f);
            handle.GetComponent<Image>().color = new Color(0.85f, 0.9f, 1f, 0.8f);

            MobileJoystick joystickComponent = joystick.AddComponent<MobileJoystick>();
            SerializedObject joystickSerialized = new SerializedObject(joystickComponent);
            joystickSerialized.FindProperty("handle").objectReferenceValue = handleRect;
            joystickSerialized.FindProperty("playerInput").objectReferenceValue = playerInput;
            joystickSerialized.ApplyModifiedPropertiesWithoutUndo();

            GameObject lookArea = CreateUIObject("Camera Look Area", safeRoot.transform, typeof(Image));
            RectTransform lookRect = lookArea.GetComponent<RectTransform>();
            lookRect.anchorMin = new Vector2(0.5f, 0f);
            lookRect.anchorMax = new Vector2(1f, 1f);
            lookRect.offsetMin = Vector2.zero;
            lookRect.offsetMax = Vector2.zero;
            Image lookImage = lookArea.GetComponent<Image>();
            lookImage.color = new Color(1f, 1f, 1f, 0.01f);
            TouchCameraLook touchLook = lookArea.AddComponent<TouchCameraLook>();
            SerializedObject lookSerialized = new SerializedObject(touchLook);
            lookSerialized.FindProperty("cameraRig").objectReferenceValue = cameraRig;
            lookSerialized.ApplyModifiedPropertiesWithoutUndo();

            GameObject telemetry = CreateText("Performance Telemetry", safeRoot.transform,
                "FPS: --\nFrame: -- ms\nUnity memory: --", 24);
            RectTransform telemetryRect = telemetry.GetComponent<RectTransform>();
            telemetryRect.anchorMin = new Vector2(0f, 1f);
            telemetryRect.anchorMax = new Vector2(0f, 1f);
            telemetryRect.pivot = new Vector2(0f, 1f);
            telemetryRect.anchoredPosition = new Vector2(24f, -24f);
            telemetryRect.sizeDelta = new Vector2(500f, 150f);
            PerformanceTelemetryOverlay overlay = canvasObject.AddComponent<PerformanceTelemetryOverlay>();
            SerializedObject overlaySerialized = new SerializedObject(overlay);
            overlaySerialized.FindProperty("telemetryText").objectReferenceValue = telemetry.GetComponent<Text>();
            overlaySerialized.ApplyModifiedPropertiesWithoutUndo();

            if (Object.FindObjectOfType<EventSystem>() == null)
            {
                new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            }
        }

        private static GameObject CreateText(string name, Transform parent, string value, int fontSize)
        {
            GameObject textObject = CreateUIObject(name, parent, typeof(Text));
            Text text = textObject.GetComponent<Text>();
            text.text = value;
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = fontSize;
            text.color = Color.white;
            text.alignment = TextAnchor.UpperLeft;
            text.raycastTarget = false;
            return textObject;
        }

        private static GameObject CreateUIObject(string name, Transform parent, System.Type component)
        {
            GameObject value = new GameObject(name, typeof(RectTransform), component);
            value.transform.SetParent(parent, false);
            return value;
        }

        private static void Stretch(RectTransform rect)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        private static void EnsureFolder(string parent, string child)
        {
            string path = parent + "/" + child;
            if (!AssetDatabase.IsValidFolder(path))
                AssetDatabase.CreateFolder(parent, child);
        }

        private static void AddSceneToBuildSettings(string path)
        {
            EditorBuildSettingsScene[] current = EditorBuildSettings.scenes;
            foreach (EditorBuildSettingsScene entry in current)
                if (entry.path == path)
                    return;

            var updated = new EditorBuildSettingsScene[current.Length + 1];
            current.CopyTo(updated, 0);
            updated[current.Length] = new EditorBuildSettingsScene(path, true);
            EditorBuildSettings.scenes = updated;
        }
    }
}
#endif
