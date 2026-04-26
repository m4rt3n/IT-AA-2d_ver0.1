/*
 * Datei: QuizQuestion.cs
 * Zweck: Beschreibt eine einzelne Quizfrage inklusive Schwierigkeit, Thema, Antworten, optionalen Freitextantworten und Erklaerung.
 * Verantwortung: Haelt Fragetext, optionale Metadaten, Antwortoptionen, korrekten Antwortindex, akzeptierte Freitextantworten und Feedbacktext.
 * Abhaengigkeiten: QuizAnswerOption, QuizDifficulty, System.Serializable, System.Collections.Generic.
 * Verwendung: Wird in QuizSet-Assets serialisiert und vom QuizRunner ausgewertet.
 */

using System;
using System.Collections.Generic;

namespace ITAA.Quiz
{
    [Serializable]
    public class QuizQuestion
    {
        public string QuestionId;
        public string Topic;
        public QuizDifficulty Difficulty = QuizDifficulty.Medium;
        public string QuestionText;
        public List<QuizAnswerOption> AnswerOptions = new();
        public int CorrectAnswerIndex;
        public List<string> AcceptedTextAnswers = new();
        public bool AllowFuzzyTextMatch = true;
        public int MaxTextAnswerDistance = 1;
        public string Explanation;

        public bool HasValidAnswerIndex()
        {
            return AnswerOptions != null &&
                   AnswerOptions.Count > 0 &&
                   CorrectAnswerIndex >= 0 &&
                   CorrectAnswerIndex < AnswerOptions.Count;
        }

        public bool HasAcceptedTextAnswers()
        {
            return AcceptedTextAnswers != null && AcceptedTextAnswers.Count > 0;
        }
    }
}
