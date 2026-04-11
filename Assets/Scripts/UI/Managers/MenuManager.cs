using UnityEngine;

public class MenuManager : MonoBehaviour
{
    #region Inspector

    [Header("Root")]
    [SerializeField] private GameObject backgroundDim;
    [SerializeField] private GameObject menuPanel;

    [Header("Panels")]
    [SerializeField] private GameObject startMenuPanel;
    [SerializeField] private GameObject loadGamePanel;

    #endregion

    #region Unity

    private void Awake()
    {
        HideAllMenus();
    }

    #endregion

    #region Public Methods

    public void ShowStartMenu()
    {
        SetRoot(true);

        if (startMenuPanel != null) startMenuPanel.SetActive(true);
        if (loadGamePanel != null) loadGamePanel.SetActive(false);

        Debug.Log("[MenuManager] Startmenü aktiv.");
    }

    public void ShowLoadGameMenu()
    {
        SetRoot(true);

        if (startMenuPanel != null) startMenuPanel.SetActive(false);
        if (loadGamePanel != null) loadGamePanel.SetActive(true);

        Debug.Log("[MenuManager] LoadGamePanel aktiv.");
    }

    public void HideAllMenus()
    {
        SetRoot(false);

        if (startMenuPanel != null) startMenuPanel.SetActive(false);
        if (loadGamePanel != null) loadGamePanel.SetActive(false);

        Debug.Log("[MenuManager] Alle Menüs verborgen.");
    }

    #endregion

    #region Private Methods

    private void SetRoot(bool visible)
    {
        if (backgroundDim != null) backgroundDim.SetActive(visible);
        if (menuPanel != null) menuPanel.SetActive(visible);
    }

    #endregion
}