/*
 * Datei: LoadGamePanel.cs
 * Zweck: Verwaltet die Darstellung des Load-Game-Panels und erzeugt Save-Slot-Einträge.
 * Verantwortung:
 *   - Erzeugt UI-Einträge für Save-Slots
 *   - Löscht alte Einträge vor dem Neuaufbau
 *   - Aktualisiert die Liste beim Öffnen des Panels
 *   - Unterstützt horizontale Scroll-Layouts über den Content-Container
 *
 * Abhängigkeiten:
 *   - Transform contentRoot
 *   - GameObject saveSlotItemPrefab
 *   - SaveSlotListItemUI
 *
 * Verwendet von:
 *   - MenuManager
 *   - StartMenuController
 *   - LoadGamePanel im UI
 */
// Datei: Assets/Projekt/Runtime/Features/UI/Panels/LoadGamePanel.cs

using System.Collections.Generic;
using ITAA.System.Savegame;
using ITAA.UI.Items;
using UnityEngine;

namespace ITAA.UI.Panels
{
    public class LoadGamePanel : BasePanel
    {
        #region Inspector

        [Header("References")]
        [SerializeField] private Transform contentRoot;
        [SerializeField] private GameObject saveSlotItemPrefab;

        [Header("Settings")]
        [SerializeField] private int slotCount = 5;

        #endregion

        #region Private Fields

        private SaveSystem saveSystem;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            saveSystem = new SaveSystem();
        }

        #endregion

        #region Public Methods

        public void Refresh()
        {
            RefreshSlots();
        }

        #endregion

        #region Protected Methods

        protected override void OnOpened()
        {
            RefreshSlots();
        }

        #endregion

        #region Private Methods

        private void RefreshSlots()
        {
            if (contentRoot == null)
            {
                Debug.LogWarning($"[{nameof(LoadGamePanel)}] ContentRoot ist nicht zugewiesen.");
                return;
            }

            if (saveSlotItemPrefab == null)
            {
                Debug.LogWarning($"[{nameof(LoadGamePanel)}] SaveSlotItemPrefab ist nicht zugewiesen.");
                return;
            }

            ClearSlots();

            IReadOnlyList<SaveSlotEntity> slots = saveSystem.GetAllSlots(slotCount);

            foreach (SaveSlotEntity slot in slots)
            {
                CreateSlot(slot);
            }
        }

        private void ClearSlots()
        {
            for (int i = contentRoot.childCount - 1; i >= 0; i--)
            {
                Destroy(contentRoot.GetChild(i).gameObject);
            }
        }

        private void CreateSlot(SaveSlotEntity slot)
        {
            GameObject instance = Instantiate(saveSlotItemPrefab, contentRoot);
            SaveSlotListItemUI itemUi = instance.GetComponent<SaveSlotListItemUI>();

            if (itemUi == null)
            {
                Debug.LogWarning(
                    $"[{nameof(LoadGamePanel)}] Prefab '{saveSlotItemPrefab.name}' hat keine {nameof(SaveSlotListItemUI)}-Komponente.");
                return;
            }

            itemUi.Setup(slot, HandleSlotSelected);
        }

        private void HandleSlotSelected(SaveSlotEntity slot)
        {
            if (slot == null)
            {
                return;
            }

            if (!slot.hasData)
            {
                Debug.Log($"[{nameof(LoadGamePanel)}] Slot {slot.slotId} ist leer.");
                return;
            }

            Debug.Log($"[{nameof(LoadGamePanel)}] Lade Slot {slot.slotId} aus Szene '{slot.sceneName}'.");
            // Hier später echte Ladelogik anbinden.
        }

        #endregion
    }
}