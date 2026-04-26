/*
 * Datei: ScenarioFailureCause.cs
 * Zweck: Beschreibt eine optionale Fehlerursache innerhalb eines IT-Lernszenarios.
 * Verantwortung: Haelt stabile Ursache-ID, Titel, Beschreibung, Hinweistext und optionale Integrations-IDs.
 * Abhaengigkeiten: System.Serializable.
 * Verwendung: Wird in ScenarioDefinition serialisiert und vom ScenarioManager beim Szenariostart optional ausgewaehlt.
 */

using System;

namespace ITAA.Features.Scenarios
{
    [Serializable]
    public class ScenarioFailureCause
    {
        public string CauseId;
        public string Title;
        public string Description;
        public string Hint;
        public string RelatedStepId;
        public string RelatedKnowledgeArticleId;
        public bool IsEnabled = true;

        public ScenarioFailureCause()
        {
        }

        public ScenarioFailureCause(string causeId, string title, string description)
        {
            CauseId = causeId;
            Title = title;
            Description = description;
        }

        public bool HasValidId()
        {
            return !string.IsNullOrWhiteSpace(CauseId);
        }

        public string GetResolvedTitle()
        {
            return string.IsNullOrWhiteSpace(Title) ? CauseId : Title;
        }
    }
}
