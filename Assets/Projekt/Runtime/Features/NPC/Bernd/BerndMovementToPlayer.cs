/*
 * Datei: BerndMovementToPlayer.cs
 * Zweck: Richtet Bernd zum Spieler aus und kann sich optional leicht annähern.
 * Verantwortung:
 * - Berechnet Richtung zum Spieler
 * - Dreht Bernd in Blickrichtung
 * - Bewegt Bernd optional zum Spieler
 * - Aktualisiert AnimationController
 *
 * Hinweis:
 * - Für einen Quest-NPC reicht meist "nur ausrichten".
 * - moveToPlayer kann deaktiviert bleiben.
 */

using UnityEngine;

namespace ITAA.NPC.Bernd
{
    [DisallowMultipleComponent]
    public class BerndMovementToPlayer : MonoBehaviour
    {
        #region Inspector

        [Header("References")]
        [SerializeField] private BerndDetectionZone detectionZone;
        [SerializeField] private BerndAnimationController animationController;
        [SerializeField] private Rigidbody2D rb;

        [Header("Movement")]
        [SerializeField] private bool moveToPlayer;
        [SerializeField] private float moveSpeed = 1.5f;
        [SerializeField] private float stopDistance = 1.25f;
        [SerializeField] private bool facePlayerWhenInRange = true;

        #endregion

        #region State

        private Vector2 currentVelocity;
        private bool interactionLocked;

        #endregion

        #region Unity

        private void Awake()
        {
            if (detectionZone == null)
            {
                detectionZone = GetComponentInChildren<BerndDetectionZone>();
            }

            if (animationController == null)
            {
                animationController = GetComponent<BerndAnimationController>();
            }

            if (rb == null)
            {
                rb = GetComponent<Rigidbody2D>();
            }
        }

        private void FixedUpdate()
        {
            if (interactionLocked)
            {
                StopMovement();
                return;
            }

            if (detectionZone == null || !detectionZone.HasTargetPlayer)
            {
                StopMovement();
                return;
            }

            Transform player = detectionZone.TargetPlayer;
            Vector2 toPlayer = player.position - transform.position;
            float distance = toPlayer.magnitude;

            if (distance <= 0.0001f)
            {
                StopMovement();
                return;
            }

            Vector2 direction = toPlayer.normalized;

            if (facePlayerWhenInRange)
            {
                animationController?.SetLookDirection(direction);
            }

            if (!moveToPlayer || distance <= stopDistance)
            {
                StopMovement();
                return;
            }

            currentVelocity = direction * moveSpeed;

            if (rb != null)
            {
                rb.MovePosition(rb.position + currentVelocity * Time.fixedDeltaTime);
            }
            else
            {
                transform.position += (Vector3)(currentVelocity * Time.fixedDeltaTime);
            }

            animationController?.PlayWalk(direction);
        }

        #endregion

        #region Public API

        public void SetInteractionLocked(bool isLocked)
        {
            interactionLocked = isLocked;

            if (interactionLocked)
            {
                StopMovement();
            }
        }

        #endregion

        #region Private

        private void StopMovement()
        {
            currentVelocity = Vector2.zero;
            animationController?.SetMovement(Vector2.zero);
        }

        #endregion
    }
}