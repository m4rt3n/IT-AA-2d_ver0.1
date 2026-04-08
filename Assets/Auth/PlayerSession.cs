using UnityEngine;

public class PlayerSession : MonoBehaviour
{
    #region Singleton

    public static PlayerSession Instance;

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

    #region Data

    public int UserId;
    public string Username;

    public int SaveSlotId;
    public string SaveSlotName;

    public int Level;
    public int Score;

    #endregion

    #region Public

    public void SetSession(SaveSlotInfo save)
    {
        UserId = save.Id;
        Username = save.Username;

        SaveSlotId = save.Id;
        SaveSlotName = save.SaveSlotName;

        Level = save.Level;
        Score = save.Score;

        Debug.Log($"[Session] {Username} | {SaveSlotName} | Level {Level}");
    }

    #endregion
}