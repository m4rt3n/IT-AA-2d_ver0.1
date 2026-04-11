using UnityEngine;

public class PlayerSession : MonoBehaviour
{
    #region Singleton

    public static PlayerSession Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("[PlayerSession] Zweite Instanz gefunden, zerstöre Objekt.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("[PlayerSession] Initialisiert.");
    }

    #endregion

    #region Properties

    public int UserId { get; private set; }
    public string Username { get; private set; }

    public int SaveSlotId { get; private set; }
    public string SaveSlotName { get; private set; }

    public int Level { get; private set; }
    public int Score { get; private set; }
    public int ProgressPercent { get; private set; }

    public bool IsLoggedIn => !string.IsNullOrEmpty(Username);
    public bool HasSaveLoaded => SaveSlotId > 0;

    #endregion

    #region Public Methods

    public void SetSession(SaveSlotInfo save)
    {
        if (save == null)
        {
            Debug.LogError("[PlayerSession] SaveSlotInfo ist null.");
            return;
        }

        UserId = save.UserId;
        Username = save.Username;
        SaveSlotId = save.SaveSlotId;
        SaveSlotName = save.SaveSlotName;
        Level = save.Level;
        Score = save.Score;
        ProgressPercent = save.ProgressPercent;

        Debug.Log($"[PlayerSession] Session gesetzt: {Username} | {SaveSlotName} | Level {Level} | Score {Score} | Fortschritt {ProgressPercent}%");
    }

    public void UpdateProgress(int level, int score, int progressPercent)
    {
        Level = Mathf.Max(1, level);
        Score = Mathf.Max(0, score);
        ProgressPercent = Mathf.Clamp(progressPercent, 0, 100);

        Debug.Log($"[PlayerSession] Fortschritt aktualisiert: Level {Level} | Score {Score} | Fortschritt {ProgressPercent}%");
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

        Debug.Log("[PlayerSession] Session geleert.");
    }

    #endregion
}