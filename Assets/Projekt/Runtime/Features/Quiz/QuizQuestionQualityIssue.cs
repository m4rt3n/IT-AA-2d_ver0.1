/*
 * Datei: QuizQuestionQualityIssue.cs
 * Zweck: Beschreibt einen einzelnen Qualitaetshinweis zu einer Quizfrage.
 * Verantwortung: Haelt Schweregrad, Code und lesbare Beschreibung eines Pruefergebnisses.
 * Abhaengigkeiten: QuizQuestionQualitySeverity.
 * Verwendung: Wird in QuizQuestionQualityReport gesammelt und kann spaeter in Review-UI angezeigt werden.
 */

namespace ITAA.Quiz
{
    public readonly struct QuizQuestionQualityIssue
    {
        public QuizQuestionQualityIssue(QuizQuestionQualitySeverity severity, string code, string message)
        {
            Severity = severity;
            Code = string.IsNullOrWhiteSpace(code) ? "unknown" : code.Trim();
            Message = string.IsNullOrWhiteSpace(message) ? "Keine Beschreibung." : message.Trim();
        }

        public QuizQuestionQualitySeverity Severity { get; }
        public string Code { get; }
        public string Message { get; }
    }
}
