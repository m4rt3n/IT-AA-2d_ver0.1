/*
 * Datei: ScenarioFailureCauseSelector.cs
 * Zweck: Waehlt aus einer ScenarioDefinition eine aktive Fehlerursache aus.
 * Verantwortung: Bietet deterministische Auswahl per Index oder ID sowie optionale Zufallsauswahl fuer Runtime-MVPs.
 * Abhaengigkeiten: ScenarioDefinition, ScenarioFailureCause, System.Collections.Generic, UnityEngine.Random.
 * Verwendung: Wird vom ScenarioManager genutzt und kann spaeter in Tests direkt mit festen Indizes verwendet werden.
 */

using System.Collections.Generic;
using UnityEngine;

namespace ITAA.Features.Scenarios
{
    public static class ScenarioFailureCauseSelector
    {
        public static ScenarioFailureCause SelectRandom(ScenarioDefinition definition)
        {
            IReadOnlyList<ScenarioFailureCause> enabledCauses = GetEnabledCauses(definition);

            if (enabledCauses.Count == 0)
            {
                return null;
            }

            int index = Random.Range(0, enabledCauses.Count);
            return enabledCauses[index];
        }

        public static ScenarioFailureCause SelectByIndex(ScenarioDefinition definition, int index)
        {
            IReadOnlyList<ScenarioFailureCause> enabledCauses = GetEnabledCauses(definition);

            if (index < 0 || index >= enabledCauses.Count)
            {
                return null;
            }

            return enabledCauses[index];
        }

        public static ScenarioFailureCause SelectById(ScenarioDefinition definition, string causeId)
        {
            if (definition == null || string.IsNullOrWhiteSpace(causeId))
            {
                return null;
            }

            return definition.FindFailureCauseById(causeId);
        }

        public static IReadOnlyList<ScenarioFailureCause> GetEnabledCauses(ScenarioDefinition definition)
        {
            List<ScenarioFailureCause> enabledCauses = new();

            if (definition == null || definition.FailureCauses == null)
            {
                return enabledCauses;
            }

            for (int i = 0; i < definition.FailureCauses.Count; i++)
            {
                ScenarioFailureCause cause = definition.FailureCauses[i];

                if (cause != null && cause.IsEnabled && cause.HasValidId())
                {
                    enabledCauses.Add(cause);
                }
            }

            return enabledCauses;
        }
    }
}
