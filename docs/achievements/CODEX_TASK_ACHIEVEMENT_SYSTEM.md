# Codex Task: Achievement System vorbereiten

## Kontext
Siehe:
- docs/achievements/ACHIEVEMENT_FEATURE.md
- docs/core/FEATURE_REGISTRY.md

## Aufgabe
Bereite ein kleines Achievement-System als MVP vor.

## Anforderungen
- Namespace: ITAA.Features.Achievements
- Keine Breaking Changes an Progress-, Skill- oder Savegame-Systemen
- Keine Scene-/Prefab-Aenderungen ohne explizite Freigabe
- Eine Klasse pro Datei
- C#-Dateien mit Kopfkommentar
- README.md aktualisieren

## Erwartetes Ergebnis
- Achievement-Datenmodell
- Runtime-Status fuer Unlocks
- einfache Unlock-/Query-API
- README-/Feature-Doku aktualisieren
- Tests/Pruefung dokumentieren

## Risiken
- Keine doppelte Fortschrittslogik mit ProgressManager schaffen
- Savegame-Persistenz erst kontrolliert anbinden
- UI-Anbindung spaeter ueber Adapter oder View
