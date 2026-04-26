/*
 * Datei: GameSceneBuilder.cs
 * Zweck: Erstellt die spielbare GameScene-Testwelt ueber den Unity-Editor statt per manueller YAML-Bearbeitung.
 * Verantwortung: Baut Hierarchie, Player, NPCs, UI, Testinteraktionen und BuildSettings aus vorhandenen Runtime-Komponenten.
 * Abhaengigkeiten: UnityEditor, EditorSceneManager, vorhandene ITAA Runtime-Komponenten und Content-Assets.
 * Verwendung: Menue `ITAA/Scenes/Rebuild GameScene` oder Batchmode `ITAA.EditorTools.SceneBuilding.GameSceneBuilder.BuildGameScene`.
 */

#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using ITAA.Core.SceneManagement;
using ITAA.Features.HUD;
using ITAA.Features.Interaction;
using ITAA.Features.Terminal;
using ITAA.Gameplay.Interaction;
using ITAA.Gameplay.Scene;
using ITAA.Gameplay.World;
using ITAA.NPC.Arthur;
using ITAA.NPC.Bernd;
using ITAA.Player.Movement;
using ITAA.Player.UI;
using ITAA.UI.Panels;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ITAA.EditorTools.SceneBuilding
{
    public static class GameSceneBuilder
    {
        private const string GameScenePath = "Assets/Projekt/Content/Scenes/GameScene.unity";
        private const string StartScenePath = "Assets/Projekt/Content/Scenes/StartScene.unity";
        private const string GeneratedArtFolder = "Assets/Projekt/Content/Art/TestWorld";
        private const string SquareSpritePath = GeneratedArtFolder + "/testworld_square.png";

        [MenuItem("ITAA/Scenes/Rebuild GameScene")]
        public static void BuildGameScene()
        {
            EnsureGeneratedArt();

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            scene.name = SceneNames.GameScene;

            Sprite squareSprite = AssetDatabase.LoadAssetAtPath<Sprite>(SquareSpritePath);
            Sprite playerSprite = LoadFirstSprite("Assets/Projekt/Content/Art/Player/character.png");
            Sprite arthurSprite = LoadFirstSprite("Assets/Projekt/Content/Art/Player/character blue.png");
            Sprite berndSprite = LoadFirstSprite("Assets/Projekt/Content/Art/Player/character green.png");

            GameObject sceneRoot = CreateRoot("_SceneRoot");
            GameObject bootstrapRoot = CreateChild("_Bootstrap", sceneRoot.transform);
            GameObject systemsRoot = CreateChild("_Systems", sceneRoot.transform);
            GameObject uiRoot = CreateChild("_UI", sceneRoot.transform);
            GameObject worldRoot = CreateChild("World", sceneRoot.transform);
            GameObject charactersRoot = CreateChild("Characters", sceneRoot.transform);
            GameObject camerasRoot = CreateChild("Cameras", sceneRoot.transform);
            GameObject lightingRoot = CreateChild("Lighting", sceneRoot.transform);

            CreateWorld(worldRoot.transform, squareSprite);
            CreateUi(uiRoot.transform);
            GameObject player = CreatePlayer(charactersRoot.transform, playerSprite, uiRoot.transform);
            CreateArthur(charactersRoot.transform, arthurSprite);
            CreateBernd(charactersRoot.transform, berndSprite, uiRoot.transform);
            CreateCamera(camerasRoot.transform, player.transform);
            CreateLighting(lightingRoot.transform);
            CreateSystems(systemsRoot.transform);

            GameSceneBootstrap bootstrap = bootstrapRoot.AddComponent<GameSceneBootstrap>();
            SetObjectReference(bootstrap, "player", player.transform);
            SetObjectReference(bootstrap, "hudController", Object.FindAnyObjectByType<HudController>(FindObjectsInactive.Include));

            EditorSceneManager.SaveScene(scene, GameScenePath);
            EnsureBuildSettings();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"[SceneValidation] GameScene gebaut: {GameScenePath}");
        }

        [MenuItem("ITAA/Scenes/Open GameScene")]
        public static void OpenGameScene()
        {
            EditorSceneManager.OpenScene(GameScenePath);
        }

        private static void CreateWorld(Transform parent, Sprite squareSprite)
        {
            Transform ground = CreateChild("Ground", parent).transform;
            Transform roads = CreateChild("Roads", parent).transform;
            Transform buildings = CreateChild("Buildings", parent).transform;
            Transform nature = CreateChild("Nature", parent).transform;
            Transform walls = CreateChild("Walls", parent).transform;
            Transform interactables = CreateChild("Interactables", parent).transform;

            CreateVisualBlock("SnowGround", ground, squareSprite, new Vector2(34f, 22f), Vector2.zero, new Color(0.86f, 0.94f, 1f), false);
            CreateVisualBlock("MainRoad", roads, squareSprite, new Vector2(30f, 2.2f), new Vector2(0f, -1.2f), new Color(0.56f, 0.6f, 0.64f), false);
            CreateVisualBlock("NorthPath", roads, squareSprite, new Vector2(2.4f, 12f), new Vector2(-5f, 3.5f), new Color(0.6f, 0.64f, 0.68f), false);

            CreateVisualBlock("TestHouse_A", buildings, squareSprite, new Vector2(4f, 3f), new Vector2(-10f, 4f), new Color(0.56f, 0.72f, 0.9f), true);
            CreateVisualBlock("TestHouse_B", buildings, squareSprite, new Vector2(5f, 3.2f), new Vector2(8f, 4.5f), new Color(0.78f, 0.58f, 0.48f), true);
            CreateVisualBlock("Depot", buildings, squareSprite, new Vector2(4.5f, 2.5f), new Vector2(9f, -5f), new Color(0.62f, 0.66f, 0.74f), true);

            for (int i = 0; i < 8; i++)
            {
                float x = -13f + i * 3.8f;
                float y = i % 2 == 0 ? 7.5f : -7.5f;
                CreateTree(nature, squareSprite, new Vector2(x, y));
            }

            CreateBoundary(walls, squareSprite, "NorthBoundary", new Vector2(34f, 0.6f), new Vector2(0f, 10.8f));
            CreateBoundary(walls, squareSprite, "SouthBoundary", new Vector2(34f, 0.6f), new Vector2(0f, -10.8f));
            CreateBoundary(walls, squareSprite, "WestBoundary", new Vector2(0.6f, 22f), new Vector2(-16.8f, 0f));
            CreateBoundary(walls, squareSprite, "EastBoundary", new Vector2(0.6f, 22f), new Vector2(16.8f, 0f));

            PlayerSpawnPoint spawn = CreateChild("PlayerSpawn_Default", parent).AddComponent<PlayerSpawnPoint>();
            spawn.transform.position = new Vector3(-2f, -3.5f, 0f);

            CreatePickup(interactables, squareSprite, new Vector2(-7f, -2f));
            CreateTerminal(interactables, squareSprite, new Vector2(4f, -3.2f));
            CreateAchievementTerminal(interactables, squareSprite, new Vector2(12f, -1.2f));
        }

        private static GameObject CreatePlayer(Transform parent, Sprite sprite, Transform uiRoot)
        {
            GameObject player = CreateChild("Player", parent);
            player.tag = "Player";
            player.transform.position = new Vector3(-2f, -3.5f, 0f);

            SpriteRenderer renderer = player.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingOrder = 20;

            Animator animator = player.AddComponent<Animator>();
            animator.runtimeAnimatorController = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(
                "Assets/Projekt/Content/Art/Animations/Player/Player.controller");

            Rigidbody2D rb = player.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            CapsuleCollider2D collider = player.AddComponent<CapsuleCollider2D>();
            collider.size = new Vector2(0.72f, 1.25f);
            collider.offset = new Vector2(0f, 0.6f);

            player.AddComponent<PlayerInputReader>();
            player.AddComponent<PlayerMotor2D>();
            player.AddComponent<PlayerController>();

            GameObject detector = CreateChild("InteractionDetector", player.transform);
            detector.transform.localPosition = Vector3.zero;
            CircleCollider2D trigger = detector.AddComponent<CircleCollider2D>();
            trigger.isTrigger = true;
            trigger.radius = 1.25f;
            InteractionDetector interactionDetector = detector.AddComponent<InteractionDetector>();
            InteractionController interactionController = detector.AddComponent<InteractionController>();

            InteractionPromptView promptView = uiRoot.GetComponentInChildren<InteractionPromptView>(true);
            SetObjectReference(interactionController, "detector", interactionDetector);
            SetObjectReference(interactionController, "promptView", promptView);
            SetObjectReference(interactionController, "interactorRoot", player.transform);

            CreatePlayerNameTag(player.transform);
            return player;
        }

        private static void CreateArthur(Transform parent, Sprite sprite)
        {
            GameObject arthur = CreateCharacterRoot("Arthur", parent, sprite, new Vector3(-9f, 0.8f, 0f), 18);
            Animator animator = arthur.AddComponent<Animator>();
            animator.runtimeAnimatorController = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(
                "Assets/Projekt/Content/Art/Animations/Arthur/Arthur.controller");
            arthur.AddComponent<ArthurAnimationController>();
            CreateNameLabel(arthur.transform, "Arthur", new Color(0.72f, 0.88f, 1f));
        }

        private static void CreateBernd(Transform parent, Sprite sprite, Transform uiRoot)
        {
            GameObject bernd = CreateCharacterRoot("Bernd", parent, sprite, new Vector3(3f, 1.4f, 0f), 19);
            Animator animator = bernd.AddComponent<Animator>();
            animator.runtimeAnimatorController = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(
                "Assets/Projekt/Content/Art/Animations/Bernd/Bernd.controller");

            bernd.AddComponent<BerndAnimationController>();
            bernd.AddComponent<BerndMovementToPlayer>();
            BerndQuizStarter quizStarter = bernd.AddComponent<BerndQuizStarter>();
            SetObjectReference(quizStarter, "quizSet", AssetDatabase.LoadAssetAtPath<Object>("Assets/Projekt/Content/Quiz/BerndIntroQuiz.asset"));
            SetObjectReference(quizStarter, "quizPanel", uiRoot.GetComponentInChildren<QuizPanel>(true));

            GameObject detection = CreateChild("InteractionZone", bernd.transform);
            detection.transform.localPosition = Vector3.zero;
            CircleCollider2D trigger = detection.AddComponent<CircleCollider2D>();
            trigger.isTrigger = true;
            trigger.radius = 2.0f;
            detection.AddComponent<BerndDetectionZone>();

            BerndInteractableAdapter adapter = bernd.AddComponent<BerndInteractableAdapter>();
            SetString(adapter, "interactionPrompt", "Mit Bernd sprechen [E]");
            SetObjectReference(adapter, "quizStarter", quizStarter);
            SetObjectReference(adapter, "detectionZone", detection.GetComponent<BerndDetectionZone>());

            CreateNameLabel(bernd.transform, "Bernd", new Color(0.7f, 1f, 0.72f));
        }

        private static void CreateUi(Transform parent)
        {
            GameObject canvasObject = CreateChild("GameplayCanvas", parent);
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;
            CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;
            canvasObject.AddComponent<GraphicRaycaster>();

            GameObject hudObject = CreateUiObject("HUD", canvasObject.transform);
            hudObject.AddComponent<HudView>();
            hudObject.AddComponent<HudController>();

            GameObject promptObject = CreateUiObject("InteractionPrompt", canvasObject.transform);
            RectTransform promptRect = promptObject.GetComponent<RectTransform>();
            promptRect.anchorMin = new Vector2(0.5f, 0f);
            promptRect.anchorMax = new Vector2(0.5f, 0f);
            promptRect.pivot = new Vector2(0.5f, 0f);
            promptRect.anchoredPosition = new Vector2(0f, 112f);
            promptRect.sizeDelta = new Vector2(460f, 54f);
            Image promptImage = promptObject.AddComponent<Image>();
            promptImage.color = new Color(0.06f, 0.08f, 0.1f, 0.86f);
            TextMeshProUGUI promptText = CreateText("PromptText", promptObject.transform, "Interagieren [E]", 24f, FontStyles.Bold);
            promptText.alignment = TextAlignmentOptions.Center;
            Stretch(promptText.rectTransform);
            promptObject.AddComponent<InteractionPromptView>();

            GameObject quizObject = CreateUiObject("QuizPanel", canvasObject.transform);
            Stretch(quizObject.GetComponent<RectTransform>());
            quizObject.AddComponent<QuizPanel>();
            quizObject.SetActive(false);

            GameObject terminalObject = CreateUiObject("TerminalPanel", canvasObject.transform);
            Stretch(terminalObject.GetComponent<RectTransform>());
            terminalObject.AddComponent<TerminalPanel>();
            terminalObject.SetActive(false);

            GameObject eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
            eventSystem.transform.SetParent(parent, false);
        }

        private static void CreateCamera(Transform parent, Transform target)
        {
            GameObject cameraObject = CreateChild("Main Camera", parent);
            cameraObject.tag = "MainCamera";
            cameraObject.transform.position = new Vector3(0f, 0f, -10f);
            Camera camera = cameraObject.AddComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = 6f;
            camera.backgroundColor = new Color(0.68f, 0.82f, 0.96f);
            SimpleCameraFollow2D follow = cameraObject.AddComponent<SimpleCameraFollow2D>();
            follow.SetTarget(target);
            cameraObject.AddComponent<AudioListener>();
        }

        private static void CreateLighting(Transform parent)
        {
            GameObject lightObject = CreateChild("Global Light 2D", parent);
            Light2D light = lightObject.AddComponent<Light2D>();
            light.lightType = Light2D.LightType.Global;
            light.intensity = 1f;
        }

        private static void CreateSystems(Transform parent)
        {
            CreateChild("SceneLocalSystems", parent);
        }

        private static GameObject CreateCharacterRoot(string name, Transform parent, Sprite sprite, Vector3 position, int sortingOrder)
        {
            GameObject character = CreateChild(name, parent);
            character.transform.position = position;
            SpriteRenderer renderer = character.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingOrder = sortingOrder;

            Rigidbody2D rb = character.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Static;

            CapsuleCollider2D collider = character.AddComponent<CapsuleCollider2D>();
            collider.size = new Vector2(0.72f, 1.25f);
            collider.offset = new Vector2(0f, 0.6f);
            return character;
        }

        private static void CreatePlayerNameTag(Transform player)
        {
            GameObject canvasObject = CreateWorldCanvas("NameTagCanvas", player, new Vector3(0f, 1.7f, 0f));
            TextMeshProUGUI text = CreateText("Text", canvasObject.transform, "Player", 3.2f, FontStyles.Bold);
            text.alignment = TextAlignmentOptions.Center;
            text.color = Color.white;
            RectTransform textRect = text.rectTransform;
            textRect.sizeDelta = new Vector2(4f, 0.6f);
            PlayerNameTag nameTag = canvasObject.AddComponent<PlayerNameTag>();
            nameTag.SetTarget(player);
        }

        private static void CreateNameLabel(Transform target, string label, Color color)
        {
            GameObject canvasObject = CreateWorldCanvas("NameTagCanvas", target, new Vector3(0f, 1.7f, 0f));
            TextMeshProUGUI text = CreateText("Text", canvasObject.transform, label, 3.2f, FontStyles.Bold);
            text.alignment = TextAlignmentOptions.Center;
            text.color = color;
            RectTransform textRect = text.rectTransform;
            textRect.sizeDelta = new Vector2(4f, 0.6f);
        }

        private static GameObject CreateWorldCanvas(string name, Transform parent, Vector3 localPosition)
        {
            GameObject canvasObject = CreateChild(name, parent);
            canvasObject.transform.localPosition = localPosition;
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvasObject.AddComponent<CanvasScaler>();
            canvasObject.AddComponent<GraphicRaycaster>();
            RectTransform rect = canvasObject.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(4f, 0.8f);
            rect.localScale = Vector3.one * 0.1f;
            return canvasObject;
        }

        private static GameObject CreateVisualBlock(
            string name,
            Transform parent,
            Sprite sprite,
            Vector2 size,
            Vector2 position,
            Color color,
            bool withCollider)
        {
            GameObject block = CreateChild(name, parent);
            block.transform.position = position;
            block.transform.localScale = new Vector3(size.x, size.y, 1f);
            SpriteRenderer renderer = block.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.color = color;
            renderer.sortingOrder = withCollider ? 5 : 0;

            if (withCollider)
            {
                block.AddComponent<BoxCollider2D>();
            }

            return block;
        }

        private static void CreateBoundary(Transform parent, Sprite sprite, string name, Vector2 size, Vector2 position)
        {
            GameObject boundary = CreateVisualBlock(name, parent, sprite, size, position, new Color(0.42f, 0.48f, 0.52f), true);
            boundary.GetComponent<SpriteRenderer>().sortingOrder = 8;
        }

        private static void CreateTree(Transform parent, Sprite sprite, Vector2 position)
        {
            GameObject trunk = CreateVisualBlock("TreeTrunk", parent, sprite, new Vector2(0.45f, 1f), position, new Color(0.46f, 0.28f, 0.16f), true);
            trunk.GetComponent<SpriteRenderer>().sortingOrder = 10;
            GameObject crown = CreateVisualBlock("TreeCrown", parent, sprite, new Vector2(1.4f, 1.4f), position + Vector2.up * 0.85f, new Color(0.24f, 0.48f, 0.32f), false);
            crown.GetComponent<SpriteRenderer>().sortingOrder = 11;
        }

        private static void CreatePickup(Transform parent, Sprite sprite, Vector2 position)
        {
            GameObject pickup = CreateVisualBlock("TestItem_Diensthandy", parent, sprite, new Vector2(0.55f, 0.35f), position, new Color(0.15f, 0.18f, 0.2f), false);
            CircleCollider2D collider = pickup.AddComponent<CircleCollider2D>();
            collider.isTrigger = true;
            collider.radius = 0.8f;
            TestWorldInteractable interactable = pickup.AddComponent<TestWorldInteractable>();
            SetString(interactable, "interactionPrompt", "Diensthandy aufnehmen [E]");
            SetEnum(interactable, "interactionType", InteractionType.Pickup);
            SetEnumByName(interactable, "action", "PickupItem");
            SetBool(interactable, "disableAfterSuccessfulInteraction", true);
        }

        private static void CreateTerminal(Transform parent, Sprite sprite, Vector2 position)
        {
            GameObject terminal = CreateVisualBlock("NetworkTerminal", parent, sprite, new Vector2(1.2f, 1f), position, new Color(0.08f, 0.16f, 0.13f), true);
            CircleCollider2D trigger = terminal.AddComponent<CircleCollider2D>();
            trigger.isTrigger = true;
            trigger.radius = 1.4f;
            TestWorldInteractable interactable = terminal.AddComponent<TestWorldInteractable>();
            SetString(interactable, "interactionPrompt", "Terminal benutzen [E]");
            SetEnum(interactable, "interactionType", InteractionType.Terminal);
            SetEnumByName(interactable, "action", "OpenTerminal");
        }

        private static void CreateAchievementTerminal(Transform parent, Sprite sprite, Vector2 position)
        {
            GameObject marker = CreateVisualBlock("AchievementTrigger", parent, sprite, new Vector2(1f, 1f), position, new Color(0.95f, 0.75f, 0.22f), false);
            CircleCollider2D trigger = marker.AddComponent<CircleCollider2D>();
            trigger.isTrigger = true;
            trigger.radius = 1.1f;
            TestWorldInteractable interactable = marker.AddComponent<TestWorldInteractable>();
            SetString(interactable, "interactionPrompt", "Achievement testen [E]");
            SetEnum(interactable, "interactionType", InteractionType.Hint);
            SetEnumByName(interactable, "action", "UnlockAchievement");
            SetString(interactable, "achievementId", "first_quiz");
        }

        private static void EnsureGeneratedArt()
        {
            if (!AssetDatabase.IsValidFolder(GeneratedArtFolder))
            {
                AssetDatabase.CreateFolder("Assets/Projekt/Content/Art", "TestWorld");
            }

            if (!File.Exists(SquareSpritePath))
            {
                Texture2D texture = new Texture2D(8, 8, TextureFormat.RGBA32, false);
                Color[] pixels = new Color[64];

                for (int i = 0; i < pixels.Length; i++)
                {
                    pixels[i] = Color.white;
                }

                texture.SetPixels(pixels);
                texture.Apply();
                File.WriteAllBytes(SquareSpritePath, texture.EncodeToPNG());
            }

            AssetDatabase.ImportAsset(SquareSpritePath);
            TextureImporter importer = AssetImporter.GetAtPath(SquareSpritePath) as TextureImporter;
            if (importer != null)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spritePixelsPerUnit = 1f;
                importer.filterMode = FilterMode.Point;
                importer.SaveAndReimport();
            }
        }

        private static Sprite LoadFirstSprite(string path)
        {
            Object[] assets = AssetDatabase.LoadAllAssetsAtPath(path);
            for (int i = 0; i < assets.Length; i++)
            {
                if (assets[i] is Sprite sprite)
                {
                    return sprite;
                }
            }

            return AssetDatabase.LoadAssetAtPath<Sprite>(path);
        }

        private static GameObject CreateRoot(string name)
        {
            return new GameObject(name);
        }

        private static GameObject CreateChild(string name, Transform parent)
        {
            GameObject child = new GameObject(name);
            child.transform.SetParent(parent, false);
            return child;
        }

        private static GameObject CreateUiObject(string name, Transform parent)
        {
            GameObject child = new GameObject(name, typeof(RectTransform));
            child.transform.SetParent(parent, false);
            return child;
        }

        private static TextMeshProUGUI CreateText(string name, Transform parent, string text, float size, FontStyles style)
        {
            GameObject textObject = CreateUiObject(name, parent);
            TextMeshProUGUI tmp = textObject.AddComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.fontSize = size;
            tmp.fontStyle = style;
            tmp.color = Color.white;
            tmp.raycastTarget = false;
            return tmp;
        }

        private static void Stretch(RectTransform rect)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        private static void EnsureBuildSettings()
        {
            List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>();
            AddBuildScene(scenes, StartScenePath);
            AddBuildScene(scenes, GameScenePath);

            EditorBuildSettingsScene[] existingScenes = EditorBuildSettings.scenes;
            for (int i = 0; i < existingScenes.Length; i++)
            {
                EditorBuildSettingsScene existing = existingScenes[i];
                if (existing == null ||
                    string.IsNullOrWhiteSpace(existing.path) ||
                    existing.path == StartScenePath ||
                    existing.path == GameScenePath)
                {
                    continue;
                }

                scenes.Add(existing);
            }

            EditorBuildSettings.scenes = scenes.ToArray();
        }

        private static void AddBuildScene(List<EditorBuildSettingsScene> scenes, string path)
        {
            scenes.Add(new EditorBuildSettingsScene(path, true));
        }

        private static void SetObjectReference(Object target, string propertyName, Object value)
        {
            SerializedObject serializedObject = new SerializedObject(target);
            SerializedProperty property = serializedObject.FindProperty(propertyName);
            if (property != null)
            {
                property.objectReferenceValue = value;
                serializedObject.ApplyModifiedPropertiesWithoutUndo();
            }
        }

        private static void SetString(Object target, string propertyName, string value)
        {
            SerializedObject serializedObject = new SerializedObject(target);
            SerializedProperty property = serializedObject.FindProperty(propertyName);
            if (property != null)
            {
                property.stringValue = value;
                serializedObject.ApplyModifiedPropertiesWithoutUndo();
            }
        }

        private static void SetBool(Object target, string propertyName, bool value)
        {
            SerializedObject serializedObject = new SerializedObject(target);
            SerializedProperty property = serializedObject.FindProperty(propertyName);
            if (property != null)
            {
                property.boolValue = value;
                serializedObject.ApplyModifiedPropertiesWithoutUndo();
            }
        }

        private static void SetEnum(Object target, string propertyName, InteractionType value)
        {
            SerializedObject serializedObject = new SerializedObject(target);
            SerializedProperty property = serializedObject.FindProperty(propertyName);
            if (property != null)
            {
                property.enumValueIndex = (int)value;
                serializedObject.ApplyModifiedPropertiesWithoutUndo();
            }
        }

        private static void SetEnumByName(Object target, string propertyName, string enumName)
        {
            SerializedObject serializedObject = new SerializedObject(target);
            SerializedProperty property = serializedObject.FindProperty(propertyName);
            if (property != null)
            {
                for (int i = 0; i < property.enumNames.Length; i++)
                {
                    if (property.enumNames[i] == enumName)
                    {
                        property.enumValueIndex = i;
                        break;
                    }
                }

                serializedObject.ApplyModifiedPropertiesWithoutUndo();
            }
        }
    }
}
#endif
