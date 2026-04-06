using UnityEngine;
using TMPro;

public class LoginMenuController : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TextMeshProUGUI feedbackText;

    public void OnClickLogin()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        bool success = AuthManager.Instance.SignIn(username, password);

        if (success)
        {
            feedbackText.text = "Login erfolgreich";
        }
        else
        {
            feedbackText.text = "Falsche Daten";
        }
    }

    public void OnClickRegister()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        bool success = AuthManager.Instance.SignUp(username, password);

        if (success)
        {
            feedbackText.text = "Registriert";
        }
        else
        {
            feedbackText.text = "User existiert bereits";
        }
    }
}