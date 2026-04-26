/*
 * Datei: HudController.cs
 * Zweck: Versorgt das HUD mit Laufzeitdaten.
 * Verantwortung: Liest optionale PlayerSession- und ProgressManager-Daten und aktualisiert die HudView.
 * Abhaengigkeiten: HudView, HudNotification, PlayerSession, ProgressManager, QuizTopicProgressFormatter.
 * Verwendung: Wird als optionale Szenen-Komponente eingesetzt; fehlende Systeme werden defensiv behandelt.
 */

using ITAA.Features.Progress;
using ITAA.Player.Session;
using ITAA.Quiz;
using UnityEngine;

namespace ITAA.Features.HUD
{
    [DisallowMultipleComponent]
    public class HudController : MonoBehaviour
    {
        #region Inspector

        [Header("References")]
        [SerializeField] private HudView view;
        [SerializeField] private ProgressManager progressManager;

        [Header("HUD State")]
        [SerializeField] private string fallbackPlayerName = "Spieler";
        [SerializeField] private string currentObjective = "Sprich mit Bernd";
        [SerializeField] private string currentTopic = "Netzwerk";
        [SerializeField] private bool refreshEveryFrame;
        [SerializeField] private bool showOnStart = true;

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs;

        #endregion

        #region Unity

        private void Awake()
        {
            ResolveReferences();
        }

        private void Start()
        {
            if (showOnStart)
            {
                ShowHud();
            }

            RefreshHud();
        }

        private void Update()
        {
            if (refreshEveryFrame)
            {
                RefreshHud();
            }
        }

        #endregion

        #region Public API

        public void ShowHud()
        {
            ResolveReferences();

            if (view == null)
            {
                Debug.LogWarning($"[{nameof(HudController)}] HudView fehlt.", this);
                return;
            }

            view.SetVisible(true);
        }

        public void HideHud()
        {
            ResolveReferences();

            if (view == null)
            {
                return;
            }

            view.SetVisible(false);
        }

        public void RefreshHud()
        {
            ResolveReferences();

            if (view == null)
            {
                Debug.LogWarning($"[{nameof(HudController)}] HudView fehlt.", this);
                return;
            }

            view.SetPlayerName(ResolvePlayerName());
            view.SetCurrentObjective(currentObjective);
            ApplyTopicProgress();
            ApplyQuizScore();
        }

        public void SetCurrentObjective(string objective)
        {
            currentObjective = string.IsNullOrWhiteSpace(objective) ? "Kein aktives Ziel" : objective;
            RefreshHud();
        }

        public void SetCurrentTopic(string topic)
        {
            currentTopic = string.IsNullOrWhiteSpace(topic) ? "-" : topic;
            RefreshHud();
        }

        public void SetQuizScore(int correctAnswers, int totalAnswers)
        {
            ResolveReferences();

            if (view == null)
            {
                Debug.LogWarning($"[{nameof(HudController)}] HudView fehlt.", this);
                return;
            }

            view.SetQuizScore(correctAnswers, totalAnswers);
        }

        public void ShowNotification(string message)
        {
            ShowNotification(message, 3f);
        }

        public void ShowNotification(string message, float durationSeconds)
        {
            ResolveReferences();

            if (view == null)
            {
                Debug.LogWarning($"[{nameof(HudController)}] HudView fehlt.", this);
                return;
            }

            view.ShowNotification(new HudNotification(message, durationSeconds));
        }

        #endregion

        #region Private

        private void ResolveReferences()
        {
            if (view == null)
            {
                view = GetComponent<HudView>();

                if (view == null)
                {
                    view = GetComponentInChildren<HudView>(true);
                }
            }

            if (progressManager == null)
            {
                progressManager = FindAnyObjectByType<ProgressManager>(FindObjectsInactive.Include);
            }
        }

        private string ResolvePlayerName()
        {
            if (PlayerSession.Instance == null)
            {
                if (enableDebugLogs)
                {
                    Debug.LogWarning($"[{nameof(HudController)}] PlayerSession fehlt. Fallback-Name wird genutzt.", this);
                }

                return fallbackPlayerName;
            }

            return PlayerSession.Instance.GetResolvedPlayerName(fallbackPlayerName);
        }

        private void ApplyQuizScore()
        {
            if (progressManager == null)
            {
                view.SetQuizScore(0, 0);
                return;
            }

            ProgressProfile profile = progressManager.Profile;

            if (profile == null)
            {
                view.SetQuizScore(0, 0);
                return;
            }

            view.SetQuizScore(profile.CorrectQuizAnswers, profile.TotalQuizAnswers);
        }

        private void ApplyTopicProgress()
        {
            if (progressManager == null || progressManager.Profile == null)
            {
                view.SetTopic(currentTopic);
                return;
            }

            TopicProgress topicProgress = progressManager.Profile.GetTopicProgress(currentTopic);
            view.SetTopic(QuizTopicProgressFormatter.FormatTopicProgress(currentTopic, topicProgress));
        }

        #endregion
    }
}
