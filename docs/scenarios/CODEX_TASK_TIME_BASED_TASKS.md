# Codex Task: Zeitbasierte Aufgaben

## Kontext
Siehe:
- docs/scenarios/SCENARIO_SYSTEM_FEATURE.md

## Aufgabe
Bereite optionale zeitbasierte Aufgaben fuer Szenarien vor.

## Anforderungen
- Bestehende `ScenarioDefinition`-/`ScenarioManager`-Struktur nicht brechen
- Keine Scene-/Prefab-Aenderungen ohne ausdrueckliche Freigabe
- Zeitlogik defensiv halten und nicht an UI, HUD oder Savegame koppeln
- Eine Klasse pro Datei
- C#-Dateien mit Kopfkommentar
- README.md aktualisieren

## Erwartetes Ergebnis
- Einfaches Datenmodell oder Helper fuer optionale Zeitlimits je Szenario oder Schritt
- Laufzeitstatus kann verbleibende Zeit oder Timeout vorbereitet melden
- Demo-Szenario bleibt lauffaehig
- Tests/Pruefung dokumentieren

## Risiken
- Finale Gameplay-UX fuer Timer ist offen
- Savegame-Persistenz fuer laufende Timer ist noch nicht angebunden
- Pausen-/Menuestatus muss spaeter eindeutig definiert werden
