/*
 * Datei: DevPanelBootstrap.cs
 * Zweck: Erzeugt bei Bedarf ein optionales Runtime-DevPanel fuer Debug-Aktionen.
 * Verantwortung: Baut eine einfache UI, stellt einen F12-Toggle bereit und verbindet Buttons mit DevPanelController.
 * Abhaengigkeiten: DevPanelController, Unity UI, TextMeshPro, EventSystem.
 * Verwendung: Kann in der StartScene auf ein GameObject gesetzt werden, ohne bestehende Menues oder Gameplay zu ersetzen.
 */

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ITAA.DevTools
{
    [DisallowMultipleComponent]
    public class DevPanelBootstrap : MonoBehaviour
    {
        #region Inspector

        [Header("Bootstrap")]
        [SerializeField] private bool createRuntimePanel = true;
        [SerializeField] private bool toggleWithKeyboard = true;
        [SerializeField] private Key toggleKey = Key.F12;
        [SerializeField] private DevPanelController controller;

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs = true;

        #endregion

        #region Unity

        private void Awake()
        {
            if (controller == null)
            {
                controller = FindAnyObjectByType<DevPanelController>(FindObjectsInactive.Include);
            }

            if (controller == null && createRuntimePanel)
            {
                controller = CreateRuntimePanel();
            }

            if (controller != null)
            {
                controller.CloseDevPanel();
            }
        }

        private void Update()
        {
            if (!toggleWithKeyboard || controller == null)
            {
                return;
            }

            Keyboard keyboard = Keyboard.current;

            if (keyboard != null && keyboard[toggleKey].wasPressedThisFrame)
            {
                controller.ToggleDevPanel();
            }
        }

        #endregion

        #region Private

        private DevPanelController CreateRuntimePanel()
        {
            EnsureEventSystem();

            GameObject canvasObject = new GameObject("DevPanelCanvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasObject.transform.SetParent(transform, false);

            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 5000;

            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;

            RectTransform canvasRect = canvasObject.GetComponent<RectTransform>();
            StretchToParent(canvasRect);

            GameObject panelObject = CreateUiObject("DevPanel", canvasRect);
            RectTransform panelRect = panelObject.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(1f, 0.5f);
            panelRect.anchorMax = new Vector2(1f, 0.5f);
            panelRect.pivot = new Vector2(1f, 0.5f);
            panelRect.anchoredPosition = new Vector2(-32f, 0f);
            panelRect.sizeDelta = new Vector2(420f, 720f);

            Image panelImage = panelObject.AddComponent<Image>();
            panelImage.color = new Color(0.08f, 0.1f, 0.14f, 0.96f);

            VerticalLayoutGroup layout = panelObject.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(20, 20, 20, 20);
            layout.spacing = 12f;
            layout.childAlignment = TextAnchor.UpperCenter;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            ContentSizeFitter fitter = panelObject.AddComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;

            CreateTitle(panelObject.transform);

            Button reloadButton = CreateButton(panelObject.transform, "Reload SaveSlots");
            Button dummySavesButton = CreateButton(panelObject.transform, "Generate Dummy Saves");
            Button addDemoItemButton = CreateButton(panelObject.transform, "Add Demo Item");
            Button resetSettingsButton = CreateButton(panelObject.transform, "Reset Settings");
            Button quizDraftButton = CreateButton(panelObject.transform, "Generate Dummy Quiz Draft");
            Button startQuizButton = CreateButton(panelObject.transform, "Start Demo Quiz");
            Button printSessionButton = CreateButton(panelObject.transform, "Print Player Session");
            Button printSceneButton = CreateButton(panelObject.transform, "Print Current Scene");
            Button printFeatureManagersButton = CreateButton(panelObject.transform, "Print Feature Managers");
            Button grantSkillXpButton = CreateButton(panelObject.transform, "Grant Demo Skill XP");
            Button unlockAchievementButton = CreateButton(panelObject.transform, "Unlock Demo Achievement");
            Button closeButton = CreateButton(panelObject.transform, "Close DevPanel");

            DevPanelController runtimeController = panelObject.AddComponent<DevPanelController>();
            runtimeController.AssignPanelRoot(panelObject);
            runtimeController.AssignButtons(
                reloadButton,
                dummySavesButton,
                addDemoItemButton,
                resetSettingsButton,
                quizDraftButton,
                startQuizButton,
                printSessionButton,
                printSceneButton,
                printFeatureManagersButton,
                grantSkillXpButton,
                unlockAchievementButton,
                closeButton);

            Log("Runtime-DevPanel erzeugt.");
            return runtimeController;
        }

        private static void EnsureEventSystem()
        {
            EventSystem eventSystem = FindAnyObjectByType<EventSystem>(FindObjectsInactive.Include);

            if (eventSystem != null)
            {
                return;
            }

            GameObject eventSystemObject = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            DontDestroyOnLoad(eventSystemObject);
        }

        private static void CreateTitle(Transform parent)
        {
            TextMeshProUGUI title = CreateText("DevPanelTitle", parent, 28f);
            title.fontStyle = FontStyles.Bold;
            title.text = "DevPanel";
            title.alignment = TextAlignmentOptions.Center;

            LayoutElement layoutElement = title.gameObject.AddComponent<LayoutElement>();
            layoutElement.preferredHeight = 42f;
        }

        private static Button CreateButton(Transform parent, string label)
        {
            GameObject buttonObject = CreateUiObject(label.Replace(" ", string.Empty), parent);
            Image image = buttonObject.AddComponent<Image>();
            image.color = new Color(0.2f, 0.27f, 0.36f, 1f);

            Button button = buttonObject.AddComponent<Button>();
            ColorBlock colors = button.colors;
            colors.normalColor = new Color(0.2f, 0.27f, 0.36f, 1f);
            colors.highlightedColor = new Color(0.28f, 0.38f, 0.5f, 1f);
            colors.pressedColor = new Color(0.14f, 0.19f, 0.27f, 1f);
            colors.selectedColor = colors.highlightedColor;
            button.colors = colors;

            LayoutElement buttonLayout = buttonObject.AddComponent<LayoutElement>();
            buttonLayout.preferredHeight = 52f;

            TextMeshProUGUI text = CreateText("Label", buttonObject.transform, 22f);
            text.text = label;
            text.alignment = TextAlignmentOptions.Center;

            RectTransform textRect = text.rectTransform;
            StretchToParent(textRect);
            textRect.offsetMin = new Vector2(12f, 0f);
            textRect.offsetMax = new Vector2(-12f, 0f);

            return button;
        }

        private static TextMeshProUGUI CreateText(string objectName, Transform parent, float fontSize)
        {
            GameObject textObject = CreateUiObject(objectName, parent);
            TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();
            text.fontSize = fontSize;
            text.color = Color.white;
            text.textWrappingMode = TextWrappingModes.NoWrap;
            text.raycastTarget = false;
            return text;
        }

        private static GameObject CreateUiObject(string objectName, Transform parent)
        {
            GameObject gameObject = new GameObject(objectName, typeof(RectTransform));
            gameObject.transform.SetParent(parent, false);
            return gameObject;
        }

        private static void StretchToParent(RectTransform rectTransform)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
        }

        private void Log(string message)
        {
            if (!enableDebugLogs)
            {
                return;
            }

            Debug.Log($"[{nameof(DevPanelBootstrap)}] {message}", this);
        }

        #endregion
    }
}
