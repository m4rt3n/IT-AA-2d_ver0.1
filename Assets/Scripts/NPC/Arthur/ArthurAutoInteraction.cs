using ITAA.UI.Managers;
using UnityEngine;

namespace ITAA.NPC.Arthur
{
    public class ArthurAutoInteraction : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 2.5f;
        [SerializeField] private float stopDistance = 1.2f;
        [SerializeField] private LayerMask obstacleLayer;

        [Header("References")]
        [SerializeField] private MenuManager menuManager;
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer spriteRenderer;

        private Transform targetPlayer;
        private bool interactionTriggered;

        private void Start()
        {
            if (menuManager == null)
            {
                menuManager = FindAnyObjectByType<MenuManager>();
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
            if (targetPlayer == null || interactionTriggered)
            {
                SetAnimation(false);
                return;
            }

            Vector2 direction = targetPlayer.position - transform.position;
            float distance = direction.magnitude;

            if (distance <= stopDistance)
            {
                SetAnimation(false);
                interactionTriggered = true;
                menuManager?.ShowStartMenu();
                return;
            }

            direction.Normalize();

            if (IsPathBlocked(direction))
            {
                SetAnimation(false);
                return;
            }

            transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);
            UpdateFacing(direction);
            SetAnimation(true);
        }

        public void SetTargetPlayer(Transform player)
        {
            targetPlayer = player;
            interactionTriggered = false;
        }

        private bool IsPathBlocked(Vector2 direction)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.5f, obstacleLayer);
            return hit.collider != null;
        }

        private void UpdateFacing(Vector2 direction)
        {
            if (spriteRenderer != null && Mathf.Abs(direction.x) > 0.01f)
            {
                spriteRenderer.flipX = direction.x < 0f;
            }
        }

        private void SetAnimation(bool isMoving)
        {
            if (animator != null)
            {
                animator.SetBool("IsMoving", isMoving);
            }
        }
    }
}