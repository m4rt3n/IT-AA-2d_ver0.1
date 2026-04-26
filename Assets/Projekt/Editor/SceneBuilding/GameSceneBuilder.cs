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
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace ITAA.EditorTools.SceneBuilding
{
    public static class GameSceneBuilder
    {
        private const string GameScenePath = "Assets/Projekt/Content/Scenes/GameScene.unity";
        private const string StartScenePath = "Assets/Projekt/Content/Scenes/StartScene.unity";
        private const string GeneratedArtFolder = "Assets/Projekt/Content/Art/TestWorld";
        private const string SquareSpritePath = GeneratedArtFolder + "/testworld_square.png";
        private const string TilesFolder = "Assets/Projekt/Content/Art/Tiles";
        private const string TilesetPath = "Assets/Projekt/Content/Art/Floors/cloud_tileset.png";

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
            VisualAssetSet visualAssets = LoadVisualAssets(squareSprite);

            GameObject sceneRoot = CreateRoot("_SceneRoot");
            GameObject bootstrapRoot = CreateChild("_Bootstrap", sceneRoot.transform);
            GameObject systemsRoot = CreateChild("_Systems", sceneRoot.transform);
            GameObject uiRoot = CreateChild("_UI", sceneRoot.transform);
            GameObject worldRoot = CreateChild("World", sceneRoot.transform);
            GameObject charactersRoot = CreateChild("Characters", sceneRoot.transform);
            GameObject camerasRoot = CreateChild("Cameras", sceneRoot.transform);
            GameObject lightingRoot = CreateChild("Lighting", sceneRoot.transform);

            CreateWorld(worldRoot.transform, visualAssets);
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

        [MenuItem("ITAA/Scenes/Rebuild GameScene Visual World")]
        public static void RebuildVisualWorld()
        {
            if (!File.Exists(GameScenePath))
            {
                Debug.LogError($"[SceneValidation] GameScene fehlt: {GameScenePath}");
                return;
            }

            EnsureGeneratedArt();
            EditorSceneManager.OpenScene(GameScenePath);

            GameObject world = FindObjectByName("World");
            if (world == null)
            {
                GameObject sceneRoot = FindObjectByName("_SceneRoot") ?? CreateRoot("_SceneRoot");
                world = CreateChild("World", sceneRoot.transform);
            }

            ReplacePlaceholderSprites(world.transform);
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("[SceneValidation] GameScene Visual World neu aufgebaut.");
        }

        public static void ReplacePlaceholderSprites(Transform worldRoot)
        {
            if (worldRoot == null)
            {
                Debug.LogWarning("[SceneValidation] ReplacePlaceholderSprites ohne World-Root aufgerufen.");
                return;
            }

            ClearChildren(worldRoot);
            Sprite squareSprite = AssetDatabase.LoadAssetAtPath<Sprite>(SquareSpritePath);
            CreateWorld(worldRoot, LoadVisualAssets(squareSprite));
        }

        public static bool ValidateWorldSprites()
        {
            SpriteRenderer[] renderers = Object.FindObjectsByType<SpriteRenderer>(FindObjectsInactive.Include);
            bool isValid = true;

            for (int i = 0; i < renderers.Length; i++)
            {
                SpriteRenderer renderer = renderers[i];
                if (renderer == null || renderer.sprite != null)
                {
                    continue;
                }

                isValid = false;
                Debug.LogWarning($"[SceneValidation] SpriteRenderer ohne Sprite: {renderer.gameObject.name}", renderer);
            }

            return isValid;
        }

        private static void CreateWorld(Transform parent, VisualAssetSet assets)
        {
            Transform ground = CreateChild("Ground", parent).transform;
            Transform roads = CreateChild("Roads", parent).transform;
            Transform buildings = CreateChild("Buildings", parent).transform;
            Transform nature = CreateChild("Nature", parent).transform;
            Transform walls = CreateChild("Walls", parent).transform;
            Transform interactables = CreateChild("Interactables", parent).transform;

            CreateTilemap("SnowGround_Tilemap", ground, assets.SnowTile, new Vector2Int(34, 22), Vector2Int.zero, 0);
            CreateRoads(roads, assets);

            CreateHouse(buildings, "BlueHouse", new Vector2(-10f, 4f), assets.BlueWall, assets.BlueRoof, assets.WallTop, assets.Door, 4);
            CreateHouse(buildings, "OrangeHouse", new Vector2(8f, 4.5f), assets.OrangeWall, assets.OrangeRoof, assets.WallTop, assets.Door, 5);
            CreateDepot(buildings, new Vector2(9f, -5f), assets);

            for (int i = 0; i < 8; i++)
            {
                float x = -13f + i * 3.8f;
                float y = i % 2 == 0 ? 7.5f : -7.5f;
                CreateTree(nature, assets, new Vector2(x, y));
            }

            CreateFlowerPatch(nature, assets, new Vector2(-2f, 5.8f));
            CreateFlowerPatch(nature, assets, new Vector2(12f, 2.3f));

            CreateBoundary(walls, assets.FenceTile, "NorthFence", new Vector2(34f, 0.6f), new Vector2(0f, 10.8f));
            CreateBoundary(walls, assets.FenceTile, "SouthFence", new Vector2(34f, 0.6f), new Vector2(0f, -10.8f));
            CreateBoundary(walls, assets.FencePost, "WestFence", new Vector2(0.6f, 22f), new Vector2(-16.8f, 0f));
            CreateBoundary(walls, assets.FencePost, "EastFence", new Vector2(0.6f, 22f), new Vector2(16.8f, 0f));

            PlayerSpawnPoint spawn = CreateChild("PlayerSpawn_Default", parent).AddComponent<PlayerSpawnPoint>();
            spawn.transform.position = new Vector3(-2f, -3.5f, 0f);

            CreatePickup(interactables, assets.Phone, new Vector2(-7f, -2f));
            CreateTerminal(interactables, assets.Terminal, new Vector2(4f, -3.2f));
            CreateAchievementTerminal(interactables, assets.Marker, new Vector2(12f, -1.2f));
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

        private static void CreateTilemap(
            string name,
            Transform parent,
            TileBase tile,
            Vector2Int size,
            Vector2Int origin,
            int sortingOrder)
        {
            GameObject gridObject = CreateChild(name + "_Grid", parent);
            Grid grid = gridObject.AddComponent<Grid>();
            grid.cellSize = Vector3.one;

            GameObject tilemapObject = CreateChild(name, gridObject.transform);
            Tilemap tilemap = tilemapObject.AddComponent<Tilemap>();
            TilemapRenderer renderer = tilemapObject.AddComponent<TilemapRenderer>();
            renderer.sortingOrder = sortingOrder;

            if (tile == null)
            {
                Debug.LogWarning($"[SceneValidation] Tile fuer {name} fehlt.");
                return;
            }

            int halfWidth = size.x / 2;
            int halfHeight = size.y / 2;
            for (int x = -halfWidth; x < halfWidth; x++)
            {
                for (int y = -halfHeight; y < halfHeight; y++)
                {
                    tilemap.SetTile(new Vector3Int(origin.x + x, origin.y + y, 0), tile);
                }
            }
        }

        private static void CreateRoads(Transform parent, VisualAssetSet assets)
        {
            CreateTilemap("MainRoad_Tilemap", parent, assets.RoadTile, new Vector2Int(30, 3), new Vector2Int(0, -1), 1);
            CreateTilemap("NorthPath_Tilemap", parent, assets.PathTile, new Vector2Int(3, 12), new Vector2Int(-5, 3), 1);
            CreateTilemap("NpcPlaza_Tilemap", parent, assets.PlazaTile, new Vector2Int(6, 4), new Vector2Int(3, 1), 1);
        }

        private static void CreateHouse(
            Transform parent,
            string name,
            Vector2 position,
            Sprite wall,
            Sprite roof,
            Sprite trim,
            Sprite door,
            int sortingOrder)
        {
            GameObject root = CreateChild(name, parent);
            root.transform.position = position;

            CreateSpriteChild("Roof", root.transform, roof, new Vector2(0f, 1.15f), new Vector2(4.8f, 1.8f), sortingOrder + 1, Color.white);
            CreateSpriteChild("Walls", root.transform, wall, Vector2.zero, new Vector2(4.8f, 2.4f), sortingOrder, Color.white);
            CreateSpriteChild("SnowTrim", root.transform, trim, new Vector2(0f, 1.28f), new Vector2(4.8f, 0.35f), sortingOrder + 2, Color.white);
            CreateSpriteChild("Door", root.transform, door, new Vector2(0f, -0.75f), new Vector2(0.8f, 1.2f), sortingOrder + 3, Color.white);

            BoxCollider2D collider = root.AddComponent<BoxCollider2D>();
            collider.size = new Vector2(4.6f, 2.2f);
            collider.offset = new Vector2(0f, -0.1f);
        }

        private static void CreateDepot(Transform parent, Vector2 position, VisualAssetSet assets)
        {
            GameObject root = CreateChild("Depot", parent);
            root.transform.position = position;

            CreateSpriteChild("Walls", root.transform, assets.StoneWall, Vector2.zero, new Vector2(4.6f, 2f), 5, Color.white);
            CreateSpriteChild("Roof", root.transform, assets.BlueRoof, new Vector2(0f, 1.05f), new Vector2(4.6f, 1.4f), 6, Color.white);
            CreateSpriteChild("Crates", root.transform, assets.Crate, new Vector2(-1.2f, -0.65f), new Vector2(1.2f, 0.7f), 7, Color.white);

            BoxCollider2D collider = root.AddComponent<BoxCollider2D>();
            collider.size = new Vector2(4.4f, 2f);
            collider.offset = new Vector2(0f, -0.1f);
        }

        private static GameObject CreateSpriteChild(
            string name,
            Transform parent,
            Sprite sprite,
            Vector2 localPosition,
            Vector2 size,
            int sortingOrder,
            Color color)
        {
            GameObject child = CreateChild(name, parent);
            child.transform.localPosition = localPosition;
            child.transform.localScale = Vector3.one;
            SpriteRenderer renderer = child.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.drawMode = SpriteDrawMode.Sliced;
            renderer.size = size;
            renderer.sortingOrder = sortingOrder;
            renderer.color = color;
            return child;
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
            GameObject boundary = CreateVisualBlock(name, parent, sprite, size, position, Color.white, true);
            boundary.GetComponent<SpriteRenderer>().sortingOrder = 8;
        }

        private static void CreateTree(Transform parent, VisualAssetSet assets, Vector2 position)
        {
            GameObject tree = CreateVisualBlock("SnowTree", parent, assets.Tree, new Vector2(1.8f, 2.3f), position, Color.white, true);
            SpriteRenderer renderer = tree.GetComponent<SpriteRenderer>();
            renderer.sortingOrder = 9;
            BoxCollider2D collider = tree.GetComponent<BoxCollider2D>();
            collider.size = new Vector2(0.55f, 0.65f);
            collider.offset = new Vector2(0f, -0.75f);
        }

        private static void CreateFlowerPatch(Transform parent, VisualAssetSet assets, Vector2 position)
        {
            for (int i = 0; i < 4; i++)
            {
                Vector2 offset = new Vector2((i % 2) * 0.7f, (i / 2) * 0.45f);
                GameObject flower = CreateVisualBlock("FlowerPatch", parent, assets.Flowers, new Vector2(0.8f, 0.5f), position + offset, Color.white, false);
                flower.GetComponent<SpriteRenderer>().sortingOrder = 2;
            }
        }

        private static void CreatePickup(Transform parent, Sprite sprite, Vector2 position)
        {
            GameObject pickup = CreateVisualBlock("TestItem_Diensthandy", parent, sprite, new Vector2(0.7f, 0.7f), position, Color.white, false);
            pickup.GetComponent<SpriteRenderer>().sortingOrder = 12;
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
            GameObject terminal = CreateVisualBlock("NetworkTerminal", parent, sprite, new Vector2(1.2f, 1f), position, Color.white, true);
            terminal.GetComponent<SpriteRenderer>().sortingOrder = 7;
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
            GameObject marker = CreateVisualBlock("AchievementTrigger", parent, sprite, new Vector2(1f, 1f), position, Color.white, false);
            marker.GetComponent<SpriteRenderer>().sortingOrder = 12;
            CircleCollider2D trigger = marker.AddComponent<CircleCollider2D>();
            trigger.isTrigger = true;
            trigger.radius = 1.1f;
            TestWorldInteractable interactable = marker.AddComponent<TestWorldInteractable>();
            SetString(interactable, "interactionPrompt", "Achievement testen [E]");
            SetEnum(interactable, "interactionType", InteractionType.Hint);
            SetEnumByName(interactable, "action", "UnlockAchievement");
            SetString(interactable, "achievementId", "first_quiz");
        }

        private static VisualAssetSet LoadVisualAssets(Sprite fallbackSprite)
        {
            return new VisualAssetSet
            {
                FallbackSprite = fallbackSprite,
                SnowTile = LoadTile("cloud_tileset_0"),
                RoadTile = LoadTile("cloud_tileset_164"),
                PathTile = LoadTile("cloud_tileset_177"),
                PlazaTile = LoadTile("cloud_tileset_99"),
                BlueWall = LoadSpriteFromTileset("cloud_tileset_207", fallbackSprite),
                OrangeWall = LoadSpriteFromTileset("cloud_tileset_211", fallbackSprite),
                StoneWall = LoadSpriteFromTileset("cloud_tileset_164", fallbackSprite),
                BlueRoof = LoadSpriteFromTileset("cloud_tileset_187", fallbackSprite),
                OrangeRoof = LoadSpriteFromTileset("cloud_tileset_191", fallbackSprite),
                WallTop = LoadSpriteFromTileset("cloud_tileset_105", fallbackSprite),
                Door = LoadSpriteFromTileset("cloud_tileset_238", fallbackSprite),
                Tree = LoadSpriteFromTileset("cloud_tileset_72", fallbackSprite),
                Flowers = LoadSpriteFromTileset("cloud_tileset_487", fallbackSprite),
                FenceTile = LoadSpriteFromTileset("cloud_tileset_424", fallbackSprite),
                FencePost = LoadSpriteFromTileset("cloud_tileset_422", fallbackSprite),
                Phone = LoadSpriteFromTileset("cloud_tileset_474", fallbackSprite),
                Terminal = LoadSpriteFromTileset("cloud_tileset_464", fallbackSprite),
                Marker = LoadSpriteFromTileset("cloud_tileset_501", fallbackSprite),
                Crate = LoadSpriteFromTileset("cloud_tileset_403", fallbackSprite)
            };
        }

        private static TileBase LoadTile(string tileName)
        {
            return AssetDatabase.LoadAssetAtPath<TileBase>($"{TilesFolder}/{tileName}.asset");
        }

        private static Sprite LoadSpriteFromTileset(string spriteName, Sprite fallbackSprite)
        {
            Object[] assets = AssetDatabase.LoadAllAssetsAtPath(TilesetPath);
            for (int i = 0; i < assets.Length; i++)
            {
                if (assets[i] is Sprite sprite && sprite.name == spriteName)
                {
                    return sprite;
                }
            }

            Debug.LogWarning($"[SceneValidation] Sprite {spriteName} fehlt, nutze Platzhalter.");
            return fallbackSprite;
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

        private static void ClearChildren(Transform parent)
        {
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                Object.DestroyImmediate(parent.GetChild(i).gameObject);
            }
        }

        private static GameObject FindObjectByName(string objectName)
        {
            GameObject[] objects = Object.FindObjectsByType<GameObject>(FindObjectsInactive.Include);

            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i] != null && objects[i].name == objectName)
                {
                    return objects[i];
                }
            }

            return null;
        }

        private sealed class VisualAssetSet
        {
            public Sprite FallbackSprite;
            public TileBase SnowTile;
            public TileBase RoadTile;
            public TileBase PathTile;
            public TileBase PlazaTile;
            public Sprite BlueWall;
            public Sprite OrangeWall;
            public Sprite StoneWall;
            public Sprite BlueRoof;
            public Sprite OrangeRoof;
            public Sprite WallTop;
            public Sprite Door;
            public Sprite Tree;
            public Sprite Flowers;
            public Sprite FenceTile;
            public Sprite FencePost;
            public Sprite Phone;
            public Sprite Terminal;
            public Sprite Marker;
            public Sprite Crate;
        }
    }
}
#endif
