/*
 * Datei: KnowledgeBaseHotkeyController.cs
 * Zweck: Macht die Knowledge Base in der StartScene per Tastatur-Hotkey testbar.
 * Verantwortung: Erzeugt bei Bedarf ein Runtime-Panel, toggelt es per Unity Input System und sperrt waehrenddessen die Player-Bewegung.
 * Abhaengigkeiten: KnowledgeBasePanel, PlayerController, EventSystem, Unity Input System, Unity UI.
 * Verwendung: Wird vom StartSceneFeatureBootstrap optional erzeugt und kann alternativ manuell auf ein Scene-GameObject gelegt werden.
 */

using ITAA.Player.Movement;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace ITAA.Features.KnowledgeBase
{
    [DisallowMultipleComponent]
    public sealed class KnowledgeBaseHotkeyController : MonoBehaviour
    {
        #region Inspector

        [Header("Hotkey")]
        [SerializeField] private bool toggleWithKeyboard = true;
        [SerializeField] private Key toggleKey = Key.K;

        [Header("Runtime UI")]
        [SerializeField] private bool createRuntimePanel = true;
        [SerializeField] private KnowledgeBasePanel panel;
        [SerializeField] private Canvas targetCanvas;
        [SerializeField] private int sortingOrder = 4200;

        [Header("Gameplay")]
        [SerializeField] private bool lockPlayerMovementWhileOpen = true;
        [SerializeField] private PlayerController playerController;

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs = true;

        #endregion

        private bool playerMovementWasEnabled;
        private bool hasLockedPlayer;

        #region Unity

        private void Awake()
        {
            ResolveReferences();

            if (panel == null && createRuntimePanel)
            {
                panel = CreateRuntimePanel();
            }

            if (panel != null)
            {
                panel.SetClosedCallback(HandlePanelClosed);
                panel.Close();
            }
            else if (enableDebugLogs)
            {
                Debug.LogWarning($"[{nameof(KnowledgeBaseHotkeyController)}] KnowledgeBasePanel fehlt.", this);
            }
        }

        private void Update()
        {
            if (!toggleWithKeyboard)
            {
                return;
            }

            Keyboard keyboard = Keyboard.current;

            if (keyboard != null && keyboard[toggleKey].wasPressedThisFrame)
            {
                ToggleKnowledgeBase();
            }
        }

        #endregion

        #region Public API

        public void ToggleKnowledgeBase()
        {
            ResolveReferences();

            if (panel == null)
            {
                if (enableDebugLogs)
                {
                    Debug.LogWarning($"[{nameof(KnowledgeBaseHotkeyController)}] Toggle abgebrochen: Panel fehlt.", this);
                }

                return;
            }

            if (panel.IsOpen)
            {
                CloseKnowledgeBase();
                return;
            }

            OpenKnowledgeBase();
        }

        public void OpenKnowledgeBase()
        {
            ResolveReferences();

            if (panel == null)
            {
                return;
            }

            LockPlayerMovement();
            panel.Open();

            if (enableDebugLogs)
            {
                Debug.Log($"[{nameof(KnowledgeBaseHotkeyController)}] Knowledge Base geoeffnet.", this);
            }
        }

        public void CloseKnowledgeBase()
        {
            if (panel != null)
            {
                panel.Close();
            }
            else
            {
                ReleasePlayerMovement();
            }
        }

        #endregion

        #region Private

        private void ResolveReferences()
        {
            if (panel == null)
            {
                panel = FindAnyObjectByType<KnowledgeBasePanel>(FindObjectsInactive.Include);
            }

            if (targetCanvas == null && panel != null)
            {
                targetCanvas = panel.GetComponentInParent<Canvas>(true);
            }

            if (playerController == null)
            {
                playerController = FindAnyObjectByType<PlayerController>(FindObjectsInactive.Include);
            }
        }

        private KnowledgeBasePanel CreateRuntimePanel()
        {
            EnsureEventSystem();

            Canvas canvas = targetCanvas != null ? targetCanvas : CreateRuntimeCanvas();
            GameObject panelObject = new("KnowledgeBasePanel", typeof(RectTransform));
            panelObject.transform.SetParent(canvas.transform, false);

            RectTransform panelRect = panelObject.GetComponent<RectTransform>();
            Stretch(panelRect);

            KnowledgeBasePanel runtimePanel = panelObject.AddComponent<KnowledgeBasePanel>();
            Log("Runtime-KnowledgeBasePanel erzeugt.");
            return runtimePanel;
        }

        private Canvas CreateRuntimeCanvas()
        {
            GameObject canvasObject = new("KnowledgeBaseCanvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasObject.transform.SetParent(transform, false);

            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = sortingOrder;

            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;

            Stretch(canvasObject.GetComponent<RectTransform>());
            targetCanvas = canvas;
            return canvas;
        }

        private static void EnsureEventSystem()
        {
            EventSystem existingEventSystem = FindAnyObjectByType<EventSystem>(FindObjectsInactive.Include);

            if (existingEventSystem != null)
            {
                return;
            }

            new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
        }

        private void LockPlayerMovement()
        {
            if (!lockPlayerMovementWhileOpen || playerController == null || hasLockedPlayer)
            {
                return;
            }

            playerMovementWasEnabled = playerController.IsMovementEnabled();

            if (!playerMovementWasEnabled)
            {
                return;
            }

            playerController.SetMovementEnabled(false);
            playerController.StopImmediately();
            hasLockedPlayer = true;
        }

        private void ReleasePlayerMovement()
        {
            if (!hasLockedPlayer)
            {
                return;
            }

            if (playerController != null && playerMovementWasEnabled)
            {
                playerController.SetMovementEnabled(true);
            }

            hasLockedPlayer = false;
        }

        private void HandlePanelClosed()
        {
            ReleasePlayerMovement();

            if (enableDebugLogs)
            {
                Debug.Log($"[{nameof(KnowledgeBaseHotkeyController)}] Knowledge Base geschlossen.", this);
            }
        }

        private static void Stretch(RectTransform rectTransform)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
        }

        private void Log(string message)
        {
            if (!enableDebugLogs)
            {
                return;
            }

            Debug.Log($"[{nameof(KnowledgeBaseHotkeyController)}] {message}", this);
        }

        #endregion
    }
}
