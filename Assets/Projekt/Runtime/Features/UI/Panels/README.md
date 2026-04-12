# 🧭 UI Panels

## Zweck
Enthält alle konkreten UI-Panels des Projekts.

## Beispiele
- LoadGamePanel
- PauseMenuPanel
- SettingsPanel
- DialoguePanel

## Architektur
Alle Panels folgen einem einheitlichen Aufbau:
- Background
- Window
- Header
- Body
- Footer

## Code-Standard
Neue Panels erben nach Möglichkeit von `BasePanel`.

## Vorteil
- einheitliche Bedienung
- bessere Wartbarkeit
- einfache Erweiterung