using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Inspector

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private LayerMask solidObjectsLayer;
    [SerializeField] private float step = 0.5f;
    [SerializeField] private float radius = 0.01f;
    [SerializeField] private float collisionOffsetY = 0.2f;

    [Header("Interaction")]
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private float interactRadius = 0.35f;
    [SerializeField] private float interactDistance = 0.7f;
    [SerializeField] private float fallbackInteractRadius = 0.9f;

    #endregion

    #region State

    private bool isMoving;
    private bool interactPressed;
    private bool canMove = true;
    private bool canInteract = true;

    private Vector2 input;
    private Vector2 moveInput;
    private Vector2 lastMoveDirection = Vector2.down;

    private Animator animator;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (canInteract)
        {
            HandleInteraction();
        }

        if (!canMove)
        {
            if (animator != null)
            {
                animator.SetBool("isMoving", false);
            }

            return;
        }

        if (isMoving)
            return;

        HandleMovementInput();
    }

    #endregion

    #region Public Control

    public void SetMovementEnabled(bool enabled)
    {
        canMove = enabled;

        if (!canMove)
        {
            moveInput = Vector2.zero;
            input = Vector2.zero;
            isMoving = false;

            StopAllCoroutines();

            if (animator != null)
            {
                animator.SetBool("isMoving", false);
            }
        }
    }

    public void SetInteractionEnabled(bool enabled)
    {
        canInteract = enabled;

        if (!canInteract)
        {
            interactPressed = false;
        }
    }

    public void SetPlayerControlEnabled(bool enabled)
    {
        SetMovementEnabled(enabled);
        SetInteractionEnabled(enabled);
    }

    #endregion

    #region Input System

    public void OnMove(InputValue value)
    {
        if (!canMove)
        {
            moveInput = Vector2.zero;
            return;
        }

        moveInput = value.Get<Vector2>();
    }

    public void OnInteract(InputValue value)
    {
        if (!canInteract)
            return;

        if (value.isPressed)
        {
            interactPressed = true;
        }
    }

    #endregion

    #region Movement

    private void HandleMovementInput()
    {
        input = moveInput;

        // Keine diagonale Bewegung
        if (input.x != 0)
            input.y = 0;

        // Auf feste Richtungen reduzieren
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

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            if (!canMove)
            {
                if (animator != null)
                {
                    animator.SetBool("isMoving", false);
                }

                yield break;
            }

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
        Vector2 checkPosition = new Vector2(
            targetPos.x,
            targetPos.y - collisionOffsetY
        );

        return Physics2D.OverlapCircle(checkPosition, radius, solidObjectsLayer) == null;
    }

    #endregion

    #region Interaction

    private void HandleInteraction()
    {
        if (!interactPressed)
            return;

        interactPressed = false;

        TryInteract();
    }

    private void TryInteract()
    {
        Vector2 origin = transform.position;
        Vector2 direction = lastMoveDirection;

        if (direction == Vector2.zero)
            direction = Vector2.down;

        Vector2 forwardCheckPosition = origin + direction * interactDistance;

        // 1. Erst klassisch vor dem Spieler prüfen
        Collider2D forwardHit = Physics2D.OverlapCircle(
            forwardCheckPosition,
            interactRadius,
            interactableLayer
        );

        if (TryHandleCollider(forwardHit))
            return;

        // 2. Falls nichts gefunden wurde: Umgebung prüfen
        Collider2D[] nearbyHits = Physics2D.OverlapCircleAll(
            origin,
            fallbackInteractRadius,
            interactableLayer
        );

        foreach (Collider2D hit in nearbyHits)
        {
            if (TryHandleCollider(hit))
                return;
        }

        Debug.Log("Nichts zum Interagieren gefunden.");
    }

    private bool TryHandleCollider(Collider2D hit)
    {
        if (hit == null)
            return false;

        INPCInteractable interactable = hit.GetComponent<INPCInteractable>();

        if (interactable != null)
        {
            Debug.Log("Interaktion mit: " + hit.name);
            interactable.Interact();
            return true;
        }

        Debug.Log("Collider gefunden, aber kein INPCInteractable auf: " + hit.name);
        return false;
    }

    #endregion

    #region Debug

    private void OnDrawGizmosSelected()
    {
        // Collision
        Gizmos.color = Color.red;
        Vector3 collisionPos = transform.position + new Vector3(0f, -collisionOffsetY, 0f);
        Gizmos.DrawWireSphere(collisionPos, radius);

        // Forward interaction
        Gizmos.color = Color.yellow;
        Vector2 dir = lastMoveDirection == Vector2.zero ? Vector2.down : lastMoveDirection;
        Vector3 interactPos = transform.position + (Vector3)(dir * interactDistance);
        Gizmos.DrawWireSphere(interactPos, interactRadius);

        // Fallback interaction
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, fallbackInteractRadius);
    }

    #endregion
}