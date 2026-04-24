/*
 * Datei: StartMenuController.cs
 * Zweck: Steuert das Hauptmenü und ermöglicht das Öffnen des LoadGamePanels.
 * Verantwortung:
 *   - Reagiert auf UI-Button-Events
 *   - Öffnet und schließt Menüs
 *   - Steuert Sichtbarkeit der Panels
 *
 * Abhängigkeiten:
 *   - MenuManager
 *
 * Verwendet von:
 *   - StartMenuPanel (Canvas UI)
 */

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
            ResolveMenuManager();
        }

        #endregion

        #region Public Methods

        public void OpenLoadGame()
        {
            if (!ResolveMenuManager())
            {
                Debug.LogWarning($"[{nameof(StartMenuController)}] Kein {nameof(MenuManager)} gefunden.");
                return;
            }

            menuManager.ShowLoadGamePanel();
        }

        public void CloseLoadGame()
        {
            if (!ResolveMenuManager())
            {
                Debug.LogWarning($"[{nameof(StartMenuController)}] Kein {nameof(MenuManager)} gefunden.");
                return;
            }

            menuManager.ShowStartMenu();
        }

        public void CloseAllMenus()
        {
            if (!ResolveMenuManager())
            {
                Debug.LogWarning($"[{nameof(StartMenuController)}] Kein {nameof(MenuManager)} gefunden.");
                return;
            }

            menuManager.HideAll();
        }

        private bool ResolveMenuManager()
        {
            if (menuManager != null)
            {
                return true;
            }

            menuManager = FindAnyObjectByType<MenuManager>(FindObjectsInactive.Include);
            return menuManager != null;
        }

        #endregion
    }
}
