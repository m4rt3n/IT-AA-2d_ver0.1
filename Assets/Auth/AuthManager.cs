using UnityEngine;
using UnityEngine.SceneManagement;

public class AuthManager : MonoBehaviour
{
    #region Singleton

    public static AuthManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("[AuthManager] Zweite Instanz gefunden, zerstöre Objekt.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("[AuthManager] Initialisiert.");
    }

    #endregion

    #region Inspector

    [SerializeField] private string sceneAfterLogin = "StartScene";

    #endregion

    #region New Login Flow

    public void SignInWithSaveSlot(SaveSlotInfo save)
    {
        Debug.Log("[AuthManager] Login mit SaveSlot.");

        if (save == null)
        {
            Debug.LogError("[AuthManager] SaveSlot ist null.");
            return;
        }

        if (PlayerSession.Instance == null)
        {
            Debug.LogError("[AuthManager] PlayerSession fehlt.");
            return;
        }

        PlayerSession.Instance.SetSession(save);

        Debug.Log($"[AuthManager] Lade Szene nach Login: {sceneAfterLogin}");
        SceneManager.LoadScene(sceneAfterLogin);
    }

    #endregion

    #region Compatibility Methods

    // Diese Methoden bleiben vorübergehend erhalten,
    // damit alte Referenzen keine Compile-Fehler verursachen.

    public bool SignIn(string username, string password)
    {
        Debug.LogWarning("[AuthManager] Alte Methode SignIn(username, password) wurde aufgerufen. Nicht mehr verwendet.");
        return false;
    }

    public bool SignUp(string username, string password)
    {
        Debug.LogWarning("[AuthManager] Alte Methode SignUp(username, password) wurde aufgerufen. Nicht mehr verwendet.");
        return false;
    }

    #endregion
}