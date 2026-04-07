using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject backgroundDim;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject startMenuPanel;
    [SerializeField] private GameObject loginMenuPanel;

    private void Awake()
    {
        Debug.Log("[MenuManager] Awake");
        HideAll();
    }

    public void ShowStartMenu()
    {
        Debug.Log("[MenuManager] ShowStartMenu");

        if (backgroundDim == null) Debug.LogError("[MenuManager] backgroundDim fehlt");
        if (menuPanel == null) Debug.LogError("[MenuManager] menuPanel fehlt");
        if (startMenuPanel == null) Debug.LogError("[MenuManager] startMenuPanel fehlt");
        if (loginMenuPanel == null) Debug.LogError("[MenuManager] loginMenuPanel fehlt");

        if (backgroundDim != null) backgroundDim.SetActive(true);
        if (menuPanel != null) menuPanel.SetActive(true);
        if (startMenuPanel != null) startMenuPanel.SetActive(true);
        if (loginMenuPanel != null) loginMenuPanel.SetActive(false);
    }

    public void ShowLoginMenu()
    {
        Debug.Log("[MenuManager] ShowLoginMenu");

        if (backgroundDim != null) backgroundDim.SetActive(true);
        if (menuPanel != null) menuPanel.SetActive(true);
        if (startMenuPanel != null) startMenuPanel.SetActive(false);
        if (loginMenuPanel != null) loginMenuPanel.SetActive(true);
    }

    public void HideAll()
    {
        Debug.Log("[MenuManager] HideAll");

        if (startMenuPanel != null) startMenuPanel.SetActive(false);
        if (loginMenuPanel != null) loginMenuPanel.SetActive(false);
        if (menuPanel != null) menuPanel.SetActive(false);
        if (backgroundDim != null) backgroundDim.SetActive(false);
    }
}