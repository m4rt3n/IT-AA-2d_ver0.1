using TMPro;
using UnityEngine;

public class LoginMenuController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private StartSceneMenuController menuController;
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TMP_Text messageText;

    private void OnEnable()
    {
        ClearMessage();

        if (usernameInput != null)
        {
            usernameInput.text = "";
            usernameInput.ActivateInputField();
        }

        if (passwordInput != null)
        {
            passwordInput.text = "";
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            OnClickLogin();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickBack();
        }
    }

    public void OnClickLogin()
    {
        string username = usernameInput != null ? usernameInput.text.Trim() : "";
        string password = passwordInput != null ? passwordInput.text : "";

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            ShowMessage("Bitte Benutzername und Passwort eingeben.");
            return;
        }

        if (AuthManager.Instance == null)
        {
            ShowMessage("AuthManager nicht gefunden.");
            return;
        }

        bool success = AuthManager.Instance.SignIn(username, password);

        if (success)
        {
            ShowMessage("Login erfolgreich.");
        }
        else
        {
            ShowMessage("Login fehlgeschlagen. Benutzername oder Passwort falsch.");
        }
    }

    public void OnClickRegister()
    {
        string username = usernameInput != null ? usernameInput.text.Trim() : "";
        string password = passwordInput != null ? passwordInput.text : "";

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            ShowMessage("Bitte Benutzername und Passwort eingeben.");
            return;
        }

        if (AuthManager.Instance == null)
        {
            ShowMessage("AuthManager nicht gefunden.");
            return;
        }

        bool success = AuthManager.Instance.SignUp(username, password);

        if (success)
        {
            ShowMessage("Registrierung erfolgreich. Du kannst dich jetzt einloggen.");
        }
        else
        {
            ShowMessage("Registrierung fehlgeschlagen. Benutzername existiert bereits.");
        }
    }

    public void OnClickBack()
    {
        if (usernameInput != null)
            usernameInput.text = "";

        if (passwordInput != null)
            passwordInput.text = "";

        ClearMessage();

        if (menuController != null)
        {
            menuController.OnClickBackToStart();
        }
    }

    private void ShowMessage(string message)
    {
        if (messageText != null)
            messageText.text = message;

        Debug.Log(message);
    }

    private void ClearMessage()
    {
        if (messageText != null)
            messageText.text = "";
    }
}