/*
 * Datei: ToolbeltController.cs
 * Zweck: Verwaltet eine einfache Toolbelt-Auswahl fuer Items aus einem Runtime-Inventar.
 * Verantwortung: Speichert Slot-Zuweisungen, Auswahlindex und meldet Use-/Selection-Events.
 * Abhaengigkeiten: RuntimeInventory, InventoryItemStack, Unity MonoBehaviour.
 * Verwendung: Kann spaeter von UI, Input oder Gameplay-Systemen genutzt werden, ohne direkt an ein konkretes Panel gekoppelt zu sein.
 */

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ITAA.Features.Inventory
{
    [DisallowMultipleComponent]
    public class ToolbeltController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RuntimeInventory inventory;

        [Header("Toolbelt")]
        [SerializeField] private int slotCount = 4;
        [SerializeField] private List<string> assignedItemIds = new List<string>();
        [SerializeField] private int selectedSlotIndex;

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs;

        public event Action<int, string> SelectedSlotChanged;
        public event Action<int, string> ToolbeltSlotUsed;

        public int SlotCount => Mathf.Max(1, slotCount);
        public int SelectedSlotIndex => selectedSlotIndex;

        private void Awake()
        {
            if (inventory == null)
            {
                inventory = GetComponent<RuntimeInventory>();
            }

            EnsureSlotList();
            selectedSlotIndex = Mathf.Clamp(selectedSlotIndex, 0, SlotCount - 1);
        }

        public bool AssignItemToSlot(int slotIndex, string itemId)
        {
            EnsureSlotList();

            if (!IsValidSlot(slotIndex) || string.IsNullOrWhiteSpace(itemId))
            {
                return false;
            }

            if (inventory != null && !inventory.ContainsItem(itemId))
            {
                Log($"Zuweisung abgelehnt, Item nicht im Inventar: {itemId}");
                return false;
            }

            assignedItemIds[slotIndex] = itemId.Trim();

            if (slotIndex == selectedSlotIndex)
            {
                NotifySelectedSlotChanged();
            }

            return true;
        }

        public void ClearSlot(int slotIndex)
        {
            EnsureSlotList();

            if (!IsValidSlot(slotIndex))
            {
                return;
            }

            assignedItemIds[slotIndex] = string.Empty;

            if (slotIndex == selectedSlotIndex)
            {
                NotifySelectedSlotChanged();
            }
        }

        public void SelectSlot(int slotIndex)
        {
            EnsureSlotList();

            if (!IsValidSlot(slotIndex))
            {
                return;
            }

            if (selectedSlotIndex == slotIndex)
            {
                NotifySelectedSlotChanged();
                return;
            }

            selectedSlotIndex = slotIndex;
            NotifySelectedSlotChanged();
        }

        public void SelectNextSlot()
        {
            EnsureSlotList();
            SelectSlot((selectedSlotIndex + 1) % SlotCount);
        }

        public void SelectPreviousSlot()
        {
            EnsureSlotList();
            int nextIndex = selectedSlotIndex - 1;

            if (nextIndex < 0)
            {
                nextIndex = SlotCount - 1;
            }

            SelectSlot(nextIndex);
        }

        public bool TryUseSelectedSlot()
        {
            string itemId = GetAssignedItemId(selectedSlotIndex);

            if (string.IsNullOrWhiteSpace(itemId))
            {
                return false;
            }

            if (inventory != null && !inventory.ContainsItem(itemId))
            {
                ClearSlot(selectedSlotIndex);
                return false;
            }

            ToolbeltSlotUsed?.Invoke(selectedSlotIndex, itemId);
            Log($"Toolbelt-Slot genutzt: {selectedSlotIndex}, Item={itemId}");
            return true;
        }

        public string GetAssignedItemId(int slotIndex)
        {
            EnsureSlotList();

            if (!IsValidSlot(slotIndex))
            {
                return string.Empty;
            }

            return assignedItemIds[slotIndex];
        }

        public InventoryItemStack GetSelectedStack()
        {
            if (inventory == null)
            {
                return null;
            }

            string itemId = GetAssignedItemId(selectedSlotIndex);
            return inventory.FindFirstStack(itemId);
        }

        private void EnsureSlotList()
        {
            int safeSlotCount = SlotCount;

            while (assignedItemIds.Count < safeSlotCount)
            {
                assignedItemIds.Add(string.Empty);
            }

            while (assignedItemIds.Count > safeSlotCount)
            {
                assignedItemIds.RemoveAt(assignedItemIds.Count - 1);
            }
        }

        private bool IsValidSlot(int slotIndex)
        {
            return slotIndex >= 0 && slotIndex < SlotCount;
        }

        private void NotifySelectedSlotChanged()
        {
            SelectedSlotChanged?.Invoke(selectedSlotIndex, GetAssignedItemId(selectedSlotIndex));
        }

        private void Log(string message)
        {
            if (!enableDebugLogs)
            {
                return;
            }

            Debug.Log($"[{nameof(ToolbeltController)}] {message}", this);
        }
    }
}
