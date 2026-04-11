/*
 * Datei: PlayerController.cs
 * Zweck: Verarbeitet die Eingabe und bewegt den Spieler in der Spielwelt.
 * Verantwortung: Liest Richtungsinput, bewegt Rigidbody2D und aktualisiert Animator-Parameter.
 * Abhängigkeiten: Rigidbody2D, optional Animator.
 * Verwendet von: Player-GameObject in Gameplay-Szenen.
 */

using UnityEngine;

namespace ITAA.Player.Movement
{
    public class PlayerController : MonoBehaviour
    {
        #region Inspector

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 4f;
        [SerializeField] private bool useBlockingCheck = true;
        [SerializeField] private float skinWidth = 0.01f;

        [Header("Blocking")]
        [SerializeField] private LayerMask blockingLayer;

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs = false;

        [Header("References")]
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Animator animator;

        #endregion

        #region Private Fields

        private Vector2 movementInput;
        private Vector2 lastMoveDirection = Vector2.down;
        private readonly RaycastHit2D[] castResults = new RaycastHit2D[8];

        private bool hasMoveX;
        private bool hasMoveY;
        private bool hasLastMoveX;
        private bool hasLastMoveY;
        private bool hasIsMoving;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (rb == null)
            {
                rb = GetComponent<Rigidbody2D>();
            }

            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }

            CacheAnimatorParameters();
        }

        private void Update()
        {
            movementInput = ReadInput();
            UpdateVisuals(movementInput);
        }

        private void FixedUpdate()
        {
            Move(movementInput);
        }

        #endregion

        #region Private Methods

        private Vector2 ReadInput()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");

            if (Mathf.Abs(x) > 0f)
            {
                y = 0f;
            }

            Vector2 input = new Vector2(x, y).normalized;

            if (input != Vector2.zero)
            {
                lastMoveDirection = input;
            }

            if (enableDebugLogs && input != Vector2.zero)
            {
                Debug.Log($"[PlayerController] Input erkannt: {input}");
            }

            return input;
        }

        private void Move(Vector2 direction)
        {
            if (rb == null)
            {
                Debug.LogError("[PlayerController] Rigidbody2D fehlt.");
                return;
            }

            if (direction == Vector2.zero)
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }

            if (useBlockingCheck && IsBlocked(direction))
            {
                if (enableDebugLogs)
                {
                    Debug.Log($"[PlayerController] Bewegung blockiert: {direction}");
                }

                rb.linearVelocity = Vector2.zero;
                return;
            }

            rb.linearVelocity = direction * moveSpeed;

            if (enableDebugLogs)
            {
                Debug.Log($"[PlayerController] Velocity gesetzt: {rb.linearVelocity}");
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
                    Debug.Log($"[PlayerController] Blockiert durch: {hit.collider.name}");
                }

                return true;
            }

            return false;
        }

        private void UpdateVisuals(Vector2 direction)
        {
            if (animator != null)
            {
                if (hasMoveX) animator.SetFloat("MoveX", direction.x);
                if (hasMoveY) animator.SetFloat("MoveY", direction.y);
                if (hasLastMoveX) animator.SetFloat("LastMoveX", lastMoveDirection.x);
                if (hasLastMoveY) animator.SetFloat("LastMoveY", lastMoveDirection.y);
                if (hasIsMoving) animator.SetBool("IsMoving", direction != Vector2.zero);
            }
        }

        private void CacheAnimatorParameters()
        {
            if (animator == null)
            {
                return;
            }

            hasMoveX = HasParameter("MoveX");
            hasMoveY = HasParameter("MoveY");
            hasLastMoveX = HasParameter("LastMoveX");
            hasLastMoveY = HasParameter("LastMoveY");
            hasIsMoving = HasParameter("IsMoving");
        }

        private bool HasParameter(string parameterName)
        {
            foreach (AnimatorControllerParameter parameter in animator.parameters)
            {
                if (parameter.name == parameterName)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}