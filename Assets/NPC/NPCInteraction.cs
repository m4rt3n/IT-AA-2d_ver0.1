using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    [SerializeField] private string npcName = "Arthur";
    [SerializeField] private MenuManager menuManager;

    private bool isInteracting;

    public void Interact()
    {
        if (isInteracting)
            return;

        isInteracting = true;

        Debug.Log("NPC Interaktion gestartet: " + npcName);

        if (menuManager != null)
        {
            menuManager.ShowStartMenu();
        }
        else
        {
            Debug.LogWarning("MenuManager fehlt!");
        }

        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player != null)
        {
            player.SetPlayerControlEnabled(false);
        }
    }

    public void EndInteraction()
    {
        isInteracting = false;

        if (menuManager != null)
        {
            menuManager.HideAll();
        }

        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player != null)
        {
            player.SetPlayerControlEnabled(true);
        }
    }
}