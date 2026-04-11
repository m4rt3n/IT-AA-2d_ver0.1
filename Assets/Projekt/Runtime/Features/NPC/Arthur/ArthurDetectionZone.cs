/*
 * Datei: ArthurDetectionZone.cs
 * Zweck: Erkennt, wenn der Spieler Arthurs Erfassungsbereich betritt.
 * Verantwortung: Meldet den Spieler an ArthurAutoInteraction weiter.
 * Abhängigkeiten: ArthurAutoInteraction, 2D-Trigger-Collider.
 * Verwendet von: DetectionZone-Childobjekt von Arthur.
 */
using UnityEngine;

namespace ITAA.NPC.Arthur
{
    public class ArthurDetectionZone : MonoBehaviour
    {
        [SerializeField] private ArthurAutoInteraction arthurAutoInteraction;
        [SerializeField] private string playerTag = "Player";

        private void Start()
        {
            if (arthurAutoInteraction == null)
            {
                arthurAutoInteraction = GetComponentInParent<ArthurAutoInteraction>();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(playerTag))
            {
                arthurAutoInteraction?.SetTargetPlayer(other.transform);
            }
        }
    }
}