/*
 * Datei: StartMenuController.cs
 * Zweck: Steuert die Aktionen des Startmenüs.
 * Verantwortung: Öffnet das LoadGamePanel, schließt das Menü oder beendet das Spiel.
 * Abhängigkeiten: MenuManager.
 * Verwendet von: StartMenuPanel und seine Buttons.
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

        [Header("State")]
        [SerializeField] private bool openOnStart;

        #endregion

        #region Unity Methods

        private void Start()
        {
            if (menuManager == null)
            {
                menuManager = FindAnyObjectByType<MenuManager>();
            }

            if (openOnStart && menuManager != null)
            {
                menuManager.ShowStartMenu();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        #endregion

        #region Public Methods

        public void OpenLoadGame()
        {
            if (menuManager != null)
            {
                menuManager.ShowLoadGamePanel();
            }
        }

        public void CloseMenu()
        {
            if (menuManager != null)
            {
                menuManager.HideAll();
            }
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        #endregion
    }
}