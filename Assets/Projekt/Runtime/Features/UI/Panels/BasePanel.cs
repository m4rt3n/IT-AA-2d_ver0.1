/*
 * Datei: BasePanel.cs
 * Zweck: Stellt eine gemeinsame Basis für UI-Panels bereit.
 * Verantwortung:
 *   - Öffnen und Schließen von Panels
 *   - Optionales Initialisieren beim Öffnen
 *   - Optionales Aufräumen beim Schließen
 *
 * Abhängigkeiten:
 *   - GameObject / MonoBehaviour
 *
 * Verwendet von:
 *   - Alle konkreten UI-Panels
 */
// Datei: Assets/Projekt/Runtime/Features/UI/Panels/BasePanel.cs

using UnityEngine;

namespace ITAA.UI.Panels
{
    public class BasePanel : MonoBehaviour
    {
        public bool IsOpen => gameObject.activeSelf;

        public virtual void Open()
        {
            if (gameObject.activeSelf)
            {
                OnOpened();
                return;
            }

            gameObject.SetActive(true);
            OnOpened();
        }

        public virtual void Close()
        {
            if (!gameObject.activeSelf)
            {
                return;
            }

            OnClosed();
            gameObject.SetActive(false);
        }

        protected virtual void OnOpened()
        {
        }

        protected virtual void OnClosed()
        {
        }
    }
}