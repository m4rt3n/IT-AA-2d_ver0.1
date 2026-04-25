# Codex Task: Skill / Level System vorbereiten

## Kontext
Siehe:
- docs/skills/SKILL_LEVEL_FEATURE.md
- docs/core/FEATURE_REGISTRY.md

## Aufgabe
Bereite ein kleines Skill-/Level-System als MVP vor.

## Anforderungen
- Namespace: ITAA.Features.Skills
- Keine Breaking Changes an PlayerSession oder Progress-System
- Keine Scene-/Prefab-Aenderungen ohne explizite Freigabe
- Eine Klasse pro Datei
- C#-Dateien mit Kopfkommentar
- README.md aktualisieren

## Erwartetes Ergebnis
- Skill-Datenmodell
- Runtime-Profil fuer XP und Level
- einfache XP-Vergabe und Level-Up-Regeln
- README-/Feature-Doku aktualisieren
- Tests/Pruefung dokumentieren

## Risiken
- Keine doppelte Fortschrittsquelle mit ProgressManager schaffen
- Savegame-Persistenz erst kontrolliert anbinden
- UI-Anbindung spaeter ueber Adapter oder View
