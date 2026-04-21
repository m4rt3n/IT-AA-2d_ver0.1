/*
 * Datei: ArthurAutoInteraction.cs
 * Zweck:
 *   Steuert Arthurs automatische Interaktion mit dem Spieler.
 *
 * Verantwortung:
 * - Erkennt den Spieler über Trigger oder externe DetectionZone
 * - Lässt Arthur optional zum Spieler laufen
 * - Öffnet bei erreichter Distanz das Startmenü
 * - Sperrt die Spielerbewegung, solange irgendein Menü offen ist
 * - Gibt die Spielerbewegung wieder frei, sobald alle Menüs geschlossen sind
 *
 * Wichtige Abhängigkeiten:
 * - MenuManager
 * - ArthurMovementToPlayer
 * - ArthurAnimationController (optional)
 */

using ITAA.UI.Managers;
using UnityEngine;

namespace ITAA.NPC.Arthur
{
    public class ArthurAutoInteraction : MonoBehaviour
    {
        #region Inspector

        [Header("References")]
        [SerializeField] private MenuManager menuManager;
        [SerializeField] private ArthurMovementToPlayer movementToPlayer;
        [SerializeField] private ArthurAnimationController animationController;

        [Header("Player Control")]
        [SerializeField] private Behaviour playerMovementController;

        [Header("Detection")]
        [SerializeField] private string playerTag = "Player";
        [SerializeField] private float openDistance = 1.35f;
        [SerializeField] private bool requirePlayerInRange = true;

        [Header("Behaviour")]
        [SerializeField] private bool openMenuAutomatically = true;
        [SerializeField] private bool moveToPlayerBeforeOpen = true;
        [SerializeField] private bool openOnlyOncePerApproach = true;
        [SerializeField] private bool facePlayerWhenMenuOpens = true;

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs = true;

        #endregion

        #region Private Fields

        private Transform targetPlayer;
        private bool playerInRange;
        private bool hasOpenedForCurrentApproach;
        private bool playerMovementLockedByMenu;

        #endregion

        #region Unity

        private void Awake()
        {
            if (menuManager == null)
            {
                menuManager = FindFirstObjectByType<MenuManager>(FindObjectsInactive.Include);
            }

            if (movementToPlayer == null)
            {
                movementToPlayer = GetComponent<ArthurMovementToPlayer>();
            }

            if (animationController == null)
            {
                animationController = GetComponent<ArthurAnimationController>();

                if (animationController == null)
                {
                    animationController = GetComponentInChildren<ArthurAnimationController>();
                }
            }

            Log($"Awake. MenuManager gefunden: {menuManager != null}");
        }

        private void Update()
        {
            UpdatePlayerMovementLock();

            if (!openMenuAutomatically)
            {
                return;
            }

            if (menuManager == null)
            {
                return;
            }

            if (menuManager.IsOpen)
            {
                return;
            }

            if (targetPlayer == null)
            {
                return;
            }

            if (requirePlayerInRange && !playerInRange)
            {
                return;
            }

            if (openOnlyOncePerApproach && hasOpenedForCurrentApproach)
            {
                return;
            }

            float distance = Vector2.Distance(transform.position, targetPlayer.position);

            if (distance > openDistance)
            {
                Log($"Arthur noch zu weit weg. Distanz={distance:0.00}, benötigt<={openDistance:0.00}");
                return;
            }

            Log("Arthur ist nah genug. Öffne Startmenü.");

            if (moveToPlayerBeforeOpen && movementToPlayer != null)
            {
                movementToPlayer.DisableMovement();
            }

            if (facePlayerWhenMenuOpens)
            {
                FacePlayer();
            }

            menuManager.ShowStartMenu();
            hasOpenedForCurrentApproach = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsPlayer(other))
            {
                return;
            }

            SetTargetPlayer(other.transform);
            playerInRange = true;

            Log("Spieler in Reichweite.");
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!IsPlayer(other))
            {
                return;
            }

            ClearTargetPlayer(other.transform);
            playerInRange = false;
            hasOpenedForCurrentApproach = false;

            Log("Spieler hat Reichweite verlassen.");
        }

        #endregion

        #region Public API

        public void SetTargetPlayer(Transform playerTransform)
        {
            if (playerTransform == null)
            {
                return;
            }

            targetPlayer = playerTransform;
            playerInRange = true;

            if (moveToPlayerBeforeOpen && movementToPlayer != null)
            {
                movementToPlayer.EnableMovement(playerTransform);
            }

            Log($"SetTargetPlayer -> {playerTransform.name}");
        }

        public void ClearTargetPlayer(Transform playerTransform = null)
        {
            if (playerTransform != null && targetPlayer != null && playerTransform != targetPlayer)
            {
                return;
            }

            if (moveToPlayerBeforeOpen && movementToPlayer != null)
            {
                movementToPlayer.DisableMovement();
            }

            targetPlayer = null;
            playerInRange = false;
            hasOpenedForCurrentApproach = false;

            if (animationController != null)
            {
                animationController.ForceIdle();
            }

            Log("ClearTargetPlayer");
        }

        #endregion

        #region Private Helpers

        private void UpdatePlayerMovementLock()
        {
            if (menuManager == null || playerMovementController == null)
            {
                return;
            }

            bool shouldLockPlayer = menuManager.IsOpen;

            if (shouldLockPlayer && !playerMovementLockedByMenu)
            {
                playerMovementController.enabled = false;
                playerMovementLockedByMenu = true;
                Log("Player-Steuerung deaktiviert, weil ein Menü offen ist.");
            }
            else if (!shouldLockPlayer && playerMovementLockedByMenu)
            {
                playerMovementController.enabled = true;
                playerMovementLockedByMenu = false;
                Log("Player-Steuerung wieder aktiviert, weil alle Menüs geschlossen sind.");
            }
        }

        private void FacePlayer()
        {
            if (targetPlayer == null || animationController == null)
            {
                return;
            }

            Vector2 direction = targetPlayer.position - transform.position;

            if (direction.sqrMagnitude <= 0.0001f)
            {
                return;
            }

            animationController.ForceIdle(direction.normalized);
        }

        private bool IsPlayer(Collider2D other)
        {
            return other != null && other.CompareTag(playerTag);
        }

        private void Log(string message)
        {
            if (!enableDebugLogs)
            {
                return;
            }

            Debug.Log($"[{nameof(ArthurAutoInteraction)}] {message}");
        }

        #endregion
    }
}