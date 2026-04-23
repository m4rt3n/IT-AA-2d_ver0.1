using UnityEngine;

namespace ITAA.Player.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMotor2D : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 4f;
        [SerializeField] private bool useBlockingCheck = true;
        [SerializeField] private float skinWidth = 0.01f;

        [Header("Blocking")]
        [SerializeField] private LayerMask blockingLayer;

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs;

        private readonly RaycastHit2D[] castResults = new RaycastHit2D[8];
        private Rigidbody2D rb;
        private Vector2 movementInput;
        private bool movementLocked;

        public Vector2 CurrentVelocity => rb != null ? rb.linearVelocity : Vector2.zero;
        public bool IsMovementLocked => movementLocked;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (rb == null)
            {
                return;
            }

            if (movementLocked)
            {
                movementInput = Vector2.zero;
                rb.linearVelocity = Vector2.zero;
                return;
            }

            if (movementInput == Vector2.zero)
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }

            if (useBlockingCheck && IsBlocked(movementInput))
            {
                if (enableDebugLogs)
                {
                    Debug.Log($"[{nameof(PlayerMotor2D)}] Bewegung blockiert: {movementInput}", this);
                }

                rb.linearVelocity = Vector2.zero;
                return;
            }

            rb.linearVelocity = movementInput * moveSpeed;
        }

        public void SetMovementInput(Vector2 input)
        {
            if (movementLocked)
            {
                movementInput = Vector2.zero;
                return;
            }

            movementInput = input;
        }

        public void SetMoveSpeed(float speed)
        {
            moveSpeed = Mathf.Max(0f, speed);
        }

        public void SetMovementLocked(bool locked)
        {
            movementLocked = locked;

            if (locked)
            {
                movementInput = Vector2.zero;

                if (rb != null)
                {
                    rb.linearVelocity = Vector2.zero;
                }
            }

            if (enableDebugLogs)
            {
                Debug.Log($"[{nameof(PlayerMotor2D)}] MovementLocked = {movementLocked}", this);
            }
        }

        public void Stop()
        {
            movementInput = Vector2.zero;

            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }

            if (enableDebugLogs)
            {
                Debug.Log($"[{nameof(PlayerMotor2D)}] Stop ausgeführt.", this);
            }
        }

        private bool IsBlocked(Vector2 direction)
        {
            if (rb == null)
            {
                return false;
            }

            ContactFilter2D filter = new ContactFilter2D
            {
                useLayerMask = true,
                layerMask = blockingLayer,
                useTriggers = false
            };

            float castDistance = moveSpeed * Time.fixedDeltaTime + skinWidth;
            int hitCount = rb.Cast(direction, filter, castResults, castDistance);

            for (int i = 0; i < hitCount; i++)
            {
                RaycastHit2D hit = castResults[i];

                if (hit.collider == null)
                {
                    continue;
                }

                if (enableDebugLogs)
                {
                    Debug.Log($"[{nameof(PlayerMotor2D)}] Blockiert durch: {hit.collider.name}", this);
                }

                return true;
            }

            return false;
        }
    }
}