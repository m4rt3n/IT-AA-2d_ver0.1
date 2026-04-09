using UnityEngine;

public class ArthurDetectionZone : MonoBehaviour
{
    #region Inspector

    [SerializeField] private ArthurAutoInteraction arthurAutoInteraction;

    #endregion

    #region Unity Methods

    private void Reset()
    {
        // Wird automatisch gesetzt, wenn das Script neu hinzugefügt wird
        arthurAutoInteraction = GetComponentInParent<ArthurAutoInteraction>();
    }

    private void Awake()
    {
        // Falls im Inspector nichts gesetzt ist, beim Start noch einmal suchen
        if (arthurAutoInteraction == null)
        {
            arthurAutoInteraction = GetComponentInParent<ArthurAutoInteraction>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("[ArthurDetectionZone] Trigger von: " + other.name);

        // Nur der Player darf Arthur auslösen
        if (!other.CompareTag("Player"))
        {
            Debug.Log("[ArthurDetectionZone] Ignoriert, da Objekt nicht Player ist.");
            return;
        }

        if (arthurAutoInteraction != null)
        {
            Debug.Log("[ArthurDetectionZone] ArthurAutoInteraction wird ausgelöst.");
            arthurAutoInteraction.TriggerAutoInteraction(other.gameObject);
        }
        else
        {
            Debug.LogWarning("[ArthurDetectionZone] ArthurAutoInteraction Referenz fehlt.");
        }
    }

    #endregion
}