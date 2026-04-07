using System.Collections;
using UnityEngine;

public class ArthurAutoInteraction : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float stopDistance = 0.6f;

    [Header("References")]
    [SerializeField] private NPCInteraction npcInteraction;
    [SerializeField] private Animator animator;

    private bool hasTriggered;
    private Transform playerTarget;

    public void TriggerAutoInteraction(Transform player)
    {
        Debug.Log("[ArthurAutoInteraction] TriggerAutoInteraction");

        if (hasTriggered || player == null)
            return;

        playerTarget = player;
        StartCoroutine(ApproachAndInteract());
    }

    private IEnumerator ApproachAndInteract()
    {
        Debug.Log("[ArthurAutoInteraction] ApproachAndInteract gestartet");

        hasTriggered = true;

        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player != null)
            player.SetPlayerControlEnabled(false);

        while (playerTarget != null)
        {
            Vector2 current = transform.position;
            Vector2 target = playerTarget.position;

            float distance = Vector2.Distance(current, target);
            Debug.Log("[ArthurAutoInteraction] Distanz: " + distance);

            if (distance <= stopDistance)
                break;

            Vector2 direction = (target - current).normalized;

            transform.position = Vector2.MoveTowards(
                current,
                target,
                moveSpeed * Time.deltaTime
            );

            if (animator != null && animator.runtimeAnimatorController != null)
            {
                animator.SetBool("isMoving", true);
                animator.SetFloat("moveX", Mathf.Round(direction.x));
                animator.SetFloat("moveY", Mathf.Round(direction.y));
            }

            yield return null;
        }

        if (animator != null && animator.runtimeAnimatorController != null)
            animator.SetBool("isMoving", false);

        if (npcInteraction != null)
        {
            Debug.Log("[ArthurAutoInteraction] Interact wird aufgerufen");
            npcInteraction.Interact();
        }
        else
        {
            Debug.LogError("[ArthurAutoInteraction] npcInteraction fehlt");
        }
    }
}