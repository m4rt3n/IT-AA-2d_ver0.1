/*
 * Datei: GameSceneBuilder.cs
 * Zweck: Erstellt und aktualisiert die spielbare GameScene-Testwelt ueber den Unity-Editor statt per manueller YAML-Bearbeitung.
 * Verantwortung: Baut Hierarchie, strukturierte Tilemap-Welt, Player, NPCs, UI, Testinteraktionen und BuildSettings aus vorhandenen Runtime-Komponenten.
 * Abhaengigkeiten: UnityEditor, EditorSceneManager, Tilemap-System, vorhandene ITAA Runtime-Komponenten und Content-Assets.
 * Verwendung: Menue `ITAA/Scenes/Rebuild GameScene`, `ITAA/Scenes/Rebuild Structured Winter Hub` oder Batchmode-ExecuteMethod.
 */

#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Text;
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
        private const string WinterTownSourcePath = "Assets/Projekt/Content/Art/Floors/winter_town_tileset.png";
        private const string CleanTilesetPath = GeneratedArtFolder + "/WinterTownTileset_Clean_32.png";
        private const string TestWorldTilesRoot = "Assets/Projekt/Content/Tiles/TestWorld";
        private const string TileMappingPath = TestWorldTilesRoot + "/tile_mapping.json";
        private const int CleanTileSize = 32;
        private const int CleanTilesPerRow = 8;

        private static readonly Vector3 PlayerSpawnPosition = new Vector3(20f, 14f, 0f);
        private static readonly Vector3 BerndPosition = new Vector3(20f, 19f, 0f);
        private static readonly Vector3 ArthurPosition = new Vector3(20f, 8f, 0f);
        private static readonly Vector2Int ItemPosition = new Vector2Int(7, 14);
        private static readonly Vector2Int TerminalPosition = new Vector2Int(32, 14);

        [MenuItem("ITAA/Scenes/Rebuild GameScene")]
        public static void BuildGameScene()
        {
            EnsureGeneratedArt();
            EnsureCleanTilesetAndTiles();

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            scene.name = SceneNames.GameScene;

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

            CreateStructuredWorld(worldRoot.transform);
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
            RebuildStructuredWinterHub();
        }

        [MenuItem("ITAA/Scenes/Rebuild Structured Winter Hub")]
        public static void RebuildStructuredWinterHub()
        {
            if (!File.Exists(GameScenePath))
            {
                Debug.LogError($"[SceneValidation] GameScene fehlt: {GameScenePath}");
                return;
            }

            EnsureGeneratedArt();
            EnsureCleanTilesetAndTiles();
            EditorSceneManager.OpenScene(GameScenePath);

            GameObject world = FindObjectByName("World");
            if (world == null)
            {
                GameObject sceneRoot = FindObjectByName("_SceneRoot") ?? CreateRoot("_SceneRoot");
                world = CreateChild("World", sceneRoot.transform);
            }

            ClearChildren(world.transform);
            CreateStructuredWorld(world.transform);
            RepositionSceneActors();
            EnsureBuildSettings();

            EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("[SceneValidation] Structured Winter Hub neu aufgebaut.");
        }

        public static void ReplacePlaceholderSprites(Transform worldRoot)
        {
            if (worldRoot == null)
            {
                Debug.LogWarning("[SceneValidation] ReplacePlaceholderSprites ohne World-Root aufgerufen.");
                return;
            }

            ClearChildren(worldRoot);
            CreateStructuredWorld(worldRoot);
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

        [MenuItem("ITAA/Tilesets/Rebuild Clean Winter Town Tileset")]
        public static void RebuildCleanWinterTownTileset()
        {
            EnsureGeneratedArt();
            EnsureCleanTilesetAndTiles();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"[SceneValidation] Clean Winter Town Tileset erzeugt: {CleanTilesetPath}");
        }

        private static void CreateStructuredWorld(Transform parent)
        {
            EnsureCleanTilesetAndTiles();
            VisualAssetSet assets = LoadVisualAssets(AssetDatabase.LoadAssetAtPath<Sprite>(SquareSpritePath));
            GameObject gridObject = CreateChild("Grid", parent);
            Grid grid = gridObject.AddComponent<Grid>();
            grid.cellSize = Vector3.one;

            TilemapLayer groundBase = CreateTilemapLayer(gridObject.transform, "Ground_Base", 0, false);
            TilemapLayer groundDetails = CreateTilemapLayer(gridObject.transform, "Ground_Details", 2, false);
            TilemapLayer roads = CreateTilemapLayer(gridObject.transform, "Roads", 1, false);
            TilemapLayer buildings = CreateTilemapLayer(gridObject.transform, "Buildings", 5, false);
            TilemapLayer roofs = CreateTilemapLayer(gridObject.transform, "Roofs", 12, false);
            TilemapLayer nature = CreateTilemapLayer(gridObject.transform, "Nature", 6, false);
            TilemapLayer props = CreateTilemapLayer(gridObject.transform, "Props", 4, false);
            TilemapLayer interactables = CreateTilemapLayer(gridObject.transform, "Interactables", 8, false);
            TilemapLayer collision = CreateTilemapLayer(gridObject.transform, "Collision", 20, true);

            FillGround(groundBase.Tilemap, assets);
            PaintGroundDetails(groundDetails.Tilemap, assets);
            PaintRoads(roads.Tilemap, assets);
            PaintQuizZone(buildings.Tilemap, roofs.Tilemap, collision.Tilemap, assets);
            PaintInventoryZone(props.Tilemap, collision.Tilemap, assets);
            PaintSkillZone(props.Tilemap, interactables.Tilemap, collision.Tilemap, assets);
            PaintSouthExit(roads.Tilemap, props.Tilemap, collision.Tilemap, assets);
            PaintBoundary(nature.Tilemap, collision.Tilemap, assets);
            PaintNature(nature.Tilemap, assets);
            PaintInteractableMarkers(interactables.Tilemap, assets);

            PlayerSpawnPoint spawn = CreateChild("PlayerSpawn_Default", parent).AddComponent<PlayerSpawnPoint>();
            spawn.transform.position = PlayerSpawnPosition;

            Transform propsRoot = CreateChild("RuntimeInteractables", parent).transform;
            CreatePickup(propsRoot, assets.Phone, ItemPosition);
            CreateTerminal(propsRoot, assets.Terminal, TerminalPosition);
            CreateAchievementTerminal(propsRoot, assets.Marker, new Vector2Int(22, 16));
        }

        private static void FillGround(Tilemap tilemap, VisualAssetSet assets)
        {
            for (int x = 0; x < 40; x++)
            {
                for (int y = 0; y < 30; y++)
                {
                    TileBase tile = assets.GroundVariants[(x * 17 + y * 7) % assets.GroundVariants.Length];
                    tilemap.SetTile(new Vector3Int(x, y, 0), tile);
                }
            }
        }

        private static void PaintGroundDetails(Tilemap tilemap, VisualAssetSet assets)
        {
            Vector2Int[] details =
            {
                new Vector2Int(3, 4), new Vector2Int(6, 22), new Vector2Int(11, 7),
                new Vector2Int(14, 24), new Vector2Int(18, 6), new Vector2Int(24, 22),
                new Vector2Int(28, 8), new Vector2Int(31, 23), new Vector2Int(36, 5),
                new Vector2Int(37, 19), new Vector2Int(9, 25), new Vector2Int(26, 3)
            };

            for (int i = 0; i < details.Length; i++)
            {
                tilemap.SetTile(ToCell(details[i]), assets.GroundDetailTile);
            }
        }

        private static void PaintRoads(Tilemap tilemap, VisualAssetSet assets)
        {
            PaintRect(tilemap, new RectInt(15, 10, 11, 9), assets.PlazaTile);
            PaintRect(tilemap, new RectInt(0, 14, 40, 1), assets.RoadTile);
            PaintRect(tilemap, new RectInt(20, 0, 1, 30), assets.RoadTile);
            PaintRect(tilemap, new RectInt(18, 0, 5, 5), assets.RoadTile);

            PaintRoadEdges(tilemap, assets, new RectInt(15, 10, 11, 9));
            PaintRoadEdges(tilemap, assets, new RectInt(0, 14, 40, 1));
            PaintRoadEdges(tilemap, assets, new RectInt(20, 0, 1, 30));
        }

        private static void PaintRoadEdges(Tilemap tilemap, VisualAssetSet assets, RectInt rect)
        {
            if (assets.RoadEdgeTile == null)
            {
                return;
            }

            for (int x = rect.xMin; x < rect.xMax; x++)
            {
                SetIfEmpty(tilemap, new Vector3Int(x, rect.yMin - 1, 0), assets.RoadEdgeTile);
                SetIfEmpty(tilemap, new Vector3Int(x, rect.yMax, 0), assets.RoadEdgeTile);
            }

            for (int y = rect.yMin; y < rect.yMax; y++)
            {
                SetIfEmpty(tilemap, new Vector3Int(rect.xMin - 1, y, 0), assets.RoadEdgeTile);
                SetIfEmpty(tilemap, new Vector3Int(rect.xMax, y, 0), assets.RoadEdgeTile);
            }
        }

        private static void PaintQuizZone(Tilemap visual, Tilemap roofs, Tilemap collision, VisualAssetSet assets)
        {
            PaintRect(visual, new RectInt(17, 20, 7, 4), assets.BuildingWallTile);
            PaintRect(roofs, new RectInt(17, 24, 7, 3), assets.RoofTile);
            visual.SetTile(new Vector3Int(20, 20, 0), assets.DoorTile);

            PaintRect(collision, new RectInt(17, 20, 7, 7), assets.CollisionTile);
            ClearRect(collision, new RectInt(20, 20, 1, 1));
        }

        private static void PaintInventoryZone(Tilemap visual, Tilemap collision, VisualAssetSet assets)
        {
            Vector2Int[] crates =
            {
                new Vector2Int(5, 12), new Vector2Int(6, 12), new Vector2Int(9, 12),
                new Vector2Int(10, 13), new Vector2Int(5, 16), new Vector2Int(8, 17),
                new Vector2Int(10, 17)
            };

            for (int i = 0; i < crates.Length; i++)
            {
                visual.SetTile(ToCell(crates[i]), assets.CrateTile);
                collision.SetTile(ToCell(crates[i]), assets.CollisionTile);
            }
        }

        private static void PaintSkillZone(Tilemap visual, Tilemap interactables, Tilemap collision, VisualAssetSet assets)
        {
            Vector2Int[] props =
            {
                new Vector2Int(30, 12), new Vector2Int(31, 12), new Vector2Int(35, 12),
                new Vector2Int(30, 17), new Vector2Int(34, 17), new Vector2Int(35, 17),
                new Vector2Int(33, 15)
            };

            for (int i = 0; i < props.Length; i++)
            {
                visual.SetTile(ToCell(props[i]), assets.CrateTile);
            }

            interactables.SetTile(ToCell(TerminalPosition), assets.TerminalTile);
        }

        private static void PaintInteractableMarkers(Tilemap visual, VisualAssetSet assets)
        {
            visual.SetTile(ToCell(ItemPosition), assets.ItemMarkerTile);
            visual.SetTile(ToCell(new Vector2Int(20, 19)), assets.NpcQuizMarkerTile);
            visual.SetTile(ToCell(new Vector2Int(20, 8)), assets.NpcArthurMarkerTile);
            visual.SetTile(ToCell(new Vector2Int(20, 14)), assets.PlayerSpawnMarkerTile);
        }

        private static void PaintSouthExit(Tilemap roads, Tilemap props, Tilemap collision, VisualAssetSet assets)
        {
            PaintRect(roads, new RectInt(18, 0, 5, 5), assets.RoadTile);

            for (int x = 18; x <= 22; x++)
            {
                props.SetTile(new Vector3Int(x, 0, 0), assets.GateTile);
                collision.SetTile(new Vector3Int(x, 0, 0), assets.CollisionTile);
            }
        }

        private static void PaintBoundary(Tilemap visual, Tilemap collision, VisualAssetSet assets)
        {
            for (int x = 0; x < 40; x++)
            {
                TileBase tile = x % 3 == 0 ? assets.TreeTile : assets.FenceTile;
                visual.SetTile(new Vector3Int(x, 0, 0), tile);
                visual.SetTile(new Vector3Int(x, 29, 0), tile);
                collision.SetTile(new Vector3Int(x, 0, 0), assets.CollisionTile);
                collision.SetTile(new Vector3Int(x, 29, 0), assets.CollisionTile);
            }

            for (int y = 0; y < 30; y++)
            {
                TileBase tile = y % 4 == 0 ? assets.TreeTile : assets.FenceTile;
                visual.SetTile(new Vector3Int(0, y, 0), tile);
                visual.SetTile(new Vector3Int(39, y, 0), tile);
                collision.SetTile(new Vector3Int(0, y, 0), assets.CollisionTile);
                collision.SetTile(new Vector3Int(39, y, 0), assets.CollisionTile);
            }

            for (int x = 18; x <= 22; x++)
            {
                visual.SetTile(new Vector3Int(x, 0, 0), assets.GateTile);
            }
        }

        private static void PaintNature(Tilemap visual, VisualAssetSet assets)
        {
            Vector2Int[] trees =
            {
                new Vector2Int(2, 3), new Vector2Int(3, 8), new Vector2Int(4, 24),
                new Vector2Int(7, 27), new Vector2Int(12, 2), new Vector2Int(14, 27),
                new Vector2Int(25, 3), new Vector2Int(29, 26), new Vector2Int(33, 4),
                new Vector2Int(36, 9), new Vector2Int(37, 24), new Vector2Int(11, 22)
            };

            for (int i = 0; i < trees.Length; i++)
            {
                visual.SetTile(ToCell(trees[i]), assets.TreeTile);
            }

            Vector2Int[] flowers =
            {
                new Vector2Int(13, 11), new Vector2Int(14, 17), new Vector2Int(26, 11),
                new Vector2Int(27, 18), new Vector2Int(6, 18), new Vector2Int(34, 18)
            };

            for (int i = 0; i < flowers.Length; i++)
            {
                visual.SetTile(ToCell(flowers[i]), assets.FlowersTile);
            }
        }

        private static GameObject CreatePlayer(Transform parent, Sprite sprite, Transform uiRoot)
        {
            GameObject player = CreateChild("Player", parent);
            player.tag = "Player";
            player.transform.position = PlayerSpawnPosition;

            SpriteRenderer renderer = player.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingOrder = 10;

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
            GameObject arthur = CreateCharacterRoot("Arthur", parent, sprite, ArthurPosition);
            Animator animator = arthur.AddComponent<Animator>();
            animator.runtimeAnimatorController = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(
                "Assets/Projekt/Content/Art/Animations/Arthur/Arthur.controller");
            arthur.AddComponent<ArthurAnimationController>();
            CreateNameLabel(arthur.transform, "Arthur", new Color(0.72f, 0.88f, 1f));
        }

        private static void CreateBernd(Transform parent, Sprite sprite, Transform uiRoot)
        {
            GameObject bernd = CreateCharacterRoot("Bernd", parent, sprite, BerndPosition);
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
            cameraObject.transform.position = new Vector3(20f, 14f, -10f);
            Camera camera = cameraObject.AddComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = 8f;
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

        private static GameObject CreateCharacterRoot(string name, Transform parent, Sprite sprite, Vector3 position)
        {
            GameObject character = CreateChild(name, parent);
            character.transform.position = position;
            SpriteRenderer renderer = character.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingOrder = 10;

            Rigidbody2D rb = character.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Static;

            CapsuleCollider2D collider = character.AddComponent<CapsuleCollider2D>();
            collider.size = new Vector2(0.65f, 0.85f);
            collider.offset = new Vector2(0f, 0.35f);
            return character;
        }

        private static void CreatePickup(Transform parent, Sprite sprite, Vector2Int gridPosition)
        {
            GameObject pickup = CreateSpriteObject("TestItem_Diensthandy", parent, sprite, gridPosition, new Vector2(0.75f, 0.75f), 4, false);
            CircleCollider2D collider = pickup.AddComponent<CircleCollider2D>();
            collider.isTrigger = true;
            collider.radius = 0.8f;
            TestWorldInteractable interactable = pickup.AddComponent<TestWorldInteractable>();
            SetString(interactable, "interactionPrompt", "Diensthandy aufnehmen [E]");
            SetEnum(interactable, "interactionType", InteractionType.Pickup);
            SetEnumByName(interactable, "action", "PickupItem");
            SetBool(interactable, "disableAfterSuccessfulInteraction", true);
        }

        private static void CreateTerminal(Transform parent, Sprite sprite, Vector2Int gridPosition)
        {
            GameObject terminal = CreateSpriteObject("NetworkTerminal", parent, sprite, gridPosition, new Vector2(1.1f, 1.1f), 4, false);
            CircleCollider2D trigger = terminal.AddComponent<CircleCollider2D>();
            trigger.isTrigger = true;
            trigger.radius = 1.4f;
            TestWorldInteractable interactable = terminal.AddComponent<TestWorldInteractable>();
            SetString(interactable, "interactionPrompt", "Terminal benutzen [E]");
            SetEnum(interactable, "interactionType", InteractionType.Terminal);
            SetEnumByName(interactable, "action", "OpenTerminal");
        }

        private static void CreateAchievementTerminal(Transform parent, Sprite sprite, Vector2Int gridPosition)
        {
            GameObject marker = CreateSpriteObject("AchievementTrigger", parent, sprite, gridPosition, new Vector2(0.95f, 0.95f), 4, false);
            CircleCollider2D trigger = marker.AddComponent<CircleCollider2D>();
            trigger.isTrigger = true;
            trigger.radius = 1.1f;
            TestWorldInteractable interactable = marker.AddComponent<TestWorldInteractable>();
            SetString(interactable, "interactionPrompt", "Achievement testen [E]");
            SetEnum(interactable, "interactionType", InteractionType.Hint);
            SetEnumByName(interactable, "action", "UnlockAchievement");
            SetString(interactable, "achievementId", "first_quiz");
        }

        private static GameObject CreateSpriteObject(
            string name,
            Transform parent,
            Sprite sprite,
            Vector2Int gridPosition,
            Vector2 size,
            int sortingOrder,
            bool withCollider)
        {
            GameObject spriteObject = CreateChild(name, parent);
            spriteObject.transform.position = new Vector3(gridPosition.x, gridPosition.y, 0f);
            spriteObject.transform.localScale = new Vector3(size.x, size.y, 1f);
            SpriteRenderer renderer = spriteObject.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingOrder = sortingOrder;

            if (withCollider)
            {
                spriteObject.AddComponent<BoxCollider2D>();
            }

            return spriteObject;
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

        private static TilemapLayer CreateTilemapLayer(Transform parent, string name, int sortingOrder, bool withCollision)
        {
            GameObject tilemapObject = CreateChild(name, parent);
            Tilemap tilemap = tilemapObject.AddComponent<Tilemap>();
            TilemapRenderer renderer = tilemapObject.AddComponent<TilemapRenderer>();
            renderer.sortingOrder = sortingOrder;

            if (withCollision)
            {
                TilemapCollider2D tilemapCollider = tilemapObject.AddComponent<TilemapCollider2D>();
                tilemapCollider.compositeOperation = Collider2D.CompositeOperation.Merge;
                Rigidbody2D rb = tilemapObject.AddComponent<Rigidbody2D>();
                rb.bodyType = RigidbodyType2D.Static;
                tilemapObject.AddComponent<CompositeCollider2D>();
            }

            return new TilemapLayer(tilemap, renderer);
        }

        private static void PaintRect(Tilemap tilemap, RectInt rect, TileBase tile)
        {
            if (tile == null)
            {
                return;
            }

            for (int x = rect.xMin; x < rect.xMax; x++)
            {
                for (int y = rect.yMin; y < rect.yMax; y++)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), tile);
                }
            }
        }

        private static void ClearRect(Tilemap tilemap, RectInt rect)
        {
            for (int x = rect.xMin; x < rect.xMax; x++)
            {
                for (int y = rect.yMin; y < rect.yMax; y++)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), null);
                }
            }
        }

        private static void SetIfEmpty(Tilemap tilemap, Vector3Int position, TileBase tile)
        {
            if (position.x < 0 || position.x > 39 || position.y < 0 || position.y > 29 || tilemap.HasTile(position))
            {
                return;
            }

            tilemap.SetTile(position, tile);
        }

        private static Vector3Int ToCell(Vector2Int position)
        {
            return new Vector3Int(position.x, position.y, 0);
        }

        private static void EnsureCleanTilesetAndTiles()
        {
            TileSpec[] specs = CreateTileSpecs();
            EnsureFolder("Assets/Projekt/Content", "Tiles");
            EnsureFolder("Assets/Projekt/Content/Tiles", "TestWorld");
            EnsureCategoryFolders(specs);
            CreateCleanTilesetPng(specs);
            ConfigureCleanTilesetImporter(specs);
            CreateTileAssets(specs);
            WriteTileMapping(specs);
        }

        private static void CreateCleanTilesetPng(TileSpec[] specs)
        {
            int rows = Mathf.CeilToInt(specs.Length / (float)CleanTilesPerRow);
            Texture2D atlas = new Texture2D(CleanTilesPerRow * CleanTileSize, rows * CleanTileSize, TextureFormat.RGBA32, false);
            Color32[] transparent = new Color32[atlas.width * atlas.height];

            for (int i = 0; i < transparent.Length; i++)
            {
                transparent[i] = new Color32(0, 0, 0, 0);
            }

            atlas.SetPixels32(transparent);

            Texture2D sourceTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(WinterTownSourcePath);
            if (sourceTexture == null)
            {
                Debug.LogWarning($"[SceneValidation] Winter Town Tileset fehlt: {WinterTownSourcePath}");
                return;
            }

            TextureImporter sourceImporter = AssetImporter.GetAtPath(WinterTownSourcePath) as TextureImporter;
            bool wasReadable = sourceImporter != null && sourceImporter.isReadable;
            SetTextureReadable(WinterTownSourcePath, true);

            for (int i = 0; i < specs.Length; i++)
            {
                CopyTileSpecIntoAtlas(specs[i], sourceTexture, atlas, i);
            }

            atlas.Apply(false, false);
            File.WriteAllBytes(CleanTilesetPath, atlas.EncodeToPNG());
            Object.DestroyImmediate(atlas);
            AssetDatabase.ImportAsset(CleanTilesetPath, ImportAssetOptions.ForceUpdate);
            SetTextureReadable(WinterTownSourcePath, wasReadable);
        }

        private static void CopyTileSpecIntoAtlas(TileSpec spec, Texture2D sourceTexture, Texture2D atlas, int index)
        {
            int sourceWidth = CleanTileSize;
            int sourceHeight = CleanTileSize;
            int sourceX = spec.SourceX;
            int sourceY = sourceTexture.height - spec.SourceY - CleanTileSize;
            Color[] pixels = sourceTexture.GetPixels(sourceX, sourceY, sourceWidth, sourceHeight);

            int destinationX = (index % CleanTilesPerRow) * CleanTileSize;
            int destinationY = (index / CleanTilesPerRow) * CleanTileSize;

            for (int y = 0; y < CleanTileSize; y++)
            {
                int sampleY = Mathf.Clamp(Mathf.FloorToInt(y * sourceHeight / (float)CleanTileSize), 0, sourceHeight - 1);
                for (int x = 0; x < CleanTileSize; x++)
                {
                    int sampleX = Mathf.Clamp(Mathf.FloorToInt(x * sourceWidth / (float)CleanTileSize), 0, sourceWidth - 1);
                    atlas.SetPixel(destinationX + x, destinationY + y, pixels[sampleY * sourceWidth + sampleX]);
                }
            }
        }

        private static void ConfigureCleanTilesetImporter(TileSpec[] specs)
        {
            AssetDatabase.ImportAsset(CleanTilesetPath, ImportAssetOptions.ForceUpdate);
            TextureImporter importer = AssetImporter.GetAtPath(CleanTilesetPath) as TextureImporter;
            if (importer == null)
            {
                return;
            }

            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Multiple;
            importer.spritePixelsPerUnit = CleanTileSize;
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.mipmapEnabled = false;

#pragma warning disable 0618
            SpriteMetaData[] metadata = new SpriteMetaData[specs.Length];
            for (int i = 0; i < specs.Length; i++)
            {
                metadata[i] = new SpriteMetaData
                {
                    name = specs[i].TileName,
                    rect = new Rect(
                        (i % CleanTilesPerRow) * CleanTileSize,
                        (i / CleanTilesPerRow) * CleanTileSize,
                        CleanTileSize,
                        CleanTileSize),
                    alignment = (int)SpriteAlignment.Center,
                    pivot = new Vector2(0.5f, 0.5f)
                };
            }

            importer.spritesheet = metadata;
#pragma warning restore 0618
            importer.SaveAndReimport();
        }

        private static void CreateTileAssets(TileSpec[] specs)
        {
            Object[] sprites = AssetDatabase.LoadAllAssetsAtPath(CleanTilesetPath);
            Dictionary<string, Sprite> spritesByName = new Dictionary<string, Sprite>();
            for (int i = 0; i < sprites.Length; i++)
            {
                if (sprites[i] is Sprite sprite)
                {
                    spritesByName[sprite.name] = sprite;
                }
            }

            for (int i = 0; i < specs.Length; i++)
            {
                TileSpec spec = specs[i];
                string path = GetMappedTilePath(spec.TileName);
                Tile tile = AssetDatabase.LoadAssetAtPath<Tile>(path);
                if (tile == null)
                {
                    tile = ScriptableObject.CreateInstance<Tile>();
                    AssetDatabase.CreateAsset(tile, path);
                }

                spritesByName.TryGetValue(spec.TileName, out Sprite sprite);
                tile.sprite = sprite;
                tile.colliderType = spec.Collision ? Tile.ColliderType.Sprite : Tile.ColliderType.None;
                EditorUtility.SetDirty(tile);
            }
        }

        private static void WriteTileMapping(TileSpec[] specs)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("{");

            for (int i = 0; i < specs.Length; i++)
            {
                TileSpec spec = specs[i];
                builder.AppendLine($"  \"{spec.TileName}\": {{");
                builder.AppendLine($"    \"category\": \"{spec.Category}\",");
                builder.AppendLine($"    \"spriteName\": \"{spec.TileName}\",");
                builder.AppendLine($"    \"sourceSpriteName\": \"{spec.SourceSpriteName}\",");
                builder.AppendLine($"    \"collision\": {spec.Collision.ToString().ToLowerInvariant()},");
                builder.AppendLine($"    \"sortingOrder\": {spec.SortingOrder},");
                builder.AppendLine($"    \"usage\": \"{spec.Usage}\"");
                builder.Append(i < specs.Length - 1 ? "  },\n" : "  }\n");
            }

            builder.AppendLine("}");
            File.WriteAllText(TileMappingPath, builder.ToString());
            AssetDatabase.ImportAsset(TileMappingPath, ImportAssetOptions.ForceUpdate);
        }

        private static void SetTextureReadable(string path, bool readable)
        {
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer == null || importer.isReadable == readable)
            {
                return;
            }

            importer.isReadable = readable;
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.SaveAndReimport();
        }

        private static void EnsureCategoryFolders(TileSpec[] specs)
        {
            HashSet<string> categories = new HashSet<string>();
            for (int i = 0; i < specs.Length; i++)
            {
                categories.Add(specs[i].Category);
            }

            foreach (string category in categories)
            {
                EnsureFolder(TestWorldTilesRoot, category);
            }
        }

        private static void EnsureFolder(string parent, string child)
        {
            string path = parent + "/" + child;
            if (!AssetDatabase.IsValidFolder(path))
            {
                AssetDatabase.CreateFolder(parent, child);
            }
        }

        private static VisualAssetSet LoadVisualAssets(Sprite fallbackSprite)
        {
            return new VisualAssetSet
            {
                FallbackSprite = fallbackSprite,
                GroundVariants = new[] { LoadMappedTile("ground_snow_01"), LoadMappedTile("ground_snow_02"), LoadMappedTile("ground_snow_detail_01") },
                GroundDetailTile = LoadMappedTile("snow_shadow_01"),
                RoadTile = LoadMappedTile("road_cobble_h"),
                RoadEdgeTile = LoadMappedTile("road_cobble_edge_top"),
                PlazaTile = LoadMappedTile("road_cobble_center"),
                BuildingWallTile = LoadMappedTile("building_wall_stone_01"),
                RoofTile = LoadMappedTile("roof_blue_center"),
                DoorTile = LoadMappedTile("building_door_wood"),
                TreeTile = LoadMappedTile("tree_pine_01"),
                FenceTile = LoadMappedTile("fence_wood_h"),
                CrateTile = LoadMappedTile("crate_01"),
                TerminalTile = LoadMappedTile("terminal_01"),
                GateTile = LoadMappedTile("fence_wood_gate"),
                FlowersTile = LoadMappedTile("flower_snow_01"),
                CollisionTile = LoadMappedTile("wall_stone_h"),
                ItemMarkerTile = LoadMappedTile("item_marker_01"),
                NpcQuizMarkerTile = LoadMappedTile("npc_marker_quiz"),
                NpcArthurMarkerTile = LoadMappedTile("npc_marker_arthur"),
                PlayerSpawnMarkerTile = LoadMappedTile("player_spawn_marker"),
                Phone = LoadSpriteFromCleanTileset("item_marker_01", fallbackSprite),
                Terminal = LoadSpriteFromCleanTileset("terminal_01", fallbackSprite),
                Marker = LoadSpriteFromCleanTileset("npc_marker_quiz", fallbackSprite)
            }.NormalizeFallbacks();
        }

        private static TileBase LoadMappedTile(string tileName)
        {
            return AssetDatabase.LoadAssetAtPath<TileBase>(GetMappedTilePath(tileName));
        }

        private static string GetMappedTilePath(string tileName)
        {
            TileSpec spec = FindTileSpec(tileName);
            string category = spec != null ? spec.Category : "Details";
            return $"{TestWorldTilesRoot}/{category}/{tileName}.asset";
        }

        private static TileBase LoadTile(string tileName)
        {
            return AssetDatabase.LoadAssetAtPath<TileBase>($"{TilesFolder}/{tileName}.asset");
        }

        private static Sprite LoadSpriteFromCleanTileset(string spriteName, Sprite fallbackSprite)
        {
            Object[] assets = AssetDatabase.LoadAllAssetsAtPath(CleanTilesetPath);
            for (int i = 0; i < assets.Length; i++)
            {
                if (assets[i] is Sprite sprite && sprite.name == spriteName)
                {
                    return sprite;
                }
            }

            return fallbackSprite;
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

        private static void RepositionSceneActors()
        {
            GameObject player = FindObjectByName("Player");
            if (player != null)
            {
                player.transform.position = PlayerSpawnPosition;
            }

            GameObject bernd = FindObjectByName("Bernd");
            if (bernd != null)
            {
                bernd.transform.position = BerndPosition;
            }

            GameObject arthur = FindObjectByName("Arthur");
            if (arthur != null)
            {
                arthur.transform.position = ArthurPosition;
            }

            GameObject cameraObject = FindObjectByName("Main Camera");
            if (cameraObject != null)
            {
                cameraObject.transform.position = new Vector3(20f, 14f, -10f);
                SimpleCameraFollow2D follow = cameraObject.GetComponent<SimpleCameraFollow2D>();
                if (follow != null && player != null)
                {
                    follow.SetTarget(player.transform);
                }
            }

            GameSceneBootstrap bootstrap = Object.FindAnyObjectByType<GameSceneBootstrap>(FindObjectsInactive.Include);
            if (bootstrap != null && player != null)
            {
                SetObjectReference(bootstrap, "player", player.transform);
            }
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

        private static TileSpec FindTileSpec(string tileName)
        {
            TileSpec[] specs = CreateTileSpecs();
            for (int i = 0; i < specs.Length; i++)
            {
                if (specs[i].TileName == tileName)
                {
                    return specs[i];
                }
            }

            return null;
        }

        private static TileSpec[] CreateTileSpecs()
        {
            return new[]
            {
                new TileSpec("ground_snow_01", "Ground", 16, 68, false, 0, "Base snow ground"),
                new TileSpec("ground_snow_02", "Ground", 68, 68, false, 0, "Alternative snow ground"),
                new TileSpec("ground_snow_detail_01", "Ground", 16, 124, false, 2, "Snow variation ground detail"),
                new TileSpec("ground_ice_01", "Ground", 16, 180, false, 0, "Ice-like ground alternative"),
                new TileSpec("road_cobble_center", "Roads", 16, 267, false, 1, "Central plaza cobble"),
                new TileSpec("road_cobble_h", "Roads", 68, 267, false, 1, "Horizontal main road"),
                new TileSpec("road_cobble_v", "Roads", 120, 267, false, 1, "Vertical main road"),
                new TileSpec("road_cobble_cross", "Roads", 172, 267, false, 1, "Road crossing"),
                new TileSpec("road_cobble_corner_tl", "Roads", 224, 267, false, 1, "Cobble corner top left"),
                new TileSpec("road_cobble_corner_tr", "Roads", 276, 267, false, 1, "Cobble corner top right"),
                new TileSpec("road_cobble_corner_bl", "Roads", 328, 267, false, 1, "Cobble corner bottom left"),
                new TileSpec("road_cobble_corner_br", "Roads", 412, 267, false, 1, "Cobble corner bottom right"),
                new TileSpec("road_cobble_edge_top", "Roads", 16, 324, false, 1, "Road edge top"),
                new TileSpec("road_cobble_edge_bottom", "Roads", 68, 324, false, 1, "Road edge bottom"),
                new TileSpec("road_cobble_edge_left", "Roads", 120, 324, false, 1, "Road edge left"),
                new TileSpec("road_cobble_edge_right", "Roads", 172, 324, false, 1, "Road edge right"),
                new TileSpec("wall_stone_h", "Walls", 16, 413, true, 6, "Blocking horizontal wall"),
                new TileSpec("wall_stone_v", "Walls", 120, 413, true, 6, "Blocking vertical wall"),
                new TileSpec("wall_stone_corner", "Walls", 276, 413, true, 6, "Blocking wall corner"),
                new TileSpec("fence_wood_h", "Walls", 328, 413, true, 6, "Blocking fence horizontal"),
                new TileSpec("fence_wood_v", "Walls", 432, 413, true, 6, "Blocking fence vertical"),
                new TileSpec("fence_wood_gate", "Walls", 224, 465, true, 6, "Blocked south gate marker"),
                new TileSpec("building_wall_wood_01", "Buildings", 16, 536, true, 5, "Wood building wall"),
                new TileSpec("building_wall_stone_01", "Buildings", 224, 536, true, 5, "Stone building wall"),
                new TileSpec("building_window_blue", "Buildings", 16, 588, true, 5, "Blue window"),
                new TileSpec("building_door_wood", "Buildings", 120, 588, false, 5, "Wood building door visual"),
                new TileSpec("building_door_stone", "Buildings", 224, 588, false, 5, "Stone building door visual"),
                new TileSpec("roof_blue_center", "Roofs", 502, 68, true, 12, "Blue roof center"),
                new TileSpec("roof_blue_edge_top", "Roofs", 554, 68, true, 12, "Blue roof top edge"),
                new TileSpec("roof_blue_edge_bottom", "Roofs", 606, 68, true, 12, "Blue roof bottom edge"),
                new TileSpec("roof_blue_corner_tl", "Roofs", 658, 68, true, 12, "Blue roof corner top left"),
                new TileSpec("roof_blue_corner_tr", "Roofs", 710, 68, true, 12, "Blue roof corner top right"),
                new TileSpec("roof_orange_center", "Roofs", 502, 179, true, 12, "Orange roof center"),
                new TileSpec("roof_purple_center", "Roofs", 762, 179, true, 12, "Purple roof center"),
                new TileSpec("tree_pine_01", "Nature", 502, 326, true, 6, "Snow pine tree"),
                new TileSpec("tree_pine_02", "Nature", 554, 326, true, 6, "Alternative snow pine tree"),
                new TileSpec("bush_snow_01", "Nature", 502, 431, false, 6, "Snow bush"),
                new TileSpec("flower_snow_01", "Nature", 814, 327, false, 6, "Small snow flower decoration"),
                new TileSpec("rock_snow_01", "Nature", 710, 326, true, 6, "Snow rock"),
                new TileSpec("crate_01", "Props", 502, 477, true, 4, "Inventory zone crate"),
                new TileSpec("barrel_01", "Props", 659, 477, true, 4, "Barrel prop"),
                new TileSpec("lamp_post_01", "Props", 918, 477, true, 4, "Lamp post"),
                new TileSpec("bench_01", "Props", 502, 590, true, 4, "Bench prop"),
                new TileSpec("sign_01", "Props", 606, 536, false, 4, "Sign prop"),
                new TileSpec("fountain_01", "Props", 814, 588, true, 4, "Fountain prop"),
                new TileSpec("terminal_01", "Interactables", 1104, 73, false, 8, "Skill terminal visual"),
                new TileSpec("notice_board_01", "Interactables", 1190, 73, false, 8, "Notice board visual"),
                new TileSpec("item_marker_01", "Interactables", 1280, 134, false, 8, "Inventory item marker visual"),
                new TileSpec("npc_marker_quiz", "Interactables", 1104, 134, false, 8, "Quiz NPC marker visual"),
                new TileSpec("npc_marker_arthur", "Interactables", 1156, 134, false, 8, "Arthur marker visual"),
                new TileSpec("player_spawn_marker", "Interactables", 1208, 134, false, 8, "Player spawn marker visual"),
                new TileSpec("water_ice_01", "Water", 16, 735, false, 0, "Ice water"),
                new TileSpec("water_edge_01", "Water", 68, 735, false, 0, "Water edge"),
                new TileSpec("waterfall_01", "Water", 276, 735, false, 0, "Waterfall"),
                new TileSpec("snow_shadow_01", "Details", 1104, 520, false, 2, "Snow shadow detail"),
                new TileSpec("snow_edge_01", "Details", 1156, 520, false, 2, "Snow edge detail"),
                new TileSpec("footprints_01", "Details", 1208, 520, false, 2, "Footprints"),
                new TileSpec("small_stone_01", "Details", 1416, 646, false, 2, "Small stone detail")
            };
        }

        private readonly struct TilemapLayer
        {
            public TilemapLayer(Tilemap tilemap, TilemapRenderer renderer)
            {
                Tilemap = tilemap;
                Renderer = renderer;
            }

            public Tilemap Tilemap { get; }
            public TilemapRenderer Renderer { get; }
        }

        private sealed class TileSpec
        {
            public TileSpec(string tileName, string category, int sourceX, int sourceY, bool collision, int sortingOrder, string usage)
            {
                TileName = tileName;
                Category = category;
                SourceX = sourceX;
                SourceY = sourceY;
                SourceSpriteName = $"winter_town_tileset[{sourceX},{sourceY}]";
                Collision = collision;
                SortingOrder = sortingOrder;
                Usage = usage;
            }

            public string TileName { get; }
            public string Category { get; }
            public string SourceSpriteName { get; }
            public int SourceX { get; }
            public int SourceY { get; }
            public bool Collision { get; }
            public int SortingOrder { get; }
            public string Usage { get; }
        }

        private sealed class VisualAssetSet
        {
            public Sprite FallbackSprite;
            public TileBase[] GroundVariants;
            public TileBase GroundDetailTile;
            public TileBase RoadTile;
            public TileBase RoadEdgeTile;
            public TileBase PlazaTile;
            public TileBase BuildingWallTile;
            public TileBase RoofTile;
            public TileBase DoorTile;
            public TileBase TreeTile;
            public TileBase FenceTile;
            public TileBase CrateTile;
            public TileBase TerminalTile;
            public TileBase GateTile;
            public TileBase FlowersTile;
            public TileBase CollisionTile;
            public TileBase ItemMarkerTile;
            public TileBase NpcQuizMarkerTile;
            public TileBase NpcArthurMarkerTile;
            public TileBase PlayerSpawnMarkerTile;
            public Sprite Phone;
            public Sprite Terminal;
            public Sprite Marker;

            public VisualAssetSet NormalizeFallbacks()
            {
                TileBase fallbackTile = FirstValidTile(GroundVariants) ??
                    RoadTile ?? PlazaTile ?? FenceTile ?? CollisionTile;

                GroundVariants = NormalizeTileArray(GroundVariants, fallbackTile);
                GroundDetailTile ??= fallbackTile;
                RoadTile ??= fallbackTile;
                RoadEdgeTile ??= RoadTile;
                PlazaTile ??= RoadTile;
                BuildingWallTile ??= fallbackTile;
                RoofTile ??= BuildingWallTile;
                DoorTile ??= BuildingWallTile;
                TreeTile ??= FenceTile ?? fallbackTile;
                FenceTile ??= TreeTile ?? fallbackTile;
                CrateTile ??= FenceTile ?? fallbackTile;
                TerminalTile ??= CrateTile ?? fallbackTile;
                GateTile ??= FenceTile ?? fallbackTile;
                FlowersTile ??= GroundDetailTile ?? fallbackTile;
                CollisionTile ??= FenceTile ?? fallbackTile;
                ItemMarkerTile ??= TerminalTile ?? fallbackTile;
                NpcQuizMarkerTile ??= ItemMarkerTile;
                NpcArthurMarkerTile ??= ItemMarkerTile;
                PlayerSpawnMarkerTile ??= ItemMarkerTile;
                Phone ??= FallbackSprite;
                Terminal ??= FallbackSprite;
                Marker ??= FallbackSprite;
                return this;
            }

            private static TileBase FirstValidTile(TileBase[] tiles)
            {
                if (tiles == null)
                {
                    return null;
                }

                for (int i = 0; i < tiles.Length; i++)
                {
                    if (tiles[i] != null)
                    {
                        return tiles[i];
                    }
                }

                return null;
            }

            private static TileBase[] NormalizeTileArray(TileBase[] tiles, TileBase fallback)
            {
                if (tiles == null || tiles.Length == 0)
                {
                    return new[] { fallback };
                }

                for (int i = 0; i < tiles.Length; i++)
                {
                    tiles[i] ??= fallback;
                }

                return tiles;
            }
        }
    }
}
#endif
