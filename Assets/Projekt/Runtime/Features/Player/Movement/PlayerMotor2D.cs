/*
 * Datei: PlayerMotor2D.cs
 * Zweck: Bewegt den Spieler physisch über Rigidbody2D.
 * Verantwortung:
 *   - Anwenden von Bewegung auf den Rigidbody2D
 *   - Trennung von Input und physischer Bewegung
 *
 * Abhängigkeiten:
 *   - Rigidbody2D
 *
 * Verwendet von:
 *   - PlayerController
 */
// Datei: Assets/Projekt/Runtime/Features/Player/Movement/PlayerMotor2D.cs

using UnityEngine;

namespace ITAA.Player.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMotor2D : MonoBehaviour
    {
        #region Inspector

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 4f;
        [SerializeField] private bool useBlockingCheck = true;
        [SerializeField] private float skinWidth = 0.01f;

        [Header("Blocking")]
        [SerializeField] private LayerMask blockingLayer;

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs;

        #endregion

        #region Private Fields

        private readonly RaycastHit2D[] castResults = new RaycastHit2D[8];
        private Rigidbody2D rb;
        private Vector2 movementInput;

        #endregion

        #region Public Properties

        public Vector2 CurrentVelocity => rb != null ? rb.linearVelocity : Vector2.zero;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (rb == null)
            {
                return;
            }

            if (movementInput == Vector2.zero)
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }

            if (useBlockingCheck && IsBlocked(movementInput))
            {
                if (enableDebugLogs)
                {
                    Debug.Log($"[{nameof(PlayerMotor2D)}] Bewegung blockiert: {movementInput}");
                }

                rb.linearVelocity = Vector2.zero;
                return;
            }

            rb.linearVelocity = movementInput * moveSpeed;
        }

        #endregion

        #region Public Methods

        public void SetMovementInput(Vector2 input)
        {
            movementInput = input;
        }

        public void SetMoveSpeed(float speed)
        {
            moveSpeed = Mathf.Max(0f, speed);
        }

        public void Stop()
        {
            movementInput = Vector2.zero;

            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }

        #endregion

        #region Private Methods

        private bool IsBlocked(Vector2 direction)
        {
            if (rb == null)
            {
                return false;
            }

            ContactFilter2D filter = new ContactFilter2D
            {
                useLayerMask = true,
                layerMask = blockingLayer,
                useTriggers = false
            };

            float castDistance = moveSpeed * Time.fixedDeltaTime + skinWidth;
            int hitCount = rb.Cast(direction, filter, castResults, castDistance);

            for (int i = 0; i < hitCount; i++)
            {
                RaycastHit2D hit = castResults[i];

                if (hit.collider == null)
                {
                    continue;
                }

                if (enableDebugLogs)
                {
                    Debug.Log($"[{nameof(PlayerMotor2D)}] Blockiert durch: {hit.collider.name}");
                }

                return true;
            }

            return false;
        }

        #endregion
    }
}