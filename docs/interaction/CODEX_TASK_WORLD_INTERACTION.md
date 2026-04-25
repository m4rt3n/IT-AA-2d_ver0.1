# Codex Task: World Interaction System implementieren

## Ziel
Implementiere ein wiederverwendbares World Interaction System.

## Kontext
Bitte vorher lesen:
- docs/FEATURE_REGISTRY.md
- docs/interaction/WORLD_INTERACTION_FEATURE.md

## Anforderungen
- Namespace: ITAA.Features.Interaction
- Keine Breaking Changes
- Bestehende Arthur-/Bernd-Systeme nicht entfernen
- Schrittweise Integration ermöglichen
- Unity Input System verwenden

## Zu erstellen
- IInteractable.cs
- InteractionDetector.cs
- InteractionController.cs
- InteractionPromptView.cs
- InteractionType.cs
- README.md im Feature-Ordner

## Bestehende Dateien prüfen/anpassen
- PlayerController.cs
- BerndAutoInteraction.cs
- ArthurAutoInteraction.cs
- MenuManager.cs falls nötig

## MVP-Verhalten
- Player erkennt ein Objekt mit IInteractable
- UI zeigt "E drücken"
- Bei E wird Interact() ausgeführt
- Wenn kein Objekt vorhanden ist, passiert nichts

## Wichtig
- Keine Doppelsteuerung mit bestehender BerndAutoInteraction erzeugen
- Falls Integration riskant ist, System zunächst parallel vorbereiten
- Vollständige Dateien liefern
- Header-Kommentare ergänzen