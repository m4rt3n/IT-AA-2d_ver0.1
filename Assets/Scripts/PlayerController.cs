using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float step = 0.5f;

    [Header("Collision")]
    [SerializeField] private LayerMask solidObjectsLayer;

    [SerializeField] private LayerMask interactableLayer;

    // nur kleiner Prüfbereich an den "Füßen"
    [SerializeField] private float footRadius = 0.08f;

    // Prüfpunkt etwas unterhalb der Figurenmitte
    [SerializeField] private Vector2 footOffset = new Vector2(0f, -0.3f);

    private bool isMoving;
    private Vector2 input;
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
        if (isMoving)
        {
            if (animator != null)
                animator.SetBool("isMoving", true);
            return;
        }

        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        // keine diagonale Bewegung
        if (input.x != 0)
            input.y = 0;

        if (input != Vector2.zero)
        {
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
            else
            {
                Debug.Log("Blockiert");
            }
        }

        if (animator != null)
            animator.SetBool("isMoving", isMoving);
    }

    private IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

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
            animator.SetBool("isMoving", false);
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

    private void OnDrawGizmosSelected()
    {
        Vector2 footCheckPos = (Vector2)transform.position + footOffset;

        if (Application.isPlaying)
        {
            footCheckPos += input * step;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(footCheckPos, footRadius);
    }
}