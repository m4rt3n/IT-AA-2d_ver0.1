using UnityEngine;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] private MenuManager menuManager;

    private void Start()
    {
        if (menuManager == null)
        {
            menuManager = FindAnyObjectByType<MenuManager>();
        }
    }

    public void OnClickOpenLogin()
    {
        Debug.Log("[StartMenuController] Öffne Login-Menü.");

        if (menuManager != null)
        {
            menuManager.ShowLoginMenu();
        }
    }

    public void OnClickClose()
    {
        Debug.Log("[StartMenuController] Menü schließen.");

        if (menuManager != null)
        {
            menuManager.HideAllMenus();
        }
    }
}