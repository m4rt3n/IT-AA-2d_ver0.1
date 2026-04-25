/*
 * Datei: SettingsPanel.cs
 * Zweck: Verwaltet das Einstellungs-Panel innerhalb der UI.
 * Verantwortung: Reagiert auf Oeffnen und Schliessen des Panels.
 * Abhaengigkeiten: BasePanel, Unity Debug-Logging.
 * Verwendung: Wird als konkretes UI-Panel fuer spaetere Einstellungsoptionen genutzt.
 */

using UnityEngine;

namespace ITAA.UI.Panels
{
    public class SettingsPanel : BasePanel
    {
        protected override void OnOpened()
        {
            Debug.Log($"[{nameof(SettingsPanel)}] geöffnet.");
        }

        protected override void OnClosed()
        {
            Debug.Log($"[{nameof(SettingsPanel)}] geschlossen.");
        }
    }
}
