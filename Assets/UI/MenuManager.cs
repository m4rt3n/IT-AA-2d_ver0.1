using UnityEngine;

public class MenuManager : MonoBehaviour
{
    #region Inspector

    [SerializeField] private GameObject backgroundDim;
    [SerializeField] private GameObject startMenuPanel;
    [SerializeField] private GameObject loginMenuPanel;

    #endregion

    #region Unity Methods

    private void Start()
    {
        HideAllMenus();
    }

    #endregion

    #region Public Methods

    public void ShowStartMenu()
    {
        if (backgroundDim != null) backgroundDim.SetActive(true);
        if (startMenuPanel != null) startMenuPanel.SetActive(true);
        if (loginMenuPanel != null) loginMenuPanel.SetActive(false);

        Debug.Log("[MenuManager] Startmenü aktiv.");
    }

    public void ShowLoginMenu()
    {
        if (backgroundDim != null) backgroundDim.SetActive(true);
        if (startMenuPanel != null) startMenuPanel.SetActive(false);
        if (loginMenuPanel != null) loginMenuPanel.SetActive(true);

        Debug.Log("[MenuManager] LoginMenuPanel aktiv.");
    }

    public void HideAllMenus()
    {
        if (backgroundDim != null) backgroundDim.SetActive(false);
        if (startMenuPanel != null) startMenuPanel.SetActive(false);
        if (loginMenuPanel != null) loginMenuPanel.SetActive(false);

        Debug.Log("[MenuManager] Alle Menüs verborgen.");
    }

    #endregion
}