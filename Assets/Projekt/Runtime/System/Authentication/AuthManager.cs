/*
 * Datei: AuthManager.cs
 * Zweck: Verwaltet den Einstieg ins Spiel über einen geladenen Spielstand.
 * Verantwortung: Übergibt Save-Daten an die PlayerSession und startet die Zielszene.
 * Abhängigkeiten: PersistentSingleton, SaveSlotData, PlayerSession, SceneManager.
 * Verwendet von: LoadGamePanel und späteren Login-/Start-Workflows.
 */
using ITAA.Core.Runtime;
using ITAA.Core.SceneManagement;
using ITAA.Data.Models;
using ITAA.Player.Session;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ITAA.Authentication
{
    public class AuthManager : PersistentSingleton<AuthManager>
    {
        [Header("Scenes")]
        [SerializeField] private string gameSceneName = SceneNames.GameScene;

        // --- LOGIN / REGISTER (aktuell deaktiviert) ---

        public bool Register(string username, string password)
        {
            Debug.Log("[AuthManager] Registrierung ist aktuell deaktiviert.");
            return false;
        }

        public bool Login(string username, string password, out string message)
        {
            message = "Login ist aktuell deaktiviert. Bitte Spielstand direkt laden.";
            Debug.Log("[AuthManager] Login ist aktuell deaktiviert.");
            return false;
        }

        // --- GAME START ---

        public void StartGameWithSave(SaveSlotData selectedSave)
        {
            if (selectedSave == null)
            {
                Debug.LogError("[AuthManager] selectedSave ist null.");
                return;
            }

            if (PlayerSession.Instance == null)
            {
                Debug.LogError("[AuthManager] PlayerSession fehlt.");
                return;
            }

            Debug.Log($"[AuthManager] Starte Spiel mit Save: {selectedSave.SaveSlotName}");

            PlayerSession.Instance.SetSession(selectedSave);

            SceneManager.LoadScene(gameSceneName);
        }

        // --- LOGOUT / RESET ---

        public void Logout()
        {
            if (PlayerSession.Instance != null)
            {
                PlayerSession.Instance.ClearSession();
            }

            Debug.Log("[AuthManager] Logout durchgeführt.");
        }
    }
}
