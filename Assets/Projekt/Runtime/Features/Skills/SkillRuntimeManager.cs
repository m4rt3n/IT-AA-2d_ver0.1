/*
 * Datei: SkillRuntimeManager.cs
 * Zweck: Verwaltet Skill-Definitionen und Runtime-XP-Vergabe.
 * Verantwortung: Registriert Skills, fuehrt XP-Zuwachs aus, meldet Level-Ups und stellt SkillProfile bereit.
 * Abhaengigkeiten: SkillDefinition, SkillProfile, SkillProgress, Unity MonoBehaviour.
 * Verwendung: Kann als optionale Szenen-Komponente genutzt und spaeter an Quiz, Quest, UI oder Savegame angebunden werden.
 */

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ITAA.Features.Skills
{
    [DisallowMultipleComponent]
    public class SkillRuntimeManager : MonoBehaviour
    {
        [Header("Skill Definitions")]
        [SerializeField] private List<SkillDefinition> skillDefinitions = new List<SkillDefinition>();
        [SerializeField] private bool createDemoSkillDefinitions = true;

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs;

        private readonly SkillProfile profile = new SkillProfile();
        private readonly Dictionary<string, SkillDefinition> definitionsById = new Dictionary<string, SkillDefinition>();

        public event Action<SkillProgress> SkillChanged;
        public event Action<SkillProgress, int> SkillLeveledUp;

        public SkillProfile Profile => profile;

        private void Awake()
        {
            RebuildDefinitionLookup();
        }

        public void AddXp(string skillId, int amount)
        {
            if (string.IsNullOrWhiteSpace(skillId) || amount <= 0)
            {
                return;
            }

            SkillDefinition definition = GetSkillDefinition(skillId);

            if (definition == null)
            {
                Debug.LogWarning($"[{nameof(SkillRuntimeManager)}] Skill nicht gefunden: {skillId}", this);
                return;
            }

            SkillProgress progress = profile.GetOrCreateSkillProgress(definition);

            if (progress == null)
            {
                return;
            }

            int levelUps = progress.AddXp(definition, amount);
            SkillChanged?.Invoke(progress);

            if (levelUps > 0)
            {
                SkillLeveledUp?.Invoke(progress, levelUps);
                Log($"Skill-Level-Up: {skillId}, Level={progress.Level}, LevelUps={levelUps}");
            }
            else
            {
                Log($"Skill-XP: {skillId}, +{amount}, Level={progress.Level}, XP={progress.CurrentXp}");
            }
        }

        public SkillProgress GetSkillProgress(string skillId)
        {
            return profile.GetSkillProgress(skillId);
        }

        public SkillDefinition GetSkillDefinition(string skillId)
        {
            if (string.IsNullOrWhiteSpace(skillId))
            {
                return null;
            }

            if (definitionsById.Count == 0)
            {
                RebuildDefinitionLookup();
            }

            definitionsById.TryGetValue(skillId, out SkillDefinition definition);
            return definition;
        }

        public IReadOnlyCollection<SkillDefinition> GetSkillDefinitions()
        {
            if (definitionsById.Count == 0)
            {
                RebuildDefinitionLookup();
            }

            return definitionsById.Values;
        }

        public void ResetProfile()
        {
            profile.Clear();
            SkillChanged?.Invoke(null);
        }

        private void RebuildDefinitionLookup()
        {
            definitionsById.Clear();

            if (skillDefinitions != null)
            {
                for (int i = 0; i < skillDefinitions.Count; i++)
                {
                    RegisterDefinition(skillDefinitions[i]);
                }
            }

            if (createDemoSkillDefinitions)
            {
                RegisterDemoSkill("networking", "Netzwerk", "Grundlagen zu DNS, DHCP, Gateway und Fehlersuche.", 1, 10, 100);
                RegisterDemoSkill("support", "Support", "Strukturierte Analyse und Kommunikation im IT-Support.", 1, 10, 100);
                RegisterDemoSkill("terminal", "Terminal", "Sicherer Umgang mit simulierten IT-Terminalbefehlen.", 1, 10, 120);
            }
        }

        private void RegisterDemoSkill(
            string skillId,
            string displayName,
            string description,
            int startLevel,
            int maxLevel,
            int baseXp)
        {
            if (definitionsById.ContainsKey(skillId))
            {
                return;
            }

            SkillDefinition definition = new SkillDefinition
            {
                SkillId = skillId,
                DisplayName = displayName,
                Description = description,
                StartLevel = startLevel,
                MaxLevel = maxLevel,
                BaseXpToNextLevel = baseXp,
                XpGrowthFactor = 1.25f
            };

            RegisterDefinition(definition);
        }

        private void RegisterDefinition(SkillDefinition definition)
        {
            if (definition == null || !definition.HasValidId())
            {
                return;
            }

            definitionsById[definition.SkillId] = definition;
            profile.GetOrCreateSkillProgress(definition);
        }

        private void Log(string message)
        {
            if (!enableDebugLogs)
            {
                return;
            }

            Debug.Log($"[{nameof(SkillRuntimeManager)}] {message}", this);
        }
    }
}
