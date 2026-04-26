/*
 * Datei: ScenarioDefinition.cs
 * Zweck: Definiert ein IT-Lernszenario als datengetriebenes Unity-Asset.
 * Verantwortung: Buendelt Scenario-ID, Metadaten, Schritte, optionale Fehlerursachen, Zeitlimits und spaetere Verknuepfungen zu Quiz/Dialog.
 * Abhaengigkeiten: ScenarioStep, ScenarioFailureCause, ScenarioTimeLimit, ScriptableObject, System.Collections.Generic.
 * Verwendung: Wird vom ScenarioManager referenziert oder als Runtime-Demo-Szenario erzeugt.
 */

using System.Collections.Generic;
using UnityEngine;

namespace ITAA.Features.Scenarios
{
    [CreateAssetMenu(fileName = "ScenarioDefinition", menuName = "IT-AA/Scenarios/Scenario Definition")]
    public class ScenarioDefinition : ScriptableObject
    {
        public string ScenarioId;
        public string Title;
        public string Description;
        public string Topic;
        public string Difficulty;
        public string StartDialogueId;
        public string EndDialogueId;
        public string RequiredQuizId;
        public ScenarioTimeLimit TimeLimit = new();
        public List<ScenarioStep> Steps = new();
        public List<ScenarioFailureCause> FailureCauses = new();

        public bool HasValidId()
        {
            return !string.IsNullOrEmpty(ScenarioId);
        }

        public bool HasSteps()
        {
            return Steps != null && Steps.Count > 0;
        }

        public ScenarioStep GetStepAt(int index)
        {
            if (Steps == null || index < 0 || index >= Steps.Count)
            {
                return null;
            }

            return Steps[index];
        }

        public ScenarioStep FindStepById(string stepId)
        {
            if (Steps == null || string.IsNullOrWhiteSpace(stepId))
            {
                return null;
            }

            for (int i = 0; i < Steps.Count; i++)
            {
                ScenarioStep step = Steps[i];

                if (step != null && step.StepId == stepId)
                {
                    return step;
                }
            }

            return null;
        }

        public ScenarioStep FindStepByCompletionKey(string completionKey)
        {
            if (Steps == null || string.IsNullOrWhiteSpace(completionKey))
            {
                return null;
            }

            string resolvedKey = completionKey.Trim();

            for (int i = 0; i < Steps.Count; i++)
            {
                ScenarioStep step = Steps[i];

                if (step != null && step.GetResolvedCompletionKey() == resolvedKey)
                {
                    return step;
                }
            }

            return null;
        }

        public ScenarioStep FindStepByLinkedQuizId(string quizId)
        {
            if (Steps == null || string.IsNullOrWhiteSpace(quizId))
            {
                return null;
            }

            for (int i = 0; i < Steps.Count; i++)
            {
                ScenarioStep step = Steps[i];

                if (step != null && step.LinkedQuizId == quizId)
                {
                    return step;
                }
            }

            return null;
        }

        public ScenarioStep FindStepByLinkedDialogueId(string dialogueId)
        {
            if (Steps == null || string.IsNullOrWhiteSpace(dialogueId))
            {
                return null;
            }

            for (int i = 0; i < Steps.Count; i++)
            {
                ScenarioStep step = Steps[i];

                if (step != null && step.LinkedDialogueId == dialogueId)
                {
                    return step;
                }
            }

            return null;
        }

        public ScenarioFailureCause FindFailureCauseById(string causeId)
        {
            if (FailureCauses == null || string.IsNullOrWhiteSpace(causeId))
            {
                return null;
            }

            string resolvedId = causeId.Trim();

            for (int i = 0; i < FailureCauses.Count; i++)
            {
                ScenarioFailureCause cause = FailureCauses[i];

                if (cause != null && cause.CauseId == resolvedId)
                {
                    return cause;
                }
            }

            return null;
        }

        public bool HasEnabledFailureCauses()
        {
            if (FailureCauses == null)
            {
                return false;
            }

            for (int i = 0; i < FailureCauses.Count; i++)
            {
                ScenarioFailureCause cause = FailureCauses[i];

                if (cause != null && cause.IsEnabled && cause.HasValidId())
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasScenarioTimeLimit()
        {
            return TimeLimit != null && TimeLimit.HasValidDuration();
        }

        public void Initialize(
            string scenarioId,
            string title,
            string description,
            string topic,
            string difficulty,
            List<ScenarioStep> steps)
        {
            ScenarioId = scenarioId;
            Title = title;
            Description = description;
            Topic = topic;
            Difficulty = difficulty;
            TimeLimit ??= new ScenarioTimeLimit();
            Steps = steps ?? new List<ScenarioStep>();
            FailureCauses ??= new List<ScenarioFailureCause>();
        }
    }
}
