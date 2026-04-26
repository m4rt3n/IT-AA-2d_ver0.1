/*
 * Datei: ScenarioStepType.cs
 * Zweck: Definiert einfache Schrittarten fuer mehrstufige Szenarien.
 * Verantwortung: Stellt stabile Typen fuer Objective-, Dialogue-, Quiz-, Task- und Checkpoint-Schritte bereit.
 * Abhaengigkeiten: Keine.
 * Verwendung: Wird von ScenarioStep genutzt, damit spaetere UI-, Quiz- und Dialog-Adapter Schritte einordnen koennen.
 */

namespace ITAA.Features.Scenarios
{
    public enum ScenarioStepType
    {
        Objective = 0,
        Dialogue = 1,
        Quiz = 2,
        Task = 3,
        Checkpoint = 4
    }
}
