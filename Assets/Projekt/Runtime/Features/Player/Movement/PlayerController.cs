using UnityEngine;

namespace ITAA.Player.Movement
{
    [RequireComponent(typeof(PlayerInputReader))]
    [RequireComponent(typeof(PlayerMotor2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerInputReader inputReader;
        [SerializeField] private PlayerMotor2D motor;
        [SerializeField] private Animator animator;

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs;

        private Vector2 lastMoveDirection = Vector2.down;
        private bool hasMoveX;
        private bool hasMoveY;
        private bool hasLastMoveX;
        private bool hasLastMoveY;
        private bool hasIsMoving;

        private bool movementEnabled = true;

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
                Debug.LogError($"[{nameof(PlayerController)}] {nameof(PlayerInputReader)} fehlt.", this);
                return;
            }

            if (!movementEnabled)
            {
                UpdateVisuals(Vector2.zero);
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
                Debug.Log($"[{nameof(PlayerController)}] Input erkannt: {input}", this);
            }
        }

        private void FixedUpdate()
        {
            if (inputReader == null || motor == null)
            {
                return;
            }

            if (!movementEnabled)
            {
                motor.SetMovementInput(Vector2.zero);
                return;
            }

            motor.SetMovementInput(inputReader.MoveInput);
        }

        public void SetMovementEnabled(bool enabled)
        {
            movementEnabled = enabled;

            if (motor == null)
            {
                return;
            }

            motor.SetMovementLocked(!enabled);

            if (!enabled)
            {
                motor.SetMovementInput(Vector2.zero);
                motor.Stop();
                UpdateVisuals(Vector2.zero);

                if (enableDebugLogs)
                {
                    Debug.Log($"[{nameof(PlayerController)}] Movement gesperrt.", this);
                }
            }
            else
            {
                if (enableDebugLogs)
                {
                    Debug.Log($"[{nameof(PlayerController)}] Movement freigegeben.", this);
                }
            }
        }

        public bool IsMovementEnabled()
        {
            return movementEnabled;
        }

        public void StopImmediately()
        {
            if (motor == null)
            {
                return;
            }

            motor.SetMovementInput(Vector2.zero);
            motor.Stop();
            UpdateVisuals(Vector2.zero);

            if (enableDebugLogs)
            {
                Debug.Log($"[{nameof(PlayerController)}] StopImmediately ausgeführt.", this);
            }
        }

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
    }
}