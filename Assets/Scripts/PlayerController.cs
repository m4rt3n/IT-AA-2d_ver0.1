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
    [SerializeField] private float interactRadius = 0.2f;
    [SerializeField] private float interactDistance = 0.6f;

    #endregion

    #region State

    private bool isMoving;
    private bool interactPressed;

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
        HandleInteraction();

        if (isMoving)
            return;

        HandleMovementInput();
    }

    #endregion

    #region Input System

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnInteract(InputValue value)
    {
        if (value.isPressed)
            interactPressed = true;
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

            animator.SetFloat("moveX", input.x);
            animator.SetFloat("moveY", input.y);

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
            animator.SetBool("isMoving", false);
        }
    }

    private IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        animator.SetBool("isMoving", true);

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
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
        animator.SetBool("isMoving", false);
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

        Vector2 origin = transform.position;
        Vector2 direction = lastMoveDirection;

        if (direction == Vector2.zero)
            direction = Vector2.down;

        Vector2 checkPosition = origin + direction * interactDistance;

        Collider2D hit = Physics2D.OverlapCircle(
            checkPosition,
            interactRadius,
            interactableLayer
        );

        if (hit != null)
        {
            INPCInteractable interactable = hit.GetComponent<INPCInteractable>();

            if (interactable != null)
            {
                interactable.Interact();
                return;
            }

            NPCInteraction npc = hit.GetComponent<NPCInteraction>();
            if (npc != null)
            {
                npc.Interact();
                return;
            }

            Debug.Log("Interagierbares Objekt: " + hit.name);
        }
        else
        {
            Debug.Log("Nichts zum Interagieren.");
        }
    }

    #endregion

    #region Debug

    private void OnDrawGizmosSelected()
    {
        // Collision
        Gizmos.color = Color.red;
        Vector3 collisionPos = transform.position + new Vector3(0f, -collisionOffsetY, 0f);
        Gizmos.DrawWireSphere(collisionPos, radius);

        // Interaction
        Gizmos.color = Color.red;
        Vector2 dir = lastMoveDirection == Vector2.zero ? Vector2.down : lastMoveDirection;
        Vector3 interactPos = transform.position + (Vector3)(dir * interactDistance);
        Gizmos.DrawWireSphere(interactPos, interactRadius);
    }

    #endregion
}