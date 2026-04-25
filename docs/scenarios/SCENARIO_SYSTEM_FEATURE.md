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

## Integration
- HUD zeigt aktuelles Ziel
- Quiz kann Szenarioschritt abschließen
- Dialogue kann Start-/Enddialog anzeigen
- ProgressManager speichert Abschluss