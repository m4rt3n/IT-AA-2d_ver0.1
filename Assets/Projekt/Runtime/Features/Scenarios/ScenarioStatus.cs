/*
 * Datei: ScenarioStatus.cs
 * Zweck: Beschreibt den Laufzeitstatus eines IT-Lernszenarios.
 * Verantwortung: Stellt eindeutige Statuswerte fuer ScenarioProgress und ScenarioManager bereit.
 * Abhaengigkeiten: Keine.
 * Verwendung: Wird vom ScenarioManager genutzt, um Start, Abschluss und Fehlerzustand eines Szenarios zu melden.
 */

namespace ITAA.Features.Scenarios
{
    public enum ScenarioStatus
    {
        NotStarted,
        Running,
        Completed,
        Failed
    }
}
