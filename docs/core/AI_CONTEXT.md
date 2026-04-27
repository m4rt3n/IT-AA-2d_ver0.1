# Projektkontext (IT-AA 2D)

## Architektur
- Feature-driven
- Namespaces: ITAA.Core, ITAA.NPC, ITAA.UI, ITAA.System.Savegame

## Wichtige Systeme
- SaveSystem: JSON (save_slot_X.json)
- UI: MenuManager, LoadGamePanel, SaveSlotItemUI
- NPC: Arthur (Referenz), Bernd (Quiz-NPC)

## Aktuelle Probleme
1. SaveSlot-/LoadGamePanel-Flows weiter pruefen, wenn echte Save-Dateien genutzt werden
2. GameScene ist bewusst entfernt und nicht in Build Settings
3. Neue GameScene soll spaeter modular und coordinate-driven neu aufgebaut werden

## Erwartetes Verhalten
- Slots laden dynamisch aus JSON
- UI zeigt: Name, Scene, Zeit
- StartScene bleibt aktuell Einstiegspunkt und sicherer Save-Fallback
