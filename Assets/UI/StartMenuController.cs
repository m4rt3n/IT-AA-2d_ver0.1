using UnityEngine;
using UnityEngine.UI;

public class StartMenuController : MonoBehaviour
{
    #region Inspector

    [SerializeField] private MenuManager menuManager;

    [Header("Optional Buttons")]
    [SerializeField] private Button btnSave;
    [SerializeField] private Button btnSettings;

    #endregion

    #region Unity Methods

    private void Start()
    {
        if (menuManager == null)
        {
            menuManager = FindAnyObjectByType<MenuManager>();
        }

        // Noch nicht aktiv
        if (btnSave != null)
        {
            btnSave.interactable = false;
        }

        if (btnSettings != null)
        {
            btnSettings.interactable = false;
        }
    }

    #endregion

    #region Button Events

    public void OnClickSignInDummy()
    {
        Debug.Log("[StartMenuController] SignIn Dummy geklickt. Noch ohne Funktion.");
    }

    public void OnClickOpenLoginMenu()
    {
        Debug.Log("[StartMenuController] Wechsel ins LoginMenuPanel.");

        if (menuManager != null)
        {
            menuManager.ShowLoginMenu();
        }
        else
        {
            Debug.LogError("[StartMenuController] MenuManager fehlt.");
        }
    }

    public void OnClickSaveDisabled()
    {
        Debug.Log("[StartMenuController] Speichern ist derzeit nicht aktiv.");
    }

    public void OnClickSettingsDisabled()
    {
        Debug.Log("[StartMenuController] Einstellungen sind derzeit nicht aktiv.");
    }

    public void OnClickCloseMenu()
    {
        Debug.Log("[StartMenuController] Menü schließen.");

        if (menuManager != null)
        {
            menuManager.HideAllMenus();
        }
        else
        {
            Debug.LogError("[StartMenuController] MenuManager fehlt.");
        }
    }

    #endregion
}