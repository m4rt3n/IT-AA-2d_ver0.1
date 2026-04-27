/*
 * Datei: DevelopmentLevelBuilder.cs
 * Zweck: Erzeugt die DevelopmentLevel-Testscene reproduzierbar ueber den Unity Editor.
 * Verantwortung: Baut Szene, Hierarchie, Testobjekte, Manager, NPCs und UI aus vorhandenen Systemen auf und protokolliert fehlende Referenzen.
 * Abhaengigkeiten: UnityEditor, Unity SceneManagement, ITAA Runtime-Systeme, TextMeshPro, Unity UI, vorhandene Content-Assets.
 * Verwendung: Im Editor ueber Tools/IT-AA/Build Development Level ausfuehren oder per Batchmode ExecuteMethod starten.
 */

using System.IO;
using System.Linq;
using ITAA.Core.SceneManagement;
using ITAA.DevTools;
using ITAA.Features.Achievements;
using ITAA.Features.Inventory;
using ITAA.Features.Progress;
using ITAA.Features.Scenarios;
using ITAA.Features.Skills;
using ITAA.NPC.Arthur;
using ITAA.NPC.Bernd;
using ITAA.NPC.Routines;
using ITAA.Player.Movement;
using ITAA.Player.Session;
using ITAA.Player.UI;
using ITAA.Quiz;
using ITAA.System.Savegame;
using ITAA.System.Settings;
using ITAA.UI.Items;
using ITAA.UI.Managers;
using ITAA.UI.Panels;
using TMPro;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ITAA.Editor.SceneBuilding
{
    public static class DevelopmentLevelBuilder
    {
        private const string ScenePath = "Assets/Projekt/Content/Scenes/DevelopmentLevel.unity";
        private const string SceneFolder = "Assets/Projekt/Content/Scenes";
        private const string SaveSlotItemPrefabPath = "Assets/Projekt/Content/Prefabs/UI/SaveSlotItem.prefab";
        private const string FallbackSaveSlotItemPrefabPath = "Assets/Projekt/Content/Prefabs/SaveSlotEntry.prefab";
        private const string GroundPrefabPath = "Assets/Projekt/Content/Art/Tiles/Ground.prefab";
        private const string PlayerSpritePath = "Assets/Projekt/Content/Art/Player/character.png";
        private const string ArthurSpritePath = "Assets/Projekt/Content/Art/Player/character blue.png";
        private const string BerndSpritePath = "Assets/Projekt/Content/Art/Player/character green.png";
        private const string PlayerAnimatorPath = "Assets/Projekt/Content/Art/Animations/Player/Player.controller";
        private const string ArthurAnimatorPath = "Assets/Projekt/Content/Art/Animations/Arthur/Arthur.controller";
        private const string BerndAnimatorPath = "Assets/Projekt/Content/Art/Animations/Bernd/Bernd.controller";
        private const string BerndQuizPath = "Assets/Projekt/Content/Quiz/BerndIntroQuiz.asset";

        [MenuItem("Tools/IT-AA/Build Development Level")]
        public static void BuildDevelopmentLevel()
        {
            EnsureFolder(SceneFolder);

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            scene.name = SceneNames.DevelopmentLevel;

            GameObject sceneRoot = new GameObject(SceneNames.DevelopmentLevel);
            GameObject gameSystems = CreateChild(sceneRoot.transform, "GameSystems");
            GameObject player = BuildPlayer(sceneRoot.transform);
            GameObject npcRoot = CreateChild(sceneRoot.transform, "NPC");
            GameObject mapRoot = CreateChild(sceneRoot.transform, "Map");
            GameObject uiRoot = CreateChild(sceneRoot.transform, "UI");

            BuildCamera();
            BuildGameSystems(gameSystems.transform);
            BuildMap(mapRoot.transform);

            MenuManager menuManager = BuildMenu(uiRoot.transform, out LoadGamePanel loadGamePanel);
            QuizPanel quizPanel = BuildQuizPanel(uiRoot.transform);
            BuildDevPanel(uiRoot.transform, loadGamePanel, quizPanel);

            BuildArthur(npcRoot.transform, player.transform, menuManager);
            BuildBernd(npcRoot.transform, player.transform, quizPanel);

            BuildEventSystem(sceneRoot.transform);

            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"[{nameof(DevelopmentLevelBuilder)}] DevelopmentLevel erstellt: {ScenePath}");
        }

        private static void BuildGameSystems(Transform parent)
        {
            GameObject bootstrapObject = CreateChild(parent, nameof(GameSystemsBootstrap));
            bootstrapObject.AddComponent<GameSystemsBootstrap>();

            CreateSystemChild<AchievementManager>(parent, nameof(AchievementManager));
            CreateSystemChild<SkillRuntimeManager>(parent, nameof(SkillRuntimeManager));
            CreateSystemChild<SettingsManager>(parent, nameof(SettingsManager));
            CreateSystemChild<SavegameRuntimeSession>(parent, nameof(SavegameRuntimeSession));
            RuntimeInventory runtimeInventory = CreateSystemChild<RuntimeInventory>(parent, nameof(RuntimeInventory));
            ToolbeltController toolbeltController = CreateSystemChild<ToolbeltController>(parent, nameof(ToolbeltController));
            SetSerializedReference(toolbeltController, "inventory", runtimeInventory);
            CreateSystemChild<ProgressManager>(parent, nameof(ProgressManager));
            CreateSystemChild<QuizProgressReporter>(parent, nameof(QuizProgressReporter));
            CreateSystemChild<ScenarioManager>(parent, nameof(ScenarioManager));
            CreateSystemChild<SettingsHotkeyController>(parent, nameof(SettingsHotkeyController));
            CreateSystemChild<DevPanelBootstrap>(parent, nameof(DevPanelBootstrap));
        }

        private static GameObject BuildPlayer(Transform parent)
        {
            GameObject player = CreateChild(parent, "Player");
            player.tag = "Player";
            player.transform.position = new Vector3(0f, 0f, 0f);

            SpriteRenderer renderer = player.AddComponent<SpriteRenderer>();
            renderer.sprite = LoadFirstSprite(PlayerSpritePath, "Player");
            renderer.sortingOrder = 10;

            Animator animator = player.AddComponent<Animator>();
            animator.runtimeAnimatorController = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(PlayerAnimatorPath);
            LogMissing(animator.runtimeAnimatorController, PlayerAnimatorPath);

            Rigidbody2D rb = player.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            BoxCollider2D collider = player.AddComponent<BoxCollider2D>();
            collider.size = new Vector2(0.7f, 0.9f);
            collider.offset = new Vector2(0f, 0.45f);

            player.AddComponent<PlayerInputReader>();
            player.AddComponent<PlayerMotor2D>();
            player.AddComponent<PlayerController>();
            player.AddComponent<PlayerSession>();

            BuildWorldNameTag(player.transform, "PlayerNameTagCanvas", "Spieler", typeof(PlayerNameTag), player.transform, null);
            return player;
        }

        private static void BuildArthur(Transform parent, Transform player, MenuManager menuManager)
        {
            GameObject arthur = CreateNpcRoot(parent, "Arthur", ArthurSpritePath, ArthurAnimatorPath, new Vector3(-3f, 0.75f, 0f));
            ArthurAnimationController animationController = arthur.AddComponent<ArthurAnimationController>();
            ArthurMovementToPlayer movement = arthur.AddComponent<ArthurMovementToPlayer>();
            ArthurAutoInteraction interaction = arthur.AddComponent<ArthurAutoInteraction>();
            arthur.AddComponent<NpcRoutineController>();

            SetSerializedReference(movement, "animationController", animationController);
            SetSerializedReference(interaction, "movementToPlayer", movement);
            SetSerializedReference(interaction, "animationController", animationController);
            SetSerializedReference(interaction, "menuManager", menuManager);

            BoxCollider2D trigger = arthur.GetComponent<BoxCollider2D>();
            trigger.size = new Vector2(3.2f, 2.2f);
            trigger.isTrigger = true;

            BuildWorldNameTag(arthur.transform, "NameTagCanvas", "Arthur", typeof(ArthurNameTag), arthur.transform, player);
        }

        private static void BuildBernd(Transform parent, Transform player, QuizPanel quizPanel)
        {
            GameObject bernd = CreateNpcRoot(parent, "Bernd", BerndSpritePath, BerndAnimatorPath, new Vector3(3f, 0.75f, 0f));
            BerndAnimationController animationController = bernd.AddComponent<BerndAnimationController>();
            BerndDetectionZone detectionZone = bernd.AddComponent<BerndDetectionZone>();
            BerndMovementToPlayer movement = bernd.AddComponent<BerndMovementToPlayer>();
            BerndQuizStarter quizStarter = bernd.AddComponent<BerndQuizStarter>();
            BerndAutoInteraction interaction = bernd.AddComponent<BerndAutoInteraction>();
            BerndInteractableAdapter adapter = bernd.AddComponent<BerndInteractableAdapter>();

            QuizSet quizSet = AssetDatabase.LoadAssetAtPath<QuizSet>(BerndQuizPath);
            LogMissing(quizSet, BerndQuizPath);

            SetSerializedReference(movement, "detectionZone", detectionZone);
            SetSerializedReference(movement, "animationController", animationController);
            SetSerializedReference(quizStarter, "quizPanel", quizPanel);
            SetSerializedReference(quizStarter, "quizSet", quizSet);
            SetSerializedReference(interaction, "detectionZone", detectionZone);
            SetSerializedReference(interaction, "movementToPlayer", movement);
            SetSerializedReference(interaction, "animationController", animationController);
            SetSerializedReference(interaction, "quizStarter", quizStarter);
            SetSerializedReference(adapter, "autoInteraction", interaction);
            SetSerializedReference(adapter, "quizStarter", quizStarter);
            SetSerializedReference(adapter, "detectionZone", detectionZone);

            BoxCollider2D trigger = bernd.GetComponent<BoxCollider2D>();
            trigger.size = new Vector2(3.2f, 2.2f);
            trigger.isTrigger = true;

            BuildWorldNameTag(bernd.transform, "NameTagCanvas", "Bernd", typeof(BerndNameTag), bernd.transform, player);
        }

        private static GameObject CreateNpcRoot(Transform parent, string name, string spritePath, string animatorPath, Vector3 position)
        {
            GameObject npc = CreateChild(parent, name);
            npc.transform.position = position;

            SpriteRenderer renderer = npc.AddComponent<SpriteRenderer>();
            renderer.sprite = LoadFirstSprite(spritePath, name);
            renderer.sortingOrder = 9;

            Animator animator = npc.AddComponent<Animator>();
            animator.runtimeAnimatorController = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(animatorPath);
            LogMissing(animator.runtimeAnimatorController, animatorPath);

            Rigidbody2D rb = npc.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            BoxCollider2D collider = npc.AddComponent<BoxCollider2D>();
            collider.size = new Vector2(0.7f, 0.9f);
            collider.offset = new Vector2(0f, 0.45f);
            collider.isTrigger = true;

            return npc;
        }

        private static void BuildMap(Transform parent)
        {
            GameObject groundPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(GroundPrefabPath);
            if (groundPrefab == null)
            {
                CreateChild(parent, "TODO_Map_GroundPrefabMissing");
                Debug.LogWarning($"[{nameof(DevelopmentLevelBuilder)}] Ground-Prefab fehlt: {GroundPrefabPath}");
                return;
            }

            GameObject ground = PrefabUtility.InstantiatePrefab(groundPrefab, parent) as GameObject;
            if (ground == null)
            {
                CreateChild(parent, "TODO_Map_GroundInstantiateFailed");
                return;
            }

            ground.name = "Ground";
            ground.transform.position = Vector3.zero;
            ground.transform.localScale = new Vector3(18f, 10f, 1f);
        }

        private static MenuManager BuildMenu(Transform parent, out LoadGamePanel loadGamePanel)
        {
            GameObject menuObject = CreateUiCanvas(parent, "MenuManager", 1000);
            MenuManager menuManager = menuObject.AddComponent<MenuManager>();

            GameObject backgroundDim = CreatePanel(menuObject.transform, "BackgroundDim", new Color(0f, 0f, 0f, 0.52f));
            GameObject menuPanel = CreateCenteredPanel(backgroundDim.transform, "MenuPanel", new Vector2(760f, 620f), new Color(0.1f, 0.12f, 0.16f, 0.96f));
            GameObject startPanel = CreateChild(menuPanel.transform, "StartMenuPanel", typeof(RectTransform));
            Stretch(startPanel.GetComponent<RectTransform>());

            TMP_Text title = CreateText(startPanel.transform, "TitleText", "Development Level", 34f, FontStyles.Bold);
            SetRect(title.rectTransform, 0f, 1f, 1f, 1f, 0f, -42f, -64f, 48f);

            Button loadButton = CreateButton(startPanel.transform, "LoadGameButton", "Load Game");
            SetRect(loadButton.GetComponent<RectTransform>(), 0.5f, 0.5f, 0.5f, 0.5f, 0f, 32f, 280f, 54f);

            Button closeButton = CreateButton(startPanel.transform, "CloseMenuButton", "Close");
            SetRect(closeButton.GetComponent<RectTransform>(), 0.5f, 0.5f, 0.5f, 0.5f, 0f, -38f, 280f, 54f);

            loadGamePanel = BuildLoadGamePanel(menuPanel.transform);

            UnityEventTools.AddPersistentListener(loadButton.onClick, menuManager.ShowLoadGamePanel);
            UnityEventTools.AddPersistentListener(closeButton.onClick, menuManager.HideAllImmediate);

            SetSerializedReference(menuManager, "canvasRoot", menuObject);
            SetSerializedReference(menuManager, "backgroundDim", backgroundDim);
            SetSerializedReference(menuManager, "menuPanel", menuPanel);
            SetSerializedReference(menuManager, "startMenuPanel", startPanel);
            SetSerializedReference(menuManager, "loadGamePanel", loadGamePanel);
            SetSerializedBool(menuManager, "openStartMenuOnStart", false);

            backgroundDim.SetActive(false);
            loadGamePanel.gameObject.SetActive(false);
            return menuManager;
        }

        private static LoadGamePanel BuildLoadGamePanel(Transform parent)
        {
            GameObject panel = CreateCenteredPanel(parent, "LoadGamePanel", new Vector2(680f, 500f), new Color(0.14f, 0.16f, 0.21f, 0.98f));
            LoadGamePanel loadGamePanel = panel.AddComponent<LoadGamePanel>();

            TMP_Text header = CreateText(panel.transform, "HeaderTitleText", "Spieler", 30f, FontStyles.Bold);
            SetRect(header.rectTransform, 0f, 1f, 1f, 1f, 0f, -30f, -48f, 38f);

            SaveSlotItemUI slotItem = BuildSaveSlotItem(panel.transform);
            SetRect(slotItem.GetComponent<RectTransform>(), 0.5f, 0.5f, 0.5f, 0.5f, 0f, 20f, 500f, 250f);

            Button previous = CreateButton(panel.transform, "PreviousButton", "<");
            SetRect(previous.GetComponent<RectTransform>(), 0f, 0.5f, 0f, 0.5f, 58f, 20f, 54f, 54f);

            Button next = CreateButton(panel.transform, "NextButton", ">");
            SetRect(next.GetComponent<RectTransform>(), 1f, 0.5f, 1f, 0.5f, -58f, 20f, 54f, 54f);

            TMP_Text page = CreateText(panel.transform, "PageIndicatorText", "1 / 3", 20f, FontStyles.Normal);
            SetRect(page.rectTransform, 0.5f, 0f, 0.5f, 0f, 0f, 82f, 120f, 32f);

            Button close = CreateButton(panel.transform, "CloseButton", "Close");
            SetRect(close.GetComponent<RectTransform>(), 0.5f, 0f, 0.5f, 0f, 0f, 36f, 160f, 44f);

            SetSerializedReference(loadGamePanel, "largeSlotItem", slotItem);
            SetSerializedReference(loadGamePanel, "previousButton", previous);
            SetSerializedReference(loadGamePanel, "nextButton", next);
            SetSerializedReference(loadGamePanel, "closeButton", close);
            SetSerializedReference(loadGamePanel, "headerTitleText", header);
            SetSerializedReference(loadGamePanel, "pageIndicatorText", page);
            return loadGamePanel;
        }

        private static SaveSlotItemUI BuildSaveSlotItem(Transform parent)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(SaveSlotItemPrefabPath);
            if (prefab == null)
            {
                prefab = AssetDatabase.LoadAssetAtPath<GameObject>(FallbackSaveSlotItemPrefabPath);
            }

            if (prefab != null)
            {
                GameObject instance = PrefabUtility.InstantiatePrefab(prefab, parent) as GameObject;
                if (instance != null)
                {
                    instance.name = "LargeSaveSlotItem";
                    SaveSlotItemUI item = instance.GetComponent<SaveSlotItemUI>();
                    if (item != null)
                    {
                        return item;
                    }
                }
            }

            GameObject card = CreatePanel(parent, "LargeSaveSlotItem", new Color(0.92f, 0.94f, 0.98f, 1f));
            Button button = card.AddComponent<Button>();
            SaveSlotItemUI fallback = card.AddComponent<SaveSlotItemUI>();

            CreateText(card.transform, "SlotNameText", "Slot 1", 22f, FontStyles.Bold);
            CreateText(card.transform, "TitleText", "Spieler", 28f, FontStyles.Bold);
            CreateText(card.transform, "SceneNameText", "Scene", 18f, FontStyles.Normal);
            CreateText(card.transform, "SavedAtText", "Zeit", 18f, FontStyles.Normal);
            CreateText(card.transform, "StatusText", "Status", 18f, FontStyles.Normal);

            SetSerializedReference(fallback, "selectButton", button);
            return fallback;
        }

        private static QuizPanel BuildQuizPanel(Transform parent)
        {
            GameObject canvas = CreateUiCanvas(parent, "QuizPanel", 2000);
            QuizPanel quizPanel = canvas.AddComponent<QuizPanel>();
            canvas.SetActive(false);
            return quizPanel;
        }

        private static void BuildDevPanel(Transform parent, LoadGamePanel loadGamePanel, QuizPanel quizPanel)
        {
            GameObject canvas = CreateUiCanvas(parent, "DevPanel", 5000);
            GameObject panel = CreateCenteredPanel(canvas.transform, "DevPanelRoot", new Vector2(430f, 760f), new Color(0.08f, 0.1f, 0.14f, 0.96f));
            DevPanelController controller = panel.AddComponent<DevPanelController>();

            TMP_Text title = CreateText(panel.transform, "DevPanelTitle", "DevPanel", 28f, FontStyles.Bold);
            SetRect(title.rectTransform, 0f, 1f, 1f, 1f, 0f, -20f, -40f, 34f);

            Button reload = CreateDevButton(panel.transform, "Reload SaveSlots", -78f);
            Button dummySaves = CreateDevButton(panel.transform, "Generate Dummy Saves", -136f);
            Button addItem = CreateDevButton(panel.transform, "Add Item", -194f);
            Button resetSettings = CreateDevButton(panel.transform, "Reset Settings", -252f);
            Button quizDraft = CreateDevButton(panel.transform, "Generate Quiz Draft", -310f);
            Button startQuiz = CreateDevButton(panel.transform, "Start Quiz", -368f);
            Button printSession = CreateDevButton(panel.transform, "Print Player Session", -426f);
            Button printScene = CreateDevButton(panel.transform, "Print Current Scene", -484f);
            Button printManagers = CreateDevButton(panel.transform, "Print Feature Managers", -542f);
            Button addXp = CreateDevButton(panel.transform, "Add XP", -600f);
            Button unlockAchievement = CreateDevButton(panel.transform, "Unlock Achievement", -658f);
            Button close = CreateDevButton(panel.transform, "Close DevPanel", -716f);

            controller.AssignPanelRoot(panel);
            controller.AssignButtons(
                reload,
                dummySaves,
                addItem,
                resetSettings,
                quizDraft,
                startQuiz,
                printSession,
                printScene,
                printManagers,
                addXp,
                unlockAchievement,
                close);
            controller.AssignQuizTestTargets(quizPanel, AssetDatabase.LoadAssetAtPath<QuizSet>(BerndQuizPath));
            SetSerializedReference(controller, "loadGamePanel", loadGamePanel);

            canvas.SetActive(false);
        }

        private static Button CreateDevButton(Transform parent, string label, float y)
        {
            Button button = CreateButton(parent, label.Replace(" ", string.Empty), label);
            SetRect(button.GetComponent<RectTransform>(), 0f, 1f, 1f, 1f, 0f, y, -40f, 46f);
            return button;
        }

        private static void BuildWorldNameTag(Transform parent, string objectName, string label, global::System.Type componentType, Transform target, Transform player)
        {
            GameObject canvasObject = CreateChild(parent, objectName, typeof(RectTransform), typeof(Canvas));
            canvasObject.transform.localPosition = new Vector3(0f, 1.55f, 0f);

            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;

            RectTransform rect = canvasObject.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(2.4f, 0.55f);
            rect.localScale = Vector3.one * 0.012f;

            TMP_Text text = CreateText(canvasObject.transform, "NameText", label, 24f, FontStyles.Bold);
            Stretch(text.rectTransform);

            Component component = canvasObject.AddComponent(componentType);
            SetSerializedReference(component, "target", target);
            SetSerializedReference(component, "nameText", text);

            if (component is ArthurNameTag || component is BerndNameTag)
            {
                SetSerializedReference(component, "player", player);
                SetSerializedReference(component, "canvas", canvas);
            }
        }

        private static void BuildCamera()
        {
            GameObject cameraObject = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener));
            cameraObject.tag = "MainCamera";
            cameraObject.transform.position = new Vector3(0f, 0f, -10f);

            Camera camera = cameraObject.GetComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = 5f;
            camera.backgroundColor = new Color(0.17f, 0.2f, 0.24f, 1f);
        }

        private static void BuildEventSystem(Transform parent)
        {
            GameObject eventSystem = CreateChild(parent, "EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
            eventSystem.transform.SetAsLastSibling();
        }

        private static GameObject CreateUiCanvas(Transform parent, string name, int sortingOrder)
        {
            GameObject canvasObject = CreateChild(parent, name, typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = sortingOrder;

            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;

            Stretch(canvasObject.GetComponent<RectTransform>());
            return canvasObject;
        }

        private static GameObject CreatePanel(Transform parent, string name, Color color)
        {
            GameObject panel = CreateChild(parent, name, typeof(RectTransform), typeof(Image));
            Image image = panel.GetComponent<Image>();
            image.color = color;
            Stretch(panel.GetComponent<RectTransform>());
            return panel;
        }

        private static GameObject CreateCenteredPanel(Transform parent, string name, Vector2 size, Color color)
        {
            GameObject panel = CreatePanel(parent, name, color);
            RectTransform rect = panel.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = size;
            return panel;
        }

        private static Button CreateButton(Transform parent, string name, string label)
        {
            GameObject buttonObject = CreateChild(parent, name, typeof(RectTransform), typeof(Image), typeof(Button));
            Image image = buttonObject.GetComponent<Image>();
            image.color = new Color(0.86f, 0.89f, 0.94f, 1f);

            Button button = buttonObject.GetComponent<Button>();
            TMP_Text text = CreateText(buttonObject.transform, "Label", label, 20f, FontStyles.Bold);
            text.color = new Color(0.08f, 0.1f, 0.14f, 1f);
            Stretch(text.rectTransform);
            return button;
        }

        private static TMP_Text CreateText(Transform parent, string name, string value, float fontSize, FontStyles fontStyle)
        {
            GameObject textObject = CreateChild(parent, name, typeof(RectTransform), typeof(TextMeshProUGUI));
            TextMeshProUGUI text = textObject.GetComponent<TextMeshProUGUI>();
            text.text = value;
            text.fontSize = fontSize;
            text.fontStyle = fontStyle;
            text.color = Color.white;
            text.alignment = TextAlignmentOptions.Center;
            text.textWrappingMode = TextWrappingModes.Normal;
            text.raycastTarget = false;
            return text;
        }

        private static GameObject CreateChild(Transform parent, string name, params global::System.Type[] components)
        {
            GameObject child = components == null || components.Length == 0
                ? new GameObject(name)
                : new GameObject(name, components);

            child.transform.SetParent(parent, false);
            return child;
        }

        private static T CreateSystemChild<T>(Transform parent, string name) where T : Component
        {
            GameObject child = CreateChild(parent, name);
            return child.AddComponent<T>();
        }

        private static void Stretch(RectTransform rectTransform)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
        }

        private static void SetRect(RectTransform rectTransform, float minX, float minY, float maxX, float maxY, float x, float y, float width, float height)
        {
            rectTransform.anchorMin = new Vector2(minX, minY);
            rectTransform.anchorMax = new Vector2(maxX, maxY);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.anchoredPosition = new Vector2(x, y);
            rectTransform.sizeDelta = new Vector2(width, height);
        }

        private static void SetSerializedReference(Object target, string propertyName, Object value)
        {
            if (target == null)
            {
                return;
            }

            SerializedObject serializedObject = new SerializedObject(target);
            SerializedProperty property = serializedObject.FindProperty(propertyName);

            if (property == null)
            {
                Debug.LogWarning($"[{nameof(DevelopmentLevelBuilder)}] Property fehlt: {target.GetType().Name}.{propertyName}");
                return;
            }

            property.objectReferenceValue = value;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void SetSerializedBool(Object target, string propertyName, bool value)
        {
            SerializedObject serializedObject = new SerializedObject(target);
            SerializedProperty property = serializedObject.FindProperty(propertyName);

            if (property == null)
            {
                Debug.LogWarning($"[{nameof(DevelopmentLevelBuilder)}] Property fehlt: {target.GetType().Name}.{propertyName}");
                return;
            }

            property.boolValue = value;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        private static Sprite LoadFirstSprite(string path, string featureName)
        {
            Sprite direct = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (direct != null)
            {
                return direct;
            }

            Sprite sprite = AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>().FirstOrDefault();
            if (sprite == null)
            {
                Debug.LogWarning($"[{nameof(DevelopmentLevelBuilder)}] Sprite fuer {featureName} fehlt: {path}");
            }

            return sprite;
        }

        private static void EnsureFolder(string folderPath)
        {
            if (AssetDatabase.IsValidFolder(folderPath))
            {
                return;
            }

            string parent = Path.GetDirectoryName(folderPath)?.Replace("\\", "/");
            string folder = Path.GetFileName(folderPath);

            if (!string.IsNullOrWhiteSpace(parent) && !AssetDatabase.IsValidFolder(parent))
            {
                EnsureFolder(parent);
            }

            AssetDatabase.CreateFolder(parent, folder);
        }

        private static void LogMissing(Object asset, string path)
        {
            if (asset == null)
            {
                Debug.LogWarning($"[{nameof(DevelopmentLevelBuilder)}] Referenz fehlt: {path}");
            }
        }
    }
}
