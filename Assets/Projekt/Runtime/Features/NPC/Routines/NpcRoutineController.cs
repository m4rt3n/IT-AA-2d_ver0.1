/*
 * Datei: NpcRoutineController.cs
 * Zweck: Fuehrt einfache NPC-Routinen als optionale Runtime-Komponente aus.
 * Verantwortung: Wechselt Routine-Schritte, bewegt optional zu Zielpunkten und setzt einfache Animator-Parameter.
 * Abhaengigkeiten: NpcRoutineStep, Rigidbody2D, Animator, Unity Lifecycle.
 * Verwendung: Kann spaeter auf NPCs oder Testobjekte gesetzt werden, ohne Arthur-/Bernd-Interaktionen automatisch zu veraendern.
 */

using System.Collections.Generic;
using UnityEngine;

namespace ITAA.NPC.Routines
{
    [DisallowMultipleComponent]
    public class NpcRoutineController : MonoBehaviour
    {
        private static readonly int MoveXHash = Animator.StringToHash("MoveX");
        private static readonly int MoveYHash = Animator.StringToHash("MoveY");
        private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");

        [Header("References")]
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Animator animator;

        [Header("Routine")]
        [SerializeField] private List<NpcRoutineStep> steps = new List<NpcRoutineStep>();
        [SerializeField] private bool playOnStart;
        [SerializeField] private bool loopRoutine = true;

        [Header("Defaults")]
        [SerializeField] private Vector2 defaultLookDirection = Vector2.down;

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs;

        private int currentStepIndex = -1;
        private float stepTimer;
        private bool isRunning;
        private bool isPaused;
        private Vector2 lastLookDirection = Vector2.down;

        private bool hasAnimator;
        private bool hasMoveXParameter;
        private bool hasMoveYParameter;
        private bool hasIsMovingParameter;

        public bool IsRunning => isRunning;
        public bool IsPaused => isPaused;
        public int CurrentStepIndex => currentStepIndex;

        private void Awake()
        {
            ResolveReferences();
            CacheAnimatorParameters();
            lastLookDirection = GetSafeDirection(defaultLookDirection);
            ApplyLookDirection(lastLookDirection);
            SetMoving(false);
        }

        private void Start()
        {
            if (playOnStart)
            {
                StartRoutine();
            }
        }

        private void OnDisable()
        {
            StopMovementVisuals();
        }

        private void Update()
        {
            if (!isRunning || isPaused || !HasUsableStep())
            {
                return;
            }

            NpcRoutineStep step = steps[currentStepIndex];

            if (step.StepType == NpcRoutineStepType.MoveToPoint)
            {
                return;
            }

            stepTimer += Time.deltaTime;

            if (stepTimer >= Mathf.Max(0f, step.DurationSeconds))
            {
                AdvanceStep();
            }
        }

        private void FixedUpdate()
        {
            if (!isRunning || isPaused || !HasUsableStep())
            {
                StopMovementVisuals();
                return;
            }

            NpcRoutineStep step = steps[currentStepIndex];

            if (step.StepType != NpcRoutineStepType.MoveToPoint)
            {
                return;
            }

            TickMoveToPoint(step);
        }

        public void StartRoutine()
        {
            if (steps == null || steps.Count == 0)
            {
                Log("Routine kann nicht starten: keine Schritte konfiguriert.");
                return;
            }

            isRunning = true;
            isPaused = false;
            SetStep(0);
        }

        public void StopRoutine()
        {
            isRunning = false;
            isPaused = false;
            currentStepIndex = -1;
            stepTimer = 0f;
            StopMovementVisuals();
        }

        public void PauseRoutine()
        {
            if (!isRunning)
            {
                return;
            }

            isPaused = true;
            StopMovementVisuals();
        }

        public void ResumeRoutine()
        {
            if (!isRunning)
            {
                return;
            }

            isPaused = false;
        }

        public void SetSteps(List<NpcRoutineStep> newSteps, bool restartRoutine)
        {
            steps = newSteps ?? new List<NpcRoutineStep>();

            if (restartRoutine)
            {
                StartRoutine();
                return;
            }

            if (!HasUsableStep())
            {
                StopRoutine();
            }
        }

        private void SetStep(int stepIndex)
        {
            if (steps == null || steps.Count == 0)
            {
                StopRoutine();
                return;
            }

            currentStepIndex = Mathf.Clamp(stepIndex, 0, steps.Count - 1);
            stepTimer = 0f;

            NpcRoutineStep step = steps[currentStepIndex];
            ApplyStepStart(step);

            Log($"Schritt gestartet: {currentStepIndex} ({step.StepName})");
        }

        private void AdvanceStep()
        {
            if (steps == null || steps.Count == 0)
            {
                StopRoutine();
                return;
            }

            int nextStepIndex = currentStepIndex + 1;

            if (nextStepIndex >= steps.Count)
            {
                if (!loopRoutine)
                {
                    StopRoutine();
                    return;
                }

                nextStepIndex = 0;
            }

            SetStep(nextStepIndex);
        }

        private void ApplyStepStart(NpcRoutineStep step)
        {
            if (step == null)
            {
                StopMovementVisuals();
                return;
            }

            if (step.StepType == NpcRoutineStepType.LookDirection)
            {
                lastLookDirection = step.GetSafeLookDirection(lastLookDirection);
                ApplyLookDirection(lastLookDirection);
                SetMoving(false);
                return;
            }

            if (step.StepType == NpcRoutineStepType.Wait)
            {
                SetMoving(false);
            }
        }

        private void TickMoveToPoint(NpcRoutineStep step)
        {
            if (step == null || !step.HasTargetPoint())
            {
                StopMovementVisuals();
                AdvanceStep();
                return;
            }

            Vector2 currentPosition = rb != null ? rb.position : (Vector2)transform.position;
            Vector2 targetPosition = step.TargetPoint.position;
            Vector2 toTarget = targetPosition - currentPosition;

            if (toTarget.magnitude <= Mathf.Max(0.01f, step.StopDistance))
            {
                StopMovementVisuals();
                AdvanceStep();
                return;
            }

            Vector2 direction = toTarget.normalized;
            float speed = Mathf.Max(0.01f, step.MoveSpeed);
            Vector2 nextPosition = Vector2.MoveTowards(
                currentPosition,
                targetPosition,
                speed * Time.fixedDeltaTime);

            if (rb != null)
            {
                rb.MovePosition(nextPosition);
            }
            else
            {
                transform.position = new Vector3(nextPosition.x, nextPosition.y, transform.position.z);
            }

            lastLookDirection = direction;
            ApplyLookDirection(direction);
            SetMoving(true);
        }

        private void StopMovementVisuals()
        {
            SetMoving(false);
            ApplyLookDirection(lastLookDirection);
        }

        private bool HasUsableStep()
        {
            return steps != null &&
                   steps.Count > 0 &&
                   currentStepIndex >= 0 &&
                   currentStepIndex < steps.Count;
        }

        private void ResolveReferences()
        {
            if (rb == null)
            {
                rb = GetComponent<Rigidbody2D>();
            }

            if (animator == null)
            {
                animator = GetComponent<Animator>();

                if (animator == null)
                {
                    animator = GetComponentInChildren<Animator>();
                }
            }
        }

        private void CacheAnimatorParameters()
        {
            hasAnimator = animator != null;

            if (!hasAnimator)
            {
                return;
            }

            hasMoveXParameter = HasAnimatorParameter(MoveXHash, AnimatorControllerParameterType.Float);
            hasMoveYParameter = HasAnimatorParameter(MoveYHash, AnimatorControllerParameterType.Float);
            hasIsMovingParameter = HasAnimatorParameter(IsMovingHash, AnimatorControllerParameterType.Bool);
        }

        private void ApplyLookDirection(Vector2 direction)
        {
            if (!hasAnimator)
            {
                return;
            }

            Vector2 safeDirection = GetSafeDirection(direction);

            if (hasMoveXParameter)
            {
                animator.SetFloat(MoveXHash, safeDirection.x);
            }

            if (hasMoveYParameter)
            {
                animator.SetFloat(MoveYHash, safeDirection.y);
            }
        }

        private void SetMoving(bool moving)
        {
            if (!hasAnimator || !hasIsMovingParameter)
            {
                return;
            }

            animator.SetBool(IsMovingHash, moving);
        }

        private bool HasAnimatorParameter(int hash, AnimatorControllerParameterType parameterType)
        {
            if (animator == null)
            {
                return false;
            }

            foreach (AnimatorControllerParameter parameter in animator.parameters)
            {
                if (parameter.nameHash == hash && parameter.type == parameterType)
                {
                    return true;
                }
            }

            return false;
        }

        private static Vector2 GetSafeDirection(Vector2 direction)
        {
            if (direction.sqrMagnitude <= 0.0001f)
            {
                return Vector2.down;
            }

            return direction.normalized;
        }

        private void Log(string message)
        {
            if (!enableDebugLogs)
            {
                return;
            }

            Debug.Log($"[{nameof(NpcRoutineController)}] {message}", this);
        }
    }
}
