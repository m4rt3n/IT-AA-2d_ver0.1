# IT Terminal Minigames

## Zweck
Dieses Feature stellt ein rein simuliertes Terminal fuer IT-Support-Minispiele bereit.

## Namespace
`ITAA.Features.Terminal`

## Dateien
- `TerminalCommandType.cs` definiert die unterstuetzten Befehle.
- `TerminalCommand.cs` parst Nutzereingaben in Befehlsdaten.
- `TerminalCommandResult.cs` beschreibt Ausgaben und UI-Aktionen.
- `TerminalEmulator.cs` erzeugt deterministische, simulierte Antworten.
- `TerminalPanel.cs` zeigt Eingabe und Ausgabe im UI an.

## MVP-Befehle
- `help`
- `ipconfig`
- `ping <ziel>`
- `nslookup <name>`
- `clear`
- `exit`

## Sicherheit
Das Terminal fuehrt keine echten OS-Kommandos aus und startet keine Netzwerkzugriffe. Alle Antworten sind lokal simuliert.

## Unity Setup
1. Neues UI-Panel in einer Canvas anlegen.
2. `TerminalPanel` auf das Panel legen.
3. Optional eigene Referenzen fuer `OutputText`, `CommandInput`, `SubmitButton` und `CloseButton` setzen.
4. Alternativ `Create Missing Ui` aktiv lassen, dann erzeugt das Panel eine einfache MVP-UI selbst.
5. Panel ueber bestehende UI-Navigation oder DevPanel spaeter oeffnen.

## Follow-Ups
- Szenario-Bridge fuer Aufgaben wie DNS-/Gateway-Pruefung anbinden.
- Styling an finale UI-Sprache angleichen.
- Command-History und Lernfeedback ergaenzen.
