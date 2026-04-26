/*
 * Datei: SettingsHotkeyController.cs
 * Zweck: Macht das bestehende Settings-System in der StartScene per Hotkey nutzbar.
 * Verantwortung: Erzeugt bei Bedarf ein Runtime-Settings-Panel, toggelt es per Unity Input System und sperrt waehrenddessen die Player-Bewegung.
 * Abhaengigkeiten: SettingsManager, SettingsUIController, PlayerController, TextMeshPro, Unity UI, Unity Input System.
 * Verwendung: Wird vom GameSystemsBootstrap optional erzeugt und kann alternativ manuell auf ein Scene-GameObject gelegt werden.
 */

using ITAA.Player.Movement;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace ITAA.System.Settings
{
    [DisallowMultipleComponent]
    public sealed class SettingsHotkeyController : MonoBehaviour
    {
        #region Inspector

        [Header("Hotkey")]
        [SerializeField] private bool toggleWithKeyboard = true;
        [SerializeField] private Key primaryToggleKey = Key.O;
        [SerializeField] private Key secondaryToggleKey = Key.F10;

        [Header("Runtime UI")]
        [SerializeField] private bool createRuntimePanel = true;
        [SerializeField] private SettingsUIController settingsUi;
        [SerializeField] private Canvas targetCanvas;
        [SerializeField] private int sortingOrder = 4300;

        [Header("Gameplay")]
        [SerializeField] private bool lockPlayerMovementWhileOpen = true;
        [SerializeField] private PlayerController playerController;

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs = true;

        #endregion

        private GameObject panelRoot;
        private bool playerMovementWasEnabled;
        private bool hasLockedPlayer;

        #region Unity

        private void Awake()
        {
            SettingsManager.GetOrCreate();
            ResolveReferences();

            if (settingsUi == null && createRuntimePanel)
            {
                settingsUi = CreateRuntimePanel();
            }

            if (settingsUi != null)
            {
                settingsUi.SetCloseRequestedCallback(CloseSettings);
                panelRoot = settingsUi.gameObject;
                CloseSettings();
            }
            else if (enableDebugLogs)
            {
                Debug.LogWarning($"[{nameof(SettingsHotkeyController)}] SettingsUIController fehlt.", this);
            }
        }

        private void Update()
        {
            if (!toggleWithKeyboard)
            {
                return;
            }

            Keyboard keyboard = Keyboard.current;

            if (keyboard == null)
            {
                return;
            }

            if (keyboard[primaryToggleKey].wasPressedThisFrame || keyboard[secondaryToggleKey].wasPressedThisFrame)
            {
                ToggleSettings();
            }
        }

        #endregion

        #region Public API

        public void ToggleSettings()
        {
            ResolveReferences();

            if (panelRoot == null)
            {
                if (enableDebugLogs)
                {
                    Debug.LogWarning($"[{nameof(SettingsHotkeyController)}] Toggle abgebrochen: SettingsPanel fehlt.", this);
                }

                return;
            }

            if (panelRoot.activeSelf)
            {
                CloseSettings();
                return;
            }

            OpenSettings();
        }

        public void OpenSettings()
        {
            ResolveReferences();

            if (panelRoot == null)
            {
                return;
            }

            SettingsManager.GetOrCreate().LoadSettings();
            settingsUi?.RefreshFromManager();
            LockPlayerMovement();
            panelRoot.SetActive(true);

            Log("Settings geoeffnet.");
        }

        public void CloseSettings()
        {
            if (panelRoot != null)
            {
                panelRoot.SetActive(false);
            }

            ReleasePlayerMovement();
            Log("Settings geschlossen.");
        }

        #endregion

        #region Private

        private void ResolveReferences()
        {
            if (settingsUi == null)
            {
                settingsUi = FindAnyObjectByType<SettingsUIController>(FindObjectsInactive.Include);
            }

            if (panelRoot == null && settingsUi != null)
            {
                panelRoot = settingsUi.gameObject;
            }

            if (targetCanvas == null && settingsUi != null)
            {
                targetCanvas = settingsUi.GetComponentInParent<Canvas>(true);
            }

            if (playerController == null)
            {
                playerController = FindAnyObjectByType<PlayerController>(FindObjectsInactive.Include);
            }
        }

        private SettingsUIController CreateRuntimePanel()
        {
            EnsureEventSystem();

            Canvas canvas = targetCanvas != null ? targetCanvas : CreateRuntimeCanvas();
            GameObject panelObject = new("SettingsPanel", typeof(RectTransform));
            panelObject.transform.SetParent(canvas.transform, false);

            RectTransform panelRect = panelObject.GetComponent<RectTransform>();
            Stretch(panelRect);

            Image overlay = panelObject.AddComponent<Image>();
            overlay.color = new Color(0f, 0f, 0f, 0.58f);

            RectTransform card = CreateRect("SettingsCard", panelObject.transform);
            card.anchorMin = new Vector2(0.5f, 0.5f);
            card.anchorMax = new Vector2(0.5f, 0.5f);
            card.pivot = new Vector2(0.5f, 0.5f);
            card.anchoredPosition = Vector2.zero;
            card.sizeDelta = new Vector2(820f, 660f);

            Image cardImage = card.gameObject.AddComponent<Image>();
            cardImage.color = new Color(0.1f, 0.12f, 0.16f, 0.98f);

            TMP_Text heading = CreateText("HeadingText", card, 34f, FontStyles.Bold);
            heading.text = "Settings";
            SetRect(heading.rectTransform, 0f, 1f, 1f, 1f, 0f, -32f, -56f, 42f);

            Slider masterSlider = CreateSliderRow(card, "Master Volume", -96f);
            Slider musicSlider = CreateSliderRow(card, "Music Volume", -156f);
            Slider sfxSlider = CreateSliderRow(card, "SFX Volume", -216f);
            Toggle fullscreenToggle = CreateToggleRow(card, "Fullscreen", -286f);
            TMP_InputField languageInput = CreateInputRow(card, "Language", -346f, "de");
            TMP_InputField playerNameInput = CreateInputRow(card, "Player Name", -406f, "Spieler");

            Button applyButton = CreateButton(card, "SaveApplyButton", "Save / Apply");
            SetRect(applyButton.GetComponent<RectTransform>(), 0f, 0f, 0f, 0f, 210f, 72f, 190f, 48f);

            Button resetButton = CreateButton(card, "ResetButton", "Reset");
            SetRect(resetButton.GetComponent<RectTransform>(), 0.5f, 0f, 0.5f, 0f, 0f, 72f, 140f, 48f);

            Button closeButton = CreateButton(card, "CloseButton", "Close");
            SetRect(closeButton.GetComponent<RectTransform>(), 1f, 0f, 1f, 0f, -150f, 72f, 140f, 48f);

            SettingsUIController controller = panelObject.AddComponent<SettingsUIController>();
            controller.AssignControls(
                masterSlider,
                musicSlider,
                sfxSlider,
                fullscreenToggle,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                languageInput,
                playerNameInput,
                applyButton,
                resetButton,
                closeButton);

            panelRoot = panelObject;
            Log("Runtime-SettingsPanel erzeugt.");
            return controller;
        }

        private Canvas CreateRuntimeCanvas()
        {
            GameObject canvasObject = new("SettingsCanvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasObject.transform.SetParent(transform, false);

            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = sortingOrder;

            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;

            Stretch(canvasObject.GetComponent<RectTransform>());
            targetCanvas = canvas;
            return canvas;
        }

        private static Slider CreateSliderRow(Transform parent, string label, float y)
        {
            CreateRowLabel(parent, label, y);

            RectTransform sliderRect = CreateRect($"{label.Replace(" ", string.Empty)}Slider", parent);
            SetRect(sliderRect, 0f, 1f, 1f, 1f, 250f, y, -330f, 28f);

            Slider slider = sliderRect.gameObject.AddComponent<Slider>();
            slider.minValue = 0f;
            slider.maxValue = 1f;

            RectTransform background = CreateRect("Background", sliderRect);
            Stretch(background);
            Image backgroundImage = background.gameObject.AddComponent<Image>();
            backgroundImage.color = new Color(0.26f, 0.3f, 0.38f, 1f);

            RectTransform fillArea = CreateRect("Fill Area", sliderRect);
            Stretch(fillArea);
            fillArea.offsetMin = new Vector2(0f, 8f);
            fillArea.offsetMax = new Vector2(0f, -8f);

            RectTransform fill = CreateRect("Fill", fillArea);
            Stretch(fill);
            Image fillImage = fill.gameObject.AddComponent<Image>();
            fillImage.color = new Color(0.42f, 0.72f, 1f, 1f);

            RectTransform handleArea = CreateRect("Handle Slide Area", sliderRect);
            Stretch(handleArea);

            RectTransform handle = CreateRect("Handle", handleArea);
            handle.sizeDelta = new Vector2(24f, 34f);
            Image handleImage = handle.gameObject.AddComponent<Image>();
            handleImage.color = Color.white;

            slider.fillRect = fill;
            slider.handleRect = handle;
            slider.targetGraphic = handleImage;
            return slider;
        }

        private static Toggle CreateToggleRow(Transform parent, string label, float y)
        {
            CreateRowLabel(parent, label, y);

            RectTransform rect = CreateRect($"{label.Replace(" ", string.Empty)}Toggle", parent);
            SetRect(rect, 0f, 1f, 0f, 1f, 260f, y, 44f, 44f);

            Toggle toggle = rect.gameObject.AddComponent<Toggle>();
            Image background = rect.gameObject.AddComponent<Image>();
            background.color = new Color(0.26f, 0.3f, 0.38f, 1f);
            toggle.targetGraphic = background;

            RectTransform checkmark = CreateRect("Checkmark", rect);
            Stretch(checkmark);
            checkmark.offsetMin = new Vector2(8f, 8f);
            checkmark.offsetMax = new Vector2(-8f, -8f);
            Image checkmarkImage = checkmark.gameObject.AddComponent<Image>();
            checkmarkImage.color = new Color(0.42f, 0.72f, 1f, 1f);
            toggle.graphic = checkmarkImage;
            return toggle;
        }

        private static TMP_InputField CreateInputRow(Transform parent, string label, float y, string fallback)
        {
            CreateRowLabel(parent, label, y);

            RectTransform rect = CreateRect($"{label.Replace(" ", string.Empty)}Input", parent);
            SetRect(rect, 0f, 1f, 1f, 1f, 250f, y, -330f, 42f);

            Image image = rect.gameObject.AddComponent<Image>();
            image.color = new Color(0.92f, 0.94f, 0.98f, 1f);

            TMP_InputField input = rect.gameObject.AddComponent<TMP_InputField>();

            TMP_Text text = CreateText("Text", rect, 20f, FontStyles.Normal);
            text.color = new Color(0.08f, 0.1f, 0.14f, 1f);
            text.alignment = TextAlignmentOptions.Left;
            SetRect(text.rectTransform, 0f, 0f, 1f, 1f, 12f, 0f, -24f, 0f);

            TMP_Text placeholder = CreateText("Placeholder", rect, 20f, FontStyles.Italic);
            placeholder.color = new Color(0.42f, 0.46f, 0.52f, 1f);
            placeholder.text = fallback;
            placeholder.alignment = TextAlignmentOptions.Left;
            SetRect(placeholder.rectTransform, 0f, 0f, 1f, 1f, 12f, 0f, -24f, 0f);

            input.textComponent = text;
            input.placeholder = placeholder;
            return input;
        }

        private static void CreateRowLabel(Transform parent, string label, float y)
        {
            TMP_Text text = CreateText($"{label.Replace(" ", string.Empty)}Label", parent, 22f, FontStyles.Bold);
            text.text = label;
            text.alignment = TextAlignmentOptions.Left;
            SetRect(text.rectTransform, 0f, 1f, 0f, 1f, 150f, y, 230f, 34f);
        }

        private static Button CreateButton(Transform parent, string objectName, string label)
        {
            RectTransform rect = CreateRect(objectName, parent);
            Image image = rect.gameObject.AddComponent<Image>();
            image.color = new Color(0.86f, 0.89f, 0.94f, 1f);

            Button button = rect.gameObject.AddComponent<Button>();
            TMP_Text text = CreateText("Label", rect, 20f, FontStyles.Bold);
            text.text = label;
            text.color = new Color(0.08f, 0.1f, 0.14f, 1f);
            text.alignment = TextAlignmentOptions.Center;
            Stretch(text.rectTransform);
            return button;
        }

        private static RectTransform CreateRect(string objectName, Transform parent)
        {
            GameObject instance = new(objectName, typeof(RectTransform));
            instance.transform.SetParent(parent, false);
            return instance.GetComponent<RectTransform>();
        }

        private static TMP_Text CreateText(string objectName, Transform parent, float fontSize, FontStyles fontStyle)
        {
            RectTransform rect = CreateRect(objectName, parent);
            TextMeshProUGUI text = rect.gameObject.AddComponent<TextMeshProUGUI>();
            text.color = Color.white;
            text.fontSize = fontSize;
            text.fontStyle = fontStyle;
            text.raycastTarget = false;
            return text;
        }

        private static void EnsureEventSystem()
        {
            EventSystem existingEventSystem = FindAnyObjectByType<EventSystem>(FindObjectsInactive.Include);

            if (existingEventSystem != null)
            {
                return;
            }

            new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
        }

        private void LockPlayerMovement()
        {
            if (!lockPlayerMovementWhileOpen || playerController == null || hasLockedPlayer)
            {
                return;
            }

            playerMovementWasEnabled = playerController.IsMovementEnabled();

            if (!playerMovementWasEnabled)
            {
                return;
            }

            playerController.SetMovementEnabled(false);
            playerController.StopImmediately();
            hasLockedPlayer = true;
        }

        private void ReleasePlayerMovement()
        {
            if (!hasLockedPlayer)
            {
                return;
            }

            if (playerController != null && playerMovementWasEnabled)
            {
                playerController.SetMovementEnabled(true);
            }

            hasLockedPlayer = false;
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

        private void Log(string message)
        {
            if (!enableDebugLogs)
            {
                return;
            }

            Debug.Log($"[{nameof(SettingsHotkeyController)}] {message}", this);
        }

        #endregion
    }
}
