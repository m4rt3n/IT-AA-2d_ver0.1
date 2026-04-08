using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    #region Singleton

    public static DatabaseManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Debug.Log("[DatabaseManager] Initialisiert.");
    }

    #endregion

    #region Mock Data

    private List<SaveSlotInfo> mockData = new List<SaveSlotInfo>()
    {
        new SaveSlotInfo { Id = 1, Username = "Martin", SaveSlotName = "Slot 1", Level = 5, Score = 1200, LastPlayed = "Heute" },
        new SaveSlotInfo { Id = 2, Username = "Martin", SaveSlotName = "Slot 2", Level = 12, Score = 3400, LastPlayed = "Gestern" },
        new SaveSlotInfo { Id = 3, Username = "TestUser", SaveSlotName = "Slot A", Level = 2, Score = 300, LastPlayed = "Vor 3 Tagen" }
    };

    #endregion

    #region Public API

    public List<SaveSlotInfo> GetAllSaveSlots()
    {
        Debug.Log("[DatabaseManager] Lade alle SaveSlots...");
        return mockData;
    }

    #endregion
}