using UnityEngine;

public class StartMenuController : MonoBehaviour
{
    #region Inspector

    [SerializeField] private MenuManager menuManager;

    #endregion

    #region Unity

    private void Awake()
    {
        if (menuManager == null)
        {
            menuManager = FindFirstObjectByType<MenuManager>();
        }
    }

    #endregion

    #region Public Button Events

    public void OnClickLoadGame()
    {
        if (menuManager == null)
        {
            Debug.LogError("[StartMenuController] MenuManager fehlt.");
            return;
        }

        menuManager.ShowLoadGameMenu();
    }

    public void OnClickBackToStartMenu()
    {
        if (menuManager == null)
        {
            Debug.LogError("[StartMenuController] MenuManager fehlt.");
            return;
        }

        menuManager.ShowStartMenu();
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

    #endregion
}