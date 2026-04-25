/*
 * Datei: SkillProfile.cs
 * Zweck: Buendelt den Runtime-Fortschritt aller Skills eines Profils.
 * Verantwortung: Erstellt SkillProgress-Eintraege, sucht Fortschritt und verwaltet XP-Vergabe pro Skill.
 * Abhaengigkeiten: SkillDefinition, SkillProgress, System.Collections.Generic.
 * Verwendung: Wird vom SkillRuntimeManager als In-Memory-Profil gehalten.
 */

using System;
using System.Collections.Generic;

namespace ITAA.Features.Skills
{
    [Serializable]
    public class SkillProfile
    {
        public List<SkillProgress> Skills = new List<SkillProgress>();

        public SkillProgress GetSkillProgress(string skillId)
        {
            if (string.IsNullOrWhiteSpace(skillId))
            {
                return null;
            }

            for (int i = 0; i < Skills.Count; i++)
            {
                SkillProgress progress = Skills[i];

                if (progress != null && progress.SkillId == skillId)
                {
                    return progress;
                }
            }

            return null;
        }

        public SkillProgress GetOrCreateSkillProgress(SkillDefinition definition)
        {
            if (definition == null || !definition.HasValidId())
            {
                return null;
            }

            SkillProgress progress = GetSkillProgress(definition.SkillId);

            if (progress != null)
            {
                return progress;
            }

            progress = new SkillProgress
            {
                SkillId = definition.SkillId,
                Level = definition.GetSafeStartLevel(),
                CurrentXp = 0,
                TotalXpEarned = 0
            };

            Skills.Add(progress);
            return progress;
        }

        public bool RemoveSkill(string skillId)
        {
            SkillProgress progress = GetSkillProgress(skillId);

            if (progress == null)
            {
                return false;
            }

            return Skills.Remove(progress);
        }

        public void Clear()
        {
            Skills.Clear();
        }
    }
}
