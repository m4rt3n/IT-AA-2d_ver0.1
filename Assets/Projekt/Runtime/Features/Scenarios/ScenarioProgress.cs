/*
 * Datei: ScenarioProgress.cs
 * Zweck: Speichert den aktuellen Fortschritt eines aktiven IT-Lernszenarios.
 * Verantwortung: Verwaltet Status, aktuellen Schrittindex, aktive Fehlerursache, Timerstatus, optionale Schrittabschluesse und abgeschlossene Schritt-IDs.
 * Abhaengigkeiten: ScenarioDefinition, ScenarioStatus, ScenarioTimerState, System.Collections.Generic.
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
        public string ActiveFailureCauseId;
        public ScenarioStatus Status = ScenarioStatus.NotStarted;
        public int CurrentStepIndex;
        public List<string> CompletedStepIds = new();
        public ScenarioTimerState Timer = new();

        public void Reset()
        {
            EnsureLists();

            ScenarioId = string.Empty;
            ActiveFailureCauseId = string.Empty;
            Status = ScenarioStatus.NotStarted;
            CurrentStepIndex = 0;
            CompletedStepIds.Clear();
            Timer.Clear();
        }

        public void Start(string scenarioId)
        {
            Start(scenarioId, null);
        }

        public void Start(string scenarioId, ScenarioFailureCause activeFailureCause)
        {
            EnsureLists();

            ScenarioId = scenarioId;
            ActiveFailureCauseId = activeFailureCause != null && activeFailureCause.HasValidId()
                ? activeFailureCause.CauseId
                : string.Empty;
            Status = ScenarioStatus.Running;
            CurrentStepIndex = 0;
            CompletedStepIds.Clear();
            Timer.Clear();
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

            MarkStepCompleted(currentStep);

            CurrentStepIndex++;

            if (definition.Steps == null || CurrentStepIndex >= definition.Steps.Count)
            {
                Status = ScenarioStatus.Completed;
            }

            return true;
        }

        public bool CompleteStep(ScenarioDefinition definition, string stepId)
        {
            if (definition == null || Status != ScenarioStatus.Running || string.IsNullOrWhiteSpace(stepId))
            {
                return false;
            }

            ScenarioStep currentStep = GetCurrentStep(definition);

            if (currentStep != null && currentStep.StepId == stepId)
            {
                return CompleteCurrentStep(definition);
            }

            ScenarioStep targetStep = definition.FindStepById(stepId);

            if (targetStep == null || !targetStep.IsOptional)
            {
                return false;
            }

            MarkStepCompleted(targetStep);
            return true;
        }

        public bool SkipCurrentOptionalStep(ScenarioDefinition definition)
        {
            ScenarioStep currentStep = GetCurrentStep(definition);

            if (currentStep == null || !currentStep.IsOptional)
            {
                return false;
            }

            return CompleteCurrentStep(definition);
        }

        public float GetProgress01(ScenarioDefinition definition)
        {
            EnsureLists();

            if (definition == null || definition.Steps == null || definition.Steps.Count == 0)
            {
                return 0f;
            }

            return Math.Min(1f, Math.Max(0f, (float)CompletedStepIds.Count / definition.Steps.Count));
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

                    MarkStepCompleted(step);
                }

                CurrentStepIndex = definition.Steps.Count;
            }

            Status = ScenarioStatus.Completed;
        }

        public void FailScenario()
        {
            Status = ScenarioStatus.Failed;
        }

        public void StartTimer(ScenarioTimeLimit timeLimit, string fallbackTimerId)
        {
            EnsureTimer();
            Timer.Start(timeLimit, fallbackTimerId);
        }

        public void ClearTimer()
        {
            EnsureTimer();
            Timer.Clear();
        }

        public bool TickTimer(float deltaSeconds)
        {
            EnsureTimer();
            return Timer.Tick(deltaSeconds);
        }

        private void MarkStepCompleted(ScenarioStep step)
        {
            EnsureLists();

            if (step == null || string.IsNullOrEmpty(step.StepId) || CompletedStepIds.Contains(step.StepId))
            {
                return;
            }

            CompletedStepIds.Add(step.StepId);
        }

        private void EnsureLists()
        {
            CompletedStepIds ??= new List<string>();
            EnsureTimer();
        }

        private void EnsureTimer()
        {
            Timer ??= new ScenarioTimerState();
        }
    }
}
