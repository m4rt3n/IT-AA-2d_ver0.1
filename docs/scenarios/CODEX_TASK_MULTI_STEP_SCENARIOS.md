# Codex Task: Mehrstufige Szenarien

## Kontext
Siehe:
- docs/scenarios/SCENARIO_SYSTEM_FEATURE.md
- docs/quiz/QUIZ_FEATURE.md
- docs/quest/QUEST_PROGRESS_FEATURE.md

## Aufgabe
Bereite mehrstufige Szenarien im bestehenden Scenario-System weiter vor.

## Anforderungen
- Bestehende `ScenarioDefinition`-/`ScenarioManager`-Struktur nicht brechen
- Keine Scene-/Prefab-Aenderungen ohne ausdrueckliche Freigabe
- Keine harte Kopplung an Quiz, Dialog, HUD oder Savegame erzwingen
- Eine Klasse pro Datei
- C#-Dateien mit Kopfkommentar
- README.md aktualisieren

## Erwartetes Ergebnis
- Kleine optionale Erweiterung fuer mehrstufige Szenario-Abläufe
- Bestehendes Demo-Szenario bleibt lauffaehig
- Anschluss fuer spaetere Quiz-/Dialog-/Progress-Integration vorbereitet
- Tests/Pruefung dokumentieren

## Risiken
- Finale UX fuer Szenariofortschritt ist noch offen
- Savegame-Persistenz fuer Szenariofortschritt ist noch nicht angebunden
