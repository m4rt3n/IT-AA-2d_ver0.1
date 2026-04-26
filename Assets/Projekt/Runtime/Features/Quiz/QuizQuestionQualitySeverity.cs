/*
 * Datei: QuizQuestionQualitySeverity.cs
 * Zweck: Definiert Schweregrade fuer Hinweise aus der Quiz-Fragequalitaetspruefung.
 * Verantwortung: Stellt stabile Werte fuer Info-, Warn- und Fehlerhinweise bereit.
 * Abhaengigkeiten: Keine.
 * Verwendung: Wird von QuizQuestionQualityIssue und QuizQuestionQualityReport genutzt.
 */

namespace ITAA.Quiz
{
    public enum QuizQuestionQualitySeverity
    {
        Info = 0,
        Warning = 1,
        Error = 2
    }
}
