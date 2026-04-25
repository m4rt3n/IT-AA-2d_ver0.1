/*
 * Datei: BerndInteractableAdapter.cs
 * Zweck: Macht Bernd mit dem World Interaction System kompatibel.
 * Verantwortung: Implementiert IInteractable und delegiert die Interaktion an Bernds bestehende Quiz-/Interaktionslogik.
 * Abhaengigkeiten: IInteractable, InteractionType, BerndAutoInteraction, BerndQuizStarter.
 * Verwendung: Wird optional auf Bernds GameObject ergaenzt, ohne bestehende BerndAutoInteraction-Logik zu entfernen.
 */

using ITAA.Features.Interaction;
using UnityEngine;

namespace ITAA.NPC.Bernd
{
    [DisallowMultipleComponent]
    public class BerndInteractableAdapter : MonoBehaviour, IInteractable
    {
        #region Inspector

        [Header("References")]
        [SerializeField] private BerndAutoInteraction autoInteraction;
        [SerializeField] private BerndQuizStarter quizStarter;
        [SerializeField] private BerndDetectionZone detectionZone;

        [Header("Interaction")]
        [SerializeField] private string interactionPrompt = "E druecken";
        [SerializeField] private bool requirePlayerInDetectionZone = true;

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs;

        #endregion

        #region IInteractable

        public string InteractionPrompt => interactionPrompt;
        public InteractionType InteractionType => InteractionType.Quiz;

        public bool CanInteract(Transform interactor)
        {
            ResolveReferences();

            if (!isActiveAndEnabled)
            {
                return false;
            }

            if (autoInteraction != null && autoInteraction.IsInteractionActive())
            {
                return false;
            }

            if (!requirePlayerInDetectionZone)
            {
                return HasInteractionTarget();
            }

            return detectionZone == null || detectionZone.HasTargetPlayer;
        }

        public void Interact(Transform interactor)
        {
            ResolveReferences();

            if (!CanInteract(interactor))
            {
                return;
            }

            if (autoInteraction != null)
            {
                Log("Delegiere an BerndAutoInteraction.");
                autoInteraction.StartInteraction();
                return;
            }

            if (quizStarter != null)
            {
                Log("BerndAutoInteraction fehlt. Starte Quiz direkt ueber BerndQuizStarter.");
                quizStarter.StartQuiz();
                return;
            }

            Debug.LogWarning(
                $"[{nameof(BerndInteractableAdapter)}] Weder {nameof(BerndAutoInteraction)} noch {nameof(BerndQuizStarter)} gefunden.",
                this
            );
        }

        #endregion

        #region Unity

        private void Awake()
        {
            ResolveReferences();
        }

        private void Reset()
        {
            ResolveReferences();
        }

        #endregion

        #region Private

        private void ResolveReferences()
        {
            if (autoInteraction == null)
            {
                autoInteraction = GetComponent<BerndAutoInteraction>();
            }

            if (quizStarter == null)
            {
                quizStarter = GetComponent<BerndQuizStarter>();
            }

            if (detectionZone == null)
            {
                detectionZone = GetComponentInChildren<BerndDetectionZone>(true);
            }
        }

        private bool HasInteractionTarget()
        {
            return autoInteraction != null || quizStarter != null;
        }

        private void Log(string message)
        {
            if (!enableDebugLogs)
            {
                return;
            }

            Debug.Log($"[{nameof(BerndInteractableAdapter)}] {message}", this);
        }

        #endregion
    }
}
