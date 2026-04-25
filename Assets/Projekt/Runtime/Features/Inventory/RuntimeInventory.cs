/*
 * Datei: RuntimeInventory.cs
 * Zweck: Verwaltet ein leichtgewichtiges Runtime-Inventar fuer aufgenommene Items.
 * Verantwortung: Fuegt Items hinzu, entfernt Mengen, prueft Bestand und meldet Aenderungen.
 * Abhaengigkeiten: InventoryItemData, InventoryItemStack, Unity MonoBehaviour.
 * Verwendung: Kann spaeter auf Player- oder Manager-Objekten eingesetzt und von UI, Interaction oder Save-Adaptern gelesen werden.
 */

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ITAA.Features.Inventory
{
    [DisallowMultipleComponent]
    public class RuntimeInventory : MonoBehaviour
    {
        [Header("Inventory")]
        [SerializeField] private int capacity = 16;
        [SerializeField] private List<InventoryItemStack> items = new List<InventoryItemStack>();

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs;

        public event Action RuntimeInventoryChanged;

        public int Capacity => Mathf.Max(1, capacity);
        public int Count => items.Count;

        public IReadOnlyList<InventoryItemStack> Items => items;

        public bool AddItem(InventoryItemData itemData, int amount = 1)
        {
            if (itemData == null || !itemData.IsValid() || amount <= 0)
            {
                return false;
            }

            int remaining = amount;

            for (int i = 0; i < items.Count && remaining > 0; i++)
            {
                InventoryItemStack stack = items[i];

                if (stack != null && stack.CanStackWith(itemData))
                {
                    remaining = stack.AddQuantity(remaining);
                }
            }

            while (remaining > 0 && items.Count < Capacity)
            {
                int stackAmount = itemData.IsStackable
                    ? Mathf.Min(remaining, itemData.GetSafeMaxStack())
                    : 1;

                items.Add(new InventoryItemStack(itemData, stackAmount));
                remaining -= stackAmount;
            }

            bool changed = remaining != amount;

            if (changed)
            {
                RemoveEmptyStacks();
                NotifyChanged();
                Log($"Item hinzugefuegt: {itemData.ItemId}, Menge={amount - remaining}");
            }

            return remaining <= 0;
        }

        public bool RemoveItem(string itemId, int amount = 1)
        {
            if (string.IsNullOrWhiteSpace(itemId) || amount <= 0)
            {
                return false;
            }

            int remaining = amount;

            for (int i = items.Count - 1; i >= 0 && remaining > 0; i--)
            {
                InventoryItemStack stack = items[i];

                if (stack == null || stack.IsEmpty || stack.ItemId != itemId)
                {
                    continue;
                }

                int removed = stack.RemoveQuantity(remaining);
                remaining -= removed;
            }

            bool changed = remaining != amount;

            if (changed)
            {
                RemoveEmptyStacks();
                NotifyChanged();
                Log($"Item entfernt: {itemId}, Menge={amount - remaining}");
            }

            return remaining <= 0;
        }

        public bool ContainsItem(string itemId, int requiredAmount = 1)
        {
            return GetQuantity(itemId) >= Mathf.Max(1, requiredAmount);
        }

        public int GetQuantity(string itemId)
        {
            if (string.IsNullOrWhiteSpace(itemId))
            {
                return 0;
            }

            int total = 0;

            for (int i = 0; i < items.Count; i++)
            {
                InventoryItemStack stack = items[i];

                if (stack != null && !stack.IsEmpty && stack.ItemId == itemId)
                {
                    total += stack.Quantity;
                }
            }

            return total;
        }

        public InventoryItemStack FindFirstStack(string itemId)
        {
            if (string.IsNullOrWhiteSpace(itemId))
            {
                return null;
            }

            for (int i = 0; i < items.Count; i++)
            {
                InventoryItemStack stack = items[i];

                if (stack != null && !stack.IsEmpty && stack.ItemId == itemId)
                {
                    return stack;
                }
            }

            return null;
        }

        public void Clear()
        {
            if (items.Count == 0)
            {
                return;
            }

            items.Clear();
            NotifyChanged();
        }

        private void RemoveEmptyStacks()
        {
            for (int i = items.Count - 1; i >= 0; i--)
            {
                if (items[i] == null || items[i].IsEmpty)
                {
                    items.RemoveAt(i);
                }
            }
        }

        private void NotifyChanged()
        {
            RuntimeInventoryChanged?.Invoke();
        }

        private void Log(string message)
        {
            if (!enableDebugLogs)
            {
                return;
            }

            Debug.Log($"[{nameof(RuntimeInventory)}] {message}", this);
        }
    }
}
