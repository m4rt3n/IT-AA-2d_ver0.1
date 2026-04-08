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
    }

    #endregion

    #region Mock Data

    private List<SaveSlotInfo> saveSlots = new()
    {
        new SaveSlotInfo { Id = 1, Username = "Martin", SaveSlotName = "Slot 1", Level = 5, Score = 1200, LastPlayed = "Heute" },
        new SaveSlotInfo { Id = 2, Username = "Martin", SaveSlotName = "Slot 2", Level = 12, Score = 3400, LastPlayed = "Gestern" },
        new SaveSlotInfo { Id = 3, Username = "Test", SaveSlotName = "Slot A", Level = 2, Score = 300, LastPlayed = "Vor 3 Tagen" }
    };

    #endregion

    #region Public

    public List<SaveSlotInfo> GetAllSaveSlots()
    {
        Debug.Log("[DB] Lade SaveSlots");
        return saveSlots;
    }

    #endregion
}