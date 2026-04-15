/*
 * Datei: NPCInteraction.cs
 * Zweck: Steuert die Interaktion mit einem NPC über das neue Unity Input System.
 * Verantwortung:
 * - Erkennt, ob der Spieler im Interaktionsbereich ist
 * - Zeigt optional einen Hinweis an
 * - Löst bei Tastendruck ein konfiguriertes Event aus
 * Abhängigkeiten:
 * - Unity Input System
 * - Optional: Collider2D oder Collider für den Interaktionsbereich
 * Verwendet von:
 * - NPC-Objekten mit Trigger-Zone
 */

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace ITAA.NPC.Interactions
{
    public class NPCInteraction : MonoBehaviour
    {
        #region Inspector

        [Header("Interaction")]
        [SerializeField] private Key interactionKey = Key.E;
        [SerializeField] private string playerTag = "Player";
        [SerializeField] private bool requirePlayerInsideTrigger = true;
        [SerializeField] private bool triggerOnlyOnce;

        [Header("Optional UI")]
        [SerializeField] private GameObject interactionPrompt;

        [Header("Events")]
        [SerializeField] private UnityEvent onInteract;

        #endregion

        #region Private Fields

        private bool isPlayerInRange;
        private bool hasTriggered;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            SetPromptVisible(false);
        }

        private void Update()
        {
            if (hasTriggered && triggerOnlyOnce)
            {
                return;
            }

            if (requirePlayerInsideTrigger && !isPlayerInRange)
            {
                return;
            }

            Keyboard keyboard = Keyboard.current;
            if (keyboard == null)
            {
                return;
            }

            if (keyboard[interactionKey].wasPressedThisFrame)
            {
                ExecuteInteraction();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(playerTag))
            {
                return;
            }

            isPlayerInRange = true;
            SetPromptVisible(true);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag(playerTag))
            {
                return;
            }

            isPlayerInRange = false;
            SetPromptVisible(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(playerTag))
            {
                return;
            }

            isPlayerInRange = true;
            SetPromptVisible(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(playerTag))
            {
                return;
            }

            isPlayerInRange = false;
            SetPromptVisible(false);
        }

        #endregion

        #region Private Methods

        private void ExecuteInteraction()
        {
            if (hasTriggered && triggerOnlyOnce)
            {
                return;
            }

            hasTriggered = true;
            SetPromptVisible(false);

            if (onInteract != null)
            {
                onInteract.Invoke();
            }
            else
            {
                Debug.LogWarning($"[{nameof(NPCInteraction)}] Kein onInteract-Event konfiguriert auf {gameObject.name}.", this);
            }
        }

        private void SetPromptVisible(bool visible)
        {
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(visible);
            }
        }

        #endregion

        #region Public Methods

        public void ResetInteraction()
        {
            hasTriggered = false;

            if (isPlayerInRange)
            {
                SetPromptVisible(true);
            }
        }

        public void ForceInteract()
        {
            ExecuteInteraction();
        }

        #endregion
    }
}