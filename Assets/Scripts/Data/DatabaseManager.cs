using System.Collections.Generic;
using System.IO;
using System.Linq;
using ITAA.Core.Runtime;
using ITAA.Data.Models;
using ITAA.Data.Storage;
using UnityEngine;

namespace ITAA.Data
{
    public class DatabaseManager : PersistentSingleton<DatabaseManager>
    {
        [Header("Storage")]
        [SerializeField] private string saveDatabaseFileName = "saveSlots.json";
        [SerializeField] private string userDatabaseFileName = "users.json";

        [Header("Debug")]
        [SerializeField] private bool createDemoSaveSlotsIfEmpty = true;

        private SaveDatabaseFile saveDatabase;
        private UserDatabaseFile userDatabase;
        private string saveFilePath;
        private string userFilePath;

        protected override void Awake()
        {
            base.Awake();

            if (Instance != this)
            {
                return;
            }

            saveFilePath = Path.Combine(Application.persistentDataPath, saveDatabaseFileName);
            userFilePath = Path.Combine(Application.persistentDataPath, userDatabaseFileName);

            LoadAll();

            if (createDemoSaveSlotsIfEmpty)
            {
                EnsureDemoSaveSlots();
            }
        }

        public IReadOnlyList<SaveSlotData> GetAllSaveSlots()
        {
            return saveDatabase.SaveSlots
                .OrderBy(slot => slot.SaveSlotId)
                .Select(slot => slot.Clone())
                .ToList();
        }

        public SaveSlotData GetSaveSlotById(int saveSlotId)
        {
            SaveSlotData match = saveDatabase.SaveSlots.FirstOrDefault(slot => slot.SaveSlotId == saveSlotId);
            return match?.Clone();
        }

        public void SaveProgress(int saveSlotId, int level, int score, int progressPercent)
        {
            SaveSlotData slot = saveDatabase.SaveSlots.FirstOrDefault(item => item.SaveSlotId == saveSlotId);

            if (slot == null)
            {
                Debug.LogWarning($"[DatabaseManager] SaveSlot {saveSlotId} nicht gefunden.");
                return;
            }

            slot.Level = Mathf.Max(1, level);
            slot.Score = Mathf.Max(0, score);
            slot.ProgressPercent = Mathf.Clamp(progressPercent, 0, 100);

            SaveSaveDatabase();
        }

        public SaveSlotData CreateSaveSlot(string username, string slotName)
        {
            SaveSlotData saveSlot = new()
            {
                SaveSlotId = saveDatabase.NextSaveSlotId++,
                UserId = 0,
                Username = username,
                SaveSlotName = string.IsNullOrWhiteSpace(slotName) ? "Neuer Spielstand" : slotName.Trim(),
                Level = 1,
                Score = 0,
                ProgressPercent = 0
            };

            saveDatabase.SaveSlots.Add(saveSlot);
            SaveSaveDatabase();
            return saveSlot.Clone();
        }

        private void LoadAll()
        {
            saveDatabase = LoadJson<SaveDatabaseFile>(saveFilePath) ?? new SaveDatabaseFile();
            userDatabase = LoadJson<UserDatabaseFile>(userFilePath) ?? new UserDatabaseFile();
        }

        private T LoadJson<T>(string path) where T : class
        {
            if (!File.Exists(path))
            {
                return null;
            }

            string json = File.ReadAllText(path);
            return string.IsNullOrWhiteSpace(json) ? null : JsonUtility.FromJson<T>(json);
        }

        private void SaveSaveDatabase()
        {
            string json = JsonUtility.ToJson(saveDatabase, true);
            File.WriteAllText(saveFilePath, json);
        }

        private void EnsureDemoSaveSlots()
        {
            if (saveDatabase.SaveSlots.Count > 0)
            {
                return;
            }

            saveDatabase.SaveSlots.Add(new SaveSlotData
            {
                SaveSlotId = saveDatabase.NextSaveSlotId++,
                UserId = 1,
                Username = "Martin",
                SaveSlotName = "Arthur Start",
                Level = 1,
                Score = 0,
                ProgressPercent = 5
            });

            saveDatabase.SaveSlots.Add(new SaveSlotData
            {
                SaveSlotId = saveDatabase.NextSaveSlotId++,
                UserId = 1,
                Username = "Martin",
                SaveSlotName = "Schneepfad",
                Level = 3,
                Score = 120,
                ProgressPercent = 30
            });

            SaveSaveDatabase();
        }
    }
}