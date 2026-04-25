# Inventory / Toolbelt

## Zweck
Dieses Feature bereitet ein leichtgewichtiges Inventar und einen Toolbelt fuer spaetere IT-Support-Werkzeuge vor.

## Namespace
`ITAA.Features.Inventory`

## Dateien
- `InventoryItemCategory.cs`: Kategorien fuer Items.
- `InventoryItemData.cs`: Serialisierbares Datenmodell fuer Items.
- `InventoryItemStack.cs`: Item plus Menge.
- `RuntimeInventory.cs`: Runtime-Inventar mit Add/Remove/Contains.
- `ToolbeltController.cs`: Slot-Zuweisung und Auswahl fuer schnelle Werkzeugnutzung.

## MVP-Grenzen
- Keine automatische UI-Anbindung.
- Keine Savegame-Persistenz.
- Keine Scene- oder Prefab-Aenderungen.
- Kein World-Interaction-Pickup-Adapter in diesem Schritt.

## Integration spaeter
- `InventoryPanel` kann die Daten aus `RuntimeInventory` anzeigen.
- World Interaction kann spaeter ueber einen Pickup-Adapter Items hinzufuegen.
- Savegame kann spaeter ItemIds und Mengen persistieren.
