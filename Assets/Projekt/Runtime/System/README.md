# 🧠 System

## Zweck
Globale Systeme und Querschnittsfunktionen.

## Verantwortlichkeiten
- zentrale Logik
- systemübergreifende Funktionen
- Settings-Persistenz unter `Settings/`

## Hinweis
Wird von mehreren Features genutzt.

## Settings
- `SettingsManager` laedt und speichert `settings.json` unter `Application.persistentDataPath`.
- `SettingsHotkeyController` macht Settings in der `StartScene` per `O` oder `F10` testbar.
- Wenn keine Settings-UI in der Szene vorhanden ist, erzeugt der Controller ein kleines Runtime-Panel.
