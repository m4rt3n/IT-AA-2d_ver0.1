/*
 * Datei: ArthurMovementToPlayer.cs
 * Zweck:
 *   Bewegt Arthur in Richtung Spieler und übergibt die tatsächliche
 *   Bewegungsrichtung an den ArthurAnimationController.
 *
 * Verantwortung:
 *   - Spieler finden oder gesetztes Ziel verwenden
 *   - Arthur bis zur Stop-Distanz zum Spieler bewegen
 *   - Walk-Animation nur bei echter Bewegung abspielen
 *   - Idle-Animation erzwingen, wenn Arthur steht
 *
 * Voraussetzungen:
 *   - ArthurAnimationController auf demselben GameObject oder Kindobjekt
 *   - Animator nutzt Blend Trees mit MoveX / MoveY
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

        [Header("Auto Find")]
        [SerializeField] private bool autoFindPlayerByTag = true;
        [SerializeField] private string playerTag = "Player";

        [Header("Debug")]
        [SerializeField] private bool showDebugLogs = false;

        #endregion

        #region Fields

        private bool isStopped;

        #endregion

        #region Unity Methods

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

            if (playerTarget == null && autoFindPlayerByTag)
            {
                GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);

                if (playerObject != null)
                {
                    playerTarget = playerObject.transform;
                }
            }
        }

        private void Start()
        {
            if (animationController == null)
            {
                Debug.LogWarning(
                    $"[{nameof(ArthurMovementToPlayer)}] Kein {nameof(ArthurAnimationController)} gefunden auf '{gameObject.name}'."
                );
            }

            if (playerTarget == null)
            {
                Debug.LogWarning(
                    $"[{nameof(ArthurMovementToPlayer)}] Kein Spieler-Ziel gefunden auf '{gameObject.name}'."
                );
            }
        }

        private void FixedUpdate()
        {
            if (playerTarget == null)
            {
                ForceIdleIfNeeded();
                return;
            }

            Vector2 currentPosition = transform.position;
            Vector2 targetPosition = playerTarget.position;
            Vector2 toTarget = targetPosition - currentPosition;
            float distanceToTarget = toTarget.magnitude;

            if (distanceToTarget <= stopDistance)
            {
                ForceIdleIfNeeded();
                return;
            }

            isStopped = false;

            Vector2 nextPosition = Vector2.MoveTowards(
                currentPosition,
                targetPosition,
                moveSpeed * Time.fixedDeltaTime
            );

            Vector2 actualMovement = nextPosition - currentPosition;

            transform.position = new Vector3(nextPosition.x, nextPosition.y, transform.position.z);

            if (animationController != null)
            {
                if (showDebugLogs)
                {
                    Debug.Log($"[{nameof(ArthurMovementToPlayer)}] Move={actualMovement}");
                }

                animationController.SetMovement(actualMovement);
            }
        }

        #endregion

        #region Public API

        public void SetTarget(Transform newTarget)
        {
            playerTarget = newTarget;
            isStopped = false;
        }

        public void ClearTarget()
        {
            playerTarget = null;
            ForceIdleIfNeeded();
        }

        public void StopMoving()
        {
            ForceIdleIfNeeded();
        }

        #endregion

        #region Private Methods

        private void ForceIdleIfNeeded()
        {
            if (isStopped)
            {
                return;
            }

            isStopped = true;

            if (animationController != null)
            {
                if (showDebugLogs)
                {
                    Debug.Log($"[{nameof(ArthurMovementToPlayer)}] ForceIdle");
                }

                animationController.ForceIdle();
            }
        }

        #endregion
    }
}