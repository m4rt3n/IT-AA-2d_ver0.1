# Feature: World Interaction System

## Ziel
Ein einheitliches Interaktionssystem für NPCs, Objekte, Türen, Terminals und spätere Szenario-Objekte.

## Namespace
ITAA.Features.Interaction

## Problem
Aktuell haben NPCs eigene Interaktionslogik.
Langfristig sollen Interaktionen einheitlich ablaufen.

## Interaktionstypen
- NPC sprechen
- Quiz starten
- Tür öffnen
- Terminal benutzen
- Objekt aufnehmen
- Hinweis anzeigen

## Komponenten

### IInteractable
Interface für alle interaktiven Objekte.

### InteractionPrompt
Zeigt z. B. "E drücken" an.

### InteractionDetector
Erkennt Interaktionsobjekte in Player-Nähe.

### InteractionController
Löst Interaktion aus.

## Anforderungen
- Unity Input System verwenden
- Keine Legacy Input API
- Bestehende Bernd-/Arthur-Interaktion nicht brechen
- Schrittweise Migration möglich

## MVP
- Player erkennt ein IInteractable
- Prompt wird angezeigt
- E löst Interact() aus
- Bernd kann optional darüber Quiz starten

## Spätere Erweiterungen
- Priorisierung bei mehreren Objekten
- Interaktionsradius
- Icon pro Interaktionstyp
- Controller-Unterstützung