/*
 * Datei: QuizTopicProgressFormatter.cs
 * Zweck: Formatiert Quiz-Themenfortschritt fuer HUD- oder UI-Anzeigen.
 * Verantwortung: Erstellt kurze, robuste Texte aus TopicProgress-Daten, ohne UI oder Savegame direkt zu kennen.
 * Abhaengigkeiten: ITAA.Features.Progress.TopicProgress, UnityEngine.Mathf.
 * Verwendung: Kann von HudController, QuizPanel-Adaptern oder spaeteren Progress-Views optional genutzt werden.
 */

using ITAA.Features.Progress;
using UnityEngine;

namespace ITAA.Quiz
{
    public static class QuizTopicProgressFormatter
    {
        public static string FormatTopicProgress(string topic, TopicProgress progress)
        {
            string resolvedTopic = string.IsNullOrWhiteSpace(topic) ? "General" : topic.Trim();

            if (progress == null || progress.Answers <= 0)
            {
                return $"{resolvedTopic}: 0 / 0";
            }

            int correctAnswers = Mathf.Max(0, progress.CorrectAnswers);
            int totalAnswers = Mathf.Max(0, progress.Answers);
            int accuracyPercent = Mathf.RoundToInt(progress.GetAccuracy01() * 100f);

            return $"{resolvedTopic}: {correctAnswers} / {totalAnswers} ({accuracyPercent}%)";
        }
    }
}
