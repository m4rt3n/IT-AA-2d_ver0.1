# Codex Task: Multiple Loesungswege

## Kontext
Siehe:
- docs/scenarios/SCENARIO_SYSTEM_FEATURE.md

## Aufgabe
Bereite optionale multiple Loesungswege fuer Szenarien vor.

## Anforderungen
- Bestehende `ScenarioDefinition`-/`ScenarioManager`-Struktur nicht brechen
- Keine Scene-/Prefab-Aenderungen ohne ausdrueckliche Freigabe
- Keine harte Kopplung an UI, Quiz, Dialog, Knowledge Base oder Savegame
- Eine Klasse pro Datei
- C#-Dateien mit Kopfkommentar
- README.md aktualisieren

## Erwartetes Ergebnis
- Einfaches Datenmodell oder Helper fuer alternative Loesungswege
- ScenarioManager kann spaeter erkennen, welcher Weg erfolgreich war
- Demo-Szenario bleibt lauffaehig
- Tests/Pruefung dokumentieren

## Risiken
- Finale Bewertungslogik fuer unterschiedliche Wege ist offen
- UI-/HUD-Anzeige fuer alternative Wege ist noch nicht angebunden
- Savegame-Persistenz fuer gewaehlte Wege ist noch nicht umgesetzt
