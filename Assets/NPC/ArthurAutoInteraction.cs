using UnityEngine;

public class ArthurAutoInteraction : MonoBehaviour
{
    #region Inspector

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float stopDistance = 1.2f;

    [Header("References")]
    [SerializeField] private MenuManager menuManager;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rb;

    #endregion

    #region Private Fields

    private Transform targetPlayer;
    private bool isMovingToPlayer;
    private bool interactionTriggered;
    private bool menuOpened;

    // Letzte Blickrichtung für Idle
    private Vector2 lastMoveDirection = Vector2.down;

    #endregion

    #region Unity Methods

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

        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        ApplyDirectionToAnimator(lastMoveDirection);
        SetWalking(false);

        Debug.Log("[ArthurAutoInteraction] Initialisiert.");
    }

    private void Update()
    {
        if (!isMovingToPlayer || targetPlayer == null)
        {
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }

            SetWalking(false);
            return;
        }

        MoveTowardsPlayer();
    }

    #endregion

    #region Public API

    public void TriggerAutoInteraction(GameObject player)
    {
        if (interactionTriggered)
        {
            Debug.Log("[ArthurAutoInteraction] Bereits ausgelöst.");
            return;
        }

        if (player == null)
        {
            Debug.LogWarning("[ArthurAutoInteraction] Player ist null.");
            return;
        }

        if (rb == null)
        {
            Debug.LogError("[ArthurAutoInteraction] Rigidbody2D fehlt.");
            return;
        }

        targetPlayer = player.transform;
        isMovingToPlayer = true;
        interactionTriggered = true;
        menuOpened = false;

        SetWalking(true);

        Debug.Log($"[ArthurAutoInteraction] Arthur läuft zu: {player.name}");
    }

    public void ResetInteraction()
    {
        targetPlayer = null;
        isMovingToPlayer = false;
        interactionTriggered = false;
        menuOpened = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        SetWalking(false);
        ApplyDirectionToAnimator(lastMoveDirection);

        Debug.Log("[ArthurAutoInteraction] Interaktion zurückgesetzt.");
    }

    #endregion

    #region Private Methods

    private void MoveTowardsPlayer()
    {
        if (rb == null || targetPlayer == null)
        {
            return;
        }

        Vector2 currentPosition = rb.position;
        Vector2 targetPosition = targetPlayer.position;

        float distance = Vector2.Distance(currentPosition, targetPosition);

        if (distance <= stopDistance)
        {
            rb.linearVelocity = Vector2.zero;

            isMovingToPlayer = false;
            SetWalking(false);
            ApplyDirectionToAnimator(lastMoveDirection);

            if (!menuOpened)
            {
                OpenStartMenu();
            }

            return;
        }

        Vector2 direction = (targetPosition - currentPosition).normalized;

        if (direction.sqrMagnitude > 0.001f)
        {
            lastMoveDirection = direction;
        }

        // Bewegung über Rigidbody2D, damit Kollisionen respektiert werden
        rb.linearVelocity = direction * moveSpeed;

        UpdateFacing(direction);
        ApplyDirectionToAnimator(direction);
        SetWalking(true);
    }

    private void UpdateFacing(Vector2 direction)
    {
        // Wenn du echte Left/Right Animationen hast, kann diese Methode leer bleiben.
        // Wenn du nur eine Seitenanimation hast, kannst du flipX weiter nutzen.

        if (spriteRenderer == null)
        {
            return;
        }

        // Optionales Spiegeln nur bei horizontaler Dominanz
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0.05f)
            {
                spriteRenderer.flipX = false;
            }
            else if (direction.x < -0.05f)
            {
                spriteRenderer.flipX = true;
            }
        }
    }

    private void ApplyDirectionToAnimator(Vector2 direction)
    {
        if (animator == null)
        {
            return;
        }

        if (animator.runtimeAnimatorController == null)
        {
            return;
        }

        // Nur Hauptachse verwenden, damit die Richtung sauber bleibt
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            animator.SetFloat("MoveX", direction.x > 0 ? 1f : -1f);
            animator.SetFloat("MoveY", 0f);
        }
        else
        {
            animator.SetFloat("MoveX", 0f);
            animator.SetFloat("MoveY", direction.y > 0 ? 1f : -1f);
        }
    }

    private void SetWalking(bool isWalking)
    {
        if (animator == null)
        {
            return;
        }

        if (animator.runtimeAnimatorController == null)
        {
            return;
        }

        animator.SetBool("IsWalking", isWalking);
    }

    private void OpenStartMenu()
    {
        menuOpened = true;

        Debug.Log("[ArthurAutoInteraction] Arthur hat den Player erreicht. Öffne StartMenu.");

        if (menuManager != null)
        {
            menuManager.ShowStartMenu();
        }
        else
        {
            Debug.LogError("[ArthurAutoInteraction] MenuManager fehlt.");
        }
    }

    #endregion
}