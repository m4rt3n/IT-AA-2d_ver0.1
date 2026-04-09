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

    #region Public Button Events

    public void OnClickOpenLogin()
    {
        if (menuManager == null)
        {
            Debug.LogError("[StartMenuController] MenuManager fehlt.");
            return;
        }

        menuManager.ShowLoginMenu();
    }

    public void OnClickClose()
    {
        if (menuManager == null)
        {
            Debug.LogError("[StartMenuController] MenuManager fehlt.");
            return;
        }

        menuManager.HideAllMenus();
    }

    public void OnClickSave()
    {
        Debug.Log("[StartMenuController] Save-Button aktuell noch ohne Funktion.");
    }

    public void OnClickSettings()
    {
        Debug.Log("[StartMenuController] Settings-Button aktuell noch ohne Funktion.");
    }

    #endregion
}