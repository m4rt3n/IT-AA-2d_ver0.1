using UnityEngine;

namespace ITAA.NPC.Arthur
{
    public class ArthurAnimationController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Animator animator;

        [Header("States")]
        [SerializeField] private string idleState = "Arthur_Idle";
        [SerializeField] private string walkState = "Arthur_Walk";

        [Header("Parameters")]
        [SerializeField] private string moveXParameter = "MoveX";
        [SerializeField] private string moveYParameter = "MoveY";

        private Vector2 lastLookDirection = Vector2.down;
        private string currentState;

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

            ApplyDirection(lastLookDirection);
            PlayIdle();
        }

        public void SetMovement(Vector2 movement)
        {
            Debug.Log($"[ArthurAnimationController] SetMovement input={movement} sqrMagnitude={movement.sqrMagnitude}");

            if (movement.sqrMagnitude > 0.0001f)
            {
                Vector2 dir = movement.normalized;
                lastLookDirection = dir;

                Debug.Log($"[ArthurAnimationController] WALK dir={dir}");

                ApplyDirection(dir);
                PlayWalk();
                return;
            }

            Debug.Log($"[ArthurAnimationController] IDLE lastLookDirection={lastLookDirection}");

            ApplyDirection(lastLookDirection);
            PlayIdle();
        }

        public void SetIdleDirection(Vector2 lookDirection)
        {
            if (lookDirection.sqrMagnitude > 0.0001f)
            {
                lastLookDirection = lookDirection.normalized;
            }

            ApplyDirection(lastLookDirection);
            PlayIdle();
        }

        public void ForceIdle()
        {
            currentState = null;
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
            ApplyDirection(lastLookDirection);
            PlayIdle();
        }

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

            Debug.Log($"[ArthurAnimationController] SetFloat {moveXParameter}={dir.x}, {moveYParameter}={dir.y}");
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

            Debug.Log($"[ArthurAnimationController] PlayState -> {stateName}");

            animator.Play(hash, layer, 0f);
            currentState = stateName;
        }
    }
}