/*
 * Datei: StartMenuController.cs
 * Zweck: Steuert das Hauptmenü und ermöglicht das Öffnen des LoadGamePanels.
 * Verantwortung:
 *   - Reagiert auf UI-Button-Events
 *   - Öffnet und schließt Menüs
 *   - Steuert Sichtbarkeit der Panels
 *
 * Abhängigkeiten:
 *   - GameObject (StartMenuPanel)
 *   - GameObject (LoadGamePanel)
 *
 * Verwendet von:
 *   - StartMenuPanel (Canvas UI)
 */
// Datei: Assets/Projekt/Runtime/Features/UI/Menus/StartMenuController.cs

using ITAA.UI.Managers;
using UnityEngine;

namespace ITAA.UI.Menus
{
    public class StartMenuController : MonoBehaviour
    {
        #region Inspector

        [Header("References")]
        [SerializeField] private MenuManager menuManager;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (menuManager == null)
            {
                menuManager = FindAnyObjectByType<MenuManager>();
            }
        }

        #endregion

        #region Public Methods

        public void OpenLoadGame()
        {
            if (menuManager == null)
            {
                Debug.LogWarning($"[{nameof(StartMenuController)}] Kein {nameof(MenuManager)} gefunden.");
                return;
            }

            menuManager.OpenLoadGame();
        }

        public void CloseLoadGame()
        {
            if (menuManager == null)
            {
                Debug.LogWarning($"[{nameof(StartMenuController)}] Kein {nameof(MenuManager)} gefunden.");
                return;
            }

            menuManager.OpenStartMenu();
        }

        #endregion
    }
}