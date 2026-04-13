/*
 * Datei: PlayerController.cs
 * Zweck: Verarbeitet die Eingabe und bewegt den Spieler in der Spielwelt.
 * Verantwortung: Liest Richtungsinput, bewegt Rigidbody2D und aktualisiert Animator-Parameter.
 * Abhängigkeiten: Rigidbody2D, optional Animator.
 * Verwendet von: Player-GameObject in Gameplay-Szenen.
 */

// Datei: Assets/Projekt/Runtime/Features/Player/Movement/PlayerController.cs

using UnityEngine;

namespace ITAA.Player.Movement
{
    [RequireComponent(typeof(PlayerInputReader))]
    [RequireComponent(typeof(PlayerMotor2D))]
    public class PlayerController : MonoBehaviour
    {
        #region Inspector

        [Header("References")]
        [SerializeField] private PlayerInputReader inputReader;
        [SerializeField] private PlayerMotor2D motor;
        [SerializeField] private Animator animator;

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs;

        #endregion

        #region Private Fields

        private Vector2 lastMoveDirection = Vector2.down;
        private bool hasMoveX;
        private bool hasMoveY;
        private bool hasLastMoveX;
        private bool hasLastMoveY;
        private bool hasIsMoving;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (inputReader == null)
            {
                inputReader = GetComponent<PlayerInputReader>();
            }

            if (motor == null)
            {
                motor = GetComponent<PlayerMotor2D>();
            }

            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }

            CacheAnimatorParameters();
        }

        private void Update()
        {
            if (inputReader == null)
            {
                Debug.LogError($"[{nameof(PlayerController)}] {nameof(PlayerInputReader)} fehlt.");
                return;
            }

            Vector2 input = inputReader.MoveInput;

            if (input != Vector2.zero)
            {
                lastMoveDirection = input;
            }

            UpdateVisuals(input);

            if (enableDebugLogs && input != Vector2.zero)
            {
                Debug.Log($"[{nameof(PlayerController)}] Input erkannt: {input}");
            }
        }

        private void FixedUpdate()
        {
            if (inputReader == null || motor == null)
            {
                return;
            }

            motor.SetMovementInput(inputReader.MoveInput);
        }

        #endregion

        #region Private Methods

        private void UpdateVisuals(Vector2 direction)
        {
            if (animator == null)
            {
                return;
            }

            if (hasMoveX) animator.SetFloat("MoveX", direction.x);
            if (hasMoveY) animator.SetFloat("MoveY", direction.y);
            if (hasLastMoveX) animator.SetFloat("LastMoveX", lastMoveDirection.x);
            if (hasLastMoveY) animator.SetFloat("LastMoveY", lastMoveDirection.y);
            if (hasIsMoving) animator.SetBool("IsMoving", direction != Vector2.zero);
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