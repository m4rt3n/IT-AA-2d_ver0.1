# Scenario System

## Zweck
Das Scenario System buendelt IT-Lernsituationen zu kleinen, spielbaren Einheiten. Es ist als MVP bewusst von UI, NPCs, Quiz und Savegame entkoppelt.

## Dateien
- `ScenarioDefinition.cs` definiert ein Szenario als Unity-Asset.
- `ScenarioStep.cs` beschreibt einen einzelnen Schritt.
- `ScenarioProgress.cs` haelt den aktuellen Laufzeitfortschritt.
- `ScenarioManager.cs` startet Szenarien, schliesst Schritte ab und meldet Statuswechsel.
- `ScenarioStatus.cs` enthaelt die moeglichen Laufzeitstatus.

## Demo
Wenn im `ScenarioManager` kein `defaultScenario` gesetzt ist und `useDemoScenarioIfMissing` aktiv bleibt, erzeugt der Manager zur Laufzeit das Demo-Szenario:

- ID: `no_internet_basic`
- Titel: `Kein Internet`
- Thema: `Netzwerk`
- Schwierigkeit: `Easy`
- Schritte:
  1. Problem aufnehmen
  2. DNS/DHCP pruefen
  3. Loesung bestaetigen

## Unity Setup
1. Leeres GameObject in der Szene anlegen, z. B. `ScenarioManager`.
2. `ScenarioManager`-Komponente hinzufuegen.
3. Optional ein `ScenarioDefinition`-Asset ueber `Create > IT-AA > Scenarios > Scenario Definition` erstellen und als `Default Scenario` zuweisen.
4. Fuer schnelle Tests kann `Use Demo Scenario If Missing` aktiv bleiben.

## Integration
Das MVP schreibt nur Debug-Logs und stellt Events bereit. HUD, Quiz, Dialog und Progress-Speicherung koennen spaeter andocken, ohne dass das Scenario System sie direkt kennen muss.
