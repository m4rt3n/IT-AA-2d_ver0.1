/*
 * Datei: StartMenuController.cs
 * Zweck: Steuert das Hauptmenü und ermöglicht das Öffnen des LoadGamePanels.
 * Verantwortung:
 *   - Reagiert auf UI-Button-Events
 *   - Öffnet und schließt Menüs
 *   - Steuert Sichtbarkeit der Panels
 *
 * Abhängigkeiten:
 *   - GameObject (StartMenuPanel)
 *   - GameObject (LoadGamePanel)
 *
 * Verwendet von:
 *   - StartMenuPanel (Canvas UI)
 */
using UnityEngine;

public class StartMenuController : MonoBehaviour
{
    #region Inspector

    [Header("Panels")]
    [SerializeField] private GameObject startMenuPanel;
    [SerializeField] private GameObject loadGamePanel;

    #endregion

    #region Public Methods

    /// <summary>
    /// Öffnet das LoadGamePanel.
    /// </summary>
    public void OpenLoadGame()
    {
        Debug.Log("OpenLoadGame wurde aufgerufen.");

        if (loadGamePanel == null)
        {
            Debug.LogWarning("LoadGamePanel ist nicht zugewiesen!");
            return;
        }

        loadGamePanel.SetActive(true);

        if (startMenuPanel != null)
        {
            startMenuPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Schließt das LoadGamePanel und kehrt zum Startmenü zurück.
    /// </summary>
    public void CloseLoadGame()
    {
        if (loadGamePanel != null)
        {
            loadGamePanel.SetActive(false);
        }

        if (startMenuPanel != null)
        {
            startMenuPanel.SetActive(true);
        }
    }

    #endregion
}