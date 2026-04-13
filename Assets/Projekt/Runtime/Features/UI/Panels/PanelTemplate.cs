/*
 * Datei: PanelTemplate.cs
 * Zweck: Vorlage für neue UI-Panels.
 * Verantwortung:
 *   - Panel-spezifische Initialisierung
 *   - Reaktion auf Buttons
 *   - Nutzung der gemeinsamen Panel-Basis
 *
 * Abhängigkeiten:
 *   - BasePanel
 *   - Unity UI
 *
 * Verwendet von:
 *   - Neue UI-Panels wie SettingsPanel, PauseMenuPanel, DialoguePanel
 */
using UnityEngine;
using ITAA.UI.Panels;

namespace ITAA.UI.Panels
{
    public class PanelTemplate : BasePanel
    {
        protected override void OnOpened()
        {
            Debug.Log("Panel geöffnet");
        }

        protected override void OnClosed()
        {
            Debug.Log("Panel geschlossen");
        }
    }
}