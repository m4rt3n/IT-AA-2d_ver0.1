// Datei: Assets/Projekt/Runtime/Features/UI/Panels/SettingsPanel.cs

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