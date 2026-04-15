/*
 * Datei: LoadGamePanel.cs
 * Pfad: Assets/Projekt/Runtime/Features/UI/Panels/LoadGamePanel.cs
 * Zweck: Baut die Ladeansicht für Spielstände auf und verarbeitet die Slot-Auswahl.
 * Verantwortung:
 * - Lädt alle vorhandenen Save-Slots aus dem SaveSystem
 * - Erzeugt UI-Einträge für jeden Slot
 * - Reagiert auf die Auswahl eines Slots
 * - Lädt die Zielszene des ausgewählten Speicherstands
 */

using System.Collections.Generic;
using ITAA.System.Savegame;
using ITAA.UI.Items;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ITAA.UI.Panels
{
    public class LoadGamePanel : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private int slotCount = 3;

        [Header("References")]
        [SerializeField] private Transform contentRoot;
        [SerializeField] private SaveSlotListItemUI slotItemPrefab;

        private readonly List<SaveSlotListItemUI> spawnedItems = new List<SaveSlotListItemUI>();
        private SaveSystem saveSystem;

        private void Awake()
        {
            saveSystem = new SaveSystem();
        }

        private void OnEnable()
        {
            Refresh();
        }

        public void Refresh()
        {
            if (contentRoot == null)
            {
                Debug.LogError("LoadGamePanel: contentRoot ist nicht zugewiesen.");
                return;
            }

            if (slotItemPrefab == null)
            {
                Debug.LogError("LoadGamePanel: slotItemPrefab ist nicht zugewiesen.");
                return;
            }

            ClearItems();

            IReadOnlyList<SaveSlotEntity> slots = saveSystem.GetAllSlots(slotCount);

            for (int i = 0; i < slots.Count; i++)
            {
                SaveSlotListItemUI item = Instantiate(slotItemPrefab, contentRoot);
                item.Setup(slots[i], HandleSlotSelected);
                spawnedItems.Add(item);
            }
        }

        private void HandleSlotSelected(SaveSlotEntity slot)
        {
            if (slot == null)
            {
                Debug.LogWarning("LoadGamePanel: Ausgewählter Slot ist null.");
                return;
            }

            if (!slot.HasData)
            {
                Debug.Log("LoadGamePanel: Leerer Slot wurde ausgewählt.");
                return;
            }

            SaveGameData data = saveSystem.Load(slot.SlotId);

            if (data == null)
            {
                Debug.LogWarning($"LoadGamePanel: Kein SaveGameData für Slot {slot.SlotId} gefunden.");
                Refresh();
                return;
            }

            if (string.IsNullOrWhiteSpace(data.SceneName))
            {
                Debug.LogWarning($"LoadGamePanel: Slot {slot.SlotId} enthält keinen gültigen Szenennamen.");
                return;
            }

            SceneManager.LoadScene(data.SceneName);
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
        }
    }
}