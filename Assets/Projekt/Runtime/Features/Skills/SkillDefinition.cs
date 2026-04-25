/*
 * Datei: SkillDefinition.cs
 * Zweck: Beschreibt einen lern- oder gameplaybezogenen Skill.
 * Verantwortung: Haelt ID, Anzeigename, Beschreibung, Startlevel, Maxlevel und XP-Basiskosten.
 * Abhaengigkeiten: System.Serializable, Unity Mathf.
 * Verwendung: Wird von SkillProfile und SkillRuntimeManager als Grundlage fuer Skill-Fortschritt genutzt.
 */

using System;
using UnityEngine;

namespace ITAA.Features.Skills
{
    [Serializable]
    public class SkillDefinition
    {
        public string SkillId = "skill_id";
        public string DisplayName = "Skill";
        [TextArea] public string Description;
        [Min(1)] public int StartLevel = 1;
        [Min(1)] public int MaxLevel = 10;
        [Min(1)] public int BaseXpToNextLevel = 100;
        [Min(1f)] public float XpGrowthFactor = 1.25f;

        public bool HasValidId()
        {
            return !string.IsNullOrWhiteSpace(SkillId);
        }

        public int GetSafeStartLevel()
        {
            return Mathf.Clamp(StartLevel, 1, GetSafeMaxLevel());
        }

        public int GetSafeMaxLevel()
        {
            return Mathf.Max(1, MaxLevel);
        }

        public int GetXpRequiredForLevel(int currentLevel)
        {
            int safeLevel = Mathf.Clamp(currentLevel, 1, GetSafeMaxLevel());
            float multiplier = Mathf.Pow(Mathf.Max(1f, XpGrowthFactor), safeLevel - 1);
            return Mathf.Max(1, Mathf.RoundToInt(BaseXpToNextLevel * multiplier));
        }
    }
}
