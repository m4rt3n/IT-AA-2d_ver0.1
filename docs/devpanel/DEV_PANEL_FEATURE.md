# Feature: DevPanel

## Ziel
Ein Entwickler-Panel für Debugging und Testdaten.

Das DevPanel soll während der Entwicklung helfen, häufige Aktionen direkt im Spiel auszulösen.

## Namespace
ITAA.DevTools

## Funktionen

### Savegame
- SaveSlots neu laden
- Dummy-SaveSlots erzeugen
- Save-Daten im Debug.Log ausgeben

### Settings
- Settings zurücksetzen
- aktuelle Settings anzeigen

### Quiz
- Dummy-Frage erzeugen
- Dummy-Draft-Frage erzeugen
- QuizPanel testen

### Player
- aktuelle PlayerSession anzeigen
- Spielernamen anzeigen
- aktuelle Szene anzeigen

## UI
Panel:
DevPanel

Buttons:
- Reload SaveSlots
- Generate Dummy Saves
- Reset Settings
- Generate Dummy Quiz Draft
- Print Player Session
- Print Current Scene
- Close DevPanel

## Aktivierung
Für MVP:
- Button im Startmenü
oder
- Tastenkürzel F12

## Regeln
- DevPanel darf keine Gameplay-Pflicht werden
- DevPanel muss optional sein
- DevPanel darf keine bestehenden Systeme brechen
- Methoden sollen defensiv prüfen, ob abhängige Systeme vorhanden sind