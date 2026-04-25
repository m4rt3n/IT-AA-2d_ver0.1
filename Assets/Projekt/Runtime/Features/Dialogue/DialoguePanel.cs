/*
 * Datei: DialoguePanel.cs
 * Zweck: Zeigt eine Dialogzeile im UI an und bietet einen Weiter-/Schliessen-Button.
 * Verantwortung: Baut bei Bedarf eine einfache UI, zeigt Sprecher/Text und leitet Weiter-Klicks an DialogueManager weiter.
 * Abhaengigkeiten: BasePanel, DialogueLine, DialogueManager, TextMeshPro, Unity UI.
 * Verwendung: Wird vom DialogueManager geoeffnet; NPCs starten Dialoge ueber den Manager statt direkt ueber dieses Panel.
 */

using ITAA.UI.Panels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ITAA.Features.Dialogue
{
    public class DialoguePanel : BasePanel
    {
        #region Inspector

        [Header("Optional UI References")]
        [SerializeField] private RectTransform panelRoot;
        [SerializeField] private TMP_Text speakerNameText;
        [SerializeField] private TMP_Text dialogueText;
        [SerializeField] private Button nextButton;
        [SerializeField] private TMP_Text nextButtonLabelText;

        [Header("Generated UI")]
        [SerializeField] private bool createMissingUi = true;

        #endregion

        private DialogueManager dialogueManager;

        #region Unity

        private void Awake()
        {
            if (createMissingUi)
            {
                EnsureGeneratedUi();
            }

            WireButton();
        }

        private void OnDestroy()
        {
            if (nextButton != null)
            {
                nextButton.onClick.RemoveListener(HandleNextClicked);
            }
        }

        #endregion

        #region Public API

        public void Show(DialogueManager manager)
        {
            dialogueManager = manager;

            if (createMissingUi)
            {
                EnsureGeneratedUi();
            }

            WireButton();
            Open();
        }

        public void SetLine(DialogueLine line, bool isLastLine)
        {
            if (line == null)
            {
                SetText(speakerNameText, string.Empty);
                SetText(dialogueText, string.Empty);
                SetText(nextButtonLabelText, "Schliessen");
                return;
            }

            SetText(speakerNameText, line.SpeakerName);
            SetText(dialogueText, line.Text);
            SetText(nextButtonLabelText, isLastLine ? "Schliessen" : "Weiter");
        }

        public override void Close()
        {
            base.Close();
            dialogueManager = null;
        }

        #endregion

        #region Private

        private void WireButton()
        {
            if (nextButton == null)
            {
                return;
            }

            nextButton.onClick.RemoveListener(HandleNextClicked);
            nextButton.onClick.AddListener(HandleNextClicked);
        }

        private void HandleNextClicked()
        {
            if (dialogueManager == null)
            {
                Close();
                return;
            }

            dialogueManager.ShowNextLine();
        }

        private void EnsureGeneratedUi()
        {
            if (panelRoot != null &&
                speakerNameText != null &&
                dialogueText != null &&
                nextButton != null)
            {
                return;
            }

            RectTransform parent = transform as RectTransform;

            if (parent == null)
            {
                return;
            }

            panelRoot = CreateRect("DialoguePanelRoot", parent);
            Stretch(panelRoot);

            Image overlay = panelRoot.gameObject.AddComponent<Image>();
            overlay.color = new Color(0f, 0f, 0f, 0.35f);

            RectTransform card = CreateRect("DialogueCard", panelRoot);
            card.anchorMin = new Vector2(0.5f, 0f);
            card.anchorMax = new Vector2(0.5f, 0f);
            card.pivot = new Vector2(0.5f, 0f);
            card.anchoredPosition = new Vector2(0f, 36f);
            card.sizeDelta = new Vector2(900f, 210f);

            Image cardImage = card.gameObject.AddComponent<Image>();
            cardImage.color = new Color(0.08f, 0.1f, 0.15f, 0.96f);

            speakerNameText = CreateText("SpeakerNameText", card, 26f, FontStyles.Bold);
            speakerNameText.alignment = TextAlignmentOptions.Left;
            speakerNameText.color = new Color(0.55f, 0.78f, 1f, 1f);
            SetRect(speakerNameText.rectTransform, 0f, 1f, 1f, 1f, 0f, -28f, -56f, 34f);

            dialogueText = CreateText("DialogueText", card, 25f, FontStyles.Normal);
            dialogueText.alignment = TextAlignmentOptions.TopLeft;
            dialogueText.textWrappingMode = TextWrappingModes.Normal;
            SetRect(dialogueText.rectTransform, 0f, 0f, 1f, 1f, 0f, -94f, -56f, 88f);

            RectTransform buttonRect = CreateButton("NextButton", card, out nextButton, out nextButtonLabelText);
            SetRect(buttonRect, 1f, 0f, 1f, 0f, -112f, 28f, 160f, 44f);
        }

        private static RectTransform CreateButton(string objectName, Transform parent, out Button button, out TMP_Text label)
        {
            RectTransform rect = CreateRect(objectName, parent);
            Image image = rect.gameObject.AddComponent<Image>();
            image.color = new Color(0.86f, 0.89f, 0.94f, 1f);

            button = rect.gameObject.AddComponent<Button>();
            label = CreateText("Label", rect, 20f, FontStyles.Bold);
            label.color = new Color(0.08f, 0.1f, 0.14f, 1f);
            label.alignment = TextAlignmentOptions.Center;
            Stretch(label.rectTransform);

            return rect;
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
            text.color = Color.white;
            text.fontSize = fontSize;
            text.fontStyle = fontStyle;
            text.alignment = TextAlignmentOptions.Center;
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

        private static void SetText(TMP_Text target, string value)
        {
            if (target != null)
            {
                target.text = value ?? string.Empty;
            }
        }

        #endregion
    }
}
