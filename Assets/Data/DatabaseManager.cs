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

    #region Data Models

    [Serializable]
    private class UserRecord
    {
        public int UserId;
        public string Username;
        public string Password;
    }

    [Serializable]
    private class SaveSlotRecord
    {
        public int SaveSlotId;
        public int UserId;
        public string Username;
        public string SaveSlotName;
        public int Level;
        public int Score;
        public string LastPlayedUtc;
        public int ProgressPercent;
    }

    [Serializable]
    private class DatabaseFile
    {
        public List<UserRecord> Users = new List<UserRecord>();
        public List<SaveSlotRecord> SaveSlots = new List<SaveSlotRecord>();
        public int NextUserId = 1;
        public int NextSaveSlotId = 1;
    }

    #endregion

    #region Inspector

    [Header("JSON Storage")]
    [SerializeField] private string databaseFileName = "database.json";

    #endregion

    #region Private Fields

    private DatabaseFile database;
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

        Debug.Log($"[DatabaseManager] Initialisiert. Pfad: {filePath}");
    }

    #endregion

    #region User Methods

    public bool RegisterUser(string username, string password)
    {
        username = Normalize(username);
        password = Normalize(password);

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            Debug.LogWarning("[DatabaseManager] Registrierung fehlgeschlagen: Leere Eingaben.");
            return false;
        }

        bool alreadyExists = database.Users.Any(u =>
            string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));

        if (alreadyExists)
        {
            Debug.LogWarning("[DatabaseManager] Registrierung fehlgeschlagen: Benutzer existiert bereits.");
            return false;
        }

        UserRecord user = new UserRecord
        {
            UserId = database.NextUserId++,
            Username = username,
            Password = password
        };

        database.Users.Add(user);
        SaveDatabase();

        EnsureDefaultSaveSlotExists(user.UserId, user.Username);

        Debug.Log($"[DatabaseManager] Benutzer registriert: {username}");
        return true;
    }

    public bool ValidateLogin(string username, string password, out int userId, out string normalizedUsername)
    {
        userId = 0;
        normalizedUsername = string.Empty;

        username = Normalize(username);
        password = Normalize(password);

        UserRecord user = database.Users.FirstOrDefault(u =>
            string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase) &&
            u.Password == password);

        if (user == null)
        {
            Debug.LogWarning("[DatabaseManager] Login ungültig.");
            return false;
        }

        userId = user.UserId;
        normalizedUsername = user.Username;

        Debug.Log($"[DatabaseManager] Login gültig für: {normalizedUsername}");
        return true;
    }

    #endregion

    #region Save Slot Methods

    public void EnsureDefaultSaveSlotExists(int userId, string username)
    {
        List<SaveSlotRecord> userSlots = database.SaveSlots
            .Where(s => s.UserId == userId)
            .ToList();

        if (userSlots.Count > 0)
        {
            return;
        }

        SaveSlotRecord slot = new SaveSlotRecord
        {
            SaveSlotId = database.NextSaveSlotId++,
            UserId = userId,
            Username = username,
            SaveSlotName = "Slot 1",
            Level = 1,
            Score = 0,
            LastPlayedUtc = DateTime.UtcNow.ToString("o"),
            ProgressPercent = 0
        };

        database.SaveSlots.Add(slot);
        SaveDatabase();

        Debug.Log($"[DatabaseManager] Standard-SaveSlot erstellt für {username}");
    }

    public List<SaveSlotInfo> GetSaveSlotsByUsername(string username)
    {
        username = Normalize(username);

        List<SaveSlotInfo> result = database.SaveSlots
            .Where(s => string.Equals(s.Username, username, StringComparison.OrdinalIgnoreCase))
            .OrderBy(s => s.SaveSlotId)
            .Select(ToSaveSlotInfo)
            .ToList();

        Debug.Log($"[DatabaseManager] {result.Count} SaveSlots für {username} geladen.");
        return result;
    }

    public SaveSlotInfo CreateNewSaveSlot(string username)
    {
        username = Normalize(username);

        UserRecord user = database.Users.FirstOrDefault(u =>
            string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));

        if (user == null)
        {
            Debug.LogError("[DatabaseManager] Benutzer für neuen SaveSlot nicht gefunden.");
            return null;
        }

        int nextSlotNumber = database.SaveSlots
            .Where(s => s.UserId == user.UserId)
            .Count() + 1;

        SaveSlotRecord slot = new SaveSlotRecord
        {
            SaveSlotId = database.NextSaveSlotId++,
            UserId = user.UserId,
            Username = user.Username,
            SaveSlotName = $"Slot {nextSlotNumber}",
            Level = 1,
            Score = 0,
            LastPlayedUtc = DateTime.UtcNow.ToString("o"),
            ProgressPercent = 0
        };

        database.SaveSlots.Add(slot);
        SaveDatabase();

        Debug.Log($"[DatabaseManager] Neuer SaveSlot erstellt: {slot.SaveSlotName}");
        return ToSaveSlotInfo(slot);
    }

    public void UpdateSaveProgress(int saveSlotId, int level, int score, int progressPercent)
    {
        SaveSlotRecord slot = database.SaveSlots.FirstOrDefault(s => s.SaveSlotId == saveSlotId);

        if (slot == null)
        {
            Debug.LogError("[DatabaseManager] SaveSlot für Update nicht gefunden.");
            return;
        }

        slot.Level = Mathf.Max(1, level);
        slot.Score = Mathf.Max(0, score);
        slot.ProgressPercent = Mathf.Clamp(progressPercent, 0, 100);
        slot.LastPlayedUtc = DateTime.UtcNow.ToString("o");

        SaveDatabase();

        Debug.Log($"[DatabaseManager] SaveSlot aktualisiert: {slot.SaveSlotName} | Level {slot.Level} | {slot.ProgressPercent}%");
    }

    #endregion

    #region Private Helpers

    private void LoadDatabase()
    {
        if (!File.Exists(filePath))
        {
            database = new DatabaseFile();
            SaveDatabase();
            return;
        }

        string json = File.ReadAllText(filePath);

        if (string.IsNullOrWhiteSpace(json))
        {
            database = new DatabaseFile();
            SaveDatabase();
            return;
        }

        database = JsonUtility.FromJson<DatabaseFile>(json);

        if (database == null)
        {
            database = new DatabaseFile();
        }

        if (database.Users == null)
        {
            database.Users = new List<UserRecord>();
        }

        if (database.SaveSlots == null)
        {
            database.SaveSlots = new List<SaveSlotRecord>();
        }

        if (database.NextUserId <= 0)
        {
            database.NextUserId = database.Users.Count > 0 ? database.Users.Max(u => u.UserId) + 1 : 1;
        }

        if (database.NextSaveSlotId <= 0)
        {
            database.NextSaveSlotId = database.SaveSlots.Count > 0 ? database.SaveSlots.Max(s => s.SaveSlotId) + 1 : 1;
        }
    }

    private void SaveDatabase()
    {
        string json = JsonUtility.ToJson(database, true);
        File.WriteAllText(filePath, json);
    }

    private static string Normalize(string value)
    {
        return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
    }

    private static SaveSlotInfo ToSaveSlotInfo(SaveSlotRecord record)
    {
        return new SaveSlotInfo
        {
            SaveSlotId = record.SaveSlotId,
            UserId = record.UserId,
            Username = record.Username,
            SaveSlotName = record.SaveSlotName,
            Level = record.Level,
            Score = record.Score,
            LastPlayedUtc = record.LastPlayedUtc,
            ProgressPercent = record.ProgressPercent
        };
    }

    #endregion
}