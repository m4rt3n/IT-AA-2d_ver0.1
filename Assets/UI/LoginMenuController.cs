using TMPro;
using UnityEngine;

public class LoginMenuController : MonoBehaviour
{
    #region Inspector

    [SerializeField] private MenuManager menuManager;

    [Header("Input")]
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField passwordInput;

    [Header("Feedback")]
    [SerializeField] private TMP_Text statusText;

    [Header("Save Slot UI")]
    [SerializeField] private SaveSlotSelectionPopup saveSlotSelectionPopup;

    #endregion

    #region Unity

    private void OnEnable()
    {
        ClearStatus();
    }

    #endregion

    #region Public Button Events

    public void OnClickRegister()
    {
        string username = GetUsername();
        string password = GetPassword();

        if (!ValidateFields(username, password))
        {
            return;
        }

        bool success = AuthManager.Instance != null && AuthManager.Instance.Register(username, password);

        if (success)
        {
            SetStatus("Registrierung erfolgreich.");
        }
        else
        {
            SetStatus("Registrierung fehlgeschlagen. Benutzer existiert evtl. bereits.");
        }
    }

    public void OnClickLogin()
    {
        string username = GetUsername();
        string password = GetPassword();

        if (!ValidateFields(username, password))
        {
            return;
        }

        if (AuthManager.Instance == null)
        {
            SetStatus("AuthManager fehlt.");
            return;
        }

        bool success = AuthManager.Instance.Login(username, password, out string message);
        SetStatus(message);

        if (!success)
        {
            return;
        }

        if (menuManager != null)
        {
            menuManager.ShowSaveSlotMenu();
        }

        if (saveSlotSelectionPopup != null)
        {
            saveSlotSelectionPopup.OpenForUser(username);
        }
    }

    public void OnClickBack()
    {
        menuManager?.ShowStartMenu();
    }

    #endregion

    #region Private Helpers

    private string GetUsername()
    {
        return usernameInput != null ? usernameInput.text.Trim() : string.Empty;
    }

    private string GetPassword()
    {
        return passwordInput != null ? passwordInput.text : string.Empty;
    }

    private bool ValidateFields(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            SetStatus("Bitte Benutzername und Passwort eingeben.");
            return false;
        }

        return true;
    }

    private void SetStatus(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }

        Debug.Log($"[LoginMenuController] {message}");
    }

    private void ClearStatus()
    {
        if (statusText != null)
        {
            statusText.text = string.Empty;
        }
    }

    #endregion
}