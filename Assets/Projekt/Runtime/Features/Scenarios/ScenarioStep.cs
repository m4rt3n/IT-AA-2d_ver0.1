/*
 * Datei: ScenarioStep.cs
 * Zweck: Beschreibt einen einzelnen Schritt innerhalb eines IT-Lernszenarios.
 * Verantwortung: Haelt Schritt-ID, Titel, Beschreibung und optionale Integrationshinweise fuer spaetere Systeme.
 * Abhaengigkeiten: System.Serializable.
 * Verwendung: Wird in ScenarioDefinition serialisiert und vom ScenarioManager als aktuelles Ziel ausgegeben.
 */

using System;

namespace ITAA.Features.Scenarios
{
    [Serializable]
    public class ScenarioStep
    {
        public string StepId;
        public string Title;
        public string Description;
        public string LinkedQuizId;
        public string LinkedDialogueId;
        public bool IsOptional;

        public ScenarioStep()
        {
        }

        public ScenarioStep(string stepId, string title, string description)
        {
            StepId = stepId;
            Title = title;
            Description = description;
        }

        public bool HasValidId()
        {
            return !string.IsNullOrEmpty(StepId);
        }
    }
}
