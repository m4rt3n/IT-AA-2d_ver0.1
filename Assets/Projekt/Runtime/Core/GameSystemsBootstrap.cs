/*
 * Datei: GameSystemsBootstrap.cs
 * Zweck: Initialisiert zentrale Runtime-Systeme defensiv auf einem gemeinsamen GameObject.
 * Verantwortung: Erzeugt fehlende Core-Manager, verhindert doppelte Manager-Instanzen und haelt GameSystems szenenuebergreifend aktiv.
 * Abhaengigkeiten: AchievementManager, SkillRuntimeManager, ProgressManager, ScenarioManager, SettingsManager, SettingsHotkeyController, RuntimeInventory, ToolbeltController, DevPanelBootstrap, SavegameRuntimeSession, Unity SceneManagement.
 * Verwendung: Laeuft automatisch nach dem Laden unterstuetzter Szenen und kann in Integrationsszenen als sichtbare Bootstrap-Komponente platziert werden.
 */

using ITAA.DevTools;
using ITAA.Features.Achievements;
using ITAA.Features.Inventory;
using ITAA.Features.Progress;
using ITAA.Features.Scenarios;
using ITAA.Features.Skills;
using ITAA.System.Savegame;
using ITAA.System.Settings;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ITAA.Core.SceneManagement
{
    [DisallowMultipleComponent]
    public sealed class GameSystemsBootstrap : MonoBehaviour
    {
        private const string GameSystemsRootName = "GameSystems";

        private static GameObject gameSystemsRoot;

        private void Awake()
        {
            EnsureForScene(SceneManager.GetActiveScene());
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InitializeAfterFirstSceneLoad()
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
            SceneManager.sceneLoaded += HandleSceneLoaded;
            EnsureForScene(SceneManager.GetActiveScene());
        }

        public static void EnsureForStartScene(Scene scene)
        {
            EnsureForScene(scene);
        }

        public static void EnsureForScene(Scene scene)
        {
            if (!scene.IsValid() ||
                (scene.name != SceneNames.StartScene &&
                 scene.name != SceneNames.GameScene &&
                 scene.name != SceneNames.DevelopmentLevel))
            {
                return;
            }

            GameObject root = GetOrCreateRoot();

            EnsureBootstrapComponent(root);
            EnsureSettingsManager(root);
            EnsureComponent<SettingsHotkeyController>(root, true);
            EnsureComponent<SavegameRuntimeSession>(root, true);
            EnsureComponent<AchievementManager>(root, true);
            EnsureComponent<SkillRuntimeManager>(root, true);
            EnsureComponent<RuntimeInventory>(root, true);
            EnsureComponent<ToolbeltController>(root, true);
            EnsureComponent<ProgressManager>(root, true);
            EnsureComponent<QuizProgressReporter>(root, true);
            EnsureComponent<ScenarioManager>(root, true);
            EnsureComponent<DevPanelBootstrap>(root, true);
        }

        private static void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            EnsureForScene(scene);
        }

        private static GameObject GetOrCreateRoot()
        {
            if (gameSystemsRoot != null)
            {
                return gameSystemsRoot;
            }

            gameSystemsRoot = GameObject.Find(GameSystemsRootName);

            if (gameSystemsRoot == null)
            {
                gameSystemsRoot = new GameObject(GameSystemsRootName);
                Debug.Log($"[{nameof(GameSystemsBootstrap)}] {GameSystemsRootName} initialized.");
            }

            Object.DontDestroyOnLoad(gameSystemsRoot);
            return gameSystemsRoot;
        }

        private static GameSystemsBootstrap EnsureBootstrapComponent(GameObject root)
        {
            GameSystemsBootstrap existing = Object.FindAnyObjectByType<GameSystemsBootstrap>(FindObjectsInactive.Include);

            if (existing != null)
            {
                LogExisting(nameof(GameSystemsBootstrap), existing.gameObject);
                return existing;
            }

            GameSystemsBootstrap created = root.AddComponent<GameSystemsBootstrap>();
            LogInitialized(nameof(GameSystemsBootstrap), root);
            return created;
        }

        private static SettingsManager EnsureSettingsManager(GameObject root)
        {
            SettingsManager existing = Object.FindAnyObjectByType<SettingsManager>(FindObjectsInactive.Include);

            if (existing != null)
            {
                LogExisting(nameof(SettingsManager), existing.gameObject);
                return existing;
            }

            SettingsManager created = root.AddComponent<SettingsManager>();
            LogInitialized(nameof(SettingsManager), root);
            return created;
        }

        private static T EnsureComponent<T>(GameObject root, bool persistRoot) where T : Component
        {
            T existing = Object.FindAnyObjectByType<T>(FindObjectsInactive.Include);

            if (existing != null)
            {
                LogExisting(typeof(T).Name, existing.gameObject);
                return existing;
            }

            T created = root.AddComponent<T>();

            if (persistRoot)
            {
                Object.DontDestroyOnLoad(root);
            }

            LogInitialized(typeof(T).Name, root);
            return created;
        }

        private static void LogInitialized(string systemName, GameObject owner)
        {
            Debug.Log($"[{nameof(GameSystemsBootstrap)}] System {systemName} initialized on {owner.name}.");
        }

        private static void LogExisting(string systemName, GameObject owner)
        {
            Debug.Log($"[{nameof(GameSystemsBootstrap)}] System {systemName} initialized from existing object {owner.name}.");
        }
    }
}
