/*
 * Datei: BerndAutoInteraction.cs
 * Zweck: Zentrale Interaktion für Bernd als Questgeber.
 * Verantwortung: Prüft Nähe und Interaktionstaste, sperrt Bewegung während der Interaktion und startet Bernds Quiz.
 * Abhaengigkeiten: BerndDetectionZone, BerndMovementToPlayer, BerndAnimationController, BerndQuizStarter, Unity Input System.
 * Verwendung: Wird auf Bernds GameObject in der StartScene eingesetzt und per YAML mit dem Quiz-System verbunden.
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
        [SerializeField] private BerndQuizStarter quizStarter;

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

            if (quizStarter == null)
            {
                quizStarter = GetComponent<BerndQuizStarter>();
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
            quizStarter?.StartQuiz(EndInteraction);
        }

        public void EndInteraction()
        {
            isInteractionActive = false;

            movementToPlayer?.SetInteractionLocked(false);
            animationController?.SetTalking(false);

            onInteractionEnded?.Invoke();
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
