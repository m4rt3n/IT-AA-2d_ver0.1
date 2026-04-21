/*
 * Datei: BerndDetectionZone.cs
 * Zweck: Erkennt, ob sich der Spieler in Bernds Interaktionsbereich befindet.
 * Verantwortung:
 * - Speichert Zielspieler bei Trigger Enter
 * - Entfernt Zielspieler bei Trigger Exit
 * - Bietet lesbare Properties für andere Bernd-Komponenten
 *
 * Voraussetzungen:
 * - Collider2D auf diesem Objekt oder Child-Objekt
 * - IsTrigger aktiviert
 * - Spieler besitzt Tag "Player"
 */

using UnityEngine;

namespace ITAA.NPC.Bernd
{
    [DisallowMultipleComponent]
    public class BerndDetectionZone : MonoBehaviour
    {
        #region Inspector

        [Header("Config")]
        [SerializeField] private string playerTag = "Player";

        #endregion

        #region Properties

        public bool HasTargetPlayer => targetPlayer != null;
        public Transform TargetPlayer => targetPlayer;

        #endregion

        #region State

        private Transform targetPlayer;

        #endregion

        #region Unity

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(playerTag))
            {
                return;
            }

            targetPlayer = other.transform;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag(playerTag))
            {
                return;
            }

            if (targetPlayer == other.transform)
            {
                targetPlayer = null;
            }
        }

        #endregion
    }
}