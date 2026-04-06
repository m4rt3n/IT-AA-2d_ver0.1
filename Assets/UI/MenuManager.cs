using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject startMenuPanel;
    [SerializeField] private GameObject loginMenuPanel;

    private void Start()
    {
        ShowStartMenu();
    }

    public void ShowStartMenu()
    {
        if (startMenuPanel != null)
            startMenuPanel.SetActive(true);

        if (loginMenuPanel != null)
            loginMenuPanel.SetActive(false);
    }

    public void ShowLoginMenu()
    {
        if (startMenuPanel != null)
            startMenuPanel.SetActive(false);

        if (loginMenuPanel != null)
            loginMenuPanel.SetActive(true);
    }
}