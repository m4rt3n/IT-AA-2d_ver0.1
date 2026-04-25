/*
 * Datei: UIManager.cs
 * Zweck: Zentrale Steuerung aller UI-Elemente.
 * Verantwortung:
 *   - Öffnen und Schließen von Panels
 *   - Verwaltung aktiver UI-Zustände
 *
 * Abhängigkeiten:
 *   - BasePanel
 *
 * Verwendet von:
 *   - Buttons
 *   - GameManager
 */
using UnityEngine;

namespace ITAA.UI.Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public void OpenPanel(GameObject panel)
        {
            panel.SetActive(true);
        }

        public void ClosePanel(GameObject panel)
        {
            panel.SetActive(false);
        }
    }
}
