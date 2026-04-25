/*
 * Datei: QuizResult.cs
 * Zweck: Beschreibt das Ergebnis einer beantworteten Quizfrage.
 * Verantwortung: Haelt Frage, gewaehlten Antwortindex und Richtig/Falsch-Status.
 * Abhaengigkeiten: QuizQuestion.
 * Verwendung: Wird vom QuizRunner an UI- oder Gameplay-Systeme zur Auswertung weitergegeben.
 */

namespace ITAA.Quiz
{
    public readonly struct QuizResult
    {
        public QuizResult(QuizQuestion question, int selectedAnswerIndex, bool isCorrect)
        {
            Question = question;
            SelectedAnswerIndex = selectedAnswerIndex;
            IsCorrect = isCorrect;
        }

        public QuizQuestion Question { get; }
        public int SelectedAnswerIndex { get; }
        public bool IsCorrect { get; }
    }
}
