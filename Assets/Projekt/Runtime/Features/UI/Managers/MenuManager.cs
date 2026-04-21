/*
 * Datei: MenuManager.cs
 * Zweck:
 *   Zentrale Steuerung für CanvasRoot, BackgroundDim, MenuPanel,
 *   StartMenuPanel und LoadGamePanel.
 *
 * Funktionen:
 * - ShowStartMenu()      -> öffnet Hauptmenü
 * - ShowLoadGamePanel()  -> öffnet LoadGamePanel
 * - HideAllImmediate()   -> schließt alles sofort
 * - HideAll()            -> Kompatibilitäts-Wrapper für alten Code
 * - IsOpen               -> meldet, ob das Menüsystem aktuell offen ist
 *
 * Wichtig:
 * - Der Close-Button im LoadGamePanel ruft in dieser Variante HideAllImmediate() auf
 * - Falls die Inspector-Referenz auf LoadGamePanel fehlt, wird sie automatisch gesucht
 * - HideAllImmediate() wird nur einmal in Awake() aufgerufen
 */

using ITAA.UI.Panels;
using UnityEngine;

namespace ITAA.UI.Managers
{
    public class MenuManager : MonoBehaviour
    {
        #region Inspector

        [Header("Root")]
        [SerializeField] private GameObject canvasRoot;
        [SerializeField] private GameObject backgroundDim;
        [SerializeField] private GameObject menuPanel;

        [Header("Panels")]
        [SerializeField] private GameObject startMenuPanel;
        [SerializeField] private LoadGamePanel loadGamePanel;

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs = true;

        #endregion

        #region Public Properties

        public bool IsOpen
        {
            get
            {
                bool canvasActive = canvasRoot != null && canvasRoot.activeSelf;
                bool dimActive = backgroundDim != null && backgroundDim.activeSelf;
                bool menuActive = menuPanel != null && menuPanel.activeSelf;
                bool startActive = startMenuPanel != null && startMenuPanel.activeSelf;
                bool loadActive = loadGamePanel != null && loadGamePanel.gameObject.activeSelf;

                return canvasActive || dimActive || menuActive || startActive || loadActive;
            }
        }

        #endregion

        #region Unity

        private void Awake()
        {
            AutoAssignMissingReferences();
            ConfigurePanels();
            HideAllImmediate();
        }

        #endregion

        #region Public API

        public void ShowStartMenu()
        {
            Log("ShowStartMenu");

            SetActive(canvasRoot, true);
            SetActive(backgroundDim, true);
            SetActive(menuPanel, true);
            SetActive(startMenuPanel, true);

            if (loadGamePanel != null)
            {
                loadGamePanel.Hide();
            }

            LogState("ShowStartMenu");
        }

        public void ShowLoadGamePanel()
        {
            Log("ShowLoadGamePanel");

            SetActive(canvasRoot, true);
            SetActive(backgroundDim, true);
            SetActive(menuPanel, true);
            SetActive(startMenuPanel, false);

            if (loadGamePanel != null)
            {
                loadGamePanel.Show();
            }
            else
            {
                Debug.LogWarning($"[{nameof(MenuManager)}] LoadGamePanel ist null und kann nicht geöffnet werden.", this);
            }

            LogState("ShowLoadGamePanel");
        }

        public void HideAll()
        {
            HideAllImmediate();
        }

        public void HideAllImmediate()
        {
            Log("HideAllImmediate");

            if (loadGamePanel != null)
            {
                loadGamePanel.Hide();
            }

            SetActive(startMenuPanel, false);
            SetActive(menuPanel, false);
            SetActive(backgroundDim, false);
            SetActive(canvasRoot, false);

            LogState("HideAllImmediate");
        }

        #endregion

        #region Private

        private void AutoAssignMissingReferences()
        {
            if (canvasRoot == null)
            {
                Canvas canvas = FindFirstObjectByType<Canvas>(FindObjectsInactive.Include);
                if (canvas != null)
                {
                    canvasRoot = canvas.gameObject;
                }
            }

            if (loadGamePanel == null)
            {
                loadGamePanel = FindFirstObjectByType<LoadGamePanel>(FindObjectsInactive.Include);
            }

            if (menuPanel == null && canvasRoot != null)
            {
                Transform found = canvasRoot.transform.Find("BackgroundDim/MenuPanel");
                if (found != null)
                {
                    menuPanel = found.gameObject;
                }
            }

            if (backgroundDim == null && canvasRoot != null)
            {
                Transform found = canvasRoot.transform.Find("BackgroundDim");
                if (found != null)
                {
                    backgroundDim = found.gameObject;
                }
            }

            if (startMenuPanel == null && menuPanel != null)
            {
                Transform found = menuPanel.transform.Find("StartMenuPanel");
                if (found != null)
                {
                    startMenuPanel = found.gameObject;
                }
            }
        }

        private void ConfigurePanels()
        {
            if (loadGamePanel != null)
            {
                loadGamePanel.Configure(HideAllImmediate);
            }
        }

        private void SetActive(GameObject target, bool value)
        {
            if (target == null)
            {
                return;
            }

            target.SetActive(value);
        }

        private void Log(string message)
        {
            if (!enableDebugLogs)
            {
                return;
            }

            Debug.Log($"[{nameof(MenuManager)}] {message}", this);
        }

        private void LogState(string context)
        {
            if (!enableDebugLogs)
            {
                return;
            }

            bool canvasState = canvasRoot != null && canvasRoot.activeSelf;
            bool dimState = backgroundDim != null && backgroundDim.activeSelf;
            bool menuState = menuPanel != null && menuPanel.activeSelf;
            bool startState = startMenuPanel != null && startMenuPanel.activeSelf;
            bool loadState = loadGamePanel != null && loadGamePanel.gameObject.activeSelf;

            Debug.Log(
                $"[{nameof(MenuManager)}::{context}] " +
                $"canvasRoot={canvasState}, " +
                $"backgroundDim={dimState}, " +
                $"menuPanel={menuState}, " +
                $"startMenuPanel={startState}, " +
                $"loadGamePanel={loadState}",
                this
            );
        }

        #endregion
    }
}