/*
 * Datei: ArthurAnimationController.cs
 * Zweck: Steuert Arthurs Idle- und Walk-Animationen in 4 Richtungen.
 * Verantwortung:
 *   - Spielt passende Walk-Animationen je nach Bewegungsrichtung
 *   - Spielt passende Idle-Animationen anhand der letzten Blickrichtung
 *   - Nutzt den vollständigen State-Pfad auf dem Base Layer
 *
 * Voraussetzungen im Animator:
 *   - States auf Base Layer mit exakt diesen Namen:
 *     Arthur_IdleDown
 *     Arthur_IdleUp
 *     Arthur_IdleLeft
 *     Arthur_IdleRight
 *     Arthur_WalkDown
 *     Arthur_WalkUp
 *     Arthur_WalkLeft
 *     Arthur_WalkRight
 */

using UnityEngine;

namespace ITAA.NPC.Arthur
{
    public class ArthurAnimationController : MonoBehaviour
    {
        #region Inspector

        [Header("References")]
        [SerializeField] private Animator animator;

        [Header("Animator")]
        [SerializeField] private string layerName = "Base Layer";

        [Header("Idle States")]
        [SerializeField] private string idleDownState = "Arthur_IdleDown";
        [SerializeField] private string idleUpState = "Arthur_IdleUp";
        [SerializeField] private string idleLeftState = "Arthur_IdleLeft";
        [SerializeField] private string idleRightState = "Arthur_IdleRight";

        [Header("Walk States")]
        [SerializeField] private string walkDownState = "Arthur_WalkDown";
        [SerializeField] private string walkUpState = "Arthur_WalkUp";
        [SerializeField] private string walkLeftState = "Arthur_WalkLeft";
        [SerializeField] private string walkRightState = "Arthur_WalkRight";

        #endregion

        #region Fields

        private Vector2 lastLookDirection = Vector2.down;
        private string currentStatePath;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();

                if (animator == null)
                {
                    animator = GetComponentInChildren<Animator>();
                }
            }

            if (animator == null)
            {
                Debug.LogWarning($"[{nameof(ArthurAnimationController)}] Kein Animator gefunden auf '{gameObject.name}'.");
                return;
            }

            if (animator.runtimeAnimatorController == null)
            {
                Debug.LogWarning($"[{nameof(ArthurAnimationController)}] Animator hat keinen RuntimeAnimatorController auf '{gameObject.name}'.");
            }
            else
            {
                Debug.Log($"[{nameof(ArthurAnimationController)}] Animator Controller: {animator.runtimeAnimatorController.name}");
            }
        }

        #endregion

        #region Public Methods

        public void SetMovement(Vector2 movement)
        {
            if (animator == null)
            {
                return;
            }

            if (movement.sqrMagnitude <= 0.0001f)
            {
                PlayIdle();
                return;
            }

            lastLookDirection = movement.normalized;
            PlayWalk(lastLookDirection);
        }

        public void ForceIdle()
        {
            PlayIdle();
        }

        #endregion

        #region Private Methods

        private void PlayWalk(Vector2 direction)
        {
            string nextStateName;

            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                nextStateName = direction.x > 0f ? walkRightState : walkLeftState;
            }
            else
            {
                nextStateName = direction.y > 0f ? walkUpState : walkDownState;
            }

            PlayState(nextStateName);
        }

        private void PlayIdle()
        {
            string nextStateName;

            if (Mathf.Abs(lastLookDirection.x) > Mathf.Abs(lastLookDirection.y))
            {
                nextStateName = lastLookDirection.x > 0f ? idleRightState : idleLeftState;
            }
            else
            {
                nextStateName = lastLookDirection.y > 0f ? idleUpState : idleDownState;
            }

            PlayState(nextStateName);
        }

        private void PlayState(string shortStateName)
        {
            if (animator == null)
            {
                return;
            }

            string fullStatePath = $"{layerName}.{shortStateName}";

            if (currentStatePath == fullStatePath)
            {
                return;
            }

            int stateHash = Animator.StringToHash(fullStatePath);

            if (!HasStateOnLayer(0, stateHash))
            {
                Debug.LogWarning(
                    $"[{nameof(ArthurAnimationController)}] State nicht gefunden: '{fullStatePath}' " +
                    $"auf Animator '{animator.gameObject.name}' mit Controller " +
                    $"'{(animator.runtimeAnimatorController != null ? animator.runtimeAnimatorController.name : "NULL")}'."
                );
                return;
            }

            Debug.Log($"[ArthurAnimation] Play: {fullStatePath}");

            animator.Play(stateHash, 0, 0f);
            currentStatePath = fullStatePath;
        }

        private bool HasStateOnLayer(int layerIndex, int stateHash)
        {
            return animator != null && animator.HasState(layerIndex, stateHash);
        }

        #endregion
    }
}