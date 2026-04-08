using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    [SerializeField] private GameObject interactionHint;
    [SerializeField] private bool openLoginMenu = true;

    private bool playerInside;
    private MenuManager menuManager;

    private void Start()
    {
        menuManager = FindAnyObjectByType<MenuManager>();

        if (interactionHint != null)
        {
            interactionHint.SetActive(false);
        }
    }

    private void Update()
    {
        if (!playerInside)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("[NPCInteraction] Interaktion mit NPC ausgelöst.");

            if (openLoginMenu && menuManager != null)
            {
                menuManager.ShowLoginMenu();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        playerInside = true;

        if (interactionHint != null)
        {
            interactionHint.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        playerInside = false;

        if (interactionHint != null)
        {
            interactionHint.SetActive(false);
        }
    }
}