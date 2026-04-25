# Settings Architektur

## Prinzipien
- Single Responsibility
- Zentrale Verwaltung über SettingsManager
- Lose Kopplung

## Komponenten

### SettingsManager
- Laden / Speichern
- Zugriffspunkt für alle Systeme

### SettingsData
- Reines Datenmodell
- Keine Logik

### SettingsUI
- Anzeige + Änderung

### InputRebinder
- Ändert Keybindings

## Zugriff

Beispiel:
SettingsManager.Instance.GetMasterVolume()

## Speicherort
Application.persistentDataPath/settings.json