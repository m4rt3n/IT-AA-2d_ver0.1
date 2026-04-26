# Scenario System

## Zweck
Das Scenario System buendelt IT-Lernsituationen zu kleinen, spielbaren Einheiten. Es ist als MVP bewusst von UI, NPCs, Quiz und Savegame entkoppelt.

## Dateien
- `ScenarioDefinition.cs` definiert ein Szenario als Unity-Asset.
- `ScenarioFailureCause.cs` beschreibt eine optionale Ursache fuer ein Szenario.
- `ScenarioFailureCauseSelector.cs` waehlt Ursachen zufaellig oder deterministisch aus.
- `ScenarioTimeLimit.cs` beschreibt optionale Zeitlimits.
- `ScenarioTimerState.cs` speichert den Laufzeitstatus eines aktiven Timers.
- `ScenarioStep.cs` beschreibt einen einzelnen Schritt.
- `ScenarioStepType.cs` ordnet Schritte als Objective, Dialogue, Quiz, Task oder Checkpoint ein.
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
- Zeitlimit:
  - DNS/DHCP pruefen hat ein optionales 180-Sekunden-Zeitlimit mit 30-Sekunden-Warnschwelle.
- Optionale Fehlerursachen:
  - Falscher DNS-Server
  - Fehlendes Gateway per DHCP
  - Gateway nicht erreichbar

## Unity Setup
1. Leeres GameObject in der Szene anlegen, z. B. `ScenarioManager`.
2. `ScenarioManager`-Komponente hinzufuegen.
3. Optional ein `ScenarioDefinition`-Asset ueber `Create > IT-AA > Scenarios > Scenario Definition` erstellen und als `Default Scenario` zuweisen.
4. Fuer schnelle Tests kann `Use Demo Scenario If Missing` aktiv bleiben.
5. `Select Random Failure Cause On Start` kann aktiv bleiben, wenn beim Start eine Ursache automatisch ausgewaehlt werden soll.

## Integration
Das MVP schreibt nur Debug-Logs und stellt Events bereit. HUD, Quiz, Dialog und Progress-Speicherung koennen spaeter andocken, ohne dass das Scenario System sie direkt kennen muss.

Mehrstufige Ablaeufe koennen kontrolliert ueber folgende Methoden angebunden werden:
- `CompleteStepById`
- `CompleteStepByCompletionKey`
- `CompleteStepByLinkedQuiz`
- `CompleteStepByLinkedDialogue`
- `SkipCurrentOptionalStep`

Diese Methoden veraendern keine externen Systeme. Adapter aus Quiz, Dialog oder Progress koennen sie spaeter gezielt aufrufen.

Fehlerursachen koennen kontrolliert ueber folgende APIs genutzt werden:
- `ActiveFailureCause`
- `StartScenarioWithFailureCauseIndex`
- `StartScenarioWithFailureCauseId`

Die aktive Ursache wird aktuell nur im `ScenarioManager` und `ScenarioProgress` gehalten. Savegame-Persistenz, HUD-Anzeige und konkrete Spielerhinweise sind Folgearbeiten.

Zeitlimits koennen kontrolliert ueber folgende APIs genutzt werden:
- `ScenarioDefinition.TimeLimit`
- `ScenarioStep.TimeLimit`
- `ScenarioManager.Timer`
- `ScenarioManager.TimerChanged`
- `ScenarioManager.TimerTimedOut`
- `ScenarioManager.GetRemainingTimeSeconds`

Der aktuelle Schritt-Timer hat Vorrang vor einem Szenario-Timer. Timeouts werden standardmaessig nur gemeldet; `Fail Scenario On Timeout` kann im `ScenarioManager` optional aktiviert werden.
