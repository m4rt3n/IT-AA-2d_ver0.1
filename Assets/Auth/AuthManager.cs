using UnityEngine;
using UnityEngine.SceneManagement;

public class AuthManager : MonoBehaviour
{
    public static AuthManager Instance { get; private set; }

    [SerializeField] private string sceneAfterLogin = "StartScene";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool SignUp(string username, string password)
    {
        if (DatabaseManager.Instance == null)
        {
            Debug.LogError("AuthManager: DatabaseManager.Instance ist null.");
            return false;
        }

        return DatabaseManager.Instance.RegisterUser(username, password);
    }

    public bool SignIn(string username, string password)
    {
        if (DatabaseManager.Instance == null)
        {
            Debug.LogError("AuthManager: DatabaseManager.Instance ist null.");
            return false;
        }

        UserEntity user = DatabaseManager.Instance.LoginUser(username, password);

        if (user == null)
        {
            Debug.Log("Login fehlgeschlagen: Benutzer nicht gefunden oder Passwort falsch.");
            return false;
        }

        if (PlayerSession.Instance == null)
        {
            Debug.LogError("AuthManager: PlayerSession.Instance ist null.");
            return false;
        }

        PlayerSession.Instance.SetUser(user.Id, user.Username);
        Debug.Log("Login erfolgreich: " + user.Username);

        SceneManager.LoadScene(sceneAfterLogin);
        return true;
    }

    public void Logout()
    {
        if (PlayerSession.Instance != null)
        {
            PlayerSession.Instance.ClearUser();
        }
    }
}