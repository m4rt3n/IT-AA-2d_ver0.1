/*
 * Datei: PlayerInputReader.cs
 * Zweck: Liest Bewegungs- und Sprint-Eingaben über das neue Unity Input System.
 * Verantwortung: Stellt anderen Komponenten die aktuelle Eingaberichtung bereit.
 * Abhängigkeiten: Unity Input System Package
 * Verwendet von: PlayerController / PlayerMotor2D / weitere Movement-Komponenten
 */

using UnityEngine;
using UnityEngine.InputSystem;

namespace ITAA.Player.Movement
{
    public class PlayerInputReader : MonoBehaviour
    {
        #region Public Properties

        public Vector2 MoveInput { get; private set; }
        public bool IsSprintPressed { get; private set; }

        #endregion

        #region Unity Methods

        private void Update()
        {
            MoveInput = ReadMoveInput();
            IsSprintPressed = ReadSprintInput();
        }

        #endregion

        #region Private Methods

        private static Vector2 ReadMoveInput()
        {
            Vector2 input = Vector2.zero;

            Keyboard keyboard = Keyboard.current;
            if (keyboard != null)
            {
                if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
                {
                    input.x -= 1f;
                }

                if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
                {
                    input.x += 1f;
                }

                if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
                {
                    input.y -= 1f;
                }

                if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
                {
                    input.y += 1f;
                }
            }

            Gamepad gamepad = Gamepad.current;
            if (gamepad != null)
            {
                Vector2 stickInput = gamepad.leftStick.ReadValue();

                if (stickInput.sqrMagnitude > input.sqrMagnitude)
                {
                    input = stickInput;
                }
            }

            return input.sqrMagnitude > 1f ? input.normalized : input;
        }

        private static bool ReadSprintInput()
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard != null &&
                (keyboard.leftShiftKey.isPressed || keyboard.rightShiftKey.isPressed))
            {
                return true;
            }

            Gamepad gamepad = Gamepad.current;
            if (gamepad != null && gamepad.leftStickButton.isPressed)
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}