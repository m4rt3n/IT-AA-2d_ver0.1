/*
 * Datei: BerndQuizStarter.cs
 * Zweck: Verbindet Bernds Interaktion mit einem datengetriebenen QuizSet.
 * Verantwortung: Findet bei Bedarf das QuizPanel und startet Bernds lokales Quiz.
 * Abhaengigkeiten: QuizSet, QuizPanel.
 * Verwendung: Wird auf Bernds GameObject neben BerndAutoInteraction platziert.
 */

using ITAA.Quiz;
using ITAA.UI.Panels;
using UnityEngine;


namespace ITAA.NPC.Bernd
{
    [DisallowMultipleComponent]
    public class BerndQuizStarter : MonoBehaviour
    {
        [Header("Quiz")]
        [SerializeField] private QuizSet quizSet;
        [SerializeField] private QuizPanel quizPanel;
        [SerializeField] private bool autoFindQuizPanel = true;

        private void Awake()
        {
            ResolvePanel();
        }

        public void StartQuiz()
        {
            StartQuiz(null);
        }

        public void StartQuiz(global::System.Action closedCallback)
        {
            ResolvePanel();

            if (quizSet == null || quizPanel == null)
            {
                Debug.LogWarning($"[{nameof(BerndQuizStarter)}] QuizSet oder QuizPanel fehlt.", this);
                return;
            }

            quizPanel.StartQuiz(quizSet, closedCallback);
        }

        private void ResolvePanel()
        {
            if (quizPanel != null || !autoFindQuizPanel)
            {
                return;
            }

            quizPanel = FindAnyObjectByType<QuizPanel>(FindObjectsInactive.Include);
        }
    }
}
