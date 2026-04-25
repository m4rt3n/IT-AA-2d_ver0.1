# Feature: Dialogue System

## Ziel
Ein wiederverwendbares Dialogsystem für NPCs.

NPCs sollen kurze Dialoge anzeigen können, z. B.:
- Begrüßung
- Erklärung vor einem Quiz
- Reaktion nach richtiger/falscher Antwort
- Hinweis auf nächste Aufgabe

## Namespace
ITAA.Features.Dialogue

## Anforderungen
- Dialoge unabhängig vom konkreten NPC
- Arthur und Bernd sollen das System verwenden können
- DialogPanel zeigt Text an
- Dialog kann per Button oder Taste weitergeschaltet werden
- Dialog kann am Ende eine Aktion auslösen

## Beispiele

### Bernd
Vor Quiz:
"Hallo, ich bin Bernd. Ich stelle dir ein paar IT-Support-Fragen."

Nach Quiz:
"Gut gemacht. Deine DNS-Grundlagen werden besser."

### Arthur
"Willkommen zurück. Lade deinen Spielstand oder starte neu."

## Komponenten

### DialogueLine
Ein einzelner Dialogsatz.

### DialogueSequence
Eine Liste von DialogueLines.

### DialogueManager
Startet und beendet Dialoge.

### DialoguePanel
Zeigt Text im UI an.

### IDialogueTrigger
Interface für Objekte, die Dialoge starten.

## Spätere Erweiterungen
- Portraits
- Schreibmaschinen-Effekt
- mehrere Antwortoptionen
- Bedingungen
- Lokalisierung DE/EN