# Feature: Scenario System

## Ziel
Szenarien bündeln Aufgaben, Dialoge, Quizfragen und IT-Probleme zu spielbaren Lerneinheiten.

## Namespace
ITAA.Features.Scenarios

## Beispiele
- Kein Internet
- Drucker geht nicht
- VPN defekt
- DNS falsch konfiguriert
- IP-Konflikt
- Benutzer kann sich nicht anmelden

## Bestandteile eines Szenarios
- ScenarioId
- Titel
- Beschreibung
- Thema
- Schwierigkeit
- Startdialog
- Ziele
- benötigte Quizfragen
- Abschlussbedingung
- Ergebnisbewertung

## Komponenten

### ScenarioDefinition
Datenmodell für ein Szenario.

### ScenarioStep
Ein Schritt innerhalb eines Szenarios.

### ScenarioStepType
Einfache Einordnung fuer Objective-, Dialogue-, Quiz-, Task- und Checkpoint-Schritte.

### ScenarioFailureCause
Optionales Datenmodell fuer moegliche Fehlerursachen innerhalb eines Szenarios.

### ScenarioFailureCauseSelector
Helper fuer Zufallsauswahl sowie deterministische Auswahl per Index oder ID.

### ScenarioTimeLimit
Optionales Zeitlimit fuer ein gesamtes Szenario oder einen einzelnen Szenarioschritt.

### ScenarioTimerState
Laufzeitstatus fuer aktive Timer inklusive verbleibender Zeit, Warnschwelle und Timeout-Status.

### ScenarioManager
Startet, pausiert und beendet Szenarien.

### ScenarioProgress
Speichert aktuellen Fortschritt.

## MVP
- Szenario starten
- Ziel anzeigen
- Schritt abschließen
- Szenario beenden
- Debug.Log-Ausgabe

## Aktueller Mehrschritt-MVP
- `ScenarioStep` enthaelt jetzt Step-Typ, Completion-Key sowie optionale Quiz-, Dialog- und Quest-Hinweise.
- `ScenarioDefinition` kann Schritte per StepId, Completion-Key, QuizId oder DialogueId finden.
- `ScenarioManager` kann Schritte ueber diese IDs abschliessen, ohne Quiz, Dialog oder Progress direkt zu kennen.
- Optionale Schritte koennen separat als abgeschlossen markiert oder uebersprungen werden.
- `Progress01` stellt einfachen Laufzeitfortschritt fuer spaetere UI-/HUD-Anbindung bereit.

## Aktueller Fehlerursachen-MVP
- `ScenarioDefinition` kann optionale `FailureCauses` enthalten.
- `ScenarioManager` waehlt beim Start optional eine aktive Fehlerursache und speichert deren ID in `ScenarioProgress`.
- Fuer Tests und spaetere Tools gibt es deterministische Auswahl per Cause-ID oder aktivem Cause-Index.
- Das Demo-Szenario enthaelt Ursachen fuer DNS, DHCP/Gateway und Gateway-Erreichbarkeit.
- UI-Hinweise, Savegame-Persistenz und finale Gameplay-UX bleiben spaetere Integrationsschritte.

## Aktueller Zeitlimit-MVP
- `ScenarioDefinition` und `ScenarioStep` koennen optionale `ScenarioTimeLimit`-Daten halten.
- Der aktuelle Schritt-Timer hat Vorrang vor einem Szenario-Timer.
- `ScenarioManager` tickt den aktiven Timer im `Update`, meldet `TimerChanged` und `TimerTimedOut` und kann optional bei Timeout failen.
- `ScenarioProgress` haelt einen `ScenarioTimerState` fuer spaetere Savegame- und UI-Anbindung.
- Das Demo-Szenario enthaelt ein optionales 180-Sekunden-Zeitlimit fuer den DNS/DHCP-Pruefschritt.
- Pausenlogik, HUD-Anzeige und Savegame-Persistenz bleiben Folgearbeiten.

## Integration
- HUD zeigt aktuelles Ziel
- Quiz kann Szenarioschritt abschließen
- Dialogue kann Start-/Enddialog anzeigen
- ProgressManager speichert Abschluss
