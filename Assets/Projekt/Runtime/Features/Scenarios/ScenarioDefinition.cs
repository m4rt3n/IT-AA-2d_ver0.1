/*
 * Datei: ScenarioDefinition.cs
 * Zweck: Definiert ein IT-Lernszenario als datengetriebenes Unity-Asset.
 * Verantwortung: Buendelt Scenario-ID, Metadaten, Schritte und optionale spaetere Verknuepfungen zu Quiz/Dialog.
 * Abhaengigkeiten: ScenarioStep, ScriptableObject, System.Collections.Generic.
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
        public List<ScenarioStep> Steps = new();

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
            Steps = steps ?? new List<ScenarioStep>();
        }
    }
}
