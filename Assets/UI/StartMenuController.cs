using UnityEngine;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] private MenuManager menuManager;

    public void OnClickOpenLogin()
    {
        menuManager.ShowLoginMenu();
    }

    public void OnClickClose()
    {
        menuManager.HideAll();

        NPCInteraction npc = FindFirstObjectByType<NPCInteraction>();
        if (npc != null)
            npc.EndInteraction();
    }
}