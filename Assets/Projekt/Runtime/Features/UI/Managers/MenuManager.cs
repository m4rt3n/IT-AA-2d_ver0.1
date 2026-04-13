/*
 * Datei: MenuManager.cs
 * Zweck: Verwaltet das Öffnen und Schließen von UI-Panels.
 * Verantwortung:
 *   - Panels gezielt öffnen
 *   - andere Panels optional schließen
 *   - zentrale UI-Steuerung
 *
 * Abhängigkeiten:
 *   - BasePanel
 *
 * Verwendet von:
 *   - Buttons
 *   - Gameplay-Systeme
 *   - Hauptmenü
 */

// Datei: Assets/Projekt/Runtime/Features/UI/Managers/MenuManager.cs

using ITAA.UI.Panels;
using UnityEngine;

namespace ITAA.UI.Managers
{
    public class MenuManager : MonoBehaviour
    {
        #region Inspector

        [Header("Panels")]
        [SerializeField] private BasePanel startMenuPanel;
        [SerializeField] private BasePanel loadGamePanel;
        [SerializeField] private BasePanel pauseMenuPanel;
        [SerializeField] private BasePanel settingsPanel;
        [SerializeField] private BasePanel dialoguePanel;

        #endregion

        #region Public Methods

        public void OpenStartMenu()
        {
            CloseAll();
            startMenuPanel?.Open();
        }

        public void ShowStartMenu()
        {
            OpenStartMenu();
        }

        public void OpenLoadGame()
        {
            CloseAll();
            loadGamePanel?.Open();
        }

        public void ShowLoadGame()
        {
            OpenLoadGame();
        }

        public void OpenPauseMenu()
        {
            CloseAll();
            pauseMenuPanel?.Open();
        }

        public void OpenSettings()
        {
            CloseAll();
            settingsPanel?.Open();
        }

        public void OpenDialogue()
        {
            CloseAll();
            dialoguePanel?.Open();
        }

        public void CloseAll()
        {
            startMenuPanel?.Close();
            loadGamePanel?.Close();
            pauseMenuPanel?.Close();
            settingsPanel?.Close();
            dialoguePanel?.Close();
        }

        #endregion
    }
}