using UnityEngine;

public class ArthurDetectionZone : MonoBehaviour
{
    [SerializeField] private ArthurAutoInteraction arthurAutoInteraction;

    private void Reset()
    {
        arthurAutoInteraction = GetComponentInParent<ArthurAutoInteraction>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("DetectionZone getroffen von: " + other.name);

        if (!other.CompareTag("Player"))
            return;

        if (arthurAutoInteraction != null)
        {
            Debug.Log("Arthur Auto Interaction wird ausgelöst");
            arthurAutoInteraction.TriggerAutoInteraction(other.transform);
        }
        else
        {
            Debug.LogWarning("ArthurAutoInteraction Referenz fehlt");
        }
    }
}