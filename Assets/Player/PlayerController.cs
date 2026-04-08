using System.Collections;
using UnityEngine;
using NPC;

public class PlayerController : MonoBehaviour
{
    #region Inspector - Movement

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;   // Geschwindigkeit beim Bewegen zum Zielpunkt
    [SerializeField] private float step = 0.5f;      // Raster-/Schrittweite pro Eingabe

    #endregion

    #region Inspector - Collision

    [Header("Collision")]
    [SerializeField] private LayerMask solidObjectsLayer;     // Layer für Hindernisse / blockierende Objekte
    [SerializeField] private float footRadius = 0.08f;        // Prüf-Radius für Kollisionsabfrage
    [SerializeField] private Vector2 footOffset = new(0f, -0.3f); // Offset zur "Fußposition"

    #endregion

    #region Inspector - Interaction

    [Header("Interaction")]
    [SerializeField] private LayerMask interactableLayer;     // Layer für NPCs / Interaktionsobjekte
    [SerializeField] private float interactDistance = 0.7f;   // Distanz nach vorne für direkte Interaktion
    [SerializeField] private float interactRadius = 0.35f;    // Radius für Front-Check
    [SerializeField] private float fallbackInteractRadius = 0.9f; // Radius für Notfall-/Nahbereichs-Check

    #endregion

    #region Runtime State

    private bool isMoving;
    private bool canMove = true;

    private Vector2 input;
    private Vector2 lastMoveDirection = Vector2.down; // Merkt sich die letzte Blick-/Bewegungsrichtung

    private Animator animator;

    #endregion

    #region Unity Lifecycle

    private void Awake()
    {
        // Animator einmal cachen, damit wir nicht wiederholt GetComponent aufrufen
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Reihenfolge bewusst:
        // 1. Interaktion prüfen
        // 2. Bewegung verarbeiten
        HandleInteraction();
        HandleMovement();
    }

    #endregion

    #region Public API

    /// <summary>
    /// Aktiviert oder deaktiviert die Spielersteuerung.
    /// Wird z. B. bei NPC-Dialogen verwendet.
    /// </summary>
    public void SetPlayerControlEnabled(bool enabled)
    {
        canMove = enabled;

        if (!enabled)
        {
            // Laufende Bewegungs-Coroutines stoppen
            StopAllCoroutines();
            isMoving = false;

            if (animator != null)
            {
                animator.SetBool("isMoving", false);
            }
        }
    }

    #endregion

    #region Movement

    /// <summary>
    /// Liest Eingaben aus und startet bei gültiger Richtung eine Schrittbewegung.
    /// </summary>
    private void HandleMovement()
    {
        if (!canMove || isMoving)
            return;

        input = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        // Verhindert diagonale Bewegung:
        // Sobald horizontaler Input vorhanden ist, wird vertikaler ignoriert.
        if (input.x != 0)
            input.y = 0;

        // Rasterbewegung: Eingaben auf volle Werte runden (-1, 0, 1)
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

    /// <summary>
    /// Bewegt den Spieler weich zum Zielpunkt.
    /// </summary>
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

    /// <summary>
    /// Prüft, ob das Zielfeld frei begehbar ist.
    /// </summary>
    private bool IsWalkable(Vector3 targetPos)
    {
        Vector2 checkPos = (Vector2)targetPos + footOffset;
        return Physics2D.OverlapCircle(checkPos, footRadius, solidObjectsLayer) == null;
    }

    #endregion

    #region Interaction

    /// <summary>
    /// Prüft bei E-Tastendruck, ob vor dem Spieler oder im Nahbereich
    /// ein Interaktionsobjekt vorhanden ist.
    /// </summary>
    private void HandleInteraction()
    {
        if (!canMove)
            return;

        if (!Input.GetKeyDown(KeyCode.E))
            return;

        Vector2 origin = transform.position;
        Vector2 direction = lastMoveDirection == Vector2.zero ? Vector2.down : lastMoveDirection;
        Vector2 forwardPos = origin + direction * interactDistance;

        // Primäre Interaktion: gezielt vor dem Spieler
        Collider2D hit = Physics2D.OverlapCircle(forwardPos, interactRadius, interactableLayer);

        if (hit != null)
        {
            IInteractable interactable = hit.GetComponent<IInteractable>();

            if (interactable != null)
            {
                interactable.Interact();
                return;
            }
        }

        // Fallback: Suche im direkten Umfeld
        Collider2D[] nearby = Physics2D.OverlapCircleAll(origin, fallbackInteractRadius, interactableLayer);

        foreach (Collider2D col in nearby)
        {
            IInteractable interactable = col.GetComponent<IInteractable>();

            if (interactable != null)
            {
                interactable.Interact();
                return;
            }
        }
    }

    #endregion

    #region Debug

    /// <summary>
    /// Zeichnet Debug-Hilfen im Editor:
    /// - rote Kugel für Kollisionsprüfung
    /// - gelbe Kugel für Interaktionsbereich vorne
    /// - cyanfarbene Kugel für Fallback-Bereich
    /// </summary>
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

    #endregion
}