using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    #region Singleton

    public static DatabaseManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("[DatabaseManager] Zweite Instanz gefunden, zerstöre Objekt.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("[DatabaseManager] Initialisiert.");
    }

    #endregion

    #region Mock Data

    // Übergangslösung:
    // Diese Liste dient nur dazu, das Popup und den Login-Flow sauber zu testen.
    // Später kannst du das gegen JSON oder SQLite austauschen.
    private readonly List<SaveSlotInfo> saveSlots = new List<SaveSlotInfo>()
    {
        new SaveSlotInfo
        {
            UserId = 1,
            SaveSlotId = 101,
            Username = "Martin",
            SaveSlotName = "Slot 1",
            Level = 5,
            Score = 1200,
            LastPlayed = "Heute"
        },
        new SaveSlotInfo
        {
            UserId = 1,
            SaveSlotId = 102,
            Username = "Martin",
            SaveSlotName = "Slot 2",
            Level = 12,
            Score = 3400,
            LastPlayed = "Gestern"
        },
        new SaveSlotInfo
        {
            UserId = 2,
            SaveSlotId = 201,
            Username = "ArthurTest",
            SaveSlotName = "Slot A",
            Level = 2,
            Score = 300,
            LastPlayed = "Vor 3 Tagen"
        }
    };

    #endregion

    #region Public API

    public List<SaveSlotInfo> GetAllSaveSlots()
    {
        Debug.Log($"[DatabaseManager] Lade SaveSlots. Anzahl: {saveSlots.Count}");
        return saveSlots;
    }

    public SaveSlotInfo GetSaveSlotByIds(int userId, int saveSlotId)
    {
        for (int i = 0; i < saveSlots.Count; i++)
        {
            if (saveSlots[i].UserId == userId && saveSlots[i].SaveSlotId == saveSlotId)
            {
                Debug.Log($"[DatabaseManager] SaveSlot gefunden: UserId={userId}, SaveSlotId={saveSlotId}");
                return saveSlots[i];
            }
        }

        Debug.LogWarning($"[DatabaseManager] Kein SaveSlot gefunden: UserId={userId}, SaveSlotId={saveSlotId}");
        return null;
    }

    #endregion
}