/*
 * Datei: SkillProgress.cs
 * Zweck: Speichert Level und Erfahrungspunkte eines einzelnen Skills.
 * Verantwortung: Verwaltet XP-Zuwachs, Level-Up-Regeln und Maxlevel-Grenzen fuer einen Skill.
 * Abhaengigkeiten: SkillDefinition, System.Serializable, Unity Mathf.
 * Verwendung: Wird in SkillProfile als Fortschrittseintrag pro SkillId gehalten.
 */

using System;
using UnityEngine;

namespace ITAA.Features.Skills
{
    [Serializable]
    public class SkillProgress
    {
        public string SkillId;
        public int Level;
        public int CurrentXp;
        public int TotalXpEarned;

        public bool IsAtMaxLevel(SkillDefinition definition)
        {
            return definition != null && Level >= definition.GetSafeMaxLevel();
        }

        public int AddXp(SkillDefinition definition, int amount)
        {
            if (definition == null || amount <= 0)
            {
                return 0;
            }

            if (string.IsNullOrWhiteSpace(SkillId))
            {
                SkillId = definition.SkillId;
            }

            if (Level <= 0)
            {
                Level = definition.GetSafeStartLevel();
            }

            int levelUps = 0;
            int remainingXp = amount;
            TotalXpEarned += amount;

            while (remainingXp > 0 && !IsAtMaxLevel(definition))
            {
                int xpRequired = definition.GetXpRequiredForLevel(Level);
                int xpNeeded = Mathf.Max(0, xpRequired - CurrentXp);
                int consumed = Mathf.Min(xpNeeded, remainingXp);

                CurrentXp += consumed;
                remainingXp -= consumed;

                if (CurrentXp < xpRequired)
                {
                    break;
                }

                Level++;
                levelUps++;
                CurrentXp = 0;
            }

            if (IsAtMaxLevel(definition))
            {
                CurrentXp = 0;
            }

            return levelUps;
        }

        public float GetProgress01(SkillDefinition definition)
        {
            if (definition == null || IsAtMaxLevel(definition))
            {
                return 1f;
            }

            int requiredXp = definition.GetXpRequiredForLevel(Level);
            return requiredXp <= 0 ? 0f : Mathf.Clamp01((float)CurrentXp / requiredXp);
        }
    }
}
