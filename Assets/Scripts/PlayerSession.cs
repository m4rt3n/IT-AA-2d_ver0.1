using UnityEngine;

public class PlayerSession : MonoBehaviour
{
    public static PlayerSession Instance { get; private set; }

    public int CurrentUserId { get; private set; } = -1;
    public string CurrentUsername { get; private set; } = string.Empty;
    public bool IsLoggedIn => CurrentUserId >= 0;

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

    public void SetUser(int userId, string username)
    {
        CurrentUserId = userId;
        CurrentUsername = username;
    }

    public void Logout()
    {
        CurrentUserId = -1;
        CurrentUsername = string.Empty;
    }
}