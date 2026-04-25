/*
 * Datei: AchievementManager.cs
 * Zweck: Verwaltet Achievement-Definitionen und Runtime-Unlocks.
 * Verantwortung: Registriert Achievements, schaltet sie frei, meldet Unlock-Events und stellt Query-APIs bereit.
 * Abhaengigkeiten: AchievementDefinition, AchievementProfile, AchievementProgress, Unity MonoBehaviour.
 * Verwendung: Kann spaeter optional an HUD, Progress, Skills, Quiz oder Savegame angebunden werden.
 */

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ITAA.Features.Achievements
{
    [DisallowMultipleComponent]
    public class AchievementManager : MonoBehaviour
    {
        [Header("Achievement Definitions")]
        [SerializeField] private List<AchievementDefinition> achievementDefinitions = new List<AchievementDefinition>();
        [SerializeField] private bool createDemoAchievements = true;

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs;

        private readonly AchievementProfile profile = new AchievementProfile();
        private readonly Dictionary<string, AchievementDefinition> definitionsById = new Dictionary<string, AchievementDefinition>();

        public event Action<AchievementDefinition, AchievementProgress> AchievementUnlocked;
        public event Action<AchievementProgress> AchievementProgressChanged;

        public AchievementProfile Profile => profile;

        private void Awake()
        {
            RebuildDefinitionLookup();
        }

        public bool UnlockAchievement(string achievementId)
        {
            AchievementDefinition definition = GetAchievementDefinition(achievementId);

            if (definition == null)
            {
                Debug.LogWarning($"[{nameof(AchievementManager)}] Achievement nicht gefunden: {achievementId}", this);
                return false;
            }

            AchievementProgress progress = profile.GetOrCreateAchievementProgress(definition);

            if (progress == null || progress.IsUnlocked)
            {
                return false;
            }

            progress.Unlock();
            AchievementUnlocked?.Invoke(definition, progress);
            AchievementProgressChanged?.Invoke(progress);
            Log($"Achievement freigeschaltet: {definition.AchievementId}");
            return true;
        }

        public void AddAchievementProgress(string achievementId, int amount = 1)
        {
            AchievementDefinition definition = GetAchievementDefinition(achievementId);

            if (definition == null)
            {
                Debug.LogWarning($"[{nameof(AchievementManager)}] Achievement nicht gefunden: {achievementId}", this);
                return;
            }

            AchievementProgress progress = profile.GetOrCreateAchievementProgress(definition);

            if (progress == null || progress.IsUnlocked)
            {
                return;
            }

            bool wasUnlocked = progress.IsUnlocked;
            progress.AddProgress(amount);
            AchievementProgressChanged?.Invoke(progress);

            if (!wasUnlocked && progress.IsUnlocked)
            {
                AchievementUnlocked?.Invoke(definition, progress);
                Log($"Achievement per Fortschritt freigeschaltet: {definition.AchievementId}");
                return;
            }

            Log($"Achievement-Fortschritt: {definition.AchievementId} ({progress.CurrentValue}/{progress.TargetValue})");
        }

        public bool IsUnlocked(string achievementId)
        {
            AchievementProgress progress = profile.GetAchievementProgress(achievementId);
            return progress != null && progress.IsUnlocked;
        }

        public AchievementDefinition GetAchievementDefinition(string achievementId)
        {
            if (string.IsNullOrWhiteSpace(achievementId))
            {
                return null;
            }

            if (definitionsById.Count == 0)
            {
                RebuildDefinitionLookup();
            }

            definitionsById.TryGetValue(achievementId, out AchievementDefinition definition);
            return definition;
        }

        public AchievementProgress GetAchievementProgress(string achievementId)
        {
            return profile.GetAchievementProgress(achievementId);
        }

        public IReadOnlyCollection<AchievementDefinition> GetAchievementDefinitions()
        {
            if (definitionsById.Count == 0)
            {
                RebuildDefinitionLookup();
            }

            return definitionsById.Values;
        }

        public int GetUnlockedCount()
        {
            return profile.CountUnlocked();
        }

        public void ResetProfile()
        {
            profile.Clear();
            AchievementProgressChanged?.Invoke(null);
        }

        private void RebuildDefinitionLookup()
        {
            definitionsById.Clear();

            if (achievementDefinitions != null)
            {
                for (int i = 0; i < achievementDefinitions.Count; i++)
                {
                    RegisterDefinition(achievementDefinitions[i]);
                }
            }

            if (createDemoAchievements)
            {
                RegisterDemoAchievement("first_login", "Erster Start", "Starte dein erstes IT-AA Profil.", "Foundation", 10, false);
                RegisterDemoAchievement("first_quiz", "Erstes Quiz", "Schliesse dein erstes Quiz ab.", "Quiz", 20, false);
                RegisterDemoAchievement("network_beginner", "Netzwerk Einstieg", "Sammle erste Netzwerk-Erfahrung.", "Skills", 25, false);
            }
        }

        private void RegisterDemoAchievement(
            string achievementId,
            string title,
            string description,
            string category,
            int points,
            bool hidden)
        {
            if (definitionsById.ContainsKey(achievementId))
            {
                return;
            }

            AchievementDefinition definition = new AchievementDefinition
            {
                AchievementId = achievementId,
                Title = title,
                Description = description,
                Category = category,
                Points = points,
                IsHiddenUntilUnlocked = hidden
            };

            RegisterDefinition(definition);
        }

        private void RegisterDefinition(AchievementDefinition definition)
        {
            if (definition == null || !definition.HasValidId())
            {
                return;
            }

            definitionsById[definition.AchievementId] = definition;
            profile.GetOrCreateAchievementProgress(definition);
        }

        private void Log(string message)
        {
            if (!enableDebugLogs)
            {
                return;
            }

            Debug.Log($"[{nameof(AchievementManager)}] {message}", this);
        }
    }
}
