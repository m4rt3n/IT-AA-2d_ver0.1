# Codex Task: DevPanel implementieren

## Ziel
Implementiere ein einfaches DevPanel für Debug- und Testfunktionen.

## Kontext
Bitte vorher lesen:
- docs/FEATURE_REGISTRY.md
- docs/devpanel/DEV_PANEL_FEATURE.md

## Anforderungen
- Namespace: ITAA.DevTools
- Keine Breaking Changes
- Bestehende Systeme verwenden
- Kein neues SaveSystem bauen
- Kein neues QuizSystem bauen

## Zu erstellen
- DevPanelController.cs
- DevPanelBootstrap.cs optional
- README.md im Ordner Assets/Projekt/Runtime/Features/DevTools/

## Bestehende Dateien prüfen/anpassen
- MenuManager.cs
- StartScene.unity falls nötig
- LoadGamePanel.cs falls Reload-Methode fehlt

## Funktionen
Das Panel soll Buttons anbieten für:

1. Reload SaveSlots
2. Generate Dummy Saves
3. Reset Settings
4. Generate Dummy Quiz Draft
5. Print Player Session
6. Print Current Scene
7. Close DevPanel

## Wichtig
- Wenn ein System noch nicht existiert, Stub/Platzhalter sauber vorbereiten
- Fehlende Systeme nur mit Debug.LogWarning melden
- Keine Compile Errors erzeugen
- Alle neuen Klassen mit Header-Kommentar
- Vollständige Dateien liefern, keine Snippets