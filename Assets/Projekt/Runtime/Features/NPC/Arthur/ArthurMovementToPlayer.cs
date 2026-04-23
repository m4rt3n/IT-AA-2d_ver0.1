/*
 * Datei: ArthurMovementToPlayer.cs
 * Zweck:
 * Bewegt Arthur kontrolliert in Richtung Spieler und meldet Start/Stop/Ankunft
 * an andere Systeme (z. B. ArthurAutoInteraction).
 *
 * Verantwortung:
 * - Zielspieler setzen/entfernen
 * - Arthur bis zur Stop-Distanz bewegen
 * - Bewegungsstatus über Events melden
 * - Idle/Walk-Animation sauber ansteuern
 *
 * Voraussetzungen:
 * - ArthurAnimationController auf demselben GameObject oder Kindobjekt
 * - ArthurAutoInteraction kann diese Klasse über Events abonnieren
 */

using System;
using UnityEngine;

namespace ITAA.NPC.Arthur
{
    public sealed class ArthurMovementToPlayer : MonoBehaviour
    {
        #region Events
        public event Action OnStartedMoving;
        public event Action OnStoppedMoving;
        public event Action OnReachedTarget;
        #endregion

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
        private bool movementEnabled;
        private bool isMoving;
        private bool reachedTarget;
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
                    $"[{nameof(ArthurMovementToPlayer)}] Kein {nameof(ArthurAnimationController)} gefunden auf '{gameObject.name}'.",
                    this);
            }

            if (playerTarget == null && showDebugLogs)
            {
                Debug.LogWarning(
                    $"[{nameof(ArthurMovementToPlayer)}] Beim Start kein Spieler-Ziel gefunden auf '{gameObject.name}'.",
                    this);
            }
        }

        private void FixedUpdate()
        {
            if (!movementEnabled)
            {
                ForceIdleIfNeeded();
                return;
            }

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
                HandleReachedTarget();
                return;
            }

            reachedTarget = false;

            Vector2 nextPosition = Vector2.MoveTowards(
                currentPosition,
                targetPosition,
                moveSpeed * Time.fixedDeltaTime);

            Vector2 actualMovement = nextPosition - currentPosition;

            if (!isMoving)
            {
                isMoving = true;
                OnStartedMoving?.Invoke();

                if (showDebugLogs)
                {
                    Debug.Log($"[{nameof(ArthurMovementToPlayer)}] Bewegung gestartet.", this);
                }
            }

            transform.position = new Vector3(nextPosition.x, nextPosition.y, transform.position.z);

            if (animationController != null)
            {
                animationController.SetMovement(actualMovement);
            }

            if (showDebugLogs)
            {
                Debug.Log(
                    $"[{nameof(ArthurMovementToPlayer)}] Move={actualMovement}, Distanz={distanceToTarget:0.00}",
                    this);
            }
        }
        #endregion

        #region Public API
        public void EnableMovement(Transform target)
        {
            if (target == null)
            {
                if (showDebugLogs)
                {
                    Debug.LogWarning($"[{nameof(ArthurMovementToPlayer)}] EnableMovement ohne Ziel aufgerufen.", this);
                }

                return;
            }

            playerTarget = target;
            movementEnabled = true;
            reachedTarget = false;

            if (showDebugLogs)
            {
                Debug.Log($"[{nameof(ArthurMovementToPlayer)}] Movement aktiviert.", this);
            }
        }

        public void DisableMovement()
        {
            movementEnabled = false;
            ForceIdleIfNeeded();

            if (showDebugLogs)
            {
                Debug.Log($"[{nameof(ArthurMovementToPlayer)}] Movement deaktiviert.", this);
            }
        }

        public void SetTarget(Transform newTarget)
        {
            playerTarget = newTarget;
            reachedTarget = false;
        }

        public void ClearTarget()
        {
            playerTarget = null;
            movementEnabled = false;
            ForceIdleIfNeeded();
        }

        public void StopMoving()
        {
            movementEnabled = false;
            ForceIdleIfNeeded();
        }

        public bool IsMovementEnabled()
        {
            return movementEnabled;
        }

        public bool HasReachedTarget()
        {
            return reachedTarget;
        }
        #endregion

        #region Private Methods
        private void HandleReachedTarget()
        {
            movementEnabled = false;

            if (isMoving)
            {
                isMoving = false;
                OnStoppedMoving?.Invoke();

                if (showDebugLogs)
                {
                    Debug.Log($"[{nameof(ArthurMovementToPlayer)}] Bewegung gestoppt.", this);
                }
            }

            if (!reachedTarget)
            {
                reachedTarget = true;
                OnReachedTarget?.Invoke();

                if (showDebugLogs)
                {
                    Debug.Log($"[{nameof(ArthurMovementToPlayer)}] Ziel erreicht.", this);
                }
            }

            if (animationController != null)
            {
                animationController.ForceIdle();
            }
        }

        private void ForceIdleIfNeeded()
        {
            if (isMoving)
            {
                isMoving = false;
                OnStoppedMoving?.Invoke();

                if (showDebugLogs)
                {
                    Debug.Log($"[{nameof(ArthurMovementToPlayer)}] ForceIdle -> Stop-Event gesendet.", this);
                }
            }

            if (animationController != null)
            {
                animationController.ForceIdle();
            }
        }
        #endregion
    }
}