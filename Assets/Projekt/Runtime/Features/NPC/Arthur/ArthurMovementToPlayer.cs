/*
 * Datei: ArthurMovementToPlayer.cs
 * Zweck: Lässt Arthur automatisch zum Spieler laufen.
 * Verantwortung:
 *   - Nutzt Ziel aus ArthurAutoInteraction
 *   - Bewegt Arthur in Richtung Spieler
 *   - Stoppt in Interaktionsdistanz
 *   - Übergibt Bewegungsrichtung an ArthurAnimationController
 *
 * Abhängigkeiten:
 *   - Rigidbody2D
 *   - ArthurAutoInteraction
 *   - ArthurAnimationController
 *
 * Verwendet von:
 *   - Arthur (NPC)
 */

using UnityEngine;

namespace ITAA.NPC.Arthur
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ArthurMovementToPlayer : MonoBehaviour
    {
        #region Inspector

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 2.5f;
        [SerializeField] private float stopDistance = 1.2f;

        [Header("References")]
        [SerializeField] private ArthurAutoInteraction interaction;
        [SerializeField] private ArthurAnimationController animationController;

        #endregion

        #region Fields

        private Rigidbody2D rb;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();

            if (interaction == null)
            {
                interaction = GetComponent<ArthurAutoInteraction>();
            }

            if (animationController == null)
            {
                animationController = GetComponent<ArthurAnimationController>();

                if (animationController == null)
                {
                    animationController = GetComponentInChildren<ArthurAnimationController>();
                }
            }

            if (animationController == null)
            {
                Debug.LogWarning($"[{nameof(ArthurMovementToPlayer)}] Kein ArthurAnimationController gefunden auf '{gameObject.name}'.");
            }
        }

        private void FixedUpdate()
        {
            if (interaction == null || interaction.TargetPlayer == null)
            {
                StopMovement();
                return;
            }

            Vector2 direction = (Vector2)(interaction.TargetPlayer.position - transform.position);
            float distance = direction.magnitude;

            if (distance <= stopDistance)
            {
                StopMovement();
                return;
            }

            direction.Normalize();
            rb.linearVelocity = direction * moveSpeed;

            if (animationController != null)
            {
                animationController.SetMovement(direction);
            }
        }

        private void OnDisable()
        {
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }

            if (animationController != null)
            {
                animationController.ForceIdle();
            }
        }

        #endregion

        #region Private Methods

        private void StopMovement()
        {
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }

            if (animationController != null)
            {
                animationController.SetMovement(Vector2.zero);
            }
        }

        #endregion
    }
}