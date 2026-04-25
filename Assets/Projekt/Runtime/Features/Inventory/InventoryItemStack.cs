/*
 * Datei: InventoryItemStack.cs
 * Zweck: Speichert ein Item mit Menge innerhalb eines Runtime-Inventars.
 * Verantwortung: Kapselt Stack-Regeln, Mengenanpassung und lesbare Zugriffsdaten.
 * Abhaengigkeiten: InventoryItemData, Mathf.
 * Verwendung: Wird von RuntimeInventory als einzelner Inventareintrag verwaltet.
 */

using System;
using UnityEngine;

namespace ITAA.Features.Inventory
{
    [Serializable]
    public class InventoryItemStack
    {
        [SerializeField] private InventoryItemData item;
        [SerializeField] private int quantity = 1;

        public InventoryItemData Item => item;
        public int Quantity => quantity;
        public bool IsEmpty => item == null || quantity <= 0 || !item.IsValid();
        public string ItemId => item != null ? item.ItemId : string.Empty;

        public InventoryItemStack()
        {
        }

        public InventoryItemStack(InventoryItemData itemData, int amount)
        {
            item = itemData;
            quantity = Mathf.Max(0, amount);
            ClampQuantity();
        }

        public bool CanStackWith(InventoryItemData itemData)
        {
            if (IsEmpty || itemData == null || !itemData.IsValid())
            {
                return false;
            }

            return item.IsStackable &&
                   item.ItemId == itemData.ItemId &&
                   quantity < item.GetSafeMaxStack();
        }

        public int AddQuantity(int amount)
        {
            if (amount <= 0 || item == null)
            {
                return amount;
            }

            int capacity = item.GetSafeMaxStack() - quantity;
            int accepted = Mathf.Min(capacity, amount);
            quantity += accepted;
            return amount - accepted;
        }

        public int RemoveQuantity(int amount)
        {
            if (amount <= 0)
            {
                return 0;
            }

            int removed = Mathf.Min(quantity, amount);
            quantity -= removed;
            return removed;
        }

        private void ClampQuantity()
        {
            if (item == null)
            {
                quantity = 0;
                return;
            }

            quantity = Mathf.Clamp(quantity, 0, item.GetSafeMaxStack());
        }
    }
}
