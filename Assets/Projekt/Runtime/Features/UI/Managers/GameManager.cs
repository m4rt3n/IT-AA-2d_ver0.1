/*
 * Datei: GameManager.cs
 * Zweck: Zentrale Steuerung des Spielzustands.
 * Verantwortung:
 *   - Verwaltung des aktuellen GameStates
 *   - Pause / Resume Logik
 *   - Einstiegspunkt für globale Spielsteuerung
 *
 * Abhängigkeiten:
 *   - UnityEngine
 *
 * Verwendet von:
 *   - UI (PauseMenu, StartMenu)
 *   - Gameplay-Systeme
 */
using UnityEngine;

namespace ITAA.UI.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public enum GameState
        {
            MainMenu,
            Gameplay,
            Paused
        }

        public GameState CurrentState { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void SetState(GameState newState)
        {
            CurrentState = newState;

            switch (newState)
            {
                case GameState.MainMenu:
                    Time.timeScale = 1f;
                    break;

                case GameState.Gameplay:
                    Time.timeScale = 1f;
                    break;

                case GameState.Paused:
                    Time.timeScale = 0f;
                    break;
            }
        }
    }
}
