using UnityEngine;
using UnityEngine.InputSystem;
using ITAA.Player.Movement;
using ITAA.UI.Managers;

namespace ITAA.NPC.Arthur
{
    [RequireComponent(typeof(Collider2D))]
    public sealed class ArthurAutoInteraction : MonoBehaviour
    {
        [Header("Arthur References")]
        [SerializeField] private ArthurMovementToPlayer movementToPlayer;
        [SerializeField] private ArthurAnimationController animationController;
        [SerializeField] private ArthurNameUI nameUI;

        [Header("UI")]
        [SerializeField] private MenuManager menuManager;
        [SerializeField] private bool openLoadMenuWhenArthurReachesPlayer = true;

        [Header("Input")]
        [SerializeField] private InputActionReference interactAction;

        [Header("Detection")]
        [SerializeField] private string playerTag = "Player";

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs = true;

        private Transform currentTargetPlayer;
        private PlayerController currentPlayerController;

        private bool playerInRange;
        private bool playerLocked;
        private bool hasArthurApproachedPlayer;
        private bool menuOpenedForCurrentInteraction;

        private void Reset()
        {
            Collider2D collider2D = GetComponent<Collider2D>();
            if (collider2D != null)
            {
                collider2D.isTrigger = true;
            }
        }

        private void Awake()
        {
            if (nameUI != null)
            {
                nameUI.HideImmediate();
            }

            if (menuManager == null)
            {
                menuManager = FindAnyObjectByType<MenuManager>(FindObjectsInactive.Include);
            }
        }

        private void OnEnable()
        {
            if (movementToPlayer != null)
            {
                movementToPlayer.OnStartedMoving += HandleArthurStartedMoving;
                movementToPlayer.OnStoppedMoving += HandleArthurStoppedMoving;
                movementToPlayer.OnReachedTarget += HandleArthurReachedTarget;
            }

            if (interactAction != null && interactAction.action != null)
            {
                interactAction.action.Enable();
                interactAction.action.performed += HandleInteractPerformed;
            }
        }

        private void OnDisable()
        {
            if (movementToPlayer != null)
            {
                movementToPlayer.OnStartedMoving -= HandleArthurStartedMoving;
                movementToPlayer.OnStoppedMoving -= HandleArthurStoppedMoving;
                movementToPlayer.OnReachedTarget -= HandleArthurReachedTarget;
            }

            if (interactAction != null && interactAction.action != null)
            {
                interactAction.action.performed -= HandleInteractPerformed;
                interactAction.action.Disable();
            }

            ReleasePlayerMovement();
        }

        private void Update()
        {
            if (menuManager == null)
            {
                return;
            }

            if (!menuManager.IsOpen && playerLocked)
            {
                ReleasePlayerMovement();

                if (enableDebugLogs)
                {
                    Debug.Log($"[{nameof(ArthurAutoInteraction)}] Menu closed -> player released.", this);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(playerTag))
            {
                return;
            }

            Transform playerRoot = GetPlayerRootTransform(other);
            SetTargetPlayer(playerRoot);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag(playerTag))
            {
                return;
            }

            Transform playerRoot = GetPlayerRootTransform(other);
            ClearTargetPlayer(playerRoot);
        }

        public void SetTargetPlayer(Transform player)
        {
            if (player == null)
            {
                return;
            }

            currentTargetPlayer = player;
            currentPlayerController = player.GetComponentInParent<PlayerController>();
            playerInRange = true;
            menuOpenedForCurrentInteraction = false;

            if (enableDebugLogs)
            {
                Debug.Log(
                    $"[{nameof(ArthurAutoInteraction)}] Player entered detection zone. Controller found: {currentPlayerController != null}",
                    this);
            }

            if (!hasArthurApproachedPlayer && movementToPlayer != null)
            {
                movementToPlayer.EnableMovement(player);
            }
        }

        public void ClearTargetPlayer(Transform player)
        {
            if (player == null)
            {
                return;
            }

            if (currentTargetPlayer != player)
            {
                return;
            }

            if (enableDebugLogs)
            {
                Debug.Log($"[{nameof(ArthurAutoInteraction)}] Player left detection zone.", this);
            }

            playerInRange = false;
            menuOpenedForCurrentInteraction = false;
            hasArthurApproachedPlayer = false;

            if (movementToPlayer != null)
            {
                movementToPlayer.DisableMovement();
            }

            if (animationController != null)
            {
                animationController.ForceIdle();
            }

            if (nameUI != null)
            {
                nameUI.Hide();
            }

            if (menuManager != null && menuManager.IsOpen)
            {
                menuManager.HideAllImmediate();
            }

            ReleasePlayerMovement();

            currentTargetPlayer = null;
            currentPlayerController = null;
        }

        private void HandleArthurStartedMoving()
        {
            if (!playerInRange)
            {
                return;
            }

            if (enableDebugLogs)
            {
                Debug.Log($"[{nameof(ArthurAutoInteraction)}] Arthur started moving.", this);
            }

            if (nameUI != null)
            {
                nameUI.ShowName("Arthur");
            }

            LockPlayerMovement();
        }

        private void HandleArthurStoppedMoving()
        {
            if (!playerInRange)
            {
                return;
            }

            if (enableDebugLogs)
            {
                Debug.Log($"[{nameof(ArthurAutoInteraction)}] Arthur stopped moving.", this);
            }
        }

        private void HandleArthurReachedTarget()
        {
            if (!playerInRange)
            {
                return;
            }

            if (enableDebugLogs)
            {
                Debug.Log($"[{nameof(ArthurAutoInteraction)}] Arthur reached target.", this);
            }

            hasArthurApproachedPlayer = true;

            if (movementToPlayer != null)
            {
                movementToPlayer.DisableMovement();
            }

            if (animationController != null)
            {
                animationController.ForceIdle();
            }

            if (nameUI != null)
            {
                nameUI.ShowName("Arthur");
            }

            if (openLoadMenuWhenArthurReachesPlayer && !menuOpenedForCurrentInteraction)
            {
                LockPlayerMovement();
                OpenLoadMenu();
            }
        }

        private void HandleInteractPerformed(InputAction.CallbackContext context)
        {
            if (!playerInRange)
            {
                return;
            }

            if (!hasArthurApproachedPlayer)
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

            if (enableDebugLogs)
            {
                Debug.Log($"[{nameof(ArthurAutoInteraction)}] Interact pressed -> open load menu.", this);
            }

            LockPlayerMovement();
            OpenLoadMenu();
        }

        private void OpenLoadMenu()
        {
            if (menuManager == null)
            {
                if (enableDebugLogs)
                {
                    Debug.LogWarning($"[{nameof(ArthurAutoInteraction)}] MenuManager not found.", this);
                }
                return;
            }

            menuManager.OpenLoadGameFromArthur();
            menuOpenedForCurrentInteraction = true;

            if (enableDebugLogs)
            {
                Debug.Log($"[{nameof(ArthurAutoInteraction)}] Arthur opened StartMenu + LoadGamePanel.", this);
            }
        }

        private void LockPlayerMovement()
        {
            if (currentPlayerController == null)
            {
                if (enableDebugLogs)
                {
                    Debug.LogWarning($"[{nameof(ArthurAutoInteraction)}] PlayerController not found -> lock skipped.", this);
                }
                return;
            }

            if (playerLocked)
            {
                return;
            }

            currentPlayerController.SetMovementEnabled(false);
            currentPlayerController.StopImmediately();
            playerLocked = true;

            if (enableDebugLogs)
            {
                Debug.Log($"[{nameof(ArthurAutoInteraction)}] Player locked.", this);
            }
        }

        private void ReleasePlayerMovement()
        {
            if (currentPlayerController == null || !playerLocked)
            {
                return;
            }

            currentPlayerController.SetMovementEnabled(true);
            playerLocked = false;

            if (enableDebugLogs)
            {
                Debug.Log($"[{nameof(ArthurAutoInteraction)}] Player released.", this);
            }
        }

        private static Transform GetPlayerRootTransform(Collider2D other)
        {
            PlayerController controller = other.GetComponentInParent<PlayerController>();
            return controller != null ? controller.transform : other.transform;
        }
    }
}