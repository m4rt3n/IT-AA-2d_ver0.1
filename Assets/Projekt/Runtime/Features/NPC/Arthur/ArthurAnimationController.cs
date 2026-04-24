/*
 * Datei: ArthurAnimationController.cs
 * Zweck:
 *   Steuert Arthurs Idle- und Walk-Animationen über vorhandene
 *   richtungsbasierte States wie Arthur_IdleDown und Arthur_WalkLeft.
 *
 * Voraussetzungen im Animator:
 *   - States:
 *       Arthur_IdleDown
 *       Arthur_IdleUp
 *       Arthur_IdleLeft
 *       Arthur_IdleRight
 *       Arthur_WalkDown
 *       Arthur_WalkUp
 *       Arthur_WalkLeft
 *       Arthur_WalkRight
 *   - Optionale Parameter:
 *       MoveX
 *       MoveY
 *       IsMoving
 *
 * Hinweis:
 *   Arthur merkt sich seine letzte Blickrichtung und nutzt diese wieder,
 *   sobald er in einen Idle-State wechselt.
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
        [SerializeField] private string idleDownState = "Arthur_IdleDown";
        [SerializeField] private string idleUpState = "Arthur_IdleUp";
        [SerializeField] private string idleLeftState = "Arthur_IdleLeft";
        [SerializeField] private string idleRightState = "Arthur_IdleRight";
        [SerializeField] private string walkDownState = "Arthur_WalkDown";
        [SerializeField] private string walkUpState = "Arthur_WalkUp";
        [SerializeField] private string walkLeftState = "Arthur_WalkLeft";
        [SerializeField] private string walkRightState = "Arthur_WalkRight";

        [Header("Parameters")]
        [SerializeField] private string moveXParameter = "MoveX";
        [SerializeField] private string moveYParameter = "MoveY";
        [SerializeField] private string isMovingParameter = "IsMoving";

        [Header("Defaults")]
        [SerializeField] private Vector2 defaultLookDirection = Vector2.down;

        #endregion

        #region Fields

        private Vector2 lastLookDirection;
        private string currentState;

        #endregion

        #region Unity

        private void Awake()
        {
            CacheAnimator();

            if (!HasValidAnimator())
            {
                return;
            }

            lastLookDirection = GetSafeDirection(defaultLookDirection);

            animator.SetBool(isMovingParameter, false);
            ApplyDirection(lastLookDirection);

            currentState = null;
            PlayIdle();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (animator == null)
            {
                CacheAnimator();
            }
        }
#endif

        #endregion

        #region Public API

        public void SetMovement(Vector2 movement)
        {
            if (!HasValidAnimator())
            {
                return;
            }

            if (movement.sqrMagnitude > 0.0001f)
            {
                Vector2 direction = movement.normalized;
                lastLookDirection = direction;

                animator.SetBool(isMovingParameter, true);
                ApplyDirection(direction);
                PlayWalk();
                return;
            }

            animator.SetBool(isMovingParameter, false);
            ApplyDirection(lastLookDirection);
            PlayIdle();
        }

        public void SetIdleDirection(Vector2 lookDirection)
        {
            if (!HasValidAnimator())
            {
                return;
            }

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
            if (!HasValidAnimator())
            {
                return;
            }

            currentState = null;
            animator.SetBool(isMovingParameter, false);
            ApplyDirection(lastLookDirection);
            PlayIdle();
        }

        public void ForceIdle(Vector2 lookDirection)
        {
            if (!HasValidAnimator())
            {
                return;
            }

            if (lookDirection.sqrMagnitude > 0.0001f)
            {
                lastLookDirection = lookDirection.normalized;
            }

            currentState = null;
            animator.SetBool(isMovingParameter, false);
            ApplyDirection(lastLookDirection);
            PlayIdle();
        }

        public Vector2 GetLastLookDirection()
        {
            return lastLookDirection;
        }

        #endregion

        #region Private

        private void CacheAnimator()
        {
            if (animator != null)
            {
                return;
            }

            animator = GetComponent<Animator>();

            if (animator == null)
            {
                animator = GetComponentInChildren<Animator>();
            }
        }

        private bool HasValidAnimator()
        {
            if (animator == null)
            {
                Debug.LogWarning($"[{nameof(ArthurAnimationController)}] Kein Animator gefunden auf '{gameObject.name}'.");
                return false;
            }

            if (animator.runtimeAnimatorController == null)
            {
                Debug.LogWarning($"[{nameof(ArthurAnimationController)}] Kein Animator Controller auf '{animator.gameObject.name}' zugewiesen.");
                return false;
            }

            return true;
        }

        private Vector2 GetSafeDirection(Vector2 direction)
        {
            if (direction.sqrMagnitude <= 0.0001f)
            {
                return Vector2.down;
            }

            return direction.normalized;
        }

        private void PlayWalk()
        {
            PlayState(GetDirectionalStateName(lastLookDirection, true));
        }

        private void PlayIdle()
        {
            PlayState(GetDirectionalStateName(lastLookDirection, false));
        }

        private void ApplyDirection(Vector2 direction)
        {
            if (animator == null)
            {
                return;
            }

            Vector2 safeDirection = GetSafeDirection(direction);

            animator.SetFloat(moveXParameter, safeDirection.x);
            animator.SetFloat(moveYParameter, safeDirection.y);
        }

        private void PlayState(string stateName)
        {
            if (animator == null)
            {
                return;
            }

            const int layer = 0;

            if (string.IsNullOrWhiteSpace(stateName))
            {
                Debug.LogWarning($"[{nameof(ArthurAnimationController)}] State-Name ist leer.");
                return;
            }

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

            animator.Play(hash, layer, 0f);
            currentState = stateName;
        }

        private string GetDirectionalStateName(Vector2 direction, bool isMoving)
        {
            Vector2 safeDirection = GetSafeDirection(direction);

            if (Mathf.Abs(safeDirection.x) > Mathf.Abs(safeDirection.y))
            {
                if (safeDirection.x >= 0f)
                {
                    return isMoving ? walkRightState : idleRightState;
                }

                return isMoving ? walkLeftState : idleLeftState;
            }

            if (safeDirection.y >= 0f)
            {
                return isMoving ? walkUpState : idleUpState;
            }

            return isMoving ? walkDownState : idleDownState;
        }

        #endregion
    }
}
