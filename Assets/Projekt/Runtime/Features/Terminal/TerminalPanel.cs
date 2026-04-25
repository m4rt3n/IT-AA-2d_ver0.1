/*
 * Datei: TerminalPanel.cs
 * Zweck: Stellt eine UI fuer simulierte IT-Terminal-Minispiele bereit.
 * Verantwortung: Nimmt Nutzereingaben entgegen, zeigt Terminal-Ausgaben an und delegiert Befehlslogik an TerminalEmulator.
 * Abhaengigkeiten: BasePanel, TerminalEmulator, TextMeshPro, Unity UI.
 * Verwendung: Kann als Panel in eine Canvas gelegt oder mit aktivierter Generated-UI zur Laufzeit aufgebaut werden.
 */

using ITAA.UI.Panels;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ITAA.Features.Terminal
{
    public class TerminalPanel : BasePanel
    {
        #region Inspector

        [Header("Optional UI References")]
        [SerializeField] private RectTransform panelRoot;
        [SerializeField] private TMP_Text outputText;
        [SerializeField] private TMP_InputField commandInput;
        [SerializeField] private Button submitButton;
        [SerializeField] private Button closeButton;

        [Header("Generated UI")]
        [SerializeField] private bool createMissingUi = true;

        [Header("Terminal")]
        [SerializeField] private bool echoCommands = true;
        [SerializeField] private bool focusInputOnOpen = true;
        [SerializeField] private string promptPrefix = "support> ";

        #endregion

        private readonly TerminalEmulator emulator = new TerminalEmulator();
        private readonly global::System.Text.StringBuilder outputBuilder = new global::System.Text.StringBuilder();
        private bool isWired;

        #region Unity

        private void Awake()
        {
            if (createMissingUi)
            {
                EnsureGeneratedUi();
            }

            WireUi();
            PrintWelcomeIfEmpty();
        }

        private void OnDestroy()
        {
            UnwireUi();
        }

        #endregion

        #region Public API

        public override void Open()
        {
            base.Open();
            FocusInput();
        }

        public void SubmitCurrentInput()
        {
            if (commandInput == null)
            {
                return;
            }

            string input = commandInput.text;
            commandInput.SetTextWithoutNotify(string.Empty);
            ExecuteCommand(input);
            FocusInput();
        }

        public void ExecuteCommand(string rawInput)
        {
            TerminalCommandResult result = emulator.Execute(rawInput);

            if (result == null)
            {
                return;
            }

            if (result.ShouldClearOutput)
            {
                ClearOutput();
                return;
            }

            if (echoCommands && result.Command != null && !string.IsNullOrEmpty(result.Command.RawInput))
            {
                AppendLine($"{promptPrefix}{result.Command.RawInput}");
            }

            if (!string.IsNullOrEmpty(result.Output))
            {
                AppendLine(result.Output);
            }

            RefreshOutputText();

            if (result.ShouldClosePanel)
            {
                Close();
            }
        }

        public void ClearOutput()
        {
            outputBuilder.Length = 0;
            RefreshOutputText();
        }

        #endregion

        #region Private

        private void WireUi()
        {
            if (isWired)
            {
                return;
            }

            if (submitButton != null)
            {
                submitButton.onClick.AddListener(SubmitCurrentInput);
            }

            if (closeButton != null)
            {
                closeButton.onClick.AddListener(Close);
            }

            if (commandInput != null)
            {
                commandInput.onSubmit.AddListener(HandleInputSubmitted);
            }

            isWired = true;
        }

        private void UnwireUi()
        {
            if (!isWired)
            {
                return;
            }

            if (submitButton != null)
            {
                submitButton.onClick.RemoveListener(SubmitCurrentInput);
            }

            if (closeButton != null)
            {
                closeButton.onClick.RemoveListener(Close);
            }

            if (commandInput != null)
            {
                commandInput.onSubmit.RemoveListener(HandleInputSubmitted);
            }

            isWired = false;
        }

        private void HandleInputSubmitted(string submittedText)
        {
            if (!Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                return;
            }

            SubmitCurrentInput();
        }

        private void PrintWelcomeIfEmpty()
        {
            if (outputBuilder.Length > 0)
            {
                RefreshOutputText();
                return;
            }

            AppendLine("IT-AA Support Terminal");
            AppendLine("Gib help ein, um verfuegbare Befehle zu sehen.");
            RefreshOutputText();
        }

        private void AppendLine(string text)
        {
            if (outputBuilder.Length > 0)
            {
                outputBuilder.AppendLine();
            }

            outputBuilder.Append(text ?? string.Empty);
            outputBuilder.AppendLine();
        }

        private void RefreshOutputText()
        {
            if (outputText != null)
            {
                outputText.text = outputBuilder.ToString();
            }
        }

        private void FocusInput()
        {
            if (!focusInputOnOpen || commandInput == null || !isActiveAndEnabled)
            {
                return;
            }

            commandInput.ActivateInputField();

            if (EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(commandInput.gameObject);
            }
        }

        private void EnsureGeneratedUi()
        {
            if (panelRoot != null &&
                outputText != null &&
                commandInput != null &&
                submitButton != null &&
                closeButton != null)
            {
                return;
            }

            RectTransform parent = transform as RectTransform;

            if (parent == null)
            {
                return;
            }

            panelRoot = CreateRect("TerminalPanelRoot", parent);
            Stretch(panelRoot);

            Image overlay = panelRoot.gameObject.AddComponent<Image>();
            overlay.color = new Color(0f, 0f, 0f, 0.42f);

            RectTransform window = CreateRect("TerminalWindow", panelRoot);
            window.anchorMin = new Vector2(0.5f, 0.5f);
            window.anchorMax = new Vector2(0.5f, 0.5f);
            window.pivot = new Vector2(0.5f, 0.5f);
            window.anchoredPosition = Vector2.zero;
            window.sizeDelta = new Vector2(860f, 520f);

            Image windowImage = window.gameObject.AddComponent<Image>();
            windowImage.color = new Color(0.035f, 0.04f, 0.045f, 0.98f);

            TMP_Text titleText = CreateText("TitleText", window, 24f, FontStyles.Bold);
            titleText.text = "IT-AA Support Terminal";
            titleText.alignment = TextAlignmentOptions.Left;
            titleText.color = new Color(0.62f, 0.95f, 0.73f, 1f);
            SetRect(titleText.rectTransform, 0f, 1f, 1f, 1f, 24f, -32f, -104f, 34f);

            RectTransform outputBackground = CreateRect("OutputBackground", window);
            SetRect(outputBackground, 0f, 0f, 1f, 1f, 24f, 92f, -48f, -164f);

            Image outputBackgroundImage = outputBackground.gameObject.AddComponent<Image>();
            outputBackgroundImage.color = new Color(0.01f, 0.014f, 0.016f, 1f);

            outputText = CreateText("OutputText", outputBackground, 18f, FontStyles.Normal);
            outputText.alignment = TextAlignmentOptions.TopLeft;
            outputText.color = new Color(0.78f, 1f, 0.82f, 1f);
            outputText.textWrappingMode = TextWrappingModes.Normal;
            outputText.font = TMP_Settings.defaultFontAsset;
            SetRect(outputText.rectTransform, 0f, 0f, 1f, 1f, 14f, -14f, -28f, -28f);

            commandInput = CreateInputField("CommandInput", window);
            SetRect(commandInput.GetComponent<RectTransform>(), 0f, 0f, 1f, 0f, 24f, 32f, -168f, 44f);

            RectTransform submitRect = CreateButton("SubmitButton", window, "Ausfuehren", out submitButton);
            SetRect(submitRect, 1f, 0f, 1f, 0f, -94f, 32f, 124f, 44f);

            RectTransform closeRect = CreateButton("CloseButton", window, "X", out closeButton);
            SetRect(closeRect, 1f, 1f, 1f, 1f, -40f, -32f, 44f, 36f);
        }

        private static RectTransform CreateButton(string objectName, Transform parent, string labelText, out Button button)
        {
            RectTransform rect = CreateRect(objectName, parent);

            Image image = rect.gameObject.AddComponent<Image>();
            image.color = new Color(0.18f, 0.28f, 0.22f, 1f);

            button = rect.gameObject.AddComponent<Button>();

            TMP_Text label = CreateText("Label", rect, 18f, FontStyles.Bold);
            label.text = labelText;
            label.alignment = TextAlignmentOptions.Center;
            label.color = Color.white;
            Stretch(label.rectTransform);

            return rect;
        }

        private static TMP_InputField CreateInputField(string objectName, Transform parent)
        {
            RectTransform rect = CreateRect(objectName, parent);

            Image image = rect.gameObject.AddComponent<Image>();
            image.color = new Color(0.015f, 0.02f, 0.018f, 1f);

            TMP_InputField input = rect.gameObject.AddComponent<TMP_InputField>();
            input.targetGraphic = image;
            input.lineType = TMP_InputField.LineType.SingleLine;

            TMP_Text text = CreateText("Text", rect, 18f, FontStyles.Normal);
            text.alignment = TextAlignmentOptions.Left;
            text.color = new Color(0.82f, 1f, 0.86f, 1f);
            text.textWrappingMode = TextWrappingModes.NoWrap;
            SetRect(text.rectTransform, 0f, 0f, 1f, 1f, 12f, 0f, -24f, 0f);
            input.textComponent = text;

            TMP_Text placeholder = CreateText("Placeholder", rect, 18f, FontStyles.Italic);
            placeholder.text = "Befehl eingeben...";
            placeholder.alignment = TextAlignmentOptions.Left;
            placeholder.color = new Color(0.45f, 0.58f, 0.48f, 1f);
            placeholder.textWrappingMode = TextWrappingModes.NoWrap;
            SetRect(placeholder.rectTransform, 0f, 0f, 1f, 1f, 12f, 0f, -24f, 0f);
            input.placeholder = placeholder;

            return input;
        }

        private static RectTransform CreateRect(string objectName, Transform parent)
        {
            GameObject instance = new GameObject(objectName, typeof(RectTransform));
            instance.transform.SetParent(parent, false);
            return instance.GetComponent<RectTransform>();
        }

        private static TMP_Text CreateText(string objectName, Transform parent, float fontSize, FontStyles fontStyle)
        {
            RectTransform rect = CreateRect(objectName, parent);
            TextMeshProUGUI text = rect.gameObject.AddComponent<TextMeshProUGUI>();
            text.fontSize = fontSize;
            text.fontStyle = fontStyle;
            text.raycastTarget = false;
            return text;
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

        #endregion
    }
}
