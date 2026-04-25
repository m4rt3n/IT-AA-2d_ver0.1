# Codex Task: NPC Routine System vorbereiten

## Kontext
Siehe:
- docs/npc/NPC_ROUTINE_FEATURE.md
- Assets/Projekt/Runtime/Features/NPC/Readme.md

## Aufgabe
Bereite ein kleines NPC Routine System als MVP vor.

## Anforderungen
- Namespace: ITAA.NPC.Routines
- Keine Breaking Changes an Arthur oder Bernd
- Keine Scene-/Prefab-Aenderungen ohne explizite Freigabe
- Bestehende NPC-Struktur beibehalten
- Eine Klasse pro Datei
- C#-Dateien mit Kopfkommentar

## Zu pruefen
- ArthurAutoInteraction.cs
- BerndAutoInteraction.cs
- BerndMovementToPlayer.cs
- ArthurMovementToPlayer.cs

## Erwartetes Ergebnis
- Kleines Datenmodell fuer Routine-Schritte
- Runtime-Komponente fuer einfache Sequenzen
- README-/Feature-Doku aktualisieren
- Tests/Pruefung dokumentieren

## Risiken
- Doppelte Bewegungslogik mit Arthur/Bernd vermeiden
- Keine Inspector-Referenzen brechen
- Routine-System erst anbinden, wenn die Szenenstruktur klar ist
