/*
 * Datei: PlayerSprintController.cs
 * Zweck: Verwaltet die Sprint-Logik des Spielers.
 * Verantwortung:
 *   - Prüfen, ob Sprint aktiv ist
 *   - Bereitstellen eines Sprint-Multiplikators
 *
 * Abhängigkeiten:
 *   - Unity Input Manager
 *
 * Verwendet von:
 *   - PlayerController
 *   - PlayerMotor2D
 */
using UnityEngine;

public class PlayerSprintController : MonoBehaviour
{
    #region Inspector

    [Header("Sprint")]
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private float sprintMultiplier = 1.5f;

    #endregion

    #region Public Properties

    public bool IsSprinting { get; private set; }

    public float SprintMultiplier => sprintMultiplier;

    #endregion

    #region Unity Methods

    private void Update()
    {
        IsSprinting = Input.GetKey(sprintKey);
    }

    #endregion
}