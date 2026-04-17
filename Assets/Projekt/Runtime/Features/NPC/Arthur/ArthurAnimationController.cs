/*
 * Datei: ArthurAnimationController.cs
 * Zweck:
 *   Steuert Arthurs Idle- und Walk-Animationen über Blend Trees
 *   und setzt zusätzlich den Bool-Parameter "IsMoving".
 *
 * Voraussetzungen im Animator:
 *   - States:
 *       Arthur_Idle
 *       Arthur_Walk
 *   - Blend Tree Parameter:
 *       MoveX
 *       MoveY
 *   - Bool Parameter:
 *       IsMoving
 */

using UnityEngine;

namespace ITAA.NPC.Arthur
{
    public class ArthurAnimationController : MonoBehaviour
    {
        #region Inspector

        [Header("References")]
        [SerializeField] private Animator animator;

        [Header("States")]
        [SerializeField] private string idleState = "Arthur_Idle";
        [SerializeField] private string walkState = "Arthur_Walk";

        [Header("Parameters")]
        [SerializeField] private string moveXParameter = "MoveX";
        [SerializeField] private string moveYParameter = "MoveY";
        [SerializeField] private string isMovingParameter = "IsMoving";

        #endregion

        #region Fields

        private Vector2 lastLookDirection = Vector2.down;
        private string currentState;

        #endregion

        #region Unity

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
                Debug.LogWarning($"[{nameof(ArthurAnimationController)}] Kein Animator Controller zugewiesen.");
                return;
            }

            animator.SetBool(isMovingParameter, false);
            ApplyDirection(lastLookDirection);
            PlayIdle();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();

                if (animator == null)
                {
                    animator = GetComponentInChildren<Animator>();
                }
            }
        }
#endif

        #endregion

        #region Public API

        public void SetMovement(Vector2 movement)
        {
            Debug.Log($"[{nameof(ArthurAnimationController)}] SetMovement input={movement} sqrMagnitude={movement.sqrMagnitude}");

            if (movement.sqrMagnitude > 0.0001f)
            {
                Vector2 dir = movement.normalized;
                lastLookDirection = dir;

                animator.SetBool(isMovingParameter, true);

                Debug.Log($"[{nameof(ArthurAnimationController)}] WALK dir={dir}");

                ApplyDirection(dir);
                PlayWalk();
                return;
            }

            animator.SetBool(isMovingParameter, false);

            Debug.Log($"[{nameof(ArthurAnimationController)}] IDLE lastLookDirection={lastLookDirection}");

            ApplyDirection(lastLookDirection);
            PlayIdle();
        }

        public void SetIdleDirection(Vector2 lookDirection)
        {
            if (lookDirection.sqrMagnitude > 0.0001f)
            {
                lastLookDirection = lookDirection.normalized;
            }

            animator.SetBool(isMovingParameter, false);
            ApplyDirection(lastLookDirection);
            PlayIdle();
        }

        public void ForceIdle()
        {
            currentState = null;
            animator.SetBool(isMovingParameter, false);
            ApplyDirection(lastLookDirection);
            PlayIdle();
        }

        public void ForceIdle(Vector2 lookDirection)
        {
            if (lookDirection.sqrMagnitude > 0.0001f)
            {
                lastLookDirection = lookDirection.normalized;
            }

            currentState = null;
            animator.SetBool(isMovingParameter, false);
            ApplyDirection(lastLookDirection);
            PlayIdle();
        }

        #endregion

        #region Private

        private void PlayWalk()
        {
            PlayState(walkState);
        }

        private void PlayIdle()
        {
            PlayState(idleState);
        }

        private void ApplyDirection(Vector2 dir)
        {
            if (animator == null)
            {
                return;
            }

            animator.SetFloat(moveXParameter, dir.x);
            animator.SetFloat(moveYParameter, dir.y);

            Debug.Log($"[{nameof(ArthurAnimationController)}] SetFloat {moveXParameter}={dir.x}, {moveYParameter}={dir.y}");
        }

        private void PlayState(string stateName)
        {
            if (animator == null)
            {
                return;
            }

            const int layer = 0;

            if (currentState == stateName)
            {
                return;
            }

            int hash = Animator.StringToHash(stateName);

            if (!animator.HasState(layer, hash))
            {
                Debug.LogWarning(
                    $"[{nameof(ArthurAnimationController)}] State '{stateName}' nicht gefunden auf Animator '{animator.gameObject.name}'."
                );
                return;
            }

            Debug.Log($"[{nameof(ArthurAnimationController)}] PlayState -> {stateName}");

            animator.Play(hash, layer, 0f);
            currentState = stateName;
        }

        #endregion
    }
}