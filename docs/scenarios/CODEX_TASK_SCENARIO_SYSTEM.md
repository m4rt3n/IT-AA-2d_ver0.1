# Codex Task: Scenario System implementieren

## Ziel
Implementiere ein einfaches Scenario System für IT-Lernsituationen.

## Kontext
Bitte vorher lesen:
- docs/FEATURE_REGISTRY.md
- docs/scenarios/SCENARIO_SYSTEM_FEATURE.md

## Anforderungen
- Namespace: ITAA.Features.Scenarios
- Keine Breaking Changes
- Keine harte Kopplung an Bernd
- HUD/Quiz/Dialog nur anbinden, wenn vorhanden

## Zu erstellen
- ScenarioDefinition.cs
- ScenarioStep.cs
- ScenarioProgress.cs
- ScenarioManager.cs
- ScenarioStatus.cs
- README.md im Feature-Ordner

## Demo-Szenario
Erstelle ein Demo-Szenario:
- ID: no_internet_basic
- Titel: Kein Internet
- Thema: Netzwerk
- Schwierigkeit: Easy
- Schritte:
  1. Problem aufnehmen
  2. DNS/DHCP prüfen
  3. Lösung bestätigen

## Verhalten
- Szenario starten
- aktuellen Schritt anzeigen
- Schritt abschließen
- Szenario abschließen
- Debug.Log bei Statuswechsel

## Wichtig
- Datenmodell sauber von UI trennen
- JSON-/ScriptableObject-Persistenz nur vorbereiten
- Vollständige Dateien liefern
- Header-Kommentare ergänzen