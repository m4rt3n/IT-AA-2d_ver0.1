/*
 * Datei: LoadGamePanel.cs
 * Zweck: Lädt und zeigt alle verfügbaren Save-Slots im UI an.
 * Verantwortung:
 * - Holt Save-Slots aus dem SaveSystem
 * - Erzeugt UI-Items für jeden Slot
 * - Reagiert auf Slot-Auswahl
 * - Öffnet vorhandene Spielstände oder startet leere Slots
 *
 * Abhängigkeiten:
 * - ITAA.System.Savegame.SaveSystem
 * - ITAA.System.Savegame.SaveSlotEntity
 * - ITAA.UI.Items.SaveSlotItemUI
 * - Unity UI / TMP optional je nach Statusanzeige
 *
 * Verwendet von:
 * - Startmenü / Load-Game-Menü
 */

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
        #region Inspector

        [Header("Save Slot Setup")]
        [SerializeField] private SaveSlotItemUI itemPrefab;
        [SerializeField] private Transform contentRoot;
        [SerializeField] private int slotCount = 3;

        [Header("Optional UI")]
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text infoText;
        [SerializeField] private Button refreshButton;
        [SerializeField] private Button closeButton;

        [Header("Scene Loading")]
        [SerializeField] private bool loadSceneOnSelection = true;
        [SerializeField] private string fallbackSceneName = "GameScene";

        #endregion

        #region Private Fields

        private readonly List<SaveSlotItemUI> spawnedItems = new List<SaveSlotItemUI>();
        private SaveSystem saveSystem;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            saveSystem = new SaveSystem();

            if (refreshButton != null)
            {
                refreshButton.onClick.RemoveListener(RefreshSlots);
                refreshButton.onClick.AddListener(RefreshSlots);
            }

            if (closeButton != null)
            {
                closeButton.onClick.RemoveListener(HidePanel);
                closeButton.onClick.AddListener(HidePanel);
            }
        }

        private void OnEnable()
        {
            RefreshSlots();
        }

        private void OnDestroy()
        {
            if (refreshButton != null)
            {
                refreshButton.onClick.RemoveListener(RefreshSlots);
            }

            if (closeButton != null)
            {
                closeButton.onClick.RemoveListener(HidePanel);
            }
        }

        #endregion

        #region Public Methods

        public void RefreshSlots()
        {
            if (!ValidateSetup())
            {
                return;
            }

            ClearItems();

            IReadOnlyList<SaveSlotEntity> slots = saveSystem.GetAllSlots(slotCount);

            for (int i = 0; i < slots.Count; i++)
            {
                SaveSlotItemUI item = Instantiate(itemPrefab, contentRoot);
                item.Setup(slots[i], HandleSlotSelected);
                spawnedItems.Add(item);
            }

            UpdateInfoText(slots);
        }

        public void ShowPanel()
        {
            gameObject.SetActive(true);
            RefreshSlots();
        }

        public void HidePanel()
        {
            gameObject.SetActive(false);
        }

        #endregion

        #region Private Methods

        private bool ValidateSetup()
        {
            if (itemPrefab == null)
            {
                Debug.LogError($"[{nameof(LoadGamePanel)}] Item Prefab ist nicht gesetzt.", this);
                return false;
            }

            if (contentRoot == null)
            {
                Debug.LogError($"[{nameof(LoadGamePanel)}] Content Root ist nicht gesetzt.", this);
                return false;
            }

            if (slotCount <= 0)
            {
                Debug.LogWarning($"[{nameof(LoadGamePanel)}] Slot Count war <= 0 und wird auf 1 gesetzt.", this);
                slotCount = 1;
            }

            if (titleText != null && string.IsNullOrWhiteSpace(titleText.text))
            {
                titleText.text = "Spielstand laden";
            }

            return true;
        }

        private void ClearItems()
        {
            for (int i = 0; i < spawnedItems.Count; i++)
            {
                if (spawnedItems[i] != null)
                {
                    Destroy(spawnedItems[i].gameObject);
                }
            }

            spawnedItems.Clear();

            for (int i = contentRoot.childCount - 1; i >= 0; i--)
            {
                Destroy(contentRoot.GetChild(i).gameObject);
            }
        }

        private void HandleSlotSelected(SaveSlotEntity slot)
        {
            if (slot == null)
            {
                Debug.LogWarning($"[{nameof(LoadGamePanel)}] Ausgewählter Slot ist null.", this);
                return;
            }

            Debug.Log(
                $"[{nameof(LoadGamePanel)}] Slot gewählt | Id: {slot.SlotId} | Name: {slot.DisplayName} | HasData: {slot.HasData} | Scene: {slot.SceneName}",
                this);

            if (!loadSceneOnSelection)
            {
                return;
            }

            string targetScene = ResolveTargetScene(slot);

            if (string.IsNullOrWhiteSpace(targetScene))
            {
                Debug.LogWarning($"[{nameof(LoadGamePanel)}] Keine gültige Zielszene für Slot {slot.SlotId} gefunden.", this);
                return;
            }

            SceneManager.LoadScene(targetScene);
        }

        private string ResolveTargetScene(SaveSlotEntity slot)
        {
            if (slot.HasData && !string.IsNullOrWhiteSpace(slot.SceneName))
            {
                return slot.SceneName;
            }

            return fallbackSceneName;
        }

        private void UpdateInfoText(IReadOnlyList<SaveSlotEntity> slots)
        {
            if (infoText == null)
            {
                return;
            }

            int usedSlots = 0;

            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i] != null && slots[i].HasData)
                {
                    usedSlots++;
                }
            }

            infoText.text = $"Belegte Slots: {usedSlots}/{slots.Count}";
        }

        #endregion
    }
}