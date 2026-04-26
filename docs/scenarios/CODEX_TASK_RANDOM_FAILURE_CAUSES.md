# Codex Task: Zufällige Fehlerursachen

## Kontext
Siehe:
- docs/scenarios/SCENARIO_SYSTEM_FEATURE.md

## Aufgabe
Bereite optionale zufaellige Fehlerursachen fuer Szenarien vor.

## Anforderungen
- Bestehende `ScenarioDefinition`-/`ScenarioManager`-Struktur nicht brechen
- Keine Scene-/Prefab-Aenderungen ohne ausdrueckliche Freigabe
- Deterministische Tests spaeter durch klare Auswahl-API ermoeglichen
- Eine Klasse pro Datei
- C#-Dateien mit Kopfkommentar
- README.md aktualisieren

## Erwartetes Ergebnis
- Einfaches Datenmodell oder Helper fuer moegliche Fehlerursachen
- Optionale Auswahl einer aktiven Ursache beim Szenariostart vorbereitet
- Demo-Szenario bleibt lauffaehig
- Tests/Pruefung dokumentieren

## Risiken
- Finale Gameplay-UX fuer Ursachenhinweise ist noch offen
- Savegame-Persistenz fuer aktive Ursachen ist noch nicht angebunden
