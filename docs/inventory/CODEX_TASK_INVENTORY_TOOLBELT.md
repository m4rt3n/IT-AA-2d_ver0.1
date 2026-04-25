# Codex Task: Inventory / Toolbelt vorbereiten

## Kontext
Siehe:
- docs/inventory/INVENTORY_TOOLBELT_FEATURE.md
- docs/core/FEATURE_REGISTRY.md

## Aufgabe
Bereite ein kleines Inventory-/Toolbelt-System als MVP vor.

## Anforderungen
- Namespace: ITAA.Features.Inventory
- Keine Breaking Changes an Player, UI oder Savegame
- Keine Scene-/Prefab-Aenderungen ohne explizite Freigabe
- Eine Klasse pro Datei
- C#-Dateien mit Kopfkommentar
- README.md aktualisieren

## Erwartetes Ergebnis
- Kleines Item-Datenmodell
- Runtime-Inventar fuer Items
- Toolbelt-Auswahl als vorbereitete Logik
- README-/Feature-Doku aktualisieren
- Tests/Pruefung dokumentieren

## Risiken
- Keine harte Kopplung an konkretes UI
- Savegame-Persistenz erst kontrolliert anbinden
- World Interaction spaeter nur ueber Adapter anbinden
