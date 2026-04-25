/*
 * Datei: QuizQuestion.cs
 * Zweck: Beschreibt eine einzelne Quizfrage inklusive Antworten und optionaler Erklaerung.
 * Verantwortung: Haelt Fragetext, Antwortoptionen, korrekten Antwortindex und Feedbacktext.
 * Abhaengigkeiten: QuizAnswerOption, System.Serializable, System.Collections.Generic.
 * Verwendung: Wird in QuizSet-Assets serialisiert und vom QuizRunner ausgewertet.
 */

using System;
using System.Collections.Generic;

namespace ITAA.Quiz
{
    [Serializable]
    public class QuizQuestion
    {
        public string QuestionText;
        public List<QuizAnswerOption> AnswerOptions = new();
        public int CorrectAnswerIndex;
        public string Explanation;

        public bool HasValidAnswerIndex()
        {
            return AnswerOptions != null &&
                   AnswerOptions.Count > 0 &&
                   CorrectAnswerIndex >= 0 &&
                   CorrectAnswerIndex < AnswerOptions.Count;
        }
    }
}
