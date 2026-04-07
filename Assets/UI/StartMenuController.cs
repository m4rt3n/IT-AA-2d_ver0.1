using UnityEngine;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] private MenuManager menuManager;
    [SerializeField] private NPCInteraction npcInteraction;

    public void OnClickOpenLogin()
    {
        Debug.Log("[StartMenuController] OnClickOpenLogin");

        if (menuManager == null)
        {
            Debug.LogError("[StartMenuController] MenuManager fehlt");
            return;
        }

        menuManager.ShowLoginMenu();
    }

    public void OnClickClose()
    {
        Debug.Log("[StartMenuController] OnClickClose");

        if (npcInteraction != null)
        {
            npcInteraction.EndInteraction();
            return;
        }

        NPCInteraction foundNpc = FindFirstObjectByType<NPCInteraction>();
        if (foundNpc != null)
        {
            foundNpc.EndInteraction();
        }
        else
        {
            Debug.LogWarning("[StartMenuController] Keine NPCInteraction gefunden");
            if (menuManager != null)
                menuManager.HideAll();
        }
    }
}