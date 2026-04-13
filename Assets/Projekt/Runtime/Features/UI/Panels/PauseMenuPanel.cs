/*
 * Datei: PauseMenuPanel.cs
 * Zweck: Verwaltet das Pausenmenü während des Spiels.
 * Verantwortung:
 *   - Spiel pausieren
 *   - Spiel fortsetzen
 *   - Navigation zu weiteren Menüs
 *
 * Abhängigkeiten:
 *   - BasePanel
 *   - Unity UI Buttons
 *
 * Verwendet von:
 *   - Gameplay-Szenen
 */
// Datei: Assets/Projekt/Runtime/Features/UI/Panels/PauseMenuPanel.cs

using UnityEngine;
using UnityEngine.UI;

namespace ITAA.UI.Panels
{
    public class PauseMenuPanel : BasePanel
    {
        #region Inspector

        [Header("Buttons")]
        [SerializeField] private Button closeButton;
        [SerializeField] private Button resumeButton;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (closeButton != null)
            {
                closeButton.onClick.RemoveAllListeners();
                closeButton.onClick.AddListener(Close);
            }

            if (resumeButton != null)
            {
                resumeButton.onClick.RemoveAllListeners();
                resumeButton.onClick.AddListener(Close);
            }
        }

        #endregion

        #region Protected Methods

        protected override void OnOpened()
        {
            Time.timeScale = 0f;
        }

        protected override void OnClosed()
        {
            Time.timeScale = 1f;
        }

        #endregion
    }
}