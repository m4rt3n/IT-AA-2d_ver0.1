/*
 * Datei: ScenarioTimerState.cs
 * Zweck: Speichert den Laufzeitstatus eines aktiven Szenario-Timers.
 * Verantwortung: Verwaltet Timer-ID, Dauer, verbleibende Zeit, Warnstatus und Timeout-Status.
 * Abhaengigkeiten: ScenarioTimeLimit, System.Serializable.
 * Verwendung: Wird von ScenarioProgress gehalten und vom ScenarioManager pro Frame aktualisiert.
 */

using System;

namespace ITAA.Features.Scenarios
{
    [Serializable]
    public class ScenarioTimerState
    {
        public string TimerId;
        public float DurationSeconds;
        public float RemainingSeconds;
        public float WarningThresholdSeconds;
        public bool IsActive;
        public bool HasTimedOut;
        public bool WarningReached;

        public void Start(ScenarioTimeLimit timeLimit, string fallbackTimerId)
        {
            if (timeLimit == null || !timeLimit.HasValidDuration())
            {
                Clear();
                return;
            }

            TimerId = timeLimit.GetResolvedTimerId(fallbackTimerId);
            DurationSeconds = Math.Max(0f, timeLimit.DurationSeconds);
            RemainingSeconds = DurationSeconds;
            WarningThresholdSeconds = timeLimit.GetSanitizedWarningThreshold();
            IsActive = true;
            HasTimedOut = false;
            WarningReached = WarningThresholdSeconds > 0f && RemainingSeconds <= WarningThresholdSeconds;
        }

        public void Clear()
        {
            TimerId = string.Empty;
            DurationSeconds = 0f;
            RemainingSeconds = 0f;
            WarningThresholdSeconds = 0f;
            IsActive = false;
            HasTimedOut = false;
            WarningReached = false;
        }

        public bool Tick(float deltaSeconds)
        {
            if (!IsActive || HasTimedOut)
            {
                return false;
            }

            RemainingSeconds = Math.Max(0f, RemainingSeconds - Math.Max(0f, deltaSeconds));

            if (WarningThresholdSeconds > 0f && RemainingSeconds <= WarningThresholdSeconds)
            {
                WarningReached = true;
            }

            if (RemainingSeconds > 0f)
            {
                return false;
            }

            HasTimedOut = true;
            IsActive = false;
            return true;
        }

        public float GetProgress01()
        {
            if (DurationSeconds <= 0f)
            {
                return 0f;
            }

            return Math.Max(0f, Math.Min(1f, 1f - (RemainingSeconds / DurationSeconds)));
        }
    }
}
