/*
 * Datei: QuizQuestionQualityEvaluator.cs
 * Zweck: Bewertet Quizfragen anhand einfacher Mindestkriterien.
 * Verantwortung: Prueft Text, Thema, Schwierigkeit, Antworten und Erklaerung, ohne Fragen automatisch zu veraendern.
 * Abhaengigkeiten: QuizQuestion, QuizQuestionQualityReport, QuizQuestionQualityIssue, System, System.Collections.Generic.
 * Verwendung: Kann spaeter von Review-Mode, Draft-Workflows oder DevTools optional genutzt werden.
 */

using System;
using System.Collections.Generic;

namespace ITAA.Quiz
{
    public static class QuizQuestionQualityEvaluator
    {
        private const int MinimumQuestionTextLength = 10;
        private const int MinimumExplanationLength = 12;

        public static QuizQuestionQualityReport Evaluate(QuizQuestion question)
        {
            QuizQuestionQualityReport report = new QuizQuestionQualityReport(100);

            if (question == null)
            {
                AddError(report, "question_missing", "Keine Quizfrage vorhanden.", 100);
                return report;
            }

            EvaluateQuestionText(question, report);
            EvaluateTopic(question, report);
            EvaluateDifficulty(question, report);
            EvaluateAnswerModel(question, report);
            EvaluateExplanation(question, report);

            if (report.Issues.Count == 0)
            {
                report.AddIssue(
                    new QuizQuestionQualityIssue(
                        QuizQuestionQualitySeverity.Info,
                        "quality_ok",
                        "Keine offensichtlichen Qualitaetsprobleme gefunden."),
                    0);
            }

            return report;
        }

        private static void EvaluateQuestionText(QuizQuestion question, QuizQuestionQualityReport report)
        {
            if (string.IsNullOrWhiteSpace(question.QuestionText))
            {
                AddError(report, "question_text_missing", "Fragetext fehlt.", 25);
                return;
            }

            if (question.QuestionText.Trim().Length < MinimumQuestionTextLength)
            {
                AddWarning(report, "question_text_short", "Fragetext ist sehr kurz.", 10);
            }
        }

        private static void EvaluateTopic(QuizQuestion question, QuizQuestionQualityReport report)
        {
            if (string.IsNullOrWhiteSpace(question.Topic))
            {
                AddWarning(report, "topic_missing", "Thema fehlt; Themenfortschritt und Filter bleiben dadurch ungenau.", 10);
            }
        }

        private static void EvaluateDifficulty(QuizQuestion question, QuizQuestionQualityReport report)
        {
            if (!Enum.IsDefined(typeof(QuizDifficulty), question.Difficulty))
            {
                AddWarning(report, "difficulty_invalid", "Schwierigkeit ist kein bekannter QuizDifficulty-Wert.", 10);
            }
        }

        private static void EvaluateAnswerModel(QuizQuestion question, QuizQuestionQualityReport report)
        {
            int optionCount = question.AnswerOptions != null ? question.AnswerOptions.Count : 0;
            bool hasOptions = optionCount > 0;
            bool hasTextAnswers = question.HasAcceptedTextAnswers();

            if (!hasOptions && !hasTextAnswers)
            {
                AddError(report, "answers_missing", "Antwortoptionen oder akzeptierte Freitextantworten fehlen.", 30);
                return;
            }

            if (hasOptions)
            {
                EvaluateMultipleChoiceAnswers(question, report);
            }

            if (hasTextAnswers)
            {
                EvaluateFreeTextAnswers(question, report);
            }

            if (hasOptions && hasTextAnswers)
            {
                AddWarning(report, "mixed_answer_modes", "Multiple-Choice- und Freitextantworten sind gleichzeitig gesetzt.", 5);
            }
        }

        private static void EvaluateMultipleChoiceAnswers(QuizQuestion question, QuizQuestionQualityReport report)
        {
            if (question.AnswerOptions.Count < 2)
            {
                AddWarning(report, "answer_options_few", "Multiple-Choice-Fragen sollten mindestens zwei Antwortoptionen haben.", 10);
            }

            if (!question.HasValidAnswerIndex())
            {
                AddError(report, "correct_answer_invalid", "Index der korrekten Antwort ist ungueltig.", 25);
            }

            HashSet<string> normalizedAnswers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < question.AnswerOptions.Count; i++)
            {
                QuizAnswerOption option = question.AnswerOptions[i];

                if (option == null || string.IsNullOrWhiteSpace(option.Text))
                {
                    AddWarning(report, "answer_option_empty", $"Antwortoption {i + 1} ist leer.", 8);
                    continue;
                }

                string normalizedText = option.Text.Trim();

                if (!normalizedAnswers.Add(normalizedText))
                {
                    AddWarning(report, "answer_option_duplicate", $"Antwortoption ist doppelt vorhanden: {normalizedText}", 8);
                }
            }
        }

        private static void EvaluateFreeTextAnswers(QuizQuestion question, QuizQuestionQualityReport report)
        {
            int validAnswers = 0;
            HashSet<string> normalizedAnswers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < question.AcceptedTextAnswers.Count; i++)
            {
                string answer = question.AcceptedTextAnswers[i];

                if (string.IsNullOrWhiteSpace(answer))
                {
                    AddWarning(report, "text_answer_empty", $"Freitextantwort {i + 1} ist leer.", 8);
                    continue;
                }

                validAnswers++;
                string normalizedAnswer = QuizTextAnswerEvaluator.Normalize(answer);

                if (!normalizedAnswers.Add(normalizedAnswer))
                {
                    AddWarning(report, "text_answer_duplicate", $"Freitextantwort ist doppelt vorhanden: {answer.Trim()}", 5);
                }
            }

            if (validAnswers <= 0)
            {
                AddError(report, "text_answers_invalid", "Es gibt keine gueltige akzeptierte Freitextantwort.", 25);
            }
        }

        private static void EvaluateExplanation(QuizQuestion question, QuizQuestionQualityReport report)
        {
            if (string.IsNullOrWhiteSpace(question.Explanation))
            {
                AddWarning(report, "explanation_missing", "Erklaerung fehlt; Review und Lernfeedback sind dadurch schwach.", 10);
                return;
            }

            if (question.Explanation.Trim().Length < MinimumExplanationLength)
            {
                AddWarning(report, "explanation_short", "Erklaerung ist sehr kurz.", 5);
            }
        }

        private static void AddError(QuizQuestionQualityReport report, string code, string message, int penalty)
        {
            report.AddIssue(new QuizQuestionQualityIssue(QuizQuestionQualitySeverity.Error, code, message), penalty);
        }

        private static void AddWarning(QuizQuestionQualityReport report, string code, string message, int penalty)
        {
            report.AddIssue(new QuizQuestionQualityIssue(QuizQuestionQualitySeverity.Warning, code, message), penalty);
        }
    }
}
