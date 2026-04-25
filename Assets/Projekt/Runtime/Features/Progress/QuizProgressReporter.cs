/*
 * Datei: QuizProgressReporter.cs
 * Zweck: Meldet Quiz-Ergebnisse an den ProgressManager.
 * Verantwortung: Kapselt Quiz-Fortschrittsmeldungen, ohne konkrete Quiz-UI oder NPCs hart zu koppeln.
 * Abhaengigkeiten: ProgressManager.
 * Verwendung: Kann neben Quiz-Komponenten platziert oder spaeter von QuizPanel/QuizRunner-Adaptern aufgerufen werden.
 */

using UnityEngine;

namespace ITAA.Features.Progress
{
    [DisallowMultipleComponent]
    public class QuizProgressReporter : MonoBehaviour
    {
        #region Inspector

        [Header("References")]
        [SerializeField] private ProgressManager progressManager;

        [Header("Defaults")]
        [SerializeField] private string defaultTopic = "DNS";
        [SerializeField] private string defaultQuizId = "bernd_intro_quiz";
        [SerializeField] private string defaultDifficulty = "Easy";

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs = true;

        #endregion

        #region Unity

        private void Awake()
        {
            ResolveReferences();
        }

        #endregion

        #region Public API

        public void ReportCorrectAnswer()
        {
            ReportAnswer(defaultTopic, true);
        }

        public void ReportWrongAnswer()
        {
            ReportAnswer(defaultTopic, false);
        }

        public void ReportAnswer(string topic, bool isCorrect)
        {
            ResolveReferences();

            if (progressManager == null)
            {
                Debug.LogWarning($"[{nameof(QuizProgressReporter)}] ProgressManager fehlt.", this);
                return;
            }

            progressManager.ReportQuizAnswer(topic, isCorrect);
            Log($"Quiz-Antwort weitergegeben: topic={topic}, correct={isCorrect}");
        }

        public void ReportQuizCompleted(int correctAnswers, int totalAnswers)
        {
            ReportQuizCompleted(defaultQuizId, defaultDifficulty, correctAnswers, totalAnswers);
        }

        public void ReportQuizCompleted(string quizId, string difficulty, int correctAnswers, int totalAnswers)
        {
            ResolveReferences();

            if (progressManager == null)
            {
                Debug.LogWarning($"[{nameof(QuizProgressReporter)}] ProgressManager fehlt.", this);
                return;
            }

            progressManager.ReportQuizCompleted(quizId, difficulty, correctAnswers, totalAnswers);
            Log($"Quiz-Abschluss weitergegeben: quizId={quizId}, difficulty={difficulty}, score={correctAnswers}/{totalAnswers}");
        }

        #endregion

        #region Private

        private void ResolveReferences()
        {
            if (progressManager == null)
            {
                progressManager = FindAnyObjectByType<ProgressManager>(FindObjectsInactive.Include);
            }
        }

        private void Log(string message)
        {
            if (!enableDebugLogs)
            {
                return;
            }

            Debug.Log($"[{nameof(QuizProgressReporter)}] {message}", this);
        }

        #endregion
    }
}
