using UnityEngine;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] private MenuManager menuManager;

    public void OnClickOpenLogin()
    {
        if (menuManager != null)
        {
            menuManager.ShowLoginMenu();
        }
        else
        {
            Debug.LogWarning("StartMenuController: MenuManager ist nicht gesetzt.");
        }
    }

    public void OnClickQuit()
    {
        Debug.Log("Spiel wird beendet.");
        Application.Quit();
    }
}