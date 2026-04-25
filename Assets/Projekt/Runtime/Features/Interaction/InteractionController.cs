/*
 * Datei: InteractionController.cs
 * Zweck: Loest die aktuelle Welt-Interaktion per Unity Input System aus.
 * Verantwortung: Liest die Interaktionstaste, aktualisiert den Prompt und ruft Interact() auf dem aktuellen IInteractable auf.
 * Abhaengigkeiten: InteractionDetector, InteractionPromptView, Unity Input System.
 * Verwendung: Wird zusammen mit InteractionDetector auf dem Player oder einem Player-Child eingesetzt.
 */

using UnityEngine;
using UnityEngine.InputSystem;

namespace ITAA.Features.Interaction
{
    [DisallowMultipleComponent]
    public class InteractionController : MonoBehaviour
    {
        #region Inspector

        [Header("References")]
        [SerializeField] private InteractionDetector detector;
        [SerializeField] private InteractionPromptView promptView;
        [SerializeField] private Transform interactorRoot;

        [Header("Input")]
        [SerializeField] private InputActionReference interactAction;
        [SerializeField] private bool useKeyboardFallback = true;
        [SerializeField] private Key keyboardFallbackKey = Key.E;

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs;

        #endregion

        #region Unity

        private void Awake()
        {
            ResolveReferences();
        }

        private void OnEnable()
        {
            ResolveReferences();

            if (detector != null)
            {
                detector.CurrentInteractableChanged += HandleCurrentInteractableChanged;
                detector.RefreshCurrentInteractable();
                UpdatePrompt(detector.CurrentInteractable);
            }

            if (interactAction != null && interactAction.action != null)
            {
                interactAction.action.Enable();
                interactAction.action.performed += HandleInteractPerformed;
            }
        }

        private void OnDisable()
        {
            if (detector != null)
            {
                detector.CurrentInteractableChanged -= HandleCurrentInteractableChanged;
            }

            if (interactAction != null && interactAction.action != null)
            {
                interactAction.action.performed -= HandleInteractPerformed;
                interactAction.action.Disable();
            }

            if (promptView != null)
            {
                promptView.Hide();
            }
        }

        private void Update()
        {
            if (detector != null)
            {
                detector.RefreshCurrentInteractable();
                UpdatePrompt(detector.CurrentInteractable);
            }

            if (interactAction != null && interactAction.action != null)
            {
                return;
            }

            if (!useKeyboardFallback)
            {
                return;
            }

            Keyboard keyboard = Keyboard.current;

            if (keyboard != null && keyboard[keyboardFallbackKey].wasPressedThisFrame)
            {
                TryInteract();
            }
        }

        #endregion

        #region Public API

        public void TryInteract()
        {
            if (detector == null)
            {
                return;
            }

            IInteractable interactable = detector.CurrentInteractable;

            if (interactable == null || !interactable.CanInteract(interactorRoot))
            {
                return;
            }

            if (enableDebugLogs)
            {
                Debug.Log($"[{nameof(InteractionController)}] Interact -> {interactable.InteractionType}", this);
            }

            interactable.Interact(interactorRoot);
            detector.RefreshCurrentInteractable();
            UpdatePrompt(detector.CurrentInteractable);
        }

        #endregion

        #region Private

        private void ResolveReferences()
        {
            if (detector == null)
            {
                detector = GetComponent<InteractionDetector>();
            }

            if (interactorRoot == null)
            {
                interactorRoot = transform;
            }
        }

        private void HandleInteractPerformed(InputAction.CallbackContext context)
        {
            TryInteract();
        }

        private void HandleCurrentInteractableChanged(IInteractable interactable)
        {
            UpdatePrompt(interactable);
        }

        private void UpdatePrompt(IInteractable interactable)
        {
            if (promptView == null)
            {
                return;
            }

            if (interactable == null || !interactable.CanInteract(interactorRoot))
            {
                promptView.Hide();
                return;
            }

            promptView.Show(interactable.InteractionPrompt);
        }

        #endregion
    }
}
