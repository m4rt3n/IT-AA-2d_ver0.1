/*
 * Datei: QuizPanel.cs
 * Zweck: Zeigt ein QuizSet als einfaches interaktives UI-Panel an.
 * Verantwortung: Baut bei Bedarf seine UI, zeigt Fragen/Antworten und delegiert Auswertung an QuizRunner.
 * Abhaengigkeiten: BasePanel, ITAA.Quiz, TextMeshPro, Unity UI.
 * Verwendung: Wird von NPCs wie BerndQuizStarter geoeffnet, ohne Fragen hart im UI-Code zu speichern.
 */

using System;
using System.Collections.Generic;
using ITAA.Quiz;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ITAA.UI.Panels
{
    public class QuizPanel : BasePanel
    {
        [Header("Optional UI References")]
        [SerializeField] private RectTransform panelRoot;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text questionText;
        [SerializeField] private TMP_Text explanationText;
        [SerializeField] private Button[] answerButtons;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button closeButton;

        [Header("Generated UI")]
        [SerializeField] private bool createMissingUi = true;

        private readonly List<TMP_Text> answerLabels = new();
        private QuizRunner runner;
        private Action onQuizClosed;
        private bool answerSelected;

        private void Awake()
        {
            if (createMissingUi)
            {
                EnsureGeneratedUi();
            }

            WireStaticButtons();
        }

        public void StartQuiz(QuizSet quizSet, Action closedCallback)
        {
            if (createMissingUi)
            {
                EnsureGeneratedUi();
            }

            WireStaticButtons();

            runner = new QuizRunner(quizSet);
            onQuizClosed = closedCallback;
            answerSelected = false;

            Open();

            if (!runner.HasQuestions)
            {
                SetText(titleText, "Quiz");
                SetText(questionText, "Keine Fragen vorhanden.");
                SetText(explanationText, string.Empty);
                SetAnswersInteractable(false);
                SetNextButtonVisible(false);
                return;
            }

            SetText(titleText, string.IsNullOrWhiteSpace(quizSet.DisplayName) ? "Quiz" : quizSet.DisplayName);
            ShowCurrentQuestion();
        }

        public override void Close()
        {
            base.Close();
            onQuizClosed?.Invoke();
            onQuizClosed = null;
        }

        private void ShowCurrentQuestion()
        {
            QuizQuestion currentQuestion = runner.GetCurrentQuestion();
            answerSelected = false;

            SetText(explanationText, string.Empty);
            SetNextButtonVisible(false);

            if (currentQuestion == null)
            {
                SetText(questionText, "Keine Frage geladen.");
                SetAnswersInteractable(false);
                return;
            }

            SetText(questionText, currentQuestion.QuestionText);
            ApplyAnswers(currentQuestion);
        }

        private void ApplyAnswers(QuizQuestion question)
        {
            int optionCount = question.AnswerOptions != null ? question.AnswerOptions.Count : 0;

            for (int i = 0; i < answerButtons.Length; i++)
            {
                Button button = answerButtons[i];
                TMP_Text label = i < answerLabels.Count ? answerLabels[i] : null;
                bool hasOption = i < optionCount;

                if (button != null)
                {
                    button.gameObject.SetActive(hasOption);
                    button.interactable = hasOption;
                }

                SetText(label, hasOption ? question.AnswerOptions[i].Text : string.Empty);
            }
        }

        private void HandleAnswerClicked(int answerIndex)
        {
            if (runner == null || answerSelected)
            {
                return;
            }

            QuizResult result = runner.AnswerCurrentQuestion(answerIndex);
            answerSelected = true;
            SetAnswersInteractable(false);

            string prefix = result.IsCorrect ? "Richtig." : "Nicht ganz.";
            string explanation = result.Question != null ? result.Question.Explanation : string.Empty;
            SetText(explanationText, string.IsNullOrWhiteSpace(explanation) ? prefix : $"{prefix} {explanation}");
            SetNextButtonVisible(true);
        }

        private void HandleNextClicked()
        {
            if (runner == null)
            {
                Close();
                return;
            }

            if (!runner.MoveNext())
            {
                SetText(questionText, "Quiz abgeschlossen.");
                SetText(explanationText, "Danke fuer deine Antwort.");
                SetAnswersInteractable(false);
                SetNextButtonVisible(false);
                return;
            }

            ShowCurrentQuestion();
        }

        private void WireStaticButtons()
        {
            if (answerButtons != null)
            {
                for (int i = 0; i < answerButtons.Length; i++)
                {
                    int answerIndex = i;
                    Button button = answerButtons[i];

                    if (button == null)
                    {
                        continue;
                    }

                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() => HandleAnswerClicked(answerIndex));
                }
            }

            if (nextButton != null)
            {
                nextButton.onClick.RemoveAllListeners();
                nextButton.onClick.AddListener(HandleNextClicked);
            }

            if (closeButton != null)
            {
                closeButton.onClick.RemoveAllListeners();
                closeButton.onClick.AddListener(Close);
            }
        }

        private void SetAnswersInteractable(bool interactable)
        {
            if (answerButtons == null)
            {
                return;
            }

            foreach (Button button in answerButtons)
            {
                if (button != null)
                {
                    button.interactable = interactable && button.gameObject.activeSelf;
                }
            }
        }

        private void SetNextButtonVisible(bool visible)
        {
            if (nextButton != null)
            {
                nextButton.gameObject.SetActive(visible);
            }
        }

        private void EnsureGeneratedUi()
        {
            if (panelRoot != null &&
                titleText != null &&
                questionText != null &&
                explanationText != null &&
                answerButtons != null &&
                answerButtons.Length > 0 &&
                nextButton != null &&
                closeButton != null)
            {
                return;
            }

            RectTransform parent = transform as RectTransform;
            if (parent == null)
            {
                return;
            }

            panelRoot = CreateRect("QuizPanelRoot", parent);
            Stretch(panelRoot);

            Image overlay = panelRoot.gameObject.AddComponent<Image>();
            overlay.color = new Color(0f, 0f, 0f, 0.62f);

            RectTransform card = CreateRect("QuizCard", panelRoot);
            card.anchorMin = new Vector2(0.5f, 0.5f);
            card.anchorMax = new Vector2(0.5f, 0.5f);
            card.pivot = new Vector2(0.5f, 0.5f);
            card.anchoredPosition = Vector2.zero;
            card.sizeDelta = new Vector2(760f, 520f);

            Image cardImage = card.gameObject.AddComponent<Image>();
            cardImage.color = new Color(0.12f, 0.14f, 0.18f, 0.98f);

            titleText = CreateText("TitleText", card, 34f, FontStyles.Bold);
            SetRect(titleText.rectTransform, 0f, 1f, 1f, 1f, 0f, -28f, -56f, 42f);

            questionText = CreateText("QuestionText", card, 27f, FontStyles.Bold);
            questionText.enableWordWrapping = true;
            SetRect(questionText.rectTransform, 0f, 1f, 1f, 1f, 0f, -92f, -80f, 70f);

            answerButtons = new Button[4];
            answerLabels.Clear();

            for (int i = 0; i < answerButtons.Length; i++)
            {
                RectTransform answerRect = CreateButton($"AnswerButton{i + 1}", card, out Button button, out TMP_Text label);
                SetRect(answerRect, 0f, 1f, 1f, 1f, 0f, -188f - (i * 62f), -96f, 48f);
                answerButtons[i] = button;
                answerLabels.Add(label);
            }

            explanationText = CreateText("ExplanationText", card, 22f, FontStyles.Normal);
            explanationText.enableWordWrapping = true;
            SetRect(explanationText.rectTransform, 0f, 0f, 1f, 0f, 0f, 86f, -96f, 70f);

            RectTransform nextRect = CreateButton("NextButton", card, out nextButton, out TMP_Text nextLabel);
            SetText(nextLabel, "Weiter");
            SetRect(nextRect, 1f, 0f, 1f, 0f, -150f, 28f, 120f, 44f);

            RectTransform closeRect = CreateButton("CloseButton", card, out closeButton, out TMP_Text closeLabel);
            SetText(closeLabel, "Schliessen");
            SetRect(closeRect, 0f, 0f, 0f, 0f, 150f, 28f, 150f, 44f);
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

        private static RectTransform CreateButton(string objectName, Transform parent, out Button button, out TMP_Text label)
        {
            RectTransform rect = CreateRect(objectName, parent);
            Image image = rect.gameObject.AddComponent<Image>();
            image.color = new Color(0.88f, 0.9f, 0.94f, 1f);

            button = rect.gameObject.AddComponent<Button>();
            label = CreateText("Label", rect, 22f, FontStyles.Bold);
            label.color = new Color(0.08f, 0.1f, 0.14f, 1f);
            Stretch(label.rectTransform);
            return rect;
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
                target.text = value;
            }
        }
    }
}
