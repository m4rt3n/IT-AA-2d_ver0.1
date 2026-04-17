/*
 * Datei: ArthurAutoInteraction.cs
 * Zweck: Steuert Arthurs Interaktion mit dem Spieler und öffnet bei Bedarf das Menü.
 * Verantwortung:
 *   - Merkt sich den Zielspieler aus der DetectionZone
 *   - Prüft, ob der Spieler in Reichweite ist
 *   - Wartet optional auf Tastendruck
 *   - Öffnet dann das Startmenü über den MenuManager
 *
 * Abhängigkeiten:
 *   - MenuManager
 *   - ArthurDetectionZone
 *   - Player Transform
 */

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

        [Header("Interaction")]
        [SerializeField] private bool requireKeyPress = true;
        [SerializeField] private Key interactKey = Key.E;

        #endregion

        #region Fields

        private Transform targetPlayer;
        private bool playerInRange;
        private bool hasTriggered;

        #endregion

        #region Properties

        public Transform TargetPlayer => targetPlayer;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (menuManager == null)
            {
                menuManager = FindAnyObjectByType<MenuManager>();
            }

            Debug.Log($"[{nameof(ArthurAutoInteraction)}] Awake. MenuManager gefunden: {menuManager != null}");
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

            if (requireKeyPress)
            {
                if (Keyboard.current == null || !Keyboard.current[interactKey].wasPressedThisFrame)
                {
                    return;
                }
            }

            Debug.Log($"[{nameof(ArthurAutoInteraction)}] Öffne Startmenü.");
            menuManager.ShowStartMenu();
            hasTriggered = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = true;
                hasTriggered = false;
                targetPlayer = other.transform;

                Debug.Log($"[{nameof(ArthurAutoInteraction)}] Spieler in Reichweite.");
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = false;
                hasTriggered = false;

                if (targetPlayer == other.transform)
                {
                    targetPlayer = null;
                }

                Debug.Log($"[{nameof(ArthurAutoInteraction)}] Spieler hat Reichweite verlassen.");
            }
        }

        #endregion

        #region Public Methods

        public void SetTargetPlayer(Transform playerTransform)
        {
            targetPlayer = playerTransform;
            playerInRange = playerTransform != null;

            if (playerTransform == null)
            {
                hasTriggered = false;
            }

            Debug.Log(
                $"[{nameof(ArthurAutoInteraction)}] SetTargetPlayer -> " +
                $"{(playerTransform != null ? playerTransform.name : "null")}"
            );
        }

        public void ClearTargetPlayer()
        {
            targetPlayer = null;
            playerInRange = false;
            hasTriggered = false;

            Debug.Log($"[{nameof(ArthurAutoInteraction)}] ClearTargetPlayer");
        }

        #endregion
    }
}