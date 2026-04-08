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

    #endregion

    #region Private Fields

    private Transform targetPlayer;
    private bool isMovingToPlayer;
    private bool interactionTriggered;
    private bool menuOpened;

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
    }

    private void Update()
    {
        if (!isMovingToPlayer || targetPlayer == null)
        {
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

        targetPlayer = player.transform;
        isMovingToPlayer = true;
        interactionTriggered = true;
        menuOpened = false;

        Debug.Log($"[ArthurAutoInteraction] Arthur läuft zu: {player.name}");
    }

    public void ResetInteraction()
    {
        targetPlayer = null;
        isMovingToPlayer = false;
        interactionTriggered = false;
        menuOpened = false;

        SetWalking(false);

        Debug.Log("[ArthurAutoInteraction] Interaktion zurückgesetzt.");
    }

    #endregion

    #region Private Methods

    private void MoveTowardsPlayer()
    {
        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = targetPlayer.position;

        float distance = Vector2.Distance(currentPosition, targetPosition);

        if (distance <= stopDistance)
        {
            isMovingToPlayer = false;
            SetWalking(false);

            if (!menuOpened)
            {
                OpenStartMenu();
            }

            return;
        }

        Vector2 direction = (targetPosition - currentPosition).normalized;
        Vector2 newPosition = currentPosition + direction * moveSpeed * Time.deltaTime;

        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);

        UpdateFacing(direction);
        SetWalking(true);
    }

    private void UpdateFacing(Vector2 direction)
    {
        if (spriteRenderer == null)
        {
            return;
        }

        if (direction.x > 0.05f)
        {
            spriteRenderer.flipX = false;
        }
        else if (direction.x < -0.05f)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void SetWalking(bool isWalking)
    {
        if (animator != null)
        {
            animator.SetBool("IsWalking", isWalking);
        }
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