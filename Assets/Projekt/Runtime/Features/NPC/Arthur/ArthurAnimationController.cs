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
        private int cachedLayerIndex = -1;

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
@@ -131,57 +132,86 @@ namespace ITAA.NPC.Arthur

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
            int layerIndex = GetLayerIndex();

            if (currentStatePath == fullStatePath)
            {
                return;
            }

            int fullStateHash = Animator.StringToHash(fullStatePath);
            int shortStateHash = Animator.StringToHash(shortStateName);
            bool hasFullPathState = HasStateOnLayer(layerIndex, fullStateHash);
            bool hasShortState = HasStateOnLayer(layerIndex, shortStateHash);


            if (!hasFullPathState && !hasShortState)
            {
                Debug.LogWarning(
                    $"[{nameof(ArthurAnimationController)}] State nicht gefunden: '{fullStatePath}' " +
                    $"auf Animator '{animator.gameObject.name}' mit Controller " +
                    $"'{(animator.runtimeAnimatorController != null ? animator.runtimeAnimatorController.name : "NULL")}'."
                );
                return;
            }

            Debug.Log($"[ArthurAnimation] Play: {fullStatePath}");

    
            animator.Play(hasFullPathState ? fullStateHash : shortStateHash, layerIndex, 0f);
            currentStatePath = fullStatePath;
        }

        private bool HasStateOnLayer(int layerIndex, int stateHash)
        {
            return animator != null && animator.HasState(layerIndex, stateHash);
        }

        private int GetLayerIndex()
        {
            if (animator == null)
            {
                return 0;
            }

            if (cachedLayerIndex >= 0 && cachedLayerIndex < animator.layerCount)
            {
                return cachedLayerIndex;
            }

            cachedLayerIndex = animator.GetLayerIndex(layerName);

            if (cachedLayerIndex < 0)
            {
                Debug.LogWarning(
                    $"[{nameof(ArthurAnimationController)}] Layer '{layerName}' nicht gefunden auf Animator '{animator.gameObject.name}'. Fallback auf Layer 0."
                );
                cachedLayerIndex = 0;
            }

            return cachedLayerIndex;
        }

        #endregion
    }
}