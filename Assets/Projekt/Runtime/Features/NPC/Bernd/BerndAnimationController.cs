/*
 * Datei: BerndAnimationController.cs
 * Zweck: Steuert die Animationen von Bernd.
 * Verantwortung:
 * - Verwaltet Idle-, Walk- und optionale Talk-Zustände
 * - Setzt Blickrichtung über Bewegungs-/Zielrichtung
 * - Kapselt Animator-Zugriff und prüft Parameter vor dem Setzen
 *
 * Erwartete Animator-Parameter:
 * - MoveX (float)
 * - MoveY (float)
 * - IsMoving (bool)
 * - IsTalking (bool, optional)
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
        [SerializeField] private string talkingParameterName = "IsTalking";
        [SerializeField] private bool enableDebugLogs;

        #endregion

        #region Animator Parameter

        private static readonly int MoveXHash = Animator.StringToHash("MoveX");
        private static readonly int MoveYHash = Animator.StringToHash("MoveY");
        private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");

        #endregion

        #region State

        private Vector2 lastLookDirection = Vector2.down;
        private bool hasAnimator;
        private bool hasMoveXParameter;
        private bool hasMoveYParameter;
        private bool hasMovingParameter;
        private bool hasTalkingParameter;
        private int talkingHash;

        #endregion

        #region Unity

        private void Awake()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }

            hasAnimator = animator != null;
            InitializeAnimatorParameters();

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
            if (!hasAnimator || !hasTalkingParameter)
            {
                return;
            }

            animator.SetBool(talkingHash, isTalking);
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
            if (!hasAnimator || !hasMovingParameter)
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

            if (hasMoveXParameter)
            {
                animator.SetFloat(MoveXHash, direction.x);
            }

            if (hasMoveYParameter)
            {
                animator.SetFloat(MoveYHash, direction.y);
            }
        }

        private void InitializeAnimatorParameters()
        {
            if (!hasAnimator)
            {
                return;
            }

            talkingHash = Animator.StringToHash(talkingParameterName);
            hasMoveXParameter = HasParameter(animator, MoveXHash, AnimatorControllerParameterType.Float);
            hasMoveYParameter = HasParameter(animator, MoveYHash, AnimatorControllerParameterType.Float);
            hasMovingParameter = HasParameter(animator, IsMovingHash, AnimatorControllerParameterType.Bool);
            hasTalkingParameter = HasParameter(animator, talkingHash, AnimatorControllerParameterType.Bool);

            if (enableDebugLogs && !hasTalkingParameter)
            {
                Debug.Log(
                    $"[{nameof(BerndAnimationController)}] Optionaler Animator-Parameter '{talkingParameterName}' fehlt. Talking wird ignoriert.",
                    this
                );
            }
        }

        private static bool HasParameter(Animator targetAnimator, int hash, AnimatorControllerParameterType type)
        {
            if (targetAnimator == null)
            {
                return false;
            }

            foreach (AnimatorControllerParameter parameter in targetAnimator.parameters)
            {
                if (parameter.nameHash == hash && parameter.type == type)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
