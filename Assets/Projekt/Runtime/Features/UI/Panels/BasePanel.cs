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
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    #region Public Methods

    /// <summary>
    /// Öffnet das Panel.
    /// </summary>
    public virtual void Open()
    {
        gameObject.SetActive(true);
        OnOpened();
    }

    /// <summary>
    /// Schließt das Panel.
    /// </summary>
    public virtual void Close()
    {
        OnClosed();
        gameObject.SetActive(false);
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Wird nach dem Öffnen ausgeführt.
    /// </summary>
    protected virtual void OnOpened()
    {
    }

    /// <summary>
    /// Wird vor dem Schließen ausgeführt.
    /// </summary>
    protected virtual void OnClosed()
    {
    }

    #endregion
}