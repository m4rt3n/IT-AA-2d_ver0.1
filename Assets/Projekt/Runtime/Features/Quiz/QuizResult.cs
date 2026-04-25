/*
 * Datei: QuizResult.cs
 * Zweck: Beschreibt das Ergebnis einer beantworteten Quizfrage.
 * Verantwortung: Haelt Frage, gewaehlten Antwortindex, optionale Freitextantwort und Richtig/Falsch-Status.
 * Abhaengigkeiten: QuizQuestion.
 * Verwendung: Wird vom QuizRunner an UI- oder Gameplay-Systeme zur Auswertung weitergegeben.
 */

namespace ITAA.Quiz
{
    public readonly struct QuizResult
    {
        public QuizResult(QuizQuestion question, int selectedAnswerIndex, bool isCorrect)
            : this(question, selectedAnswerIndex, null, isCorrect)
        {
        }

        public QuizResult(QuizQuestion question, int selectedAnswerIndex, string textAnswer, bool isCorrect)
        {
            Question = question;
            SelectedAnswerIndex = selectedAnswerIndex;
            TextAnswer = textAnswer;
            IsCorrect = isCorrect;
        }

        public QuizQuestion Question { get; }
        public int SelectedAnswerIndex { get; }
        public string TextAnswer { get; }
        public bool IsCorrect { get; }
    }
}
