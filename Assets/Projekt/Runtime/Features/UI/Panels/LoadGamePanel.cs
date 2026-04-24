/*
 * Datei: LoadGamePanel.cs
 * Zweck:
 *   Verwaltet das große Load-Game-Menü mit genau einem sichtbaren Slot.
 *
 * Verantwortung:
 * - Lädt Save-Slots aus dem SaveSystem
 * - Zeigt immer genau einen Slot groß an
 * - Navigation per Pfeil links / rechts
 * - Startet immer mit Slot 1
 * - Setzt oberen Titel abhängig vom aktiven Slot
 * - Lädt Szene bei Auswahl eines belegten Slots
 *
 * Wichtig:
 * - Dieses Panel verwaltet nur sich selbst und seinen Inhalt.
 * - CanvasRoot, BackgroundDim und MenuPanel werden vom MenuManager verwaltet.
 * - Der Schließen-Button ruft einen Callback auf.
 * - Falls kein Callback gesetzt ist, schließt sich das Panel selbst.
 */

using System;
using System.Collections.Generic;
using ITAA.System.Savegame;
using ITAA.UI.Items;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ITAA.UI.Panels
{
    public class LoadGamePanel : MonoBehaviour
    {
        private const int FirstSlotIndex = 0;

        #region Inspector

        [Header("Config")]
        [SerializeField] private int slotCount = 3;
        [SerializeField] private string fallbackTitle = "Spieler";
        [SerializeField] private bool ensureDummySaveOnShow = true;

        [Header("UI References")]
        [SerializeField] private SaveSlotItemUI largeSlotItem;
        [SerializeField] private Button previousButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private TMP_Text headerTitleText;
        [SerializeField] private TMP_Text pageIndicatorText;

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs = false;
        [SerializeField] private bool logHideStackTrace = false;

        #endregion

        #region Private Fields

        private SaveSystem saveSystem;
        private IReadOnlyList<SaveSlotEntity> slots = Array.Empty<SaveSlotEntity>();
        private int currentIndex;
        private bool isInitialized;

        private Action onCloseRequested;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            saveSystem = new SaveSystem();
            EnsureInitialized();
        }

        private void OnDestroy()
        {
            UnwireButtons();
        }

        #endregion

        #region Public Methods

        public void Configure(Action closeCallback)
        {
            onCloseRequested = closeCallback;
        }

        public void Show()
        {
            EnsureInitialized();

            if (ensureDummySaveOnShow)
            {
                saveSystem.EnsureDummySaveExists();
            }

            if (enableDebugLogs)
            {
                Debug.Log($"[{nameof(LoadGamePanel)}] Show()", this);
            }

            gameObject.SetActive(true);

            ResetSelectionToFirstSlot();
            ReloadSlots();
            ShowFirstSlot();
        }

        public void Hide()
        {
            if (enableDebugLogs)
            {
                Debug.Log($"[{nameof(LoadGamePanel)}] Hide()", this);
            }

            if (logHideStackTrace)
            {
                Debug.Log(
                    $"[{nameof(LoadGamePanel)}] Hide() StackTrace:\n{Environment.StackTrace}",
                    this
                );
            }

            gameObject.SetActive(false);
        }

        public void ReloadSlots()
        {
            if (saveSystem == null)
            {
                saveSystem = new SaveSystem();
            }

            slots = saveSystem.GetAllSlots(Mathf.Max(1, slotCount));

            if (slots == null || slots.Count == 0)
            {
                slots = Array.Empty<SaveSlotEntity>();
                currentIndex = 0;

                if (enableDebugLogs)
                {
                    Debug.LogWarning($"[{nameof(LoadGamePanel)}] Keine Slots geladen.", this);
                }

                if (largeSlotItem != null)
                {
                    largeSlotItem.Setup(null, HandleSlotSelected);
                }

                UpdateHeader(null);
                UpdatePageIndicator();
                UpdateNavigationState();
                return;
            }

            currentIndex = Mathf.Clamp(currentIndex, 0, slots.Count - 1);
        }

        public void ShowPreviousSlot()
        {
            if (!HasSlots())
            {
                return;
            }

            currentIndex--;

            if (currentIndex < 0)
            {
                currentIndex = slots.Count - 1;
            }

            ShowSlot(currentIndex);
        }

        public void ShowNextSlot()
        {
            if (!HasSlots())
            {
                return;
            }

            currentIndex++;

            if (currentIndex >= slots.Count)
            {
                currentIndex = 0;
            }

            ShowSlot(currentIndex);
        }

        #endregion

        #region Private Methods

        private void EnsureInitialized()
        {
            if (isInitialized)
            {
                return;
            }

            WireButtons();
            ReloadSlots();

            isInitialized = true;
        }

        private void WireButtons()
        {
            if (previousButton != null)
            {
                previousButton.onClick.RemoveListener(ShowPreviousSlot);
                previousButton.onClick.AddListener(ShowPreviousSlot);
            }

            if (nextButton != null)
            {
                nextButton.onClick.RemoveListener(ShowNextSlot);
                nextButton.onClick.AddListener(ShowNextSlot);
            }

            if (closeButton != null)
            {
                closeButton.onClick.RemoveListener(HandleCloseClicked);
                closeButton.onClick.AddListener(HandleCloseClicked);
            }
        }

        private void UnwireButtons()
        {
            if (previousButton != null)
            {
                previousButton.onClick.RemoveListener(ShowPreviousSlot);
            }

            if (nextButton != null)
            {
                nextButton.onClick.RemoveListener(ShowNextSlot);
            }

            if (closeButton != null)
            {
                closeButton.onClick.RemoveListener(HandleCloseClicked);
            }
        }

        private void HandleCloseClicked()
        {
            if (enableDebugLogs)
            {
                Debug.Log($"[{nameof(LoadGamePanel)}] Close button clicked.", this);
            }

            if (onCloseRequested != null)
            {
                onCloseRequested.Invoke();
                return;
            }

            Hide();
        }

        private void ResetSelectionToFirstSlot()
        {
            currentIndex = FirstSlotIndex;
        }

        private void ShowFirstSlot()
        {
            ShowSlot(FirstSlotIndex);
        }

        private void ShowSlot(int index)
        {
            if (!HasSlots())
            {
                if (largeSlotItem != null)
                {
                    largeSlotItem.Setup(null, HandleSlotSelected);
                }

                UpdateHeader(null);
                UpdatePageIndicator();
                UpdateNavigationState();
                return;
            }

            currentIndex = Mathf.Clamp(index, 0, slots.Count - 1);

            SaveSlotEntity slot = slots[currentIndex];

            if (largeSlotItem != null)
            {
                largeSlotItem.Setup(slot, HandleSlotSelected);
            }
            else
            {
                Debug.LogWarning($"[{nameof(LoadGamePanel)}] LargeSlotItem fehlt.", this);
            }

            UpdateHeader(slot);
            UpdatePageIndicator();
            UpdateNavigationState();

            if (enableDebugLogs)
            {
                Debug.Log(
                    $"[{nameof(LoadGamePanel)}] Zeige Slot-Index {currentIndex} / SlotId {slot.SlotId}",
                    this
                );
            }
        }

        private void UpdateHeader(SaveSlotEntity slot)
        {
            if (headerTitleText == null)
            {
                return;
            }

            if (slot != null && slot.HasData && !string.IsNullOrWhiteSpace(slot.DisplayName))
            {
                headerTitleText.text = slot.DisplayName;
            }
            else
            {
                headerTitleText.text = fallbackTitle;
            }
        }

        private void UpdatePageIndicator()
        {
            if (pageIndicatorText == null)
            {
                return;
            }

            int count = slots != null ? slots.Count : 0;

            if (count <= 0)
            {
                pageIndicatorText.text = "0 / 0";
                return;
            }

            pageIndicatorText.text = $"{currentIndex + 1} / {count}";
        }

        private void UpdateNavigationState()
        {
            bool canNavigate = HasSlots() && slots.Count > 1;

            if (previousButton != null)
            {
                previousButton.gameObject.SetActive(canNavigate);
                previousButton.interactable = canNavigate;
            }

            if (nextButton != null)
            {
                nextButton.gameObject.SetActive(canNavigate);
                nextButton.interactable = canNavigate;
            }
        }

        private void HandleSlotSelected(SaveSlotEntity slot)
        {
            if (slot == null)
            {
                Debug.LogWarning($"[{nameof(LoadGamePanel)}] Slot ist null.", this);
                return;
            }

            if (!slot.HasData)
            {
                if (enableDebugLogs)
                {
                    Debug.Log($"[{nameof(LoadGamePanel)}] Slot {slot.SlotId} ist leer.", this);
                }

                return;
            }

            if (string.IsNullOrWhiteSpace(slot.SceneName))
            {
                Debug.LogWarning($"[{nameof(LoadGamePanel)}] Slot {slot.SlotId} hat keinen SceneName.", this);
                return;
            }

            SaveGameData loadedSave = saveSystem.Load(slot.SlotId);

            if (loadedSave == null)
            {
                Debug.LogWarning(
                    $"[{nameof(LoadGamePanel)}] SaveGameData für Slot {slot.SlotId} konnte nicht geladen werden.",
                    this
                );
                return;
            }

            SavegameRuntimeSession runtimeSession = SavegameRuntimeSession.Instance;

            if (runtimeSession != null)
            {
                runtimeSession.SetCurrentSave(loadedSave);
            }
            else
            {
                Debug.LogWarning(
                    $"[{nameof(LoadGamePanel)}] Keine {nameof(SavegameRuntimeSession)} in der Szene gefunden.",
                    this
                );
            }

            if (enableDebugLogs)
            {
                Debug.Log(
                    $"[{nameof(LoadGamePanel)}] Lade Szene '{slot.SceneName}' für Slot {slot.SlotId}.",
                    this
                );
            }

            SceneManager.LoadScene(slot.SceneName);
        }

        private bool HasSlots()
        {
            return slots != null && slots.Count > 0;
        }

        #endregion
    }
}
