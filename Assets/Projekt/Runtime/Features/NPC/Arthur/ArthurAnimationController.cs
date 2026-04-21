/*
 * Datei: ArthurAnimationController.cs
 * Zweck:
 *   Steuert Arthurs Idle- und Walk-Animationen über zwei Hauptstates
 *   ("Arthur_Idle" und "Arthur_Walk") und nutzt dabei Blend-Tree-Parameter
 *   für die Blick- und Bewegungsrichtung.
 *
 * Voraussetzungen im Animator:
 *   - States:
 *       Arthur_Idle
 *       Arthur_Walk
 *   - Float Parameter:
 *       MoveX
 *       MoveY
 *   - Bool Parameter:
 *       IsMoving
 *
 * Hinweis:
 *   Dieses Script ist für Blend Trees gebaut.
 *   Wenn dein Animator stattdessen einzelne Richtungsstates wie
 *   Arthur_IdleDown / Arthur_IdleUp / Arthur_WalkLeft usw. nutzt,
 *   dann muss auch der Animator entsprechend auf Blend Trees umgestellt werden.
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
            PlayState(walkState);
        }

        private void PlayIdle()
        {
            PlayState(idleState);
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

        #endregion
    }
}