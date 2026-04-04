using UnityEngine;

public class NPCInteraction : MonoBehaviour, INPCInteractable
{
    [SerializeField] private string npcName = "Arthur";
    [SerializeField] private string message = "Hallo, ich bin Arthur.";

    public void Interact()
    {
        Debug.Log(npcName + ": " + message);
    }
}