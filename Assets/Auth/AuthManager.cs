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
        return DatabaseManager.Instance.RegisterUser(username, password);
    }

    public bool SignIn(string username, string password)
    {
        UserEntity user = DatabaseManager.Instance.LoginUser(username, password);

        if (user == null)
            return false;

        PlayerSession.Instance.SetUser(user.Id, user.Username);
        SceneManager.LoadScene(sceneAfterLogin);
        return true;
    }
}