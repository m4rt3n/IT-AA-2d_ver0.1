/*
 * Datei: ScenarioTimeLimit.cs
 * Zweck: Beschreibt ein optionales Zeitlimit fuer ein Szenario oder einen Szenarioschritt.
 * Verantwortung: Haelt eine stabile Timer-ID, Dauer, Warnschwelle und Aktiv-Status fuer datengetriebene Timer.
 * Abhaengigkeiten: System.Serializable.
 * Verwendung: Wird in ScenarioDefinition oder ScenarioStep serialisiert und vom ScenarioManager zur Laufzeit ausgewertet.
 */

using System;

namespace ITAA.Features.Scenarios
{
    [Serializable]
    public class ScenarioTimeLimit
    {
        public string TimerId;
        public float DurationSeconds;
        public float WarningThresholdSeconds = 10f;
        public bool IsEnabled;

        public ScenarioTimeLimit()
        {
        }

        public ScenarioTimeLimit(string timerId, float durationSeconds)
        {
            TimerId = timerId;
            DurationSeconds = durationSeconds;
            IsEnabled = durationSeconds > 0f;
        }

        public bool HasValidDuration()
        {
            return IsEnabled && DurationSeconds > 0f;
        }

        public string GetResolvedTimerId(string fallbackId)
        {
            if (!string.IsNullOrWhiteSpace(TimerId))
            {
                return TimerId.Trim();
            }

            return string.IsNullOrWhiteSpace(fallbackId) ? "scenario_timer" : fallbackId.Trim();
        }

        public float GetSanitizedWarningThreshold()
        {
            if (WarningThresholdSeconds < 0f)
            {
                return 0f;
            }

            return WarningThresholdSeconds > DurationSeconds ? DurationSeconds : WarningThresholdSeconds;
        }
    }
}
