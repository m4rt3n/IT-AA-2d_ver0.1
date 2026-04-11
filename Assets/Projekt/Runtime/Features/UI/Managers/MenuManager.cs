/*
 * Datei: MenuManager.cs
 * Zweck: Steuert den Wechsel zwischen zentralen Menü-Panels.
 * Verantwortung: Öffnet und schließt Startmenü, LoadGamePanel und das Hintergrund-Overlay.
 * Abhängigkeiten: StartMenuController, LoadGamePanel, Hintergrundobjekt.
 * Verwendet von: UI-Buttons, NPC-Interaktionen und Menüfluss in der Startszene.
 */

using ITAA.UI.Menus;
using UnityEngine;

namespace ITAA.UI.Managers
{
    public class MenuManager : MonoBehaviour
    {
        #region Inspector

        [Header("Panels")]
        [SerializeField] private StartMenuController startMenuController;
        [SerializeField] private LoadGamePanel loadGamePanel;
        [SerializeField] private GameObject backgroundDim;

        #endregion

        #region Unity Methods

        private void Start()
        {
            HideAll();
        }

        #endregion

        #region Public Methods

        public void ShowStartMenu()
        {
            SetDim(true);
            SetActive(startMenuController, true);
            SetActive(loadGamePanel, false);
        }

        public void ShowLoadGamePanel()
        {
            SetDim(true);
            SetActive(startMenuController, false);
            SetActive(loadGamePanel, true);

            if (loadGamePanel != null)
            {
                loadGamePanel.Refresh();
            }
        }

        public void HideAll()
        {
            SetDim(false);
            SetActive(startMenuController, false);
            SetActive(loadGamePanel, false);
        }

        #endregion

        #region Private Methods

        private void SetDim(bool visible)
        {
            if (backgroundDim != null)
            {
                backgroundDim.SetActive(visible);
            }
        }

        private void SetActive(MonoBehaviour target, bool visible)
        {
            if (target != null)
            {
                target.gameObject.SetActive(visible);
            }
        }

        #endregion
    }
}