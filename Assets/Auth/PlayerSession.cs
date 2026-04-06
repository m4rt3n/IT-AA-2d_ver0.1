using UnityEngine;

public class PlayerSession : MonoBehaviour
{
    public static PlayerSession Instance { get; private set; }

    public int UserId { get; private set; }
    public string Username { get; private set; }

    public bool IsLoggedIn => !string.IsNullOrEmpty(Username);

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
        UserId = userId;
        Username = username;
    }

    public void ClearUser()
    {
        UserId = 0;
        Username = string.Empty;
    }
}