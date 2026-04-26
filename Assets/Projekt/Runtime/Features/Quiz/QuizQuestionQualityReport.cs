/*
 * Datei: QuizQuestionQualityReport.cs
 * Zweck: Buendelt das Ergebnis einer Quiz-Fragequalitaetspruefung.
 * Verantwortung: Haelt Score, Hinweise und Query-Helfer fuer Review-Workflows.
 * Abhaengigkeiten: QuizQuestionQualityIssue, QuizQuestionQualitySeverity, System.Collections.Generic.
 * Verwendung: Wird von QuizQuestionQualityEvaluator erzeugt und kann spaeter von Review-UI oder Tools gelesen werden.
 */

using System.Collections.Generic;

namespace ITAA.Quiz
{
    public class QuizQuestionQualityReport
    {
        private readonly List<QuizQuestionQualityIssue> issues = new List<QuizQuestionQualityIssue>();

        public QuizQuestionQualityReport(int score)
        {
            Score = ClampScore(score);
        }

        public int Score { get; private set; }
        public IReadOnlyList<QuizQuestionQualityIssue> Issues => issues;
        public bool HasErrors => CountIssues(QuizQuestionQualitySeverity.Error) > 0;
        public bool HasWarnings => CountIssues(QuizQuestionQualitySeverity.Warning) > 0;
        public bool IsUsable => !HasErrors;

        public void AddIssue(QuizQuestionQualityIssue issue, int scorePenalty)
        {
            issues.Add(issue);
            Score = ClampScore(Score - scorePenalty);
        }

        public int CountIssues(QuizQuestionQualitySeverity severity)
        {
            int count = 0;

            for (int i = 0; i < issues.Count; i++)
            {
                if (issues[i].Severity == severity)
                {
                    count++;
                }
            }

            return count;
        }

        private static int ClampScore(int score)
        {
            if (score < 0)
            {
                return 0;
            }

            if (score > 100)
            {
                return 100;
            }

            return score;
        }
    }
}
