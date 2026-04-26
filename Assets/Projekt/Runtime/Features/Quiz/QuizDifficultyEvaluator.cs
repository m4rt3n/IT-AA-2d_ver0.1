/*
 * Datei: QuizDifficultyEvaluator.cs
 * Zweck: Empfiehlt eine naechste Quiz-Schwierigkeit anhand bisheriger Quizleistung.
 * Verantwortung: Bewertet Trefferquote und Antwortanzahl, ohne QuizSets, UI oder Savegame direkt zu veraendern.
 * Abhaengigkeiten: QuizDifficulty, QuizDifficultyPerformance, System.
 * Verwendung: Kann von QuizRunner, Progress-Adaptern oder spaeteren Quiz-Managern optional abgefragt werden.
 */

using System;

namespace ITAA.Quiz
{
    public static class QuizDifficultyEvaluator
    {
        public const int DefaultMinimumAnswers = 3;
        public const float DefaultIncreaseThreshold = 0.8f;
        public const float DefaultDecreaseThreshold = 0.45f;

        public static QuizDifficulty RecommendNextDifficulty(QuizDifficultyPerformance performance)
        {
            return RecommendNextDifficulty(
                performance,
                DefaultMinimumAnswers,
                DefaultIncreaseThreshold,
                DefaultDecreaseThreshold);
        }

        public static QuizDifficulty RecommendNextDifficulty(
            QuizDifficultyPerformance performance,
            int minimumAnswers,
            float increaseThreshold,
            float decreaseThreshold)
        {
            if (!performance.HasEnoughAnswers(minimumAnswers))
            {
                return performance.CurrentDifficulty;
            }

            float safeIncreaseThreshold = Clamp01(increaseThreshold);
            float safeDecreaseThreshold = Clamp01(decreaseThreshold);

            if (safeDecreaseThreshold > safeIncreaseThreshold)
            {
                safeDecreaseThreshold = safeIncreaseThreshold;
            }

            if (performance.Accuracy01 >= safeIncreaseThreshold)
            {
                return Increase(performance.CurrentDifficulty);
            }

            if (performance.Accuracy01 <= safeDecreaseThreshold)
            {
                return Decrease(performance.CurrentDifficulty);
            }

            return performance.CurrentDifficulty;
        }

        public static QuizDifficulty Increase(QuizDifficulty difficulty)
        {
            int nextValue = Math.Min((int)QuizDifficulty.Expert, (int)difficulty + 1);
            return (QuizDifficulty)nextValue;
        }

        public static QuizDifficulty Decrease(QuizDifficulty difficulty)
        {
            int nextValue = Math.Max((int)QuizDifficulty.Easy, (int)difficulty - 1);
            return (QuizDifficulty)nextValue;
        }

        private static float Clamp01(float value)
        {
            if (value < 0f)
            {
                return 0f;
            }

            if (value > 1f)
            {
                return 1f;
            }

            return value;
        }
    }
}
