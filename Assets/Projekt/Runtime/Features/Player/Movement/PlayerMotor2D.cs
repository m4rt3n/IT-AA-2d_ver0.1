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
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMotor2D : MonoBehaviour
{
    #region Inspector

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    #endregion

    #region Private Fields

    private Rigidbody2D rb;
    private Vector2 movementInput;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = movementInput * moveSpeed;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Setzt den aktuellen Bewegungsinput für die Physikbewegung.
    /// </summary>
    public void SetMovementInput(Vector2 input)
    {
        movementInput = input;
    }

    /// <summary>
    /// Setzt die Bewegungsgeschwindigkeit.
    /// </summary>
    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

    #endregion
}