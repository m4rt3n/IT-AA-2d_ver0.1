using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    #region Inspector

    [Header("UI")]
    [SerializeField] private MenuManager menuManager;

    #endregion

    #region State

    private bool isInteractionOpen;

    #endregion

    #region Unity

    private void Start()
    {
        if (menuManager == null)
        {
            menuManager = FindFirstObjectByType<MenuManager>();
        }
    }

    #endregion

    #region Public Methods

    public void OpenStartMenu()
    {
        if (isInteractionOpen)
        {
            return;
        }

        isInteractionOpen = true;

        if (menuManager != null)
        {
            Debug.Log("[NPCInteraction] Öffne Startmenü.");
            menuManager.ShowStartMenu();
        }
        else
        {
            Debug.LogError("[NPCInteraction] MenuManager nicht gesetzt.");
        }
    }

    public void CloseStartMenu()
    {
        isInteractionOpen = false;

        if (menuManager != null)
        {
            Debug.Log("[NPCInteraction] Schließe Startmenü.");
            menuManager.HideAllMenus();
        }
        else
        {
            Debug.LogError("[NPCInteraction] MenuManager nicht gesetzt.");
        }
    }

    #endregion
}