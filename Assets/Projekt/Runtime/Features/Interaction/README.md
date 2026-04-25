# World Interaction System

## Zweck
Dieses Feature stellt ein wiederverwendbares MVP-System fuer Welt-Interaktionen bereit. Es soll langfristig NPCs, Objekte, Tueren, Terminals und Szenario-Elemente einheitlich ansprechbar machen.

## Namespace
`ITAA.Features.Interaction`

## Dateien
- `IInteractable.cs`: Schnittstelle fuer interaktive Ziele.
- `InteractionType.cs`: Einfache Typisierung von Interaktionen.
- `InteractionDetector.cs`: Erkennt interaktive Ziele ueber einen Trigger-Collider.
- `InteractionController.cs`: Liest Input und ruft `Interact()` aus.
- `InteractionPromptView.cs`: Zeigt den aktuellen Prompt an.

## MVP Setup
1. Auf den Player oder ein Player-Child:
   - `InteractionDetector`
   - `InteractionController`
   - `Collider2D` mit `Is Trigger`
2. Im `InteractionController`:
   - optional `Interact Action` aus dem Unity Input System zuweisen
   - alternativ bleibt der Tastatur-Fallback auf `E` aktiv
   - optional `Prompt View` zuweisen
3. Auf ein interaktives Ziel:
   - Eine Komponente implementiert `IInteractable`
   - Ein Collider2D muss vom Player-Trigger erreicht werden

## Integrationshinweis
Arthur und Bernd werden in diesem MVP noch nicht migriert. Ihre bestehenden Interaktionsklassen bleiben aktiv, damit keine doppelte E-Tasten-Steuerung oder doppelte Quiz-/Menue-Ausloesung entsteht.

## Naechste Schritte
- `BerndInteractableAdapter` auf Bernd testen und bei Bedarf in der StartScene anbinden.
- Bestehende Bernd-Eingabelogik danach kontrolliert abschalten.
- Arthur erst migrieren, wenn der Menue-Flow sauber ueber das neue System abgebildet ist.
