/*
 * Datei: DevPanelController.cs
 * Zweck: Stellt ein optionales Entwickler-Panel mit Debug- und Testaktionen bereit.
 * Verantwortung: Verdrahtet UI-Buttons mit bestehenden Systemen wie SaveSystem, LoadGamePanel, PlayerSession und Feature-Managern.
 * Abhaengigkeiten: SaveSystem, LoadGamePanel, PlayerSession, SettingsManager, Feature-Manager, Unity UI, SceneManager.
 * Verwendung: Wird auf ein DevPanel-GameObject gesetzt oder vom DevPanelBootstrap zur Laufzeit erzeugt.
 */

using System;
using ITAA.Core.SceneManagement;
using ITAA.Features.Achievements;
using ITAA.Features.Dialogue;
using ITAA.Features.Inventory;
using ITAA.Features.Progress;
using ITAA.Features.Scenarios;
using ITAA.Features.Skills;
using ITAA.Player.Session;
using ITAA.System.Savegame;
using ITAA.System.Settings;
using ITAA.UI.Panels;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ITAA.DevTools
{
    [DisallowMultipleComponent]
    public class DevPanelController : MonoBehaviour
    {
        #region Inspector

        [Header("Panel")]
        [SerializeField] private GameObject panelRoot;
        [SerializeField] private bool hideOnAwake = true;

        [Header("Existing Systems")]
        [SerializeField] private LoadGamePanel loadGamePanel;

        [Header("Buttons")]
        [SerializeField] private Button reloadSaveSlotsButton;
        [SerializeField] private Button generateDummySavesButton;
        [SerializeField] private Button resetSettingsButton;
        [SerializeField] private Button generateDummyQuizDraftButton;
        [SerializeField] private Button printPlayerSessionButton;
        [SerializeField] private Button printCurrentSceneButton;
        [SerializeField] private Button printFeatureManagersButton;
        [SerializeField] private Button grantDemoSkillXpButton;
        [SerializeField] private Button unlockDemoAchievementButton;
        [SerializeField] private Button closeDevPanelButton;

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs = true;

        #endregion

        private SaveSystem saveSystem;

        #region Unity

        private void Awake()
        {
            ResolveReferences();
            WireButtons();

            if (hideOnAwake)
            {
                CloseDevPanel();
            }
        }

        private void OnDestroy()
        {
            UnwireButtons();
        }

        #endregion

        #region Public API

        public void OpenDevPanel()
        {
            SetPanelVisible(true);
        }

        public void CloseDevPanel()
        {
            SetPanelVisible(false);
        }

        public void ToggleDevPanel()
        {
            ResolveReferences();

            if (panelRoot == null)
            {
                Debug.LogWarning($"[{nameof(DevPanelController)}] PanelRoot fehlt.", this);
                return;
            }

            SetPanelVisible(!panelRoot.activeSelf);
        }

        public void ReloadSaveSlots()
        {
            ResolveReferences();

            if (loadGamePanel == null)
            {
                Debug.LogWarning($"[{nameof(DevPanelController)}] LoadGamePanel fehlt. SaveSlots koennen nicht neu geladen werden.", this);
                return;
            }

            loadGamePanel.ReloadSlots();
            Log("SaveSlots neu geladen.");
        }

        public void GenerateDummySaves()
        {
            EnsureSaveSystem();
            CreateOrRefreshDummySave(1, "Testslot Arthur", "Martin", 3, 1200, new Vector3(2f, 1f, 0f));
            CreateOrRefreshDummySave(2, "Testslot Bernd", "Bernd", 1, 0, new Vector3(-23f, -6.55f, 0f));

            Log("Dummy-Saves fuer Slot 1 und Slot 2 erstellt oder ergaenzt.");
            ReloadSaveSlots();
        }

        public void ResetSettings()
        {
            SettingsManager settingsManager = SettingsManager.GetOrCreate();
            settingsManager.ResetToDefaults();

            Log("SettingsManager auf Standardwerte gesetzt.");
        }

        public void GenerateDummyQuizDraft()
        {
            Debug.Log(
                $"[{nameof(DevPanelController)}] Dummy Quiz Draft vorbereitet: " +
                "Frage='Was pruefst du zuerst bei Kein Internet?', Antworten='Kabel/WLAN', 'DNS/DHCP', 'Drucker', richtig=1. " +
                "Persistenz fuer Quiz-Drafts existiert noch nicht.",
                this
            );
        }

        public void PrintPlayerSession()
        {
            PlayerSession session = PlayerSession.Instance;

            if (session == null)
            {
                Debug.LogWarning($"[{nameof(DevPanelController)}] PlayerSession fehlt.", this);
                return;
            }

            Debug.Log(
                $"[{nameof(DevPanelController)}] PlayerSession: " +
                $"UserId={session.UserId}, " +
                $"Username='{session.Username}', " +
                $"PlayerName='{session.PlayerName}', " +
                $"SaveSlotId={session.SaveSlotId}, " +
                $"SaveSlotName='{session.SaveSlotName}', " +
                $"Level={session.Level}, " +
                $"Score={session.Score}, " +
                $"Progress={session.ProgressPercent}, " +
                $"IsLoggedIn={session.IsLoggedIn}, " +
                $"HasSaveLoaded={session.HasSaveLoaded}",
                this
            );
        }

        public void PrintCurrentScene()
        {
            Scene activeScene = SceneManager.GetActiveScene();

            Debug.Log(
                $"[{nameof(DevPanelController)}] Current Scene: " +
                $"Name='{activeScene.name}', " +
                $"Path='{activeScene.path}', " +
                $"BuildIndex={activeScene.buildIndex}, " +
                $"IsLoaded={activeScene.isLoaded}",
                this
            );
        }

        public void PrintFeatureManagers()
        {
            ProgressManager progressManager = FindAnyObjectByType<ProgressManager>(FindObjectsInactive.Include);
            ScenarioManager scenarioManager = FindAnyObjectByType<ScenarioManager>(FindObjectsInactive.Include);
            AchievementManager achievementManager = FindAnyObjectByType<AchievementManager>(FindObjectsInactive.Include);
            SkillRuntimeManager skillRuntimeManager = FindAnyObjectByType<SkillRuntimeManager>(FindObjectsInactive.Include);
            RuntimeInventory runtimeInventory = FindAnyObjectByType<RuntimeInventory>(FindObjectsInactive.Include);
            ToolbeltController toolbeltController = FindAnyObjectByType<ToolbeltController>(FindObjectsInactive.Include);
            DialogueManager dialogueManager = FindAnyObjectByType<DialogueManager>(FindObjectsInactive.Include);
            SavegameRuntimeSession savegameRuntimeSession = FindAnyObjectByType<SavegameRuntimeSession>(FindObjectsInactive.Include);
            SettingsManager settingsManager = SettingsManager.GetOrCreate();

            Debug.Log(
                $"[{nameof(DevPanelController)}] Feature Managers: " +
                $"Settings={(settingsManager != null ? "ok" : "fehlt")}, " +
                $"Progress={(progressManager != null ? "ok" : "fehlt")}, " +
                $"Scenario={(scenarioManager != null ? "ok" : "fehlt")}, " +
                $"Achievements={(achievementManager != null ? achievementManager.GetAchievementDefinitions().Count.ToString() : "fehlt")}, " +
                $"Skills={(skillRuntimeManager != null ? skillRuntimeManager.GetSkillDefinitions().Count.ToString() : "fehlt")}, " +
                $"Inventory={(runtimeInventory != null ? runtimeInventory.Count.ToString() : "fehlt")}, " +
                $"Toolbelt={(toolbeltController != null ? toolbeltController.SlotCount.ToString() : "fehlt")}, " +
                $"Dialogue={(dialogueManager != null ? "ok" : "fehlt")}, " +
                $"SavegameSession={(savegameRuntimeSession != null ? "ok" : "fehlt")}",
                this
            );
        }

        public void GrantDemoSkillXp()
        {
            SkillRuntimeManager skillRuntimeManager = FindAnyObjectByType<SkillRuntimeManager>(FindObjectsInactive.Include);

            if (skillRuntimeManager == null)
            {
                Debug.LogWarning($"[{nameof(DevPanelController)}] SkillRuntimeManager fehlt.", this);
                return;
            }

            skillRuntimeManager.AddXp("networking", 25);
            Log("Demo-XP fuer Skill 'networking' vergeben.");
        }

        public void UnlockDemoAchievement()
        {
            AchievementManager achievementManager = FindAnyObjectByType<AchievementManager>(FindObjectsInactive.Include);

            if (achievementManager == null)
            {
                Debug.LogWarning($"[{nameof(DevPanelController)}] AchievementManager fehlt.", this);
                return;
            }

            achievementManager.UnlockAchievement("first_login");
            Log("Demo-Achievement 'first_login' freigeschaltet oder war bereits aktiv.");
        }

        public void AssignButtons(
            Button reloadSaveSlots,
            Button generateDummySaves,
            Button resetSettings,
            Button generateDummyQuizDraft,
            Button printPlayerSession,
            Button printCurrentScene,
            Button printFeatureManagers,
            Button grantDemoSkillXp,
            Button unlockDemoAchievement,
            Button closeDevPanel)
        {
            UnwireButtons();

            reloadSaveSlotsButton = reloadSaveSlots;
            generateDummySavesButton = generateDummySaves;
            resetSettingsButton = resetSettings;
            generateDummyQuizDraftButton = generateDummyQuizDraft;
            printPlayerSessionButton = printPlayerSession;
            printCurrentSceneButton = printCurrentScene;
            printFeatureManagersButton = printFeatureManagers;
            grantDemoSkillXpButton = grantDemoSkillXp;
            unlockDemoAchievementButton = unlockDemoAchievement;
            closeDevPanelButton = closeDevPanel;

            WireButtons();
        }

        public void AssignPanelRoot(GameObject root)
        {
            panelRoot = root;
        }

        #endregion

        #region Private

        private void ResolveReferences()
        {
            if (panelRoot == null)
            {
                panelRoot = gameObject;
            }

            if (loadGamePanel == null)
            {
                loadGamePanel = FindAnyObjectByType<LoadGamePanel>(FindObjectsInactive.Include);
            }
        }

        private void WireButtons()
        {
            WireButton(reloadSaveSlotsButton, ReloadSaveSlots);
            WireButton(generateDummySavesButton, GenerateDummySaves);
            WireButton(resetSettingsButton, ResetSettings);
            WireButton(generateDummyQuizDraftButton, GenerateDummyQuizDraft);
            WireButton(printPlayerSessionButton, PrintPlayerSession);
            WireButton(printCurrentSceneButton, PrintCurrentScene);
            WireButton(printFeatureManagersButton, PrintFeatureManagers);
            WireButton(grantDemoSkillXpButton, GrantDemoSkillXp);
            WireButton(unlockDemoAchievementButton, UnlockDemoAchievement);
            WireButton(closeDevPanelButton, CloseDevPanel);
        }

        private void UnwireButtons()
        {
            UnwireButton(reloadSaveSlotsButton, ReloadSaveSlots);
            UnwireButton(generateDummySavesButton, GenerateDummySaves);
            UnwireButton(resetSettingsButton, ResetSettings);
            UnwireButton(generateDummyQuizDraftButton, GenerateDummyQuizDraft);
            UnwireButton(printPlayerSessionButton, PrintPlayerSession);
            UnwireButton(printCurrentSceneButton, PrintCurrentScene);
            UnwireButton(printFeatureManagersButton, PrintFeatureManagers);
            UnwireButton(grantDemoSkillXpButton, GrantDemoSkillXp);
            UnwireButton(unlockDemoAchievementButton, UnlockDemoAchievement);
            UnwireButton(closeDevPanelButton, CloseDevPanel);
        }

        private static void WireButton(Button button, UnityEngine.Events.UnityAction action)
        {
            if (button == null || action == null)
            {
                return;
            }

            button.onClick.RemoveListener(action);
            button.onClick.AddListener(action);
        }

        private static void UnwireButton(Button button, UnityEngine.Events.UnityAction action)
        {
            if (button == null || action == null)
            {
                return;
            }

            button.onClick.RemoveListener(action);
        }

        private void SetPanelVisible(bool visible)
        {
            ResolveReferences();

            if (panelRoot == null)
            {
                return;
            }

            panelRoot.SetActive(visible);
            Log(visible ? "DevPanel geoeffnet." : "DevPanel geschlossen.");
        }

        private void EnsureSaveSystem()
        {
            saveSystem ??= new SaveSystem();
        }

        private void CreateOrRefreshDummySave(
            int slotId,
            string displayName,
            string playerName,
            int level,
            int score,
            Vector3 playerPosition)
        {
            SaveGameData saveData = saveSystem.Load(slotId);

            if (saveData == null || !saveData.HasData)
            {
                saveData = new SaveGameData
                {
                    SlotId = slotId,
                    DisplayName = displayName,
                    PlayerName = playerName,
                    SceneName = SceneNames.StartScene,
                    SavedAtText = DateTime.Now.ToString("dd.MM.yyyy HH:mm"),
                    Level = level,
                    Score = score,
                    HasData = true
                };

                saveData.SetPlayerPosition(playerPosition);
                saveSystem.Save(slotId, saveData);
                return;
            }

            bool changed = false;

            changed |= FillIfEmpty(ref saveData.DisplayName, displayName);
            changed |= FillIfEmpty(ref saveData.PlayerName, playerName);
            changed |= FillIfEmpty(ref saveData.SavedAtText, DateTime.Now.ToString("dd.MM.yyyy HH:mm"));

            if (string.IsNullOrWhiteSpace(saveData.SceneName) || saveData.SceneName == SceneNames.LegacyGameScene)
            {
                saveData.SceneName = SceneNames.StartScene;
                changed = true;
            }

            if (saveData.Level <= 0)
            {
                saveData.Level = level;
                changed = true;
            }

            if (changed)
            {
                saveSystem.Save(slotId, saveData);
            }
        }

        private static bool FillIfEmpty(ref string value, string fallback)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            value = fallback;
            return true;
        }

        private void Log(string message)
        {
            if (!enableDebugLogs)
            {
                return;
            }

            Debug.Log($"[{nameof(DevPanelController)}] {message}", this);
        }

        #endregion
    }
}
