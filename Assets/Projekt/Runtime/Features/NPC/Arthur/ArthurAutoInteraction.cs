/*
 * Datei: ArthurAutoInteraction.cs
 * Zweck:
 *   Steuert Arthurs Interaktion mit dem Spieler und aktiviert seine Bewegung
 *   erst dann, wenn der Spieler in Reichweite ist.
 *
 * Verantwortung:
 *   - merkt sich den Zielspieler aus der Detection Zone
 *   - aktiviert/deaktiviert ArthurMovementToPlayer
 *   - sperrt/entsperrt die Bewegung des Spielers
 *   - öffnet optional per Taste das Startmenü
 *   - öffnet das Menü erst, wenn Arthur nah genug am Spieler ist
 *   - stellt kompatible Methoden für ArthurDetectionZone bereit
 *
 * Abhängigkeiten:
 *   - MenuManager
 *   - ArthurMovementToPlayer
 *   - ArthurDetectionZone
 *   - PlayerMotor2D
 */

using ITAA.Player.Movement;
using ITAA.UI.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ITAA.NPC.Arthur
{
    public class ArthurAutoInteraction : MonoBehaviour
    {
        #region Inspector

        [Header("References")]
        [SerializeField] private MenuManager menuManager;
        [SerializeField] private ArthurMovementToPlayer movement;
        [SerializeField] private PlayerMotor2D playerMotor;

        [Header("Interaction")]
        [SerializeField] private bool requireKeyPress = true;
        [SerializeField] private Key interactKey = Key.E;

        [Header("Distance Check")]
        [SerializeField] private float interactDistance = 1.35f;

        [Header("Player Lock")]
        [SerializeField] private bool lockPlayerMovement = true;

        [Header("Debug")]
        [SerializeField] private bool showDebugLogs = true;

        #endregion

        #region Fields

        private Transform targetPlayer;
        private bool playerInRange;
        private bool hasTriggered;

        #endregion

        #region Properties

        public Transform TargetPlayer => targetPlayer;
        public bool PlayerInRange => playerInRange;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (menuManager == null)
            {
                menuManager = FindAnyObjectByType<MenuManager>();
            }

            if (movement == null)
            {
                movement = GetComponent<ArthurMovementToPlayer>();

                if (movement == null)
                {
                    movement = GetComponentInChildren<ArthurMovementToPlayer>();
                }
            }

            if (showDebugLogs)
            {
                Debug.Log($"[{nameof(ArthurAutoInteraction)}] Awake. MenuManager gefunden: {menuManager != null}");
            }
        }

        private void Update()
        {
            if (!playerInRange)
            {
                return;
            }

            if (hasTriggered)
            {
                return;
            }

            if (menuManager == null)
            {
                Debug.LogWarning($"[{nameof(ArthurAutoInteraction)}] Kein MenuManager gefunden.");
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

            float distanceToPlayer = Vector2.Distance(transform.position, targetPlayer.position);

            if (distanceToPlayer > interactDistance)
            {
                if (showDebugLogs)
                {
                    Debug.Log(
                        $"[{nameof(ArthurAutoInteraction)}] Arthur noch zu weit weg. " +
                        $"Distanz={distanceToPlayer:0.00}, benötigt<={interactDistance:0.00}"
                    );
                }

                return;
            }

            if (requireKeyPress)
            {
                if (Keyboard.current == null || !Keyboard.current[interactKey].wasPressedThisFrame)
                {
                    return;
                }
            }

            if (showDebugLogs)
            {
                Debug.Log($"[{nameof(ArthurAutoInteraction)}] Arthur ist nah genug. Öffne Startmenü.");
            }

            if (movement != null)
            {
                movement.DisableMovement();
            }

            if (lockPlayerMovement && playerMotor != null)
            {
                playerMotor.SetMovementLocked(true);
            }

            menuManager.ShowStartMenu();
            hasTriggered = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }

            SetTargetPlayer(other.transform);

            if (showDebugLogs)
            {
                Debug.Log($"[{nameof(ArthurAutoInteraction)}] Spieler in Reichweite.");
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }

            if (targetPlayer == other.transform)
            {
                ClearTargetPlayer();
            }

            if (showDebugLogs)
            {
                Debug.Log($"[{nameof(ArthurAutoInteraction)}] Spieler hat Reichweite verlassen.");
            }
        }

        #endregion

        #region Public Methods

        public void SetTargetPlayer(Transform playerTransform)
        {
            targetPlayer = playerTransform;
            playerInRange = playerTransform != null;
            hasTriggered = false;

            if (playerMotor == null && playerTransform != null)
            {
                playerMotor = playerTransform.GetComponent<PlayerMotor2D>();
            }

            if (lockPlayerMovement && playerMotor != null)
            {
                playerMotor.SetMovementLocked(true);
            }

            if (movement != null)
            {
                if (playerTransform != null)
                {
                    movement.EnableMovement(playerTransform);
                }
                else
                {
                    movement.DisableMovement();
                }
            }

            if (showDebugLogs)
            {
                Debug.Log(
                    $"[{nameof(ArthurAutoInteraction)}] SetTargetPlayer -> " +
                    $"{(playerTransform != null ? playerTransform.name : "null")}"
                );
            }
        }

        public void ClearTargetPlayer()
        {
            targetPlayer = null;
            playerInRange = false;
            hasTriggered = false;

            if (movement != null)
            {
                movement.DisableMovement();
            }

            if (lockPlayerMovement && playerMotor != null)
            {
                playerMotor.SetMovementLocked(false);
            }

            if (showDebugLogs)
            {
                Debug.Log($"[{nameof(ArthurAutoInteraction)}] ClearTargetPlayer");
            }
        }

        public void ReleasePlayerControl()
        {
            if (lockPlayerMovement && playerMotor != null)
            {
                playerMotor.SetMovementLocked(false);
            }

            if (showDebugLogs)
            {
                Debug.Log($"[{nameof(ArthurAutoInteraction)}] ReleasePlayerControl");
            }
        }

        #endregion
    }
}