using UnityEngine;
using UnityEngine.SceneManagement;

public class AuthManager : MonoBehaviour
{
    #region Singleton

    public static AuthManager Instance { get; private set; }

    #endregion

    #region Inspector

    [Header("Scenes")]
    [SerializeField] private string gameSceneName = "StartScene";

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
        Debug.Log("[AuthManager] Initialisiert.");
    }

    #endregion

    #region Public API

    public bool Register(string username, string password)
    {
        Debug.Log("[AuthManager] Registrierung ist aktuell deaktiviert.");
        return false;
    }

    public bool Login(string username, string password, out string message)
    {
        message = "Login ist aktuell deaktiviert. Bitte Spielstand direkt laden.";
        Debug.Log("[AuthManager] Login ist aktuell deaktiviert.");
        return false;
    }

    public void StartGameWithSave(SaveSlotInfo selectedSave)
    {
        if (selectedSave == null)
        {
            Debug.LogError("[AuthManager] selectedSave ist null.");
            return;
        }

        if (PlayerSession.Instance == null)
        {
            Debug.LogError("[AuthManager] PlayerSession fehlt.");
            return;
        }

        PlayerSession.Instance.SetSession(selectedSave);

        Debug.Log($"[AuthManager] Starte Spiel mit SaveSlot: {selectedSave.SaveSlotName}");
        SceneManager.LoadScene(gameSceneName);
    }

    public void Logout()
    {
        PlayerSession.Instance?.ClearSession();
        Debug.Log("[AuthManager] Logout abgeschlossen.");
    }

    #endregion
}