/*
 * Datei: BerndAutoInteraction.cs
 * Zweck: Zentrale Interaktion für Bernd als Questgeber.
 * Verantwortung:
 * - Prüft, ob Spieler in Reichweite ist
 * - Reagiert auf Interaktionstaste
 * - Sperrt Bewegung während Gespräch/Quiz
 * - Startet das erste einfache Quiz über Event
 *
 * Hinweis:
 * - Dieses Script ist bewusst allgemein gehalten.
 * - Das Quiz kann später per UnityEvent an UI/Quest-System gekoppelt werden.
 *
 * Voraussetzung:
 * - Neues Input System aktiv oder Package installiert
 * - Wenn nicht vorhanden, kann die Tastenerkennung leicht umgestellt werden
 */

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace ITAA.NPC.Bernd
{
    [DisallowMultipleComponent]
    public class BerndAutoInteraction : MonoBehaviour
    {
        #region Inspector

        [Header("References")]
        [SerializeField] private BerndDetectionZone detectionZone;
        [SerializeField] private BerndMovementToPlayer movementToPlayer;
        [SerializeField] private BerndAnimationController animationController;

        [Header("Interaction")]
        [SerializeField] private Key interactionKey = Key.E;
        [SerializeField] private bool allowRepeatInteraction;
        [SerializeField] private bool lockInteractionWhileActive = true;

        [Header("Events")]
        [SerializeField] private UnityEvent onInteractionStarted;
        [SerializeField] private UnityEvent onQuizLevel1Requested;
        [SerializeField] private UnityEvent onInteractionEnded;

        #endregion

        #region State

        private bool isInteractionActive;
        private bool hasTriggeredOnce;

        #endregion

        #region Unity

        private void Awake()
        {
            if (detectionZone == null)
            {
                detectionZone = GetComponentInChildren<BerndDetectionZone>();
            }

            if (movementToPlayer == null)
            {
                movementToPlayer = GetComponent<BerndMovementToPlayer>();
            }

            if (animationController == null)
            {
                animationController = GetComponent<BerndAnimationController>();
            }
        }

        private void Update()
        {
            if (detectionZone == null || !detectionZone.HasTargetPlayer)
            {
                if (isInteractionActive && !lockInteractionWhileActive)
                {
                    EndInteraction();
                }

                return;
            }

            if (!WasInteractionPressed())
            {
                return;
            }

            if (isInteractionActive)
            {
                if (!lockInteractionWhileActive)
                {
                    EndInteraction();
                }

                return;
            }

            if (!allowRepeatInteraction && hasTriggeredOnce)
            {
                return;
            }

            StartInteraction();
        }

        #endregion

        #region Public API

        public void StartInteraction()
        {
            isInteractionActive = true;
            hasTriggeredOnce = true;

            movementToPlayer?.SetInteractionLocked(true);

            if (detectionZone != null && detectionZone.HasTargetPlayer)
            {
                Vector2 lookDirection =
                    (detectionZone.TargetPlayer.position - transform.position).normalized;

                animationController?.PlayIdle(lookDirection);
            }

            animationController?.SetTalking(true);

            onInteractionStarted?.Invoke();
            onQuizLevel1Requested?.Invoke();

            Debug.Log("Bernd Betatest: Level-1-Quiz gestartet.");
        }

        public void EndInteraction()
        {
            isInteractionActive = false;

            movementToPlayer?.SetInteractionLocked(false);
            animationController?.SetTalking(false);

            onInteractionEnded?.Invoke();

            Debug.Log("Bernd Betatest: Interaktion beendet.");
        }

        public bool IsInteractionActive()
        {
            return isInteractionActive;
        }

        #endregion

        #region Private

        private bool WasInteractionPressed()
        {
            Keyboard keyboard = Keyboard.current;

            if (keyboard == null)
            {
                return false;
            }

            return interactionKey switch
            {
                Key.E => keyboard.eKey.wasPressedThisFrame,
                Key.Space => keyboard.spaceKey.wasPressedThisFrame,
                Key.Enter => keyboard.enterKey.wasPressedThisFrame,
                Key.F => keyboard.fKey.wasPressedThisFrame,
                _ => false
            };
        }

        #endregion
    }
}