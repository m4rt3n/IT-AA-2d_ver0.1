/*
 * Datei: AchievementProgress.cs
 * Zweck: Speichert den Runtime-Status eines Achievements.
 * Verantwortung: Haelt Unlock-Status, Zeitpunkt und optionale Fortschrittswerte fuer ein Achievement.
 * Abhaengigkeiten: System.Serializable, DateTime.
 * Verwendung: Wird im AchievementProfile als Status pro AchievementId gehalten.
 */

using System;

namespace ITAA.Features.Achievements
{
    [Serializable]
    public class AchievementProgress
    {
        public string AchievementId;
        public bool IsUnlocked;
        public string UnlockedAtText;
        public int CurrentValue;
        public int TargetValue = 1;

        public bool IsCompleted => IsUnlocked || (TargetValue > 0 && CurrentValue >= TargetValue);

        public void Unlock()
        {
            if (IsUnlocked)
            {
                return;
            }

            IsUnlocked = true;
            CurrentValue = TargetValue > 0 ? TargetValue : CurrentValue;
            UnlockedAtText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public void AddProgress(int amount)
        {
            if (IsUnlocked || amount <= 0)
            {
                return;
            }

            CurrentValue += amount;

            if (TargetValue > 0 && CurrentValue >= TargetValue)
            {
                Unlock();
            }
        }
    }
}
