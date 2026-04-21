/*
 * Datei: BerndAnimationController.cs
 * Zweck: Steuert die Animationen von Bernd.
 * Verantwortung:
 * - Verwaltet Idle-, Walk- und Talk-Zustände
 * - Setzt Blickrichtung über Bewegungs-/Zielrichtung
 * - Kapselt Animator-Zugriff für andere Bernd-Komponenten
 *
 * Erwartete Animator-Parameter:
 * - MoveX (float)
 * - MoveY (float)
 * - IsMoving (bool)
 * - IsTalking (bool)
 *
 * Optional:
 * - Animator kann auch ohne alle Parameter laufen.
 *   Fehlende Parameter werden still ignoriert.
 */

using UnityEngine;

namespace ITAA.NPC.Bernd
{
    [DisallowMultipleComponent]
    public class BerndAnimationController : MonoBehaviour
    {
        #region Inspector

        [Header("References")]
        [SerializeField] private Animator animator;

        [Header("Config")]
        [SerializeField] private Vector2 defaultLookDirection = Vector2.down;

        #endregion

        #region Animator Parameter

        private static readonly int MoveXHash = Animator.StringToHash("MoveX");
        private static readonly int MoveYHash = Animator.StringToHash("MoveY");
        private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
        private static readonly int IsTalkingHash = Animator.StringToHash("IsTalking");

        #endregion

        #region State

        private Vector2 lastLookDirection = Vector2.down;
        private bool hasAnimator;

        #endregion

        #region Unity

        private void Awake()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }

            hasAnimator = animator != null;
            lastLookDirection = defaultLookDirection.sqrMagnitude > 0.0001f
                ? defaultLookDirection.normalized
                : Vector2.down;

            ApplyLookDirection(lastLookDirection);
            SetMoving(false);
            SetTalking(false);
        }

        #endregion

        #region Public API

        public void SetMovement(Vector2 movement)
        {
            if (movement.sqrMagnitude > 0.0001f)
            {
                lastLookDirection = movement.normalized;
                ApplyLookDirection(lastLookDirection);
                SetMoving(true);
            }
            else
            {
                SetMoving(false);
                ApplyLookDirection(lastLookDirection);
            }
        }

        public void SetLookDirection(Vector2 lookDirection)
        {
            if (lookDirection.sqrMagnitude <= 0.0001f)
            {
                return;
            }

            lastLookDirection = lookDirection.normalized;
            ApplyLookDirection(lastLookDirection);
        }

        public void SetTalking(bool isTalking)
        {
            if (!hasAnimator)
            {
                return;
            }

            animator.SetBool(IsTalkingHash, isTalking);
        }

        public void PlayIdle(Vector2 lookDirection)
        {
            SetLookDirection(lookDirection);
            SetMoving(false);
        }

        public void PlayWalk(Vector2 movement)
        {
            SetMovement(movement);
        }

        #endregion

        #region Private

        private void SetMoving(bool isMoving)
        {
            if (!hasAnimator)
            {
                return;
            }

            animator.SetBool(IsMovingHash, isMoving);
        }

        private void ApplyLookDirection(Vector2 direction)
        {
            if (!hasAnimator)
            {
                return;
            }

            animator.SetFloat(MoveXHash, direction.x);
            animator.SetFloat(MoveYHash, direction.y);
        }

        #endregion
    }
}