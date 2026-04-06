using UnityEngine;

public class NPCInteraction : MonoBehaviour, INPCInteractable
{
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private GameObject interactHint;
    [SerializeField] private StartMenuController startMenuController;

    private bool playerInRange = false;


        public void Interact()
        {
            Debug.Log("NPC Interaktion gestartet");

            // Hier später:
            // Login öffnen
            // Dialog starten
            // Szene wechseln etc.
        }


    private void Start()
    {
        if (interactHint != null)
        {
            interactHint.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            Debug.Log("Arthur interaction triggered");

            if (startMenuController != null)
            {
                startMenuController.OpenMenu();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;

        if (interactHint != null)
        {
            interactHint.SetActive(true);
        }

        Debug.Log("Player entered Arthur range");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;

        if (interactHint != null)
        {
            interactHint.SetActive(false);
        }

        Debug.Log("Player left Arthur range");
    }
}