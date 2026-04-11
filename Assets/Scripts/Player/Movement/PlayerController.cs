using UnityEngine;

namespace ITAA.Player.Movement
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 4f;
        [SerializeField] private LayerMask blockingLayer;
        [SerializeField] private float collisionCheckDistance = 0.55f;

        [Header("References")]
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer spriteRenderer;

        private Vector2 movementInput;
        private Vector2 lastMoveDirection = Vector2.down;

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

            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }
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

            return input;
        }

        private void Move(Vector2 direction)
        {
            if (rb == null || direction == Vector2.zero)
            {
                return;
            }

            if (IsBlocked(direction))
            {
                rb.velocity = Vector2.zero;
                return;
            }

            rb.velocity = direction * moveSpeed;
        }

        private bool IsBlocked(Vector2 direction)
        {
            RaycastHit2D hit = Physics2D.Raycast(
                transform.position,
                direction,
                collisionCheckDistance,
                blockingLayer);

            return hit.collider != null;
        }

        private void UpdateVisuals(Vector2 direction)
        {
            if (animator != null)
            {
                animator.SetFloat("MoveX", direction.x);
                animator.SetFloat("MoveY", direction.y);
                animator.SetFloat("LastMoveX", lastMoveDirection.x);
                animator.SetFloat("LastMoveY", lastMoveDirection.y);
                animator.SetBool("IsMoving", direction != Vector2.zero);
            }

            if (spriteRenderer != null && Mathf.Abs(direction.x) > 0.01f)
            {
                spriteRenderer.flipX = direction.x < 0f;
            }

            if (direction == Vector2.zero && rb != null)
            {
                rb.velocity = Vector2.zero;
            }
        }
    }
}