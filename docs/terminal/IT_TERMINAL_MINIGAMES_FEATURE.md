# Feature: IT Terminal Minigames

## Ziel
Terminal-Minispiele simulieren einfache IT-Support-Aufgaben.

## Namespace
ITAA.Features.Terminal

## Beispiele
- ping ausführen
- ipconfig anzeigen
- DNS prüfen
- DHCP-Problem erkennen
- Gateway prüfen
- einfache Linux-Kommandos erklären

## MVP-Kommandos
- help
- ipconfig
- ping
- nslookup
- clear
- exit

## Lernziel
Spieler sollen typische Support-Diagnoseschritte üben:
- Ist IP vorhanden?
- Ist Gateway vorhanden?
- Funktioniert DNS?
- Ist Ziel erreichbar?

## Komponenten

### TerminalCommand
Datenmodell für einen Befehl.

### TerminalCommandResult
Ausgabe eines Befehls.

### TerminalEmulator
Verarbeitet Eingaben.

### TerminalPanel
UI für Eingabe und Ausgabe.

### TerminalScenarioBridge
Meldet Ergebnisse an ScenarioManager.

## Anforderungen
- Keine echten Systembefehle ausführen
- Alles wird simuliert
- Keine OS-Kommandos starten
- Keine Netzwerkzugriffe nötig

## MVP
- TerminalPanel öffnen
- Befehl eingeben
- simulierte Ausgabe anzeigen
- clear löscht Ausgabe
- exit schließt Panel