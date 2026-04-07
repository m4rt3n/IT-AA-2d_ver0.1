using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("Root Objects")]
    [SerializeField] private GameObject backgroundDim;
    [SerializeField] private GameObject menuPanel;

    [Header("Panels")]
    [SerializeField] private GameObject startMenuPanel;
    [SerializeField] private GameObject loginMenuPanel;

    [Header("Canvas Groups")]
    [SerializeField] private CanvasGroup backgroundDimGroup;
    [SerializeField] private CanvasGroup startMenuGroup;
    [SerializeField] private CanvasGroup loginMenuGroup;

    private void Awake()
    {
        HideAll();
    }

    public void ShowStartMenu()
    {
        Debug.Log("[MenuManager] ShowStartMenu");

        if (backgroundDim != null) backgroundDim.SetActive(true);
        if (menuPanel != null) menuPanel.SetActive(true);
        if (startMenuPanel != null) startMenuPanel.SetActive(true);
        if (loginMenuPanel != null) loginMenuPanel.SetActive(false);

        SetCanvasGroup(backgroundDimGroup, true, 0.65f);
        SetCanvasGroup(startMenuGroup, true, 1f);
        SetCanvasGroup(loginMenuGroup, false, 0f);

        Debug.Log("[MenuManager] StartMenuGroup -> alpha=" + startMenuGroup.alpha +
                  " interactable=" + startMenuGroup.interactable +
                  " blocksRaycasts=" + startMenuGroup.blocksRaycasts);

        Debug.Log("[MenuManager] LoginMenuGroup -> alpha=" + loginMenuGroup.alpha +
                  " interactable=" + loginMenuGroup.interactable +
                  " blocksRaycasts=" + loginMenuGroup.blocksRaycasts);
    }

    public void ShowLoginMenu()
    {
        Debug.Log("[MenuManager] ShowLoginMenu");

        if (backgroundDim != null) backgroundDim.SetActive(true);
        if (menuPanel != null) menuPanel.SetActive(true);
        if (startMenuPanel != null) startMenuPanel.SetActive(false);
        if (loginMenuPanel != null) loginMenuPanel.SetActive(true);

        SetCanvasGroup(backgroundDimGroup, true, 0.65f);
        SetCanvasGroup(startMenuGroup, false, 0f);
        SetCanvasGroup(loginMenuGroup, true, 1f);

        Debug.Log("[MenuManager] StartMenuGroup -> alpha=" + startMenuGroup.alpha +
                  " interactable=" + startMenuGroup.interactable +
                  " blocksRaycasts=" + startMenuGroup.blocksRaycasts);

        Debug.Log("[MenuManager] LoginMenuGroup -> alpha=" + loginMenuGroup.alpha +
                  " interactable=" + loginMenuGroup.interactable +
                  " blocksRaycasts=" + loginMenuGroup.blocksRaycasts);
    }

    public void HideAll()
    {
        SetCanvasGroup(backgroundDimGroup, false, 0f);
        SetCanvasGroup(startMenuGroup, false, 0f);
        SetCanvasGroup(loginMenuGroup, false, 0f);

        if (startMenuPanel != null) startMenuPanel.SetActive(false);
        if (loginMenuPanel != null) loginMenuPanel.SetActive(false);
        if (menuPanel != null) menuPanel.SetActive(false);
        if (backgroundDim != null) backgroundDim.SetActive(false);
    }

    private void SetCanvasGroup(CanvasGroup group, bool visible, float alpha)
    {
        if (group == null)
            return;

        group.alpha = alpha;
        group.interactable = visible;
        group.blocksRaycasts = visible;
    }
}