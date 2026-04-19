/*
 * Datei: Assets/Projekt/Runtime/Features/UI/Panels/LoadGamePanel.cs
 *
 * Zweck:
 * Zeigt die vorhandenen Save-Slots im Load-Game-Panel an und erlaubt das Laden
 * eines ausgewählten Spielstands.
 *
 * Verantwortung:
 * - erzeugt UI-Slot-Items dynamisch aus einem Prefab
 * - liest Save-Slots aus dem SaveSystem
 * - reagiert auf Klicks auf einzelne Slots
 * - lädt optional direkt die gespeicherte Szene
 * - unterstützt optional HorizontalSnapScroll
 *
 * Abhängigkeiten:
 * - ITAA.System.Savegame.SaveSystem
 * - ITAA.System.Savegame.SaveSlotEntity
 * - ITAA.UI.Items.SaveSlotItemUI
 * - ITAA.UI.Widgets.HorizontalSnapScroll
 *
 * Hinweise:
 * - itemPrefab muss auf das neue SaveSlotItem-Prefab zeigen
 * - contentRoot muss auf ScrollView/Viewport/Content zeigen
 * - für horizontale Karten Content mit HorizontalLayoutGroup verwenden
 */

using System.Collections.Generic;
using ITAA.System.Savegame;
using ITAA.UI.Items;
using ITAA.UI.Widgets;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ITAA.UI.Panels
{
    public class LoadGamePanel : MonoBehaviour
    {
        [Header("Save Slot Setup")]
        [SerializeField] private SaveSlotItemUI itemPrefab;
        [SerializeField] private RectTransform contentRoot;
        [SerializeField] private int slotCount = 3;

        [Header("Optional UI")]
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text infoText;
        [SerializeField] private Button refreshButton;
        [SerializeField] private Button closeButton;

        [Header("Scene Loading")]
        [SerializeField] private bool loadSceneOnSelection = true;
        [SerializeField] private string fallbackSceneName = "GameScene";

        [Header("Optional Snap Scroll")]
        [SerializeField] private HorizontalSnapScroll snapScroll;

        [Header("Debug")]
        [SerializeField] private bool autoRefreshOnEnable = true;
        [SerializeField] private bool enableDebugLogs = true;

        private readonly List<SaveSlotItemUI> spawnedItems = new();
        private SaveSystem saveSystem;

        private void Awake()
        {
            saveSystem = new SaveSystem();

            BindButtons();

            if (titleText != null && string.IsNullOrWhiteSpace(titleText.text))
            {
                titleText.text = "Spielstand laden";
            }
        }

        private void OnEnable()
        {
            if (autoRefreshOnEnable)
            {
                RefreshSlots();
            }
        }

        private void OnDestroy()
        {
            UnbindButtons();
        }

        private void BindButtons()
        {
            if (refreshButton != null)
            {
                refreshButton.onClick.RemoveListener(RefreshSlots);
                refreshButton.onClick.AddListener(RefreshSlots);
            }

            if (closeButton != null)
            {
                closeButton.onClick.RemoveListener(Hide);
                closeButton.onClick.AddListener(Hide);
            }
        }

        private void UnbindButtons()
        {
            if (refreshButton != null)
            {
                refreshButton.onClick.RemoveListener(RefreshSlots);
            }

            if (closeButton != null)
            {
                closeButton.onClick.RemoveListener(Hide);
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);
            RefreshSlots();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void RefreshSlots()
        {
            if (itemPrefab == null)
            {
                Debug.LogWarning($"[{nameof(LoadGamePanel)}] Item Prefab ist nicht gesetzt.", this);
                SetInfo("Kein Slot-Prefab zugewiesen.");
                return;
            }

            if (contentRoot == null)
            {
                Debug.LogWarning($"[{nameof(LoadGamePanel)}] Content Root ist nicht gesetzt.", this);
                SetInfo("Kein Content Root zugewiesen.");
                return;
            }

            ClearSpawnedItems();

            IReadOnlyList<SaveSlotEntity> slots = saveSystem.GetAllSlots(slotCount);

            if (slots == null || slots.Count == 0)
            {
                SetInfo("Keine Spielstände gefunden.");
                RefreshSnapScroll();
                return;
            }

            for (int i = 0; i < slots.Count; i++)
            {
                CreateSlotItem(slots[i]);
            }

            SetInfo($"Gefundene Slots: {slots.Count}");
            RefreshSnapScroll();

            if (enableDebugLogs)
            {
                Debug.Log($"[{nameof(LoadGamePanel)}] RefreshSlots -> {slots.Count} Slot(s) erzeugt.", this);
            }
        }

        private void CreateSlotItem(SaveSlotEntity slot)
        {
            SaveSlotItemUI instance = Instantiate(itemPrefab, contentRoot);
            instance.name = $"SaveSlotItem_{slot.SlotId}";
            instance.Setup(slot, HandleSlotSelected);

            spawnedItems.Add(instance);

            if (enableDebugLogs)
            {
                Debug.Log(
                    $"[{nameof(LoadGamePanel)}] Slot erzeugt -> Id={slot.SlotId}, HasData={slot.HasData}, Scene={slot.SceneName}",
                    this);
            }
        }

        private void ClearSpawnedItems()
        {
            for (int i = spawnedItems.Count - 1; i >= 0; i--)
            {
                if (spawnedItems[i] != null)
                {
                    Destroy(spawnedItems[i].gameObject);
                }
            }

            spawnedItems.Clear();

            for (int i = contentRoot.childCount - 1; i >= 0; i--)
            {
                Transform child = contentRoot.GetChild(i);
                Destroy(child.gameObject);
            }
        }

        private void HandleSlotSelected(SaveSlotEntity slot)
        {
            if (slot == null)
            {
                SetInfo("Ungültiger Slot.");
                return;
            }

            if (enableDebugLogs)
            {
                Debug.Log(
                    $"[{nameof(LoadGamePanel)}] Slot gewählt -> Id={slot.SlotId}, HasData={slot.HasData}, Scene={slot.SceneName}",
                    this);
            }

            if (!slot.HasData)
            {
                SetInfo($"Slot {slot.SlotId} ist leer.");
                return;
            }

            SetInfo($"Lade Slot {slot.SlotId} ...");

            if (!loadSceneOnSelection)
            {
                return;
            }

            string sceneToLoad = string.IsNullOrWhiteSpace(slot.SceneName)
                ? fallbackSceneName
                : slot.SceneName;

            if (!CanLoadScene(sceneToLoad))
            {
                Debug.LogWarning(
                    $"[{nameof(LoadGamePanel)}] Szene '{sceneToLoad}' ist nicht in Build Settings vorhanden. Fallback wird geprüft.",
                    this);

                if (!string.IsNullOrWhiteSpace(fallbackSceneName) && CanLoadScene(fallbackSceneName))
                {
                    sceneToLoad = fallbackSceneName;
                }
                else
                {
                    SetInfo($"Szene '{sceneToLoad}' nicht ladbar.");
                    return;
                }
            }

            SceneManager.LoadScene(sceneToLoad);
        }

        private bool CanLoadScene(string sceneName)
        {
            if (string.IsNullOrWhiteSpace(sceneName))
            {
                return false;
            }

            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string path = SceneUtility.GetScenePathByBuildIndex(i);
                string name = global::System.IO.Path.GetFileNameWithoutExtension(path);

                if (name == sceneName)
                {
                    return true;
                }
            }

            return false;
        }

        private void RefreshSnapScroll()
        {
            if (snapScroll == null)
            {
                return;
            }

            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentRoot);
            snapScroll.Refresh();
        }

        private void SetInfo(string message)
        {
            if (infoText != null)
            {
                infoText.text = message;
            }

            if (enableDebugLogs)
            {
                Debug.Log($"[{nameof(LoadGamePanel)}] {message}", this);
            }
        }
    }
}