using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject startMenuPanel;
    [SerializeField] private GameObject loginMenuPanel;
    [SerializeField] private GameObject backgroundDim;

    public void ShowStartMenu()
    {
        if (backgroundDim != null)
            backgroundDim.SetActive(true);

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

        if (backgroundDim != null)
            backgroundDim.SetActive(true);
    }

    public void HideAll()
    {
        if (backgroundDim != null)
            backgroundDim.SetActive(false);

        if (startMenuPanel != null)
            startMenuPanel.SetActive(false);

        if (loginMenuPanel != null)
            loginMenuPanel.SetActive(false);
    }
}