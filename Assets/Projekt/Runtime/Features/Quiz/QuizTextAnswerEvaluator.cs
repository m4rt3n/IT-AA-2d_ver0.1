/*
 * Datei: QuizTextAnswerEvaluator.cs
 * Zweck: Bewertet Freitextantworten fuer Quizfragen robuster als ein reiner String-Vergleich.
 * Verantwortung: Normalisiert Texte, vergleicht akzeptierte Antworten und erlaubt kontrolliert kleine Tippfehler.
 * Abhaengigkeiten: System, System.Globalization, System.Text.
 * Verwendung: Wird von QuizRunner fuer FreeText-Fragen genutzt, ohne Multiple-Choice-Auswertung zu veraendern.
 */

using System;
using System.Globalization;
using System.Text;

namespace ITAA.Quiz
{
    public static class QuizTextAnswerEvaluator
    {
        public static bool IsAnswerAccepted(string userAnswer, string acceptedAnswer, bool allowFuzzyMatch, int maxDistance)
        {
            string normalizedUserAnswer = Normalize(userAnswer);
            string normalizedAcceptedAnswer = Normalize(acceptedAnswer);

            if (string.IsNullOrWhiteSpace(normalizedUserAnswer) ||
                string.IsNullOrWhiteSpace(normalizedAcceptedAnswer))
            {
                return false;
            }

            if (normalizedUserAnswer == normalizedAcceptedAnswer)
            {
                return true;
            }

            if (!allowFuzzyMatch)
            {
                return false;
            }

            int safeMaxDistance = Math.Max(0, maxDistance);
            if (safeMaxDistance <= 0)
            {
                return false;
            }

            int lengthDifference = Math.Abs(normalizedUserAnswer.Length - normalizedAcceptedAnswer.Length);
            if (lengthDifference > safeMaxDistance)
            {
                return false;
            }

            return GetLevenshteinDistance(normalizedUserAnswer, normalizedAcceptedAnswer) <= safeMaxDistance;
        }

        public static string Normalize(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            string normalized = value.Trim().ToLowerInvariant().Normalize(NormalizationForm.FormD);
            StringBuilder builder = new StringBuilder(normalized.Length);
            bool previousWasSpace = false;

            for (int i = 0; i < normalized.Length; i++)
            {
                char character = normalized[i];
                UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(character);

                if (category == UnicodeCategory.NonSpacingMark)
                {
                    continue;
                }

                if (char.IsLetterOrDigit(character))
                {
                    builder.Append(character);
                    previousWasSpace = false;
                    continue;
                }

                if (char.IsWhiteSpace(character) || character == '-' || character == '_' || character == '/')
                {
                    if (!previousWasSpace && builder.Length > 0)
                    {
                        builder.Append(' ');
                        previousWasSpace = true;
                    }
                }
            }

            return builder.ToString().Trim().Normalize(NormalizationForm.FormC);
        }

        private static int GetLevenshteinDistance(string left, string right)
        {
            if (left == right)
            {
                return 0;
            }

            if (left.Length == 0)
            {
                return right.Length;
            }

            if (right.Length == 0)
            {
                return left.Length;
            }

            int[] previous = new int[right.Length + 1];
            int[] current = new int[right.Length + 1];

            for (int j = 0; j <= right.Length; j++)
            {
                previous[j] = j;
            }

            for (int i = 1; i <= left.Length; i++)
            {
                current[0] = i;

                for (int j = 1; j <= right.Length; j++)
                {
                    int substitutionCost = left[i - 1] == right[j - 1] ? 0 : 1;
                    int deletion = previous[j] + 1;
                    int insertion = current[j - 1] + 1;
                    int substitution = previous[j - 1] + substitutionCost;
                    current[j] = Math.Min(Math.Min(deletion, insertion), substitution);
                }

                int[] temp = previous;
                previous = current;
                current = temp;
            }

            return previous[right.Length];
        }
    }
}
