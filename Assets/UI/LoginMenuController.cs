using TMPro;
using UnityEngine;

public class LoginMenuController : MonoBehaviour
{
    [Header("References")]
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

        Debug.Log("[LoginMenuController] Username: " + username);

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            SetMessage("Bitte Benutzername und Passwort eingeben.");
            return;
        }

        if (AuthManager.Instance == null)
        {
            Debug.LogError("[LoginMenuController] AuthManager.Instance ist null");
            SetMessage("AuthManager nicht gefunden.");
            return;
        }

        bool success = AuthManager.Instance.SignIn(username, password);

        if (!success)
        {
            SetMessage("Login fehlgeschlagen.");
        }
    }

    public void OnClickRegister()
    {
        Debug.Log("[LoginMenuController] OnClickRegister");

        string username = usernameInput != null ? usernameInput.text.Trim() : "";
        string password = passwordInput != null ? passwordInput.text : "";

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            SetMessage("Bitte Benutzername und Passwort eingeben.");
            return;
        }

        if (AuthManager.Instance == null)
        {
            Debug.LogError("[LoginMenuController] AuthManager.Instance ist null");
            SetMessage("AuthManager nicht gefunden.");
            return;
        }

        bool success = AuthManager.Instance.SignUp(username, password);

        if (success)
            SetMessage("Registrierung erfolgreich.");
        else
            SetMessage("Benutzername existiert bereits.");
    }

    public void OnClickBack()
    {
        Debug.Log("[LoginMenuController] OnClickBack");

        if (menuManager == null)
        {
            Debug.LogError("[LoginMenuController] MenuManager fehlt");
            return;
        }

        menuManager.ShowStartMenu();
    }

    private void SetMessage(string message)
    {
        Debug.Log("[LoginMenuController] Message: " + message);

        if (messageText != null)
            messageText.text = message;
    }
}