# Codex Task: HUD System implementieren

## Ziel
Implementiere ein einfaches HUD-System.

## Kontext
Bitte vorher lesen:
- docs/FEATURE_REGISTRY.md
- docs/hud/HUD_FEATURE.md

## Anforderungen
- Namespace: ITAA.Features.HUD
- Keine Breaking Changes
- StartScene weiter nutzbar lassen
- Bestehende PlayerSession verwenden, falls vorhanden
- Bestehende Quiz-/Progress-Systeme nur anbinden, wenn vorhanden

## Zu erstellen
- HudController.cs
- HudView.cs
- HudNotification.cs
- README.md im Feature-Ordner

## Bestehende Dateien prüfen/anpassen
- MenuManager.cs
- QuizPanel.cs
- PlayerSession.cs
- StartScene.unity falls nötig

## Verhalten
- HUD kann Spielername anzeigen
- HUD kann aktuelles Ziel anzeigen
- HUD kann Quizpunkte anzeigen
- HUD kann kurze Debug-/Info-Meldung anzeigen

## Wichtig
- Fehlende Systeme mit Debug.LogWarning behandeln
- Keine harte Kopplung an Bernd
- Vollständige Dateien liefern
- Header-Kommentare ergänzen