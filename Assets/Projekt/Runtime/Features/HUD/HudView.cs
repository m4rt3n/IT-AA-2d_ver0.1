/*
 * Datei: HudView.cs
 * Zweck: Stellt die sichtbaren HUD-Texte dar.
 * Verantwortung: Zeigt Spielername, aktuelles Ziel, Quizpunkte, Thema und kurze Meldungen an.
 * Abhaengigkeiten: HudNotification, TextMeshPro, Unity UI.
 * Verwendung: Wird vom HudController gesteuert und kann bei fehlenden Inspector-Referenzen eine einfache MVP-UI erzeugen.
 */

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ITAA.Features.HUD
{
    [DisallowMultipleComponent]
    public class HudView : MonoBehaviour
    {
        #region Inspector

        [Header("Optional UI References")]
        [SerializeField] private RectTransform hudRoot;
        [SerializeField] private TMP_Text playerNameText;
        [SerializeField] private TMP_Text currentObjectiveText;
        [SerializeField] private TMP_Text quizScoreText;
        [SerializeField] private TMP_Text topicText;
        [SerializeField] private TMP_Text notificationText;

        [Header("Generated UI")]
        [SerializeField] private bool createMissingUi = true;

        #endregion

        private Coroutine notificationRoutine;

        #region Unity

        private void Awake()
        {
            if (createMissingUi)
            {
                EnsureGeneratedUi();
            }

            HideNotification();
        }

        #endregion

        #region Public API

        public void SetVisible(bool visible)
        {
            GameObject target = hudRoot != null ? hudRoot.gameObject : gameObject;
            target.SetActive(visible);
        }

        public void SetPlayerName(string playerName)
        {
            SetText(playerNameText, string.IsNullOrWhiteSpace(playerName) ? "Spieler" : playerName);
        }

        public void SetCurrentObjective(string objective)
        {
            SetText(currentObjectiveText, string.IsNullOrWhiteSpace(objective) ? "Kein aktives Ziel" : objective);
        }

        public void SetQuizScore(int correctAnswers, int totalAnswers)
        {
            string scoreText = totalAnswers <= 0
                ? "Quiz: 0 / 0"
                : $"Quiz: {Mathf.Max(0, correctAnswers)} / {Mathf.Max(0, totalAnswers)}";

            SetText(quizScoreText, scoreText);
        }

        public void SetTopic(string topic)
        {
            SetText(topicText, string.IsNullOrWhiteSpace(topic) ? "Thema: -" : $"Thema: {topic}");
        }

        public void ShowNotification(HudNotification notification)
        {
            if (notification == null || !notification.HasMessage())
            {
                return;
            }

            if (notificationRoutine != null)
            {
                StopCoroutine(notificationRoutine);
            }

            notificationRoutine = StartCoroutine(ShowNotificationRoutine(notification));
        }

        public void HideNotification()
        {
            if (notificationText != null)
            {
                notificationText.text = string.Empty;
                notificationText.gameObject.SetActive(false);
            }
        }

        #endregion

        #region Private

        private IEnumerator ShowNotificationRoutine(HudNotification notification)
        {
            if (notificationText == null)
            {
                yield break;
            }

            notificationText.gameObject.SetActive(true);
            notificationText.text = notification.Message;

            float duration = Mathf.Max(0.1f, notification.DurationSeconds);
            yield return new WaitForSecondsRealtime(duration);

            HideNotification();
            notificationRoutine = null;
        }

        private void EnsureGeneratedUi()
        {
            if (hudRoot != null &&
                playerNameText != null &&
                currentObjectiveText != null &&
                quizScoreText != null &&
                topicText != null &&
                notificationText != null)
            {
                return;
            }

            RectTransform parent = transform as RectTransform;

            if (parent == null)
            {
                return;
            }

            hudRoot = CreateRect("HudRoot", parent);
            Stretch(hudRoot);

            RectTransform topLeftPanel = CreatePanel("HudTopLeft", hudRoot, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(24f, -24f), new Vector2(420f, 138f));
            playerNameText = CreateText("PlayerNameText", topLeftPanel, 22f, FontStyles.Bold);
            SetRect(playerNameText.rectTransform, 0f, 1f, 1f, 1f, 0f, -18f, -24f, 28f);

            currentObjectiveText = CreateText("CurrentObjectiveText", topLeftPanel, 19f, FontStyles.Normal);
            currentObjectiveText.textWrappingMode = TextWrappingModes.Normal;
            SetRect(currentObjectiveText.rectTransform, 0f, 1f, 1f, 1f, 0f, -56f, -24f, 56f);

            topicText = CreateText("TopicText", topLeftPanel, 17f, FontStyles.Bold);
            topicText.color = new Color(0.55f, 0.78f, 1f, 1f);
            SetRect(topicText.rectTransform, 0f, 0f, 1f, 0f, 0f, 18f, -24f, 24f);

            RectTransform topRightPanel = CreatePanel("HudTopRight", hudRoot, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(-24f, -24f), new Vector2(240f, 64f));
            quizScoreText = CreateText("QuizScoreText", topRightPanel, 21f, FontStyles.Bold);
            quizScoreText.alignment = TextAlignmentOptions.Center;
            Stretch(quizScoreText.rectTransform);

            RectTransform bottomPanel = CreatePanel("HudNotificationPanel", hudRoot, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, 34f), new Vector2(640f, 54f));
            notificationText = CreateText("NotificationText", bottomPanel, 21f, FontStyles.Bold);
            notificationText.alignment = TextAlignmentOptions.Center;
            Stretch(notificationText.rectTransform);
        }

        private static RectTransform CreatePanel(
            string objectName,
            Transform parent,
            Vector2 anchorMin,
            Vector2 anchorMax,
            Vector2 pivot,
            Vector2 anchoredPosition,
            Vector2 sizeDelta)
        {
            RectTransform rect = CreateRect(objectName, parent);
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.pivot = pivot;
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = sizeDelta;

            Image image = rect.gameObject.AddComponent<Image>();
            image.color = new Color(0.08f, 0.1f, 0.14f, 0.78f);
            image.raycastTarget = false;

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
            text.alignment = TextAlignmentOptions.Left;
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
