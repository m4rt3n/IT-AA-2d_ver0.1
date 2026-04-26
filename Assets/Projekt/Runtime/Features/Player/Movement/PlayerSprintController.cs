/*
 * Datei: PlayerSprintController.cs
 * Zweck: Verwaltet die Sprint-Logik des Spielers.
 * Verantwortung:
 *   - Prüfen, ob Sprint aktiv ist
 *   - Bereitstellen eines Sprint-Multiplikators
 *
 * Abhängigkeiten:
 *   - Unity Input System
 *
 * Verwendet von:
 *   - PlayerController
 *   - PlayerMotor2D
 */
using UnityEngine.InputSystem;
using UnityEngine;

namespace ITAA.Player.Movement
{
    public class PlayerSprintController : MonoBehaviour
    {
        #region Inspector

        [Header("Sprint")]
        [SerializeField] private Key sprintKey = Key.LeftShift;
        [SerializeField] private float sprintMultiplier = 1.5f;

        #endregion

        #region Public Properties

        public bool IsSprinting { get; private set; }

        public float SprintMultiplier => sprintMultiplier;

        #endregion

        #region Unity Methods

        private void Update()
        {
            Keyboard keyboard = Keyboard.current;
            IsSprinting = keyboard != null && keyboard[sprintKey].isPressed;
        }

        #endregion
    }
}
