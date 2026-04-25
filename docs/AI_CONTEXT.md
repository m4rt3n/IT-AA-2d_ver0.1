# Projektkontext (IT-AA 2D)

## Architektur
- Feature-driven
- Namespaces: ITAA.Core, ITAA.NPC, ITAA.UI, ITAA.System.Savegame

## Wichtige Systeme
- SaveSystem: JSON (save_slot_X.json)
- UI: MenuManager, LoadGamePanel, SaveSlotItemUI
- NPC: Arthur (Referenz), Bernd (Quiz-NPC)

## Aktuelle Probleme
1. SaveSlot zeigt keine Daten
2. SlotName wird nicht aus Datei geladen
3. GameScene nicht in Build Settings

## Erwartetes Verhalten
- Slots laden dynamisch aus JSON
- UI zeigt: Name, Scene, Zeit