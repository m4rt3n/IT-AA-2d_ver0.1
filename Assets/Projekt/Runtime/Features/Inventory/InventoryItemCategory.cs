/*
 * Datei: InventoryItemCategory.cs
 * Zweck: Definiert einfache Kategorien fuer Inventar- und Toolbelt-Items.
 * Verantwortung: Ermoeglicht eine grobe fachliche Einordnung von Werkzeugen, Verbrauchsitems und Quest-Objekten.
 * Abhaengigkeiten: Keine.
 * Verwendung: Wird von InventoryItemData genutzt und kann spaeter UI-Filter oder Toolbelt-Regeln unterstuetzen.
 */

namespace ITAA.Features.Inventory
{
    public enum InventoryItemCategory
    {
        Tool = 0,
        Consumable = 1,
        QuestItem = 2,
        Document = 3,
        Custom = 4
    }
}
