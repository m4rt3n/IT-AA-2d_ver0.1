using UnityEngine;

public class GameStartProgressSync : MonoBehaviour
{
    #region Inspector

    [Header("Test / Initial Progress")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int currentScore = 0;
    [SerializeField] private int currentProgressPercent = 0;

    [Header("Auto Save")]
    [SerializeField] private bool saveOnStart = true;

    #endregion

    #region Unity

    private void Start()
    {
        if (!saveOnStart)
        {
            return;
        }

        SaveCurrentProgress();
    }

    #endregion

    #region Public Methods

    public void SaveCurrentProgress()
    {
        if (PlayerSession.Instance == null || !PlayerSession.Instance.HasSaveLoaded)
        {
            Debug.LogWarning("[GameStartProgressSync] Kein geladener SaveSlot vorhanden.");
            return;
        }

        DatabaseManager.Instance?.UpdateSaveProgress(
            PlayerSession.Instance.SaveSlotId,
            currentLevel,
            currentScore,
            currentProgressPercent
        );

        PlayerSession.Instance.UpdateProgress(currentLevel, currentScore, currentProgressPercent);

        Debug.Log("[GameStartProgressSync] Fortschritt gespeichert.");
    }

    public void SetProgress(int level, int score, int progressPercent)
    {
        currentLevel = level;
        currentScore = score;
        currentProgressPercent = Mathf.Clamp(progressPercent, 0, 100);
    }

    #endregion
}