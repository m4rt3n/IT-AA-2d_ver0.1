/*
 * Datei: QuizDifficulty.cs
 * Zweck: Definiert einfache Schwierigkeitsstufen fuer Quizfragen.
 * Verantwortung: Stellt stabile Difficulty-Werte fuer Datenmodelle und optionale Empfehlungssysteme bereit.
 * Abhaengigkeiten: Keine.
 * Verwendung: Wird von QuizQuestion und QuizDifficultyEvaluator genutzt, ohne bestehende QuizSets automatisch umzustellen.
 */

namespace ITAA.Quiz
{
    public enum QuizDifficulty
    {
        Easy = 0,
        Medium = 1,
        Hard = 2,
        Expert = 3
    }
}
