/*
 * Datei: QuizDifficultyPerformance.cs
 * Zweck: Beschreibt die bisherige Leistung innerhalb einer Quizrunde.
 * Verantwortung: Haelt beantwortete Fragen, richtige Antworten, aktuelle Schwierigkeit und berechnete Genauigkeit.
 * Abhaengigkeiten: QuizDifficulty, System.Serializable.
 * Verwendung: Wird an QuizDifficultyEvaluator uebergeben, um optional eine naechste Schwierigkeit vorzuschlagen.
 */

using System;

namespace ITAA.Quiz
{
    [Serializable]
    public readonly struct QuizDifficultyPerformance
    {
        public QuizDifficultyPerformance(int answeredQuestions, int correctAnswers, QuizDifficulty currentDifficulty)
        {
            AnsweredQuestions = Math.Max(0, answeredQuestions);
            CorrectAnswers = Math.Max(0, Math.Min(correctAnswers, AnsweredQuestions));
            CurrentDifficulty = currentDifficulty;
        }

        public int AnsweredQuestions { get; }
        public int CorrectAnswers { get; }
        public QuizDifficulty CurrentDifficulty { get; }

        public float Accuracy01
        {
            get
            {
                if (AnsweredQuestions <= 0)
                {
                    return 0f;
                }

                return (float)CorrectAnswers / AnsweredQuestions;
            }
        }

        public bool HasEnoughAnswers(int minimumAnswers)
        {
            return AnsweredQuestions >= Math.Max(1, minimumAnswers);
        }
    }
}
