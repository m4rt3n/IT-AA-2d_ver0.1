/*
 * Datei: ScenarioStep.cs
 * Zweck: Beschreibt einen einzelnen Schritt innerhalb eines IT-Lernszenarios.
 * Verantwortung: Haelt Schritt-ID, Typ, Titel, Beschreibung, optionales Zeitlimit und Integrationshinweise fuer spaetere Systeme.
 * Abhaengigkeiten: ScenarioStepType, ScenarioTimeLimit, System.Serializable.
 * Verwendung: Wird in ScenarioDefinition serialisiert und vom ScenarioManager als aktuelles Ziel ausgegeben.
 */

using System;

namespace ITAA.Features.Scenarios
{
    [Serializable]
    public class ScenarioStep
    {
        public string StepId;
        public ScenarioStepType StepType = ScenarioStepType.Objective;
        public string Title;
        public string Description;
        public string CompletionKey;
        public string LinkedQuizId;
        public string LinkedDialogueId;
        public string ProgressQuestId;
        public ScenarioTimeLimit TimeLimit = new();
        public bool IsOptional;
        public bool RequiresManualCompletion = true;

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

        public string GetResolvedCompletionKey()
        {
            if (!string.IsNullOrWhiteSpace(CompletionKey))
            {
                return CompletionKey.Trim();
            }

            return string.IsNullOrWhiteSpace(StepId) ? string.Empty : StepId.Trim();
        }

        public bool HasTimeLimit()
        {
            return TimeLimit != null && TimeLimit.HasValidDuration();
        }
    }
}
