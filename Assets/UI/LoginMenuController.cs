using TMPro;
using UnityEngine;

public class LoginMenuController : MonoBehaviour
{
    [SerializeField] private MenuManager menuManager;
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TMP_Text messageText;

    private void OnEnable()
    {
        Debug.Log("[LoginMenuController] OnEnable");

        if (usernameInput != null) usernameInput.text = "";
        if (passwordInput != null) passwordInput.text = "";
        if (messageText != null) messageText.text = "";
    }

    public void OnClickLogin()
    {
        Debug.Log("[LoginMenuController] OnClickLogin");

        string username = usernameInput != null ? usernameInput.text.Trim() : "";
        string password = passwordInput != null ? passwordInput.text : "";

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            if (messageText != null)
                messageText.text = "Bitte Benutzername und Passwort eingeben.";
            return;
        }

        if (AuthManager.Instance == null)
        {
            if (messageText != null)
                messageText.text = "AuthManager nicht gefunden.";
            return;
        }

        bool success = AuthManager.Instance.SignIn(username, password);

        if (!success && messageText != null)
            messageText.text = "Login fehlgeschlagen.";
    }

    public void OnClickBack()
    {
        Debug.Log("[LoginMenuController] OnClickBack");

        if (menuManager != null)
            menuManager.ShowStartMenu();
    }
}