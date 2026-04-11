/*
 * Datei: ArthurAutoInteraction.cs
 * Zweck: Lässt Arthur automatisch auf den Spieler zulaufen und ein Menü öffnen.
 * Verantwortung: Bewegt Arthur zum Ziel, prüft Hindernisse und aktualisiert Bewegungs- und Idle-Animationen.
 * Abhängigkeiten: MenuManager, optional Animator, Physics2D.
 * Verwendet von: Arthur-NPC in der Startszene.
 */

using ITAA.UI.Managers;
using UnityEngine;

namespace ITAA.NPC.Arthur
{
    public class ArthurAutoInteraction : MonoBehaviour
    {
        #region Inspector

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 2.5f;
        [SerializeField] private float stopDistance = 1.2f;
        [SerializeField] private LayerMask obstacleLayer;

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs = false;

        [Header("References")]
        [SerializeField] private MenuManager menuManager;
        [SerializeField] private Animator animator;

        #endregion

        #region Private Fields

        private Transform targetPlayer;
        private bool interactionTriggered;
        private Vector2 lastMoveDirection = Vector2.down;

        private bool hasMoveX;
        private bool hasMoveY;
        private bool hasLastMoveX;
        private bool hasLastMoveY;
        private bool hasIsMoving;

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

            CacheAnimatorParameters();
        }

        private void Update()
        {
            if (targetPlayer == null || interactionTriggered)
            {
                UpdateAnimation(Vector2.zero, false);
                return;
            }

            Vector2 direction = (targetPlayer.position - transform.position);
            float distance = direction.magnitude;

            if (distance <= stopDistance)
            {
                UpdateAnimation(Vector2.zero, false);
                interactionTriggered = true;
                menuManager?.ShowStartMenu();
                return;
            }

            direction.Normalize();

            if (IsPathBlocked(direction))
            {
                UpdateAnimation(Vector2.zero, false);
                return;
            }

            transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);

            if (direction != Vector2.zero)
            {
                lastMoveDirection = direction;
            }

            UpdateAnimation(direction, true);
        }

        #endregion

        #region Public Methods

        public void SetTargetPlayer(Transform player)
        {
            targetPlayer = player;
            interactionTriggered = false;
        }

        #endregion

        #region Private Methods

        private bool IsPathBlocked(Vector2 direction)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.5f, obstacleLayer);

            if (enableDebugLogs && hit.collider != null)
            {
                Debug.Log($"[ArthurAutoInteraction] Blockiert durch: {hit.collider.name}");
            }

            return hit.collider != null;
        }

        private void UpdateAnimation(Vector2 direction, bool isMoving)
        {
            if (animator == null)
            {
                return;
            }

            if (hasMoveX) animator.SetFloat("MoveX", direction.x);
            if (hasMoveY) animator.SetFloat("MoveY", direction.y);
            if (hasLastMoveX) animator.SetFloat("LastMoveX", lastMoveDirection.x);
            if (hasLastMoveY) animator.SetFloat("LastMoveY", lastMoveDirection.y);
            if (hasIsMoving) animator.SetBool("IsMoving", isMoving);
        }

        private void CacheAnimatorParameters()
        {
            if (animator == null)
            {
                return;
            }

            hasMoveX = HasParameter("MoveX");
            hasMoveY = HasParameter("MoveY");
            hasLastMoveX = HasParameter("LastMoveX");
            hasLastMoveY = HasParameter("LastMoveY");
            hasIsMoving = HasParameter("IsMoving");
        }

        private bool HasParameter(string parameterName)
        {
            foreach (AnimatorControllerParameter parameter in animator.parameters)
            {
                if (parameter.name == parameterName)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}