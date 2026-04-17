/*
 * Datei: ArthurMovementToPlayer.cs
 * Zweck:
 *   Bewegt Arthur nur dann zum Spieler, wenn dieser in Reichweite ist.
 *   Ansonsten bleibt Arthur idle.
 */

using UnityEngine;

namespace ITAA.NPC.Arthur
{
    public class ArthurMovementToPlayer : MonoBehaviour
    {
        #region Inspector

        [Header("References")]
        [SerializeField] private Transform playerTarget;
        [SerializeField] private ArthurAnimationController animationController;

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 2.5f;
        [SerializeField] private float stopDistance = 1.25f;

        [Header("Control")]
        [SerializeField] private bool moveOnlyWhenPlayerInRange = true;

        [Header("Debug")]
        [SerializeField] private bool showDebugLogs = false;

        #endregion

        #region Fields

        private bool canMove;

        #endregion

        #region Unity

        private void Awake()
        {
            if (animationController == null)
            {
                animationController = GetComponent<ArthurAnimationController>();

                if (animationController == null)
                {
                    animationController = GetComponentInChildren<ArthurAnimationController>();
                }
            }
        }

        private void FixedUpdate()
        {
            // ❌ Movement blockiert
            if (moveOnlyWhenPlayerInRange && !canMove)
            {
                if (animationController != null)
                {
                    animationController.ForceIdle();
                }
                return;
            }

            if (playerTarget == null)
            {
                if (animationController != null)
                {
                    animationController.ForceIdle();
                }
                return;
            }

            Vector2 currentPosition = transform.position;
            Vector2 targetPosition = playerTarget.position;

            Vector2 toTarget = targetPosition - currentPosition;
            float distance = toTarget.magnitude;

            if (distance <= stopDistance)
            {
                if (animationController != null)
                {
                    animationController.ForceIdle();
                }
                return;
            }

            Vector2 nextPosition = Vector2.MoveTowards(
                currentPosition,
                targetPosition,
                moveSpeed * Time.fixedDeltaTime
            );

            Vector2 movement = nextPosition - currentPosition;

            transform.position = new Vector3(nextPosition.x, nextPosition.y, transform.position.z);

            if (animationController != null)
            {
                if (showDebugLogs)
                {
                    Debug.Log($"[ArthurMovement] Move={movement}");
                }

                animationController.SetMovement(movement);
            }
        }

        #endregion

        #region Public API

        public void EnableMovement(Transform target)
        {
            playerTarget = target;
            canMove = true;

            if (showDebugLogs)
            {
                Debug.Log("[ArthurMovement] Movement ENABLED");
            }
        }

        public void DisableMovement()
        {
            canMove = false;

            if (animationController != null)
            {
                animationController.ForceIdle();
            }

            if (showDebugLogs)
            {
                Debug.Log("[ArthurMovement] Movement DISABLED");
            }
        }

        #endregion
    }
}