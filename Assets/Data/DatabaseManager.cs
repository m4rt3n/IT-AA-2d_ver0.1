using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    #region Singleton

    public static DatabaseManager Instance { get; private set; }

    #endregion

    #region Serializable Models

    [Serializable]
    private class SaveDatabaseFile
    {
        public List<SaveSlotInfo> SaveSlots = new List<SaveSlotInfo>();
        public int NextSaveSlotId = 1;
    }

    #endregion

    #region Inspector

    [Header("Storage")]
    [SerializeField] private string databaseFileName = "saveSlots.json";

    [Header("Debug Seed")]
    [SerializeField] private bool createDemoSaveSlotsIfEmpty = true;

    #endregion

    #region Private Fields

    private SaveDatabaseFile database;
    private string filePath;

    #endregion

    #region Unity

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        filePath = Path.Combine(Application.persistentDataPath, databaseFileName);
        LoadDatabase();

        if (createDemoSaveSlotsIfEmpty)
        {
            EnsureDemoSaveSlots();
        }

        Debug.Log($"[DatabaseManager] Initialisiert. Datei: {filePath}");
    }

    #endregion

    #region Public Save Methods

    public List<SaveSlotInfo> GetAllSaveSlots()
    {
        return database.SaveSlots
            .OrderBy(s => s.SaveSlotId)
            .Select(CloneSave)
            .ToList();
    }

    public SaveSlotInfo GetSaveSlotById(int saveSlotId)
    {
        SaveSlotInfo found = database.SaveSlots.FirstOrDefault(s => s.SaveSlotId == saveSlotId);
        return found == null ? null : CloneSave(found);
    }

    public SaveSlotInfo CreateSaveSlot(
        string username,
        string saveSlotName,
        int level = 1,
        int score = 0,
        int progressPercent = 0,
        int userId = 0)
    {
        SaveSlotInfo save = new SaveSlotInfo
        {
            SaveSlotId = database.NextSaveSlotId++,
            UserId = userId,
            Username = string.IsNullOrWhiteSpace(username) ? "Gast" : username.Trim(),
            SaveSlotName = string.IsNullOrWhiteSpace(saveSlotName) ? $"Slot {database.NextSaveSlotId - 1}" : saveSlotName.Trim(),
            Level = Mathf.Max(1, level),
            Score = Mathf.Max(0, score),
            ProgressPercent = Mathf.Clamp(progressPercent, 0, 100),
            LastPlayedUtc = DateTime.UtcNow.ToString("o")
        };

        database.SaveSlots.Add(save);
        SaveDatabase();

        Debug.Log($"[DatabaseManager] SaveSlot erstellt: {save.SaveSlotName}");
        return CloneSave(save);
    }

    public void UpdateSaveProgress(int saveSlotId, int level, int score, int progressPercent)
    {
        SaveSlotInfo save = database.SaveSlots.FirstOrDefault(s => s.SaveSlotId == saveSlotId);

        if (save == null)
        {
            Debug.LogWarning("[DatabaseManager] SaveSlot für Fortschrittsupdate nicht gefunden.");
            return;
        }

        save.Level = Mathf.Max(1, level);
        save.Score = Mathf.Max(0, score);
        save.ProgressPercent = Mathf.Clamp(progressPercent, 0, 100);
        save.LastPlayedUtc = DateTime.UtcNow.ToString("o");

        SaveDatabase();

        Debug.Log($"[DatabaseManager] SaveSlot aktualisiert: {save.SaveSlotName} | Level {save.Level} | Score {save.Score} | {save.ProgressPercent}%");
    }

    public bool HasAnySaveSlots()
    {
        return database.SaveSlots != null && database.SaveSlots.Count > 0;
    }

    #endregion

    #region Private Methods

    private void LoadDatabase()
    {
        if (!File.Exists(filePath))
        {
            database = new SaveDatabaseFile();
            SaveDatabase();
            return;
        }

        string json = File.ReadAllText(filePath);

        if (string.IsNullOrWhiteSpace(json))
        {
            database = new SaveDatabaseFile();
            SaveDatabase();
            return;
        }

        database = JsonUtility.FromJson<SaveDatabaseFile>(json);

        if (database == null)
        {
            database = new SaveDatabaseFile();
        }

        if (database.SaveSlots == null)
        {
            database.SaveSlots = new List<SaveSlotInfo>();
        }

        if (database.NextSaveSlotId <= 0)
        {
            database.NextSaveSlotId = database.SaveSlots.Count > 0
                ? database.SaveSlots.Max(s => s.SaveSlotId) + 1
                : 1;
        }
    }

    private void SaveDatabase()
    {
        string json = JsonUtility.ToJson(database, true);
        File.WriteAllText(filePath, json);
    }

    private void EnsureDemoSaveSlots()
    {
        if (database.SaveSlots.Count > 0)
        {
            return;
        }

        CreateSaveSlot("Martin", "Slot 1", 3, 1250, 25, 1);
        CreateSaveSlot("Martin", "Slot 2", 7, 4820, 58, 1);
        CreateSaveSlot("Martin", "Slot 3", 12, 9900, 91, 1);

        Debug.Log("[DatabaseManager] Demo-Spielstände erstellt.");
    }

    private static SaveSlotInfo CloneSave(SaveSlotInfo source)
    {
        return new SaveSlotInfo
        {
            SaveSlotId = source.SaveSlotId,
            UserId = source.UserId,
            Username = source.Username,
            SaveSlotName = source.SaveSlotName,
            Level = source.Level,
            Score = source.Score,
            ProgressPercent = source.ProgressPercent,
            LastPlayedUtc = source.LastPlayedUtc
        };
    }

    #endregion
}