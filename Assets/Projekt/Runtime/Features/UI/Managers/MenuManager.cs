/*
 * Datei: MenuManager.cs
 * Zweck: Verwaltet das Öffnen und Schließen der Hauptmenüs.
 * Verantwortung:
 *   - Aktiviert bei Bedarf den kompletten UI-Root
 *   - Schließt alle Menüs beim Start
 *   - Öffnet das Startmenü nur auf Anforderung
 *   - Wechselt zwischen StartMenuPanel und LoadGamePanel
 *   - Blendet BackgroundDim und MenuPanel passend ein/aus
 *
 * Abhängigkeiten:
 *   - GameObject canvasRoot
 *   - GameObject backgroundDim
 *   - GameObject menuPanel
 *   - GameObject startMenuPanel
 *   - GameObject loadGamePanel
 */

using UnityEngine;

namespace ITAA.UI.Managers
{
    public class MenuManager : MonoBehaviour
    {
        #region Inspector

        [Header("Canvas Root")]
        [SerializeField] private GameObject canvasRoot;

        [Header("Root")]
        [SerializeField] private GameObject backgroundDim;
        [SerializeField] private GameObject menuPanel;

        [Header("Panels")]
        [SerializeField] private GameObject startMenuPanel;
        [SerializeField] private GameObject loadGamePanel;

        #endregion

        #region Properties

        public bool IsOpen { get; private set; }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            AutoAssignMissingReferences();
            HideAllImmediate();
        }

        private void Start()
        {
            AutoAssignMissingReferences();
            HideAllImmediate();
        }

        #endregion

        #region Public Methods

        public void ShowStartMenu()
        {
            Debug.Log($"[{nameof(MenuManager)}] ShowStartMenu");

            EnsureCanvasRootActive();

            if (backgroundDim != null) backgroundDim.SetActive(true);
            if (menuPanel != null) menuPanel.SetActive(true);
            if (startMenuPanel != null) startMenuPanel.SetActive(true);
            if (loadGamePanel != null) loadGamePanel.SetActive(false);

            IsOpen = true;

            LogState("ShowStartMenu");
        }

        public void ShowLoadGamePanel()
        {
            Debug.Log($"[{nameof(MenuManager)}] ShowLoadGamePanel");

            EnsureCanvasRootActive();

            if (backgroundDim != null) backgroundDim.SetActive(true);
            if (menuPanel != null) menuPanel.SetActive(true);
            if (startMenuPanel != null) startMenuPanel.SetActive(false);
            if (loadGamePanel != null) loadGamePanel.SetActive(true);

            IsOpen = true;

            LogState("ShowLoadGamePanel");
        }

        public void HideAll()
        {
            HideAllImmediate();
        }

        public void OpenStartMenu()
        {
            ShowStartMenu();
        }

        public void OpenLoadGame()
        {
            ShowLoadGamePanel();
        }

        #endregion

        #region Private Methods

        private void HideAllImmediate()
        {
            if (startMenuPanel != null) startMenuPanel.SetActive(false);
            if (loadGamePanel != null) loadGamePanel.SetActive(false);
            if (menuPanel != null) menuPanel.SetActive(false);
            if (backgroundDim != null) backgroundDim.SetActive(false);

            // CanvasRoot darf aktiv bleiben oder deaktiviert werden.
            // Hier bewusst AUS, damit beim Spielstart wirklich nichts sichtbar ist.
            if (canvasRoot != null) canvasRoot.SetActive(false);

            IsOpen = false;

            LogState("HideAllImmediate");
        }

        private void EnsureCanvasRootActive()
        {
            if (canvasRoot != null && !canvasRoot.activeSelf)
            {
                canvasRoot.SetActive(true);
            }
        }

        private void AutoAssignMissingReferences()
        {
            if (canvasRoot == null)
            {
                Transform root = transform.Find("CanvasMainMenu");
                if (root != null)
                {
                    canvasRoot = root.gameObject;
                }
            }

            if (canvasRoot != null)
            {
                if (backgroundDim == null)
                {
                    Transform t = canvasRoot.transform.Find("BackgroundDim");
                    if (t != null) backgroundDim = t.gameObject;
                }

                if (menuPanel == null)
                {
                    Transform t = canvasRoot.transform.Find("MenuPanel");
                    if (t != null) menuPanel = t.gameObject;
                }

                if (startMenuPanel == null && menuPanel != null)
                {
                    Transform t = menuPanel.transform.Find("StartMenuPanel");
                    if (t != null) startMenuPanel = t.gameObject;
                }

                if (loadGamePanel == null && menuPanel != null)
                {
                    Transform t = menuPanel.transform.Find("LoadGamePanel");
                    if (t != null) loadGamePanel = t.gameObject;
                }
            }
        }

        private void LogState(string source)
        {
            Debug.Log(
                $"[{nameof(MenuManager)}::{source}] " +
                $"canvasRoot={(canvasRoot != null ? canvasRoot.activeSelf.ToString() : "null")}, " +
                $"backgroundDim={(backgroundDim != null ? backgroundDim.activeSelf.ToString() : "null")}, " +
                $"menuPanel={(menuPanel != null ? menuPanel.activeSelf.ToString() : "null")}, " +
                $"startMenuPanel={(startMenuPanel != null ? startMenuPanel.activeSelf.ToString() : "null")}, " +
                $"loadGamePanel={(loadGamePanel != null ? loadGamePanel.activeSelf.ToString() : "null")}"
            );
        }

        #endregion
    }
}