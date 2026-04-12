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
using UnityEngine;

public class PlayerInputReader : MonoBehaviour
{
    #region Public Properties

    public Vector2 MoveInput { get; private set; }

    #endregion

    #region Unity Methods

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        MoveInput = new Vector2(horizontal, vertical).normalized;
    }

    #endregion
}