# Feature: Inventory / Toolbelt

## Ziel
Ein leichtgewichtiges Inventar- und Toolbelt-System fuer spaetere IT-Support-Werkzeuge vorbereiten.

## Namespace
- ITAA.Features.Inventory

## MVP-Idee
- Datenmodell fuer Items/Werkzeuge
- Runtime-Inventar fuer aufgenommene Items
- Toolbelt-Auswahl fuer schnelle Aktionen
- keine automatische UI-, Savegame- oder Scene-Anbindung im ersten Schritt

## Implementierter MVP-Stand
- `InventoryItemCategory`: einfache Item-Kategorien
- `InventoryItemData`: serialisierbares Item-Datenmodell
- `InventoryItemStack`: Item plus Menge mit Stack-Regeln
- `RuntimeInventory`: Add/Remove/Contains/Clear und Aenderungs-Event
- `ToolbeltController`: Slot-Zuweisung, Auswahl und Use-Event
- keine automatische UI-, Savegame- oder World-Interaction-Anbindung

## Anforderungen
- Bestehende Player-, UI- und Savegame-Systeme nicht brechen
- UI und Datenmodell getrennt halten
- Keine Scene-/Prefab-Aenderungen ohne Freigabe

## Offene Entscheidungen
- Item-Daten als ScriptableObject, JSON oder reine Runtime-Daten
- Savegame-Persistenz
- Integration mit Terminal, Scenario System und World Interaction
- konkrete InventoryPanel-Darstellung
