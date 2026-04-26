/*
 * Datei: StartSceneFeatureBootstrap.cs
 * Zweck: Initialisiert optionale vorbereitete Feature-Manager sicher beim Laden der StartScene.
 * Verantwortung: Erzeugt fehlende Runtime-Manager fuer Savegame, Dialogue, Knowledge Base, Settings UI, Achievements, Skills, Inventory, Progress, Quiz-Progress, Scenarios und DevPanel.
 * Abhaengigkeiten: SceneNames, Unity SceneManager, vorbereitete ITAA Feature-Manager.
 * Verwendung: Laeuft automatisch zur Runtime und veraendert keine bestehenden Inspector-Referenzen in Szene oder Prefabs.
 */

using ITAA.DevTools;
using ITAA.Features.Achievements;
using ITAA.Features.Dialogue;
using ITAA.Features.Inventory;
using ITAA.Features.KnowledgeBase;
using ITAA.Features.Progress;
using ITAA.Features.Scenarios;
using ITAA.Features.Skills;
using ITAA.System.Savegame;
using ITAA.System.Settings;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ITAA.Core.SceneManagement
{
    public static class StartSceneFeatureBootstrap
    {
        private const string RuntimeRootName = "StartSceneRuntimeFeatures";
        private const string SavegameRuntimeSessionName = "SavegameRuntimeSession";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InitializeAfterFirstSceneLoad()
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
            SceneManager.sceneLoaded += HandleSceneLoaded;
            EnsureForScene(SceneManager.GetActiveScene());
        }

        private static void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            EnsureForScene(scene);
        }

        private static void EnsureForScene(Scene scene)
        {
            if (!scene.IsValid() || scene.name != SceneNames.StartScene)
            {
                return;
            }

            GameObject root = GetOrCreateRoot();

            SettingsManager.GetOrCreate();
            EnsureComponent<SettingsHotkeyController>(root);
            EnsureSavegameRuntimeSession();
            EnsureComponent<DialogueManager>(root);
            EnsureComponent<KnowledgeBaseHotkeyController>(root);
            EnsureComponent<ProgressManager>(root);
            EnsureComponent<QuizProgressReporter>(root);
            EnsureComponent<ScenarioManager>(root);
            EnsureComponent<AchievementManager>(root);
            EnsureComponent<SkillRuntimeManager>(root);
            EnsureComponent<RuntimeInventory>(root);
            EnsureComponent<ToolbeltController>(root);
            EnsureComponent<DevPanelBootstrap>(root);
        }

        private static GameObject GetOrCreateRoot()
        {
            GameObject existingRoot = GameObject.Find(RuntimeRootName);

            if (existingRoot != null)
            {
                return existingRoot;
            }

            return new GameObject(RuntimeRootName);
        }

        private static SavegameRuntimeSession EnsureSavegameRuntimeSession()
        {
            SavegameRuntimeSession existing = Object.FindAnyObjectByType<SavegameRuntimeSession>(FindObjectsInactive.Include);

            if (existing != null)
            {
                return existing;
            }

            GameObject sessionRoot = new(SavegameRuntimeSessionName);
            return sessionRoot.AddComponent<SavegameRuntimeSession>();
        }

        private static T EnsureComponent<T>(GameObject fallbackRoot) where T : Component
        {
            T existing = Object.FindAnyObjectByType<T>(FindObjectsInactive.Include);

            if (existing != null)
            {
                return existing;
            }

            return fallbackRoot.AddComponent<T>();
        }
    }
}
