using UnityEngine;
using UnityEngine.SceneManagement;

public class AuthManager : MonoBehaviour
{
    #region Singleton

    public static AuthManager Instance;

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

    #region Inspector

    [SerializeField] private string sceneAfterLogin = "StartScene";

    #endregion

    #region Login

    public void SignInWithSaveSlot(SaveSlotInfo save)
    {
        Debug.Log("[Auth] Login mit SaveSlot");

        PlayerSession.Instance.SetSession(save);

        SceneManager.LoadScene(sceneAfterLogin);
    }

    #endregion
}