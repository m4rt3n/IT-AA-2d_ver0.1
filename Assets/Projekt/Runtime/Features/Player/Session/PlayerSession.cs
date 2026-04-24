/*
 * Datei: PlayerSession.cs
 * Zweck: Hält die aktuell geladene Spielersitzung szenenübergreifend im Speicher.
 * Verantwortung: Speichert den aktiven Benutzer, den geladenen SaveSlot, den Spielernamen und den Fortschritt.
 * Abhängigkeiten: PersistentSingleton, SaveSlotData, SaveGameData, SavegameRuntimeSession.
 * Verwendet von: AuthManager, Gameplay-Systeme, UI-Anzeigen und Speichervorgänge.
 */
using ITAA.Core.Runtime;
using ITAA.Data.Models;
using ITAA.System.Savegame;
using UnityEngine;

namespace ITAA.Player.Session
{
    public class PlayerSession : PersistentSingleton<PlayerSession>
    {
        public int UserId { get; private set; }
        public string Username { get; private set; }
        public string PlayerName { get; private set; }
        public int SaveSlotId { get; private set; }
        public string SaveSlotName { get; private set; }
        public int Level { get; private set; }
        public int Score { get; private set; }
        public int ProgressPercent { get; private set; }

        public bool IsLoggedIn => !string.IsNullOrWhiteSpace(Username);
        public bool HasSaveLoaded => SaveSlotId > 0;

        protected override void Awake()
        {
            base.Awake();

            if (Instance != this)
            {
                return;
            }

            TryRestoreFromRuntimeSave();
        }

        public void SetSession(SaveSlotData saveSlot)
        {
            if (saveSlot == null)
            {
                Debug.LogError("[PlayerSession] SaveSlotData ist null.");
                return;
            }

            UserId = saveSlot.UserId;
            Username = saveSlot.Username;
            PlayerName = saveSlot.Username;
            SaveSlotId = saveSlot.SaveSlotId;
            SaveSlotName = saveSlot.SaveSlotName;
            Level = saveSlot.Level;
            Score = saveSlot.Score;
            ProgressPercent = saveSlot.ProgressPercent;
        }

        public void ApplySaveGameData(SaveGameData saveData)
        {
            if (saveData == null)
            {
                Debug.LogError("[PlayerSession] SaveGameData ist null.");
                return;
            }

            SaveSlotId = saveData.SlotId;
            SaveSlotName = string.IsNullOrWhiteSpace(saveData.DisplayName)
                ? $"Slot {saveData.SlotId}"
                : saveData.DisplayName;
            PlayerName = string.IsNullOrWhiteSpace(saveData.PlayerName)
                ? "Spieler"
                : saveData.PlayerName;

            if (string.IsNullOrWhiteSpace(Username))
            {
                Username = PlayerName;
            }

            Level = Mathf.Max(1, saveData.Level);
            Score = Mathf.Max(0, saveData.Score);
        }

        public string GetResolvedPlayerName(string fallbackName = "Spieler")
        {
            if (!string.IsNullOrWhiteSpace(PlayerName))
            {
                return PlayerName;
            }

            if (!string.IsNullOrWhiteSpace(Username))
            {
                return Username;
            }

            SaveGameData runtimeSave = SavegameRuntimeSession.Instance != null
                ? SavegameRuntimeSession.Instance.GetCurrentSave()
                : null;

            if (runtimeSave != null && !string.IsNullOrWhiteSpace(runtimeSave.PlayerName))
            {
                return runtimeSave.PlayerName;
            }

            return fallbackName;
        }

        public void UpdateProgress(int level, int score, int progressPercent)
        {
            Level = Mathf.Max(1, level);
            Score = Mathf.Max(0, score);
            ProgressPercent = Mathf.Clamp(progressPercent, 0, 100);
        }

        public SaveSlotData CreateSnapshot()
        {
            return new SaveSlotData
            {
                UserId = UserId,
                Username = Username,
                SaveSlotId = SaveSlotId,
                SaveSlotName = SaveSlotName,
                Level = Level,
                Score = Score,
                ProgressPercent = ProgressPercent
            };
        }

        public void ClearSession()
        {
            UserId = 0;
            Username = string.Empty;
            PlayerName = string.Empty;
            SaveSlotId = 0;
            SaveSlotName = string.Empty;
            Level = 0;
            Score = 0;
            ProgressPercent = 0;
        }

        private void TryRestoreFromRuntimeSave()
        {
            SavegameRuntimeSession runtimeSession = SavegameRuntimeSession.Instance;
            if (runtimeSession == null || !runtimeSession.HasSave())
            {
                return;
            }

            ApplySaveGameData(runtimeSession.GetCurrentSave());
        }
    }
}
