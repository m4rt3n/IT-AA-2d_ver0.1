using UnityEngine;
using TMPro;

public class LoginMenuController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private StartSceneMenuController menuController;
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TextMeshProUGUI feedbackText;

    private void OnEnable()
    {
        ClearInputs();
        ClearFeedback();

        if (usernameInput != null)
        {
            usernameInput.ActivateInputField();
        }
    }

    public void OnClickLogin()
    {
        if (usernameInput == null || passwordInput == null)
        {
            Debug.LogError("LoginMenuController: Inputfelder sind nicht zugewiesen.");
            return;
        }

        if (AuthManager.Instance == null)
        {
            Debug.LogError("LoginMenuController: AuthManager.Instance fehlt.");
            SetFeedback("AuthManager fehlt.");
            return;
        }

        string username = usernameInput.text.Trim();
        string password = passwordInput.text;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            SetFeedback("Bitte Benutzername und Passwort eingeben.");
            return;
        }

        bool success = AuthManager.Instance.SignIn(username, password);

        if (success)
        {
            SetFeedback("Login erfolgreich.");
        }
        else
        {
            SetFeedback("Falsche Daten.");
        }
    }

    public void OnClickRegister()
    {
        if (usernameInput == null || passwordInput == null)
        {
            Debug.LogError("LoginMenuController: Inputfelder sind nicht zugewiesen.");
            return;
        }

        if (AuthManager.Instance == null)
        {
            Debug.LogError("LoginMenuController: AuthManager.Instance fehlt.");
            SetFeedback("AuthManager fehlt.");
            return;
        }

        string username = usernameInput.text.Trim();
        string password = passwordInput.text;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            SetFeedback("Bitte Benutzername und Passwort eingeben.");
            return;
        }

        bool success = AuthManager.Instance.SignUp(username, password);

        if (success)
        {
            SetFeedback("Registriert. Jetzt einloggen.");
        }
        else
        {
            SetFeedback("Benutzer existiert bereits.");
        }
    }

    public void OnClickBack()
    {
        ClearInputs();
        ClearFeedback();

        if (menuController != null)
        {
            menuController.OnClickBackToStart();
        }
        else
        {
            Debug.LogWarning("LoginMenuController: menuController ist nicht zugewiesen.");
        }
    }

    private void SetFeedback(string message)
    {
        if (feedbackText != null)
        {
            feedbackText.text = message;
        }

        Debug.Log(message);
    }

    private void ClearFeedback()
    {
        if (feedbackText != null)
        {
            feedbackText.text = "";
        }
    }

    private void ClearInputs()
    {
        if (usernameInput != null)
            usernameInput.text = "";

        if (passwordInput != null)
            passwordInput.text = "";
    }
}