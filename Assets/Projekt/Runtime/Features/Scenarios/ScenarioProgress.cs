/*
 * Datei: ScenarioProgress.cs
 * Zweck: Speichert den aktuellen Fortschritt eines aktiven IT-Lernszenarios.
 * Verantwortung: Verwaltet Status, aktuellen Schrittindex und abgeschlossene Schritt-IDs.
 * Abhaengigkeiten: ScenarioDefinition, ScenarioStatus, System.Collections.Generic.
 * Verwendung: Wird vom ScenarioManager zur Laufzeit aktualisiert und kann spaeter persistiert werden.
 */

using System;
using System.Collections.Generic;

namespace ITAA.Features.Scenarios
{
    [Serializable]
    public class ScenarioProgress
    {
        public string ScenarioId;
        public ScenarioStatus Status = ScenarioStatus.NotStarted;
        public int CurrentStepIndex;
        public List<string> CompletedStepIds = new();

        public void Reset()
        {
            ScenarioId = string.Empty;
            Status = ScenarioStatus.NotStarted;
            CurrentStepIndex = 0;
            CompletedStepIds.Clear();
        }

        public void Start(string scenarioId)
        {
            ScenarioId = scenarioId;
            Status = ScenarioStatus.Running;
            CurrentStepIndex = 0;
            CompletedStepIds.Clear();
        }

        public ScenarioStep GetCurrentStep(ScenarioDefinition definition)
        {
            if (definition == null || Status != ScenarioStatus.Running)
            {
                return null;
            }

            return definition.GetStepAt(CurrentStepIndex);
        }

        public bool CompleteCurrentStep(ScenarioDefinition definition)
        {
            ScenarioStep currentStep = GetCurrentStep(definition);

            if (currentStep == null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(currentStep.StepId) && !CompletedStepIds.Contains(currentStep.StepId))
            {
                CompletedStepIds.Add(currentStep.StepId);
            }

            CurrentStepIndex++;

            if (definition.Steps == null || CurrentStepIndex >= definition.Steps.Count)
            {
                Status = ScenarioStatus.Completed;
            }

            return true;
        }

        public void CompleteScenario(ScenarioDefinition definition)
        {
            if (definition != null && definition.Steps != null)
            {
                foreach (ScenarioStep step in definition.Steps)
                {
                    if (step == null || string.IsNullOrEmpty(step.StepId))
                    {
                        continue;
                    }

                    if (!CompletedStepIds.Contains(step.StepId))
                    {
                        CompletedStepIds.Add(step.StepId);
                    }
                }

                CurrentStepIndex = definition.Steps.Count;
            }

            Status = ScenarioStatus.Completed;
        }

        public void FailScenario()
        {
            Status = ScenarioStatus.Failed;
        }
    }
}
