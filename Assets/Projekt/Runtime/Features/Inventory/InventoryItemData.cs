/*
 * Datei: InventoryItemData.cs
 * Zweck: Beschreibt ein Inventar-Item als leichtgewichtiges serialisierbares Datenmodell.
 * Verantwortung: Haelt ID, Anzeigename, Beschreibung, Kategorie und Stack-Regeln eines Items.
 * Abhaengigkeiten: InventoryItemCategory, Unity Sprite.
 * Verwendung: Wird von RuntimeInventory und ToolbeltController fuer Item-Stacks und Auswahl genutzt.
 */

using System;
using UnityEngine;

namespace ITAA.Features.Inventory
{
    [Serializable]
    public class InventoryItemData
    {
        public string ItemId = "item_id";
        public string DisplayName = "Item";
        [TextArea] public string Description;
        public InventoryItemCategory Category = InventoryItemCategory.Tool;
        public Sprite Icon;
        public bool IsStackable = true;
        [Min(1)] public int MaxStack = 1;

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(ItemId);
        }

        public int GetSafeMaxStack()
        {
            return Mathf.Max(1, MaxStack);
        }
    }
}
