/*
 * Datei: MenuManager.cs
 * Zweck:
 *   Zentrale Steuerung für CanvasRoot, BackgroundDim, MenuPanel,
 *   StartMenuPanel und LoadGamePanel.
 *
 * Funktionen:
 * - ShowStartMenu()           -> öffnet Hauptmenü
 * - ShowLoadGamePanel()       -> öffnet LoadGamePanel
 * - OpenLoadGameFromArthur()  -> öffnet StartMenu und danach LoadGamePanel
 * - HideAllImmediate()        -> schließt alles sofort
 * - HideAll()                 -> Kompatibilitäts-Wrapper für alten Code
 * - IsOpen                    -> meldet, ob das Menüsystem aktuell offen ist
 *
 * Wichtig:
 * - Der Close-Button im LoadGamePanel ruft in dieser Variante HideAllImmediate() auf
 * - Falls Inspector-Referenzen fehlen, werden sie automatisch gesucht
 * - canvasRoot wird NICHT deaktiviert, damit der MenuManager aktiv bleibt
 * - Beim Start wird das Startmenü einmal geöffnet
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

        [Header("Startup")]
        [SerializeField] private bool openStartMenuOnStart = true;

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs = true;

        #endregion

        #region Public Properties

        public bool IsOpen
        {
            get
            {
                bool dimActive = backgroundDim != null && backgroundDim.activeSelf;
                bool menuActive = menuPanel != null && menuPanel.activeSelf;
                bool startActive = startMenuPanel != null && startMenuPanel.activeSelf;
                bool loadActive = loadGamePanel != null && loadGamePanel.gameObject.activeSelf;

                return dimActive || menuActive || startActive || loadActive;
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

        private void Start()
        {
            if (openStartMenuOnStart)
            {
                ShowStartMenu();
            }
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

        public void OpenLoadGameFromArthur()
        {
            Log("OpenLoadGameFromArthur");

            ShowStartMenu();
            ShowLoadGamePanel();

            LogState("OpenLoadGameFromArthur");
        }

        public void HideAll()
        {
            HideAllImmediate();
        }

        public void HideAllImmediate()
        {
            Log("HideAllImmediate");

            SetActive(canvasRoot, true);

            if (loadGamePanel != null)
            {
                loadGamePanel.Hide();
            }

            SetActive(startMenuPanel, false);
            SetActive(menuPanel, false);
            SetActive(backgroundDim, false);

            LogState("HideAllImmediate");
        }

        #endregion

        #region Private

        private void AutoAssignMissingReferences()
        {
            if (loadGamePanel == null)
            {
                loadGamePanel = FindAnyObjectByType<LoadGamePanel>(FindObjectsInactive.Include);
            }

            if (canvasRoot == null && loadGamePanel != null)
            {
                Canvas parentCanvas = loadGamePanel.GetComponentInParent<Canvas>(true);
                if (parentCanvas != null)
                {
                    canvasRoot = parentCanvas.gameObject;
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

            if (menuPanel == null && backgroundDim != null)
            {
                Transform found = backgroundDim.transform.Find("MenuPanel");
                if (found != null)
                {
                    menuPanel = found.gameObject;
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