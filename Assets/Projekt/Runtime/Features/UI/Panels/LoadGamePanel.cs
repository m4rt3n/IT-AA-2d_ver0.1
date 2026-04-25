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
 * - Prüft vor dem Szenenstart die benötigten Dateien und zeigt einen Ladebalken in Prozent
 *
 * Wichtig:
 * - Dieses Panel verwaltet nur sich selbst und seinen Inhalt.
 * - CanvasRoot, BackgroundDim und MenuPanel werden vom MenuManager verwaltet.
 * - Der Schließen-Button ruft einen Callback auf.
 * - Falls kein Callback gesetzt ist, schließt sich das Panel selbst.
 */

using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using ITAA.Core.SceneManagement;
using ITAA.Data;
using ITAA.Player.Session;
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
        private const float InitializationProgressShare = 0.55f;

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

        [Header("Loading UI")]
        [SerializeField] private RectTransform loadingOverlayRoot;
        [SerializeField] private TMP_Text loadingStatusText;
        [SerializeField] private TMP_Text loadingPercentText;
        [SerializeField] private Image loadingProgressFillImage;

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs = false;
        [SerializeField] private bool logHideStackTrace = false;

        #endregion

        #region Private Fields

        private SaveSystem saveSystem;
        private IReadOnlyList<SaveSlotEntity> slots = Array.Empty<SaveSlotEntity>();
        private int currentIndex;
        private bool isInitialized;
        private bool isLoading;

        private Action onCloseRequested;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            saveSystem = new SaveSystem();
            EnsureInitialized();
            EnsureLoadingOverlay();
            SetLoadingOverlayVisible(false);
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
            EnsureLoadingOverlay();
            SetLoadingOverlayVisible(false);
            isLoading = false;

            if (ensureDummySaveOnShow)
            {
                EnsureDemoSaveSlots();
            }

            if (enableDebugLogs)
            {
                Debug.Log($"[{nameof(LoadGamePanel)}] Show()", this);
            }

            gameObject.SetActive(true);

            ResetSelectionToFirstSlot();
            ReloadSlots();
        }

        public void Hide()
        {
            isLoading = false;

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

            slots = NormalizeSlotsForDisplay(saveSystem.GetAllSlots(Mathf.Max(1, slotCount)));

            if (slots == null || slots.Count == 0)
            {
                slots = Array.Empty<SaveSlotEntity>();
                currentIndex = 0;

                if (enableDebugLogs)
                {
                    Debug.LogWarning($"[{nameof(LoadGamePanel)}] Keine Slots geladen.", this);
                }

                RefreshCurrentSlotView();
                return;
            }

            currentIndex = Mathf.Clamp(currentIndex, 0, slots.Count - 1);
            RefreshCurrentSlotView();
        }

        public void ShowPreviousSlot()
        {
            if (isLoading || !HasSlots())
            {
                return;
            }

            currentIndex--;

            if (currentIndex < 0)
            {
                currentIndex = slots.Count - 1;
            }

            RefreshCurrentSlotView();
        }

        public void ShowNextSlot()
        {
            if (isLoading || !HasSlots())
            {
                return;
            }

            currentIndex++;

            if (currentIndex >= slots.Count)
            {
                currentIndex = 0;
            }

            RefreshCurrentSlotView();
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

        private void EnsureLoadingOverlay()
        {
            if (loadingOverlayRoot != null &&
                loadingStatusText != null &&
                loadingPercentText != null &&
                loadingProgressFillImage != null)
            {
                return;
            }

            RectTransform parent = transform as RectTransform;
            if (parent == null)
            {
                return;
            }

            GameObject overlayObject = loadingOverlayRoot != null
                ? loadingOverlayRoot.gameObject
                : CreateUiObject("LoadingOverlay", parent);

            loadingOverlayRoot = overlayObject.GetComponent<RectTransform>();
            StretchToParent(loadingOverlayRoot);

            Image overlayImage = overlayObject.GetComponent<Image>();
            if (overlayImage == null)
            {
                overlayImage = overlayObject.AddComponent<Image>();
            }

            overlayImage.color = new Color(0.05f, 0.07f, 0.12f, 0.92f);
            overlayImage.raycastTarget = true;

            RectTransform cardRoot = CreateUiObject("LoadingCard", loadingOverlayRoot).GetComponent<RectTransform>();
            cardRoot.anchorMin = new Vector2(0.5f, 0.5f);
            cardRoot.anchorMax = new Vector2(0.5f, 0.5f);
            cardRoot.pivot = new Vector2(0.5f, 0.5f);
            cardRoot.anchoredPosition = Vector2.zero;
            cardRoot.sizeDelta = new Vector2(620f, 180f);

            Image cardImage = cardRoot.gameObject.AddComponent<Image>();
            cardImage.color = new Color(0.12f, 0.14f, 0.2f, 0.98f);

            loadingStatusText = CreateText("LoadingStatusText", cardRoot, 28, FontStyles.Bold);
            RectTransform statusRect = loadingStatusText.rectTransform;
            statusRect.anchorMin = new Vector2(0f, 1f);
            statusRect.anchorMax = new Vector2(1f, 1f);
            statusRect.pivot = new Vector2(0.5f, 1f);
            statusRect.anchoredPosition = new Vector2(0f, -24f);
            statusRect.sizeDelta = new Vector2(-48f, 40f);
            loadingStatusText.alignment = TextAlignmentOptions.Center;
            loadingStatusText.text = "Initialisiere Dateien...";

            RectTransform progressBarBackground = CreateUiObject("LoadingProgressBarBackground", cardRoot).GetComponent<RectTransform>();
            progressBarBackground.anchorMin = new Vector2(0.5f, 0.5f);
            progressBarBackground.anchorMax = new Vector2(0.5f, 0.5f);
            progressBarBackground.pivot = new Vector2(0.5f, 0.5f);
            progressBarBackground.anchoredPosition = new Vector2(0f, -6f);
            progressBarBackground.sizeDelta = new Vector2(520f, 28f);

            Image progressBarBackgroundImage = progressBarBackground.gameObject.AddComponent<Image>();
            progressBarBackgroundImage.color = new Color(0.22f, 0.26f, 0.34f, 1f);
            progressBarBackgroundImage.raycastTarget = false;

            RectTransform fillRect = CreateUiObject("LoadingProgressBarFill", progressBarBackground).GetComponent<RectTransform>();
            StretchToParent(fillRect);
            fillRect.offsetMin = Vector2.zero;
            fillRect.offsetMax = Vector2.zero;

            loadingProgressFillImage = fillRect.gameObject.AddComponent<Image>();
            loadingProgressFillImage.color = new Color(0.35f, 0.78f, 0.62f, 1f);
            loadingProgressFillImage.type = Image.Type.Filled;
            loadingProgressFillImage.fillMethod = Image.FillMethod.Horizontal;
            loadingProgressFillImage.fillOrigin = (int)Image.OriginHorizontal.Left;
            loadingProgressFillImage.fillAmount = 0f;
            loadingProgressFillImage.raycastTarget = false;

            loadingPercentText = CreateText("LoadingPercentText", cardRoot, 30, FontStyles.Bold);
            RectTransform percentRect = loadingPercentText.rectTransform;
            percentRect.anchorMin = new Vector2(0f, 0f);
            percentRect.anchorMax = new Vector2(1f, 0f);
            percentRect.pivot = new Vector2(0.5f, 0f);
            percentRect.anchoredPosition = new Vector2(0f, 20f);
            percentRect.sizeDelta = new Vector2(-48f, 40f);
            loadingPercentText.alignment = TextAlignmentOptions.Center;
            loadingPercentText.text = "0%";

            SetLoadingOverlayVisible(false);
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
            if (isLoading)
            {
                return;
            }

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

        private void RefreshCurrentSlotView()
        {
            SaveSlotEntity slot = GetCurrentSlot();

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
                    $"[{nameof(LoadGamePanel)}] Zeige Slot-Index {currentIndex} / SlotId {(slot != null ? slot.SlotId : 0)}",
                    this
                );
            }
        }

        private SaveSlotEntity GetCurrentSlot()
        {
            if (!HasSlots())
            {
                return null;
            }

            currentIndex = Mathf.Clamp(currentIndex, 0, slots.Count - 1);
            return slots[currentIndex];
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

            if (isLoading)
            {
                return;
            }

            StartCoroutine(LoadSelectedSlotAsync(slot, loadedSave));
        }

        private bool HasSlots()
        {
            return slots != null && slots.Count > 0;
        }

        private static IReadOnlyList<SaveSlotEntity> NormalizeSlotsForDisplay(IReadOnlyList<SaveSlotEntity> sourceSlots)
        {
            if (sourceSlots == null || sourceSlots.Count == 0)
            {
                return Array.Empty<SaveSlotEntity>();
            }

            List<SaveSlotEntity> normalizedSlots = new List<SaveSlotEntity>(sourceSlots.Count);

            for (int i = 0; i < sourceSlots.Count; i++)
            {
                SaveSlotEntity source = sourceSlots[i];

                if (source == null)
                {
                    normalizedSlots.Add(SaveSlotEntity.CreateEmpty(i + 1));
                    continue;
                }

                if (source.HasData)
                {
                    normalizedSlots.Add(source);
                    continue;
                }

                source.DisplayName = "Leerer Spielstand";
                source.SceneName = "-";
                source.SavedAtText = "-";
                normalizedSlots.Add(source);
            }

            return normalizedSlots;
        }

        private void EnsureDemoSaveSlots()
        {
            saveSystem.EnsureDummySaveExists();

            EnsureDemoSaveSlot(
                1,
                "Testslot Arthur",
                "Martin",
                3,
                1200,
                new Vector3(2f, 1f, 0f));

            EnsureDemoSaveSlot(
                2,
                "Testslot Bernd",
                "Bernd",
                1,
                0,
                new Vector3(-23f, -6.55f, 0f));
        }

        private void EnsureDemoSaveSlot(
            int slotId,
            string displayName,
            string playerName,
            int level,
            int score,
            Vector3 playerPosition)
        {
            SaveGameData existing = saveSystem.Load(slotId);

            if (existing != null && existing.HasData)
            {
                bool changed = false;

                if (string.IsNullOrWhiteSpace(existing.DisplayName))
                {
                    existing.DisplayName = displayName;
                    changed = true;
                }

                if (string.IsNullOrWhiteSpace(existing.PlayerName))
                {
                    existing.PlayerName = playerName;
                    changed = true;
                }

                if (string.IsNullOrWhiteSpace(existing.SceneName) || existing.SceneName == SceneNames.LegacyGameScene)
                {
                    existing.SceneName = SceneNames.StartScene;
                    changed = true;
                }

                if (string.IsNullOrWhiteSpace(existing.SavedAtText))
                {
                    existing.SavedAtText = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
                    changed = true;
                }

                if (changed)
                {
                    saveSystem.Save(slotId, existing);
                }

                return;
            }

            SaveGameData demoSave = new SaveGameData
            {
                SlotId = slotId,
                DisplayName = displayName,
                PlayerName = playerName,
                SceneName = SceneNames.StartScene,
                SavedAtText = DateTime.Now.ToString("dd.MM.yyyy HH:mm"),
                Level = level,
                Score = score,
                HasData = true
            };

            demoSave.SetPlayerPosition(playerPosition);
            saveSystem.Save(slotId, demoSave);
        }

        private IEnumerator LoadSelectedSlotAsync(SaveSlotEntity slot, SaveGameData loadedSave)
        {
            isLoading = true;
            SetLoadingOverlayVisible(true);
            SetNavigationInteractable(false);
            UpdateLoadingUi(0f, "Initialisiere Dateien...");

            yield return null;

            if (!TryInitializeRequiredFiles(slot, loadedSave, out string errorMessage))
            {
                Debug.LogWarning($"[{nameof(LoadGamePanel)}] {errorMessage}", this);
                UpdateLoadingUi(0f, errorMessage);
                yield return new WaitForSecondsRealtime(1.2f);
                SetLoadingOverlayVisible(false);
                SetNavigationInteractable(true);
                isLoading = false;
                yield break;
            }

            UpdateLoadingUi(InitializationProgressShare, "Dateien initialisiert.");
            yield return null;

            if (enableDebugLogs)
            {
                Debug.Log(
                    $"[{nameof(LoadGamePanel)}] Lade Szene '{slot.SceneName}' für Slot {slot.SlotId} asynchron.",
                    this
                );
            }

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(slot.SceneName);
            if (loadOperation == null)
            {
                Debug.LogWarning($"[{nameof(LoadGamePanel)}] Async Scene Load konnte nicht gestartet werden.", this);
                UpdateLoadingUi(0f, "Szenenstart fehlgeschlagen.");
                yield return new WaitForSecondsRealtime(1.2f);
                SetLoadingOverlayVisible(false);
                SetNavigationInteractable(true);
                isLoading = false;
                yield break;
            }

            while (!loadOperation.isDone)
            {
                float sceneProgress = Mathf.Clamp01(loadOperation.progress / 0.9f);
                float totalProgress = Mathf.Lerp(InitializationProgressShare, 1f, sceneProgress);
                UpdateLoadingUi(totalProgress, "Starte Spiel...");
                yield return null;
            }
        }

        private bool TryInitializeRequiredFiles(SaveSlotEntity slot, SaveGameData loadedSave, out string errorMessage)
        {
            try
            {
                string savegamesDirectory = Path.Combine(Application.persistentDataPath, "Savegames");

                UpdateLoadingUi(0.08f, "Prüfe Speicherpfade...");
                Directory.CreateDirectory(Application.persistentDataPath);
                Directory.CreateDirectory(savegamesDirectory);

                UpdateLoadingUi(0.18f, "Prüfe Save-Datei...");
                string saveFilePath = Path.Combine(savegamesDirectory, $"save_slot_{slot.SlotId}.json");
                if (!File.Exists(saveFilePath))
                {
                    saveSystem.Save(slot.SlotId, loadedSave);
                }

                UpdateLoadingUi(0.3f, "Prüfe Datenbanken...");
                if (DatabaseManager.Instance != null)
                {
                    DatabaseManager.Instance.EnsureStorageInitialized();
                }

                UpdateLoadingUi(0.42f, "Prüfe Runtime-Session...");
                SavegameRuntimeSession runtimeSession = SavegameRuntimeSession.Instance;
                if (runtimeSession == null)
                {
                    GameObject runtimeSessionObject = new GameObject(nameof(SavegameRuntimeSession));
                    runtimeSession = runtimeSessionObject.AddComponent<SavegameRuntimeSession>();
                }

                runtimeSession.SetCurrentSave(loadedSave);

                UpdateLoadingUi(0.5f, "Übernehme Spielerstatus...");
                if (PlayerSession.Instance != null)
                {
                    PlayerSession.Instance.ApplySaveGameData(loadedSave);
                }

                errorMessage = string.Empty;
                return true;
            }
            catch (Exception exception)
            {
                errorMessage = $"Initialisierung fehlgeschlagen: {exception.Message}";
                return false;
            }
        }

        private void SetNavigationInteractable(bool interactable)
        {
            if (previousButton != null)
            {
                previousButton.interactable = interactable && HasSlots() && slots.Count > 1;
            }

            if (nextButton != null)
            {
                nextButton.interactable = interactable && HasSlots() && slots.Count > 1;
            }

            if (closeButton != null)
            {
                closeButton.interactable = interactable;
            }
        }

        private void SetLoadingOverlayVisible(bool visible)
        {
            if (loadingOverlayRoot == null)
            {
                return;
            }

            loadingOverlayRoot.gameObject.SetActive(visible);
        }

        private void UpdateLoadingUi(float progress, string status)
        {
            EnsureLoadingOverlay();

            float clampedProgress = Mathf.Clamp01(progress);

            if (loadingStatusText != null)
            {
                loadingStatusText.text = status;
            }

            if (loadingPercentText != null)
            {
                loadingPercentText.text = $"{Mathf.RoundToInt(clampedProgress * 100f)}%";
            }

            if (loadingProgressFillImage != null)
            {
                loadingProgressFillImage.fillAmount = clampedProgress;
            }
        }

        private static GameObject CreateUiObject(string objectName, Transform parent)
        {
            GameObject gameObject = new GameObject(objectName, typeof(RectTransform));
            gameObject.transform.SetParent(parent, false);
            return gameObject;
        }

        private static void StretchToParent(RectTransform rectTransform)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
        }

        private static TextMeshProUGUI CreateText(string objectName, Transform parent, float fontSize, FontStyles fontStyle)
        {
            GameObject textObject = CreateUiObject(objectName, parent);
            TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
            textComponent.fontSize = fontSize;
            textComponent.fontStyle = fontStyle;
            textComponent.color = Color.white;
            textComponent.textWrappingMode = TextWrappingModes.NoWrap;
            return textComponent;
        }

        #endregion
    }
}
