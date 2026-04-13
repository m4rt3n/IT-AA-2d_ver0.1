/*
 * Datei: PlayerInputReader.cs
 * Zweck: Liest Bewegungsinput des Spielers aus und stellt ihn anderen Komponenten bereit.
 * Verantwortung:
 *   - Lesen von Horizontal-/Vertical-Input
 *   - Bereitstellen eines normalisierten Bewegungsvektors
 *
 * Abhängigkeiten:
 *   - Unity Input Manager
 *
 * Verwendet von:
 *   - PlayerController
 *   - PlayerMotor2D
 */
// Datei: Assets/Projekt/Runtime/Features/Player/Movement/PlayerInputReader.cs

using UnityEngine;

namespace ITAA.Player.Movement
{
    public class PlayerInputReader : MonoBehaviour
    {
        public Vector2 MoveInput { get; private set; }

        private void Update()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            if (Mathf.Abs(horizontal) > 0f)
            {
                vertical = 0f;
            }

            MoveInput = new Vector2(horizontal, vertical).normalized;
        }
    }
}