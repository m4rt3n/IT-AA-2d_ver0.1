# Codex Task: Dialogue System implementieren

## Ziel
Implementiere ein einfaches, wiederverwendbares Dialogsystem.

## Kontext
Bitte vorher lesen:
- docs/FEATURE_REGISTRY.md
- docs/dialogue/DIALOGUE_FEATURE.md

## Anforderungen
- Namespace: ITAA.Features.Dialogue
- Keine Breaking Changes
- Bernd und Arthur später anschließbar
- UI und Datenmodell trennen
- Keine feste Kopplung an Bernd

## Zu erstellen
- DialogueLine.cs
- DialogueSequence.cs
- DialogueManager.cs
- DialoguePanel.cs
- IDialogueTrigger.cs

## Bestehende Dateien prüfen/anpassen
- BerndAutoInteraction.cs
- BerndQuizStarter.cs
- ArthurAutoInteraction.cs
- MenuManager.cs falls nötig

## MVP-Verhalten
- NPC startet Dialog
- DialogPanel zeigt Text
- Button "Weiter"
- Am Ende wird Dialog geschlossen
- Optionaler Callback nach Dialogende

## Wichtig
- Vollständige Dateien liefern
- Header-Kommentare ergänzen
- Kein neues NPC-System bauen
- Bestehende Interaktionslogik nicht brechen