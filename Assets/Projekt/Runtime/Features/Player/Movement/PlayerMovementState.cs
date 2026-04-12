/*
 * Datei: PlayerMovementState.cs
 * Zweck: Verwaltet den aktuellen Bewegungszustand des Spielers.
 * Verantwortung:
 *   - Erkennen, ob der Spieler steht, läuft oder sprintet
 *   - Bereitstellen eines lesbaren Bewegungsstatus
 *
 * Abhängigkeiten:
 *   - keine direkten technischen Abhängigkeiten
 *
 * Verwendet von:
 *   - PlayerController
 *   - Animation-System
 *   - UI
 */
using UnityEngine;

public class PlayerMovementState : MonoBehaviour
{
    public enum MovementState
    {
        Idle,
        Walking,
        Sprinting
    }

    #region Public Properties

    public MovementState CurrentState { get; private set; } = MovementState.Idle;

    #endregion

    #region Public Methods

    public void UpdateState(Vector2 moveInput, bool isSprinting)
    {
        if (moveInput == Vector2.zero)
        {
            CurrentState = MovementState.Idle;
            return;
        }

        CurrentState = isSprinting ? MovementState.Sprinting : MovementState.Walking;
    }

    #endregion
}