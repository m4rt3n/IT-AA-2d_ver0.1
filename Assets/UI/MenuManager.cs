using UnityEngine;

public class MenuManager : MonoBehaviour
{
    #region Inspector

    [Header("Root")]
    [SerializeField] private GameObject backgroundDim;
    [SerializeField] private GameObject menuPanel;

    [Header("Panels")]
    [SerializeField] private GameObject startMenuPanel;
    [SerializeField] private GameObject loginMenuPanel;
    [SerializeField] private GameObject saveSlotPopup;

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
        if (loginMenuPanel != null) loginMenuPanel.SetActive(false);
        if (saveSlotPopup != null) saveSlotPopup.SetActive(false);

        Debug.Log("[MenuManager] Startmenü aktiv.");
    }

    public void ShowLoginMenu()
    {
        SetRoot(true);

        if (startMenuPanel != null) startMenuPanel.SetActive(false);
        if (loginMenuPanel != null) loginMenuPanel.SetActive(true);
        if (saveSlotPopup != null) saveSlotPopup.SetActive(false);

        Debug.Log("[MenuManager] Loginmenü aktiv.");
    }

    public void ShowSaveSlotMenu()
    {
        SetRoot(true);

        if (startMenuPanel != null) startMenuPanel.SetActive(false);
        if (loginMenuPanel != null) loginMenuPanel.SetActive(false);
        if (saveSlotPopup != null) saveSlotPopup.SetActive(true);

        Debug.Log("[MenuManager] SaveSlot-Menü aktiv.");
    }

    public void HideAllMenus()
    {
        SetRoot(false);

        if (startMenuPanel != null) startMenuPanel.SetActive(false);
        if (loginMenuPanel != null) loginMenuPanel.SetActive(false);
        if (saveSlotPopup != null) saveSlotPopup.SetActive(false);

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