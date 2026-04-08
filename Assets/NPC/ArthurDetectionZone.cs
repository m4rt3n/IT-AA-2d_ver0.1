using UnityEngine;

public class ArthurDetectionZone : MonoBehaviour
{
    #region Inspector

    [SerializeField] private ArthurAutoInteraction arthurAutoInteraction;

    #endregion

    #region Unity Methods

    private void Reset()
    {
        // Sucht automatisch die ArthurAutoInteraction am Parent
        arthurAutoInteraction = GetComponentInParent<ArthurAutoInteraction>();
    }

    private void Awake()
    {
        // Falls im Inspector nichts gesetzt wurde, beim Start noch einmal suchen
        if (arthurAutoInteraction == null)
        {
            arthurAutoInteraction = GetComponentInParent<ArthurAutoInteraction>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("[ArthurDetectionZone] DetectionZone getroffen von: " + other.name);

        // Nur der Player darf Arthur auslösen
        if (!other.CompareTag("Player"))
        {
            Debug.Log("[ArthurDetectionZone] Objekt ignoriert, da es nicht den Tag 'Player' hat.");
            return;
        }

        if (arthurAutoInteraction != null)
        {
            Debug.Log("[ArthurDetectionZone] Arthur Auto Interaction wird ausgelöst.");
            arthurAutoInteraction.TriggerAutoInteraction(other.gameObject);
        }
        else
        {
            Debug.LogWarning("[ArthurDetectionZone] ArthurAutoInteraction Referenz fehlt.");
        }
    }

    #endregion
}