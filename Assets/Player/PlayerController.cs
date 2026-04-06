using System.Collections;
using UnityEngine;
using NPC;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float step = 0.5f;

    [Header("Collision")]
    [SerializeField] private LayerMask solidObjectsLayer;
    [SerializeField] private float footRadius = 0.08f;
    [SerializeField] private Vector2 footOffset = new Vector2(0f, -0.3f);

    [Header("Interaction")]
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private float interactDistance = 0.7f;
    [SerializeField] private float interactRadius = 0.35f;
    [SerializeField] private float fallbackInteractRadius = 0.9f;

    private bool isMoving;
    private bool canMove = true;

    private Vector2 input;
    private Vector2 lastMoveDirection = Vector2.down;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleInteraction();
        HandleMovement();
    }

    public void SetPlayerControlEnabled(bool enabled)
    {
        canMove = enabled;

        if (!enabled)
        {
            StopAllCoroutines();
            isMoving = false;

            if (animator != null)
            {
                animator.SetBool("isMoving", false);
            }
        }
    }

    private void HandleMovement()
    {
        if (!canMove || isMoving)
            return;

        input = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        if (input.x != 0)
            input.y = 0;

        input.x = Mathf.Round(input.x);
        input.y = Mathf.Round(input.y);

        if (input != Vector2.zero)
        {
            lastMoveDirection = input;

            if (animator != null)
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);
            }

            Vector3 targetPos = transform.position;
            targetPos.x += input.x * step;
            targetPos.y += input.y * step;

            if (IsWalkable(targetPos))
            {
                StartCoroutine(Move(targetPos));
            }
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("isMoving", false);
            }
        }
    }

    private IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        if (animator != null)
        {
            animator.SetBool("isMoving", true);
        }

        while ((targetPos - transform.position).sqrMagnitude > 0.001f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;

        if (animator != null)
        {
            animator.SetBool("isMoving", false);
        }
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        Vector2 checkPos = (Vector2)targetPos + footOffset;
        return Physics2D.OverlapCircle(checkPos, footRadius, solidObjectsLayer) == null;
    }

    private void HandleInteraction()
    {
        if (!canMove)
            return;

        if (!Input.GetKeyDown(KeyCode.E))
            return;

        Debug.Log("E gedrückt");

        Vector2 origin = transform.position;
        Vector2 direction = lastMoveDirection == Vector2.zero ? Vector2.down : lastMoveDirection;
        Vector2 forwardPos = origin + direction * interactDistance;

        Collider2D hit = Physics2D.OverlapCircle(forwardPos, interactRadius, interactableLayer);

        if (hit != null)
        {
            IInteractable interactable = hit.GetComponent<IInteractable>();

            if (interactable != null)
            {
                Debug.Log("Interaktion mit: " + hit.name);
                interactable.Interact();
                return;
            }
        }

        Collider2D[] nearby = Physics2D.OverlapCircleAll(origin, fallbackInteractRadius, interactableLayer);

        foreach (Collider2D col in nearby)
        {
            IInteractable interactable = col.GetComponent<IInteractable>();

            if (interactable != null)
            {
                Debug.Log("Fallback Interaktion mit: " + col.name);
                interactable.Interact();
                return;
            }
        }

        Debug.Log("Nichts gefunden");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 collisionPos = transform.position + new Vector3(0f, footOffset.y, 0f);
        Gizmos.DrawWireSphere(collisionPos, footRadius);

        Gizmos.color = Color.yellow;
        Vector2 dir = lastMoveDirection == Vector2.zero ? Vector2.down : lastMoveDirection;
        Vector3 interactPos = transform.position + (Vector3)(dir * interactDistance);
        Gizmos.DrawWireSphere(interactPos, interactRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, fallbackInteractRadius);
    }
}