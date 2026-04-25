/*
 * Datei: AchievementProfile.cs
 * Zweck: Buendelt alle Achievement-Statuswerte eines Runtime-Profils.
 * Verantwortung: Erstellt, sucht und verwaltet AchievementProgress-Eintraege.
 * Abhaengigkeiten: AchievementDefinition, AchievementProgress, System.Collections.Generic.
 * Verwendung: Wird vom AchievementManager als In-Memory-Profil genutzt.
 */

using System;
using System.Collections.Generic;

namespace ITAA.Features.Achievements
{
    [Serializable]
    public class AchievementProfile
    {
        public List<AchievementProgress> Achievements = new List<AchievementProgress>();

        public AchievementProgress GetAchievementProgress(string achievementId)
        {
            if (string.IsNullOrWhiteSpace(achievementId))
            {
                return null;
            }

            for (int i = 0; i < Achievements.Count; i++)
            {
                AchievementProgress progress = Achievements[i];

                if (progress != null && progress.AchievementId == achievementId)
                {
                    return progress;
                }
            }

            return null;
        }

        public AchievementProgress GetOrCreateAchievementProgress(AchievementDefinition definition)
        {
            if (definition == null || !definition.HasValidId())
            {
                return null;
            }

            AchievementProgress progress = GetAchievementProgress(definition.AchievementId);

            if (progress != null)
            {
                return progress;
            }

            progress = new AchievementProgress
            {
                AchievementId = definition.AchievementId,
                IsUnlocked = false,
                UnlockedAtText = string.Empty,
                CurrentValue = 0,
                TargetValue = 1
            };

            Achievements.Add(progress);
            return progress;
        }

        public int CountUnlocked()
        {
            int count = 0;

            for (int i = 0; i < Achievements.Count; i++)
            {
                if (Achievements[i] != null && Achievements[i].IsUnlocked)
                {
                    count++;
                }
            }

            return count;
        }

        public void Clear()
        {
            Achievements.Clear();
        }
    }
}
