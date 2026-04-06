using UnityEngine;

public class NPCInteraction : MonoBehaviour, INPCInteractable
{
    [Header("UI")]
    [SerializeField] private GameObject interactHint;
    [SerializeField] private StartMenuController startMenuController;

    [Header("Settings")]
    [SerializeField] private bool hideHintOnInteract = true;

    private bool playerInRange = false;

    public void Interact()
    {
        if (!playerInRange)
            return;

        Debug.Log("NPC Interaktion gestartet");

        if (startMenuController != null)
        {
            startMenuController.OpenMenu();
        }

        if (hideHintOnInteract && interactHint != null)
        {
            interactHint.SetActive(false);
        }
    }

    private void Start()
    {
        if (interactHint != null)
        {
            interactHint.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInRange = true;

        if (interactHint != null)
        {
            interactHint.SetActive(true);
        }

        Debug.Log("Player entered Arthur range");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInRange = false;

        if (interactHint != null)
        {
            interactHint.SetActive(false);
        }

        Debug.Log("Player left Arthur range");
    }
}