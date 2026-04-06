using TMPro;
using UnityEngine;

public class LoginUIController : MonoBehaviour
{
    [Header("Login")]
    [SerializeField] private TMP_InputField loginUsernameInput;
    [SerializeField] private TMP_InputField loginPasswordInput;

    [Header("Sign Up")]
    [SerializeField] private TMP_InputField signupUsernameInput;
    [SerializeField] private TMP_InputField signupPasswordInput;

    [Header("Status")]
    [SerializeField] private TMP_Text statusText;

    public void OnClickSignIn()
    {
        string username = loginUsernameInput.text;
        string password = loginPasswordInput.text;

        bool success = AuthManager.Instance.SignIn(username, password);

        statusText.text = success
            ? "Login erfolgreich."
            : "Login fehlgeschlagen. Benutzername oder Passwort falsch.";
    }

    public void OnClickSignUp()
    {
        string username = signupUsernameInput.text;
        string password = signupPasswordInput.text;

        bool success = AuthManager.Instance.SignUp(username, password);

        statusText.text = success
            ? "Registrierung erfolgreich. Jetzt einloggen."
            : "Registrierung fehlgeschlagen. Name evtl. schon vergeben.";
    }
}