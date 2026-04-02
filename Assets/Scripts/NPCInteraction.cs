using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange)
        {
            Debug.Log("Player ist im Trigger");
        }

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Interaktion gestartet");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}