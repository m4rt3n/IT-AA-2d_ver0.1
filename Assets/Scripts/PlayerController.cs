using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float step = 0.07f;

    [Header("Collision")]
    [SerializeField] private LayerMask solidObjectsLayer;
    [SerializeField] private float footRadius = 0.06f;
    [SerializeField] private Vector2 footOffset = new Vector2(0f, -0.25f);

    [Header("Interaction")]
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private float interactionRadius = 0.2f;
    [SerializeField] private float interactionDistance = 0.3f;

    private bool isMoving;
    private Vector2 input;
    private Vector2 lastDirection = Vector2.down;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Kein Animator auf dem Player gefunden.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }

        if (isMoving)
        {
            if (animator != null)
            {
                animator.SetBool("isMoving", true);
            }
            return;
        }

        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        // Keine diagonale Bewegung
        if (input.x != 0)
        {
            input.y = 0;
        }

        if (input != Vector2.zero)
        {
            lastDirection = input.normalized;

            if (animator != null)
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);
                animator.SetBool("isMoving", true);
            }

            Vector3 targetPos = transform.position;
            targetPos.x += input.x * step;
            targetPos.y += input.y * step;

            if (IsWalkable(targetPos))
            {
                StartCoroutine(Move(targetPos));
            }
            else
            {
                Debug.Log("Blockiert");
                if (animator != null)
                {
                    animator.SetBool("isMoving", false);
                }
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

        while ((targetPos - transform.position).sqrMagnitude > 0.0001f)
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
        Vector2 footCheckPos = (Vector2)targetPos + footOffset;

        Collider2D hit = Physics2D.OverlapCircle(footCheckPos, footRadius, solidObjectsLayer);

        if (hit != null)
        {
            Debug.Log("Getroffen: " + hit.name);
            return false;
        }

        return true;
    }

    private void Interact()
    {
        Vector2 checkPos = (Vector2)transform.position + lastDirection * interactionDistance;

        Collider2D hit = Physics2D.OverlapCircle(checkPos, interactionRadius, interactableLayer);

        if (hit != null)
        {
            Debug.Log("Interaktion mit: " + hit.name);

            NPCInteraction npc = hit.GetComponent<NPCInteraction>();
            if (npc != null)
            {
                npc.Interact();
            }
        }
        else
        {
            Debug.Log("Nichts zum Interagieren gefunden.");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 footCheckPos = (Vector2)transform.position + footOffset;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(footCheckPos, footRadius);

        Vector2 interactCheckPos = (Vector2)transform.position + lastDirection * interactionDistance;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(interactCheckPos, interactionRadius);
    }
}