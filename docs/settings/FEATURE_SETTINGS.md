# Feature: Settings System

## Ziel
Zentrales Einstellungs-System für:
- Audio
- Video
- Input
- Gameplay

## Anforderungen

### Audio
- MasterVolume (0–1)
- MusicVolume (0–1)
- SFXVolume (0–1)

### Video
- Fullscreen (bool)
- Resolution (width/height)
- VSync (bool)

### Input
- Rebind von Tasten (E, WASD)
- Speicherung pro Aktion

### Gameplay
- TextSpeed (slow/normal/fast)
- ShowTutorials (bool)

## Persistenz
- Speicherung als JSON
- Datei: settings.json

## Integration
- Wird beim Spielstart geladen
- UI greift zentral über SettingsManager zu

## Architektur
- SettingsManager (Singleton)
- SettingsData (Model)
- SettingsUI (Panel)