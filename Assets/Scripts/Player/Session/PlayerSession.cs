/*
 * Datei: PlayerSession.cs
 * Zweck: Hält die aktuell geladene Spielersitzung szenenübergreifend im Speicher.
 * Verantwortung: Speichert den aktiven Benutzer, den geladenen SaveSlot und den Fortschritt.
 * Abhängigkeiten: PersistentSingleton, SaveSlotData.
 * Verwendet von: AuthManager, Gameplay-Systeme, UI-Anzeigen und Speichervorgänge.
 */
using ITAA.Core.Runtime;
using ITAA.Data.Models;
using UnityEngine;

namespace ITAA.Player.Session
{
    public class PlayerSession : PersistentSingleton<PlayerSession>
    {
        public int UserId { get; private set; }
        public string Username { get; private set; }
        public int SaveSlotId { get; private set; }
        public string SaveSlotName { get; private set; }
        public int Level { get; private set; }
        public int Score { get; private set; }
        public int ProgressPercent { get; private set; }

        public bool IsLoggedIn => !string.IsNullOrWhiteSpace(Username);
        public bool HasSaveLoaded => SaveSlotId > 0;

        public void SetSession(SaveSlotData saveSlot)
        {
            if (saveSlot == null)
            {
                Debug.LogError("[PlayerSession] SaveSlotData ist null.");
                return;
            }

            UserId = saveSlot.UserId;
            Username = saveSlot.Username;
            SaveSlotId = saveSlot.SaveSlotId;
            SaveSlotName = saveSlot.SaveSlotName;
            Level = saveSlot.Level;
            Score = saveSlot.Score;
            ProgressPercent = saveSlot.ProgressPercent;
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
            SaveSlotId = 0;
            SaveSlotName = string.Empty;
            Level = 0;
            Score = 0;
            ProgressPercent = 0;
        }
    }
}