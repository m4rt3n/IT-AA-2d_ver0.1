# Dialogue System

## Zweck
Das Dialogue System ist ein wiederverwendbares MVP fuer kurze NPC-Dialoge. Es ist nicht an Arthur oder Bernd gekoppelt und kann spaeter von beiden NPCs, Quiz oder Szenarien gestartet werden.

## Dateien
- `DialogueLine.cs` beschreibt eine einzelne Dialogzeile.
- `DialogueSequence.cs` speichert mehrere Dialogzeilen als Unity-Asset.
- `DialogueManager.cs` startet, schaltet weiter und beendet Dialoge.
- `DialoguePanel.cs` zeigt Sprecher, Text und einen Weiter-/Schliessen-Button.
- `IDialogueTrigger.cs` ist ein Interface fuer spaetere NPC- oder Objekt-Adapter.

## Unity Setup
1. Ein UI-GameObject in einem Canvas anlegen.
2. `DialoguePanel` hinzufuegen.
3. Optional `Create Missing Ui` aktiv lassen, damit das Panel seine MVP-UI selbst erzeugt.
4. Ein GameObject `DialogueManager` in die Szene legen.
5. `DialogueManager` hinzufuegen und optional das `DialoguePanel` zuweisen.
6. Dialogdaten ueber `Create > IT-AA > Dialogue > Dialogue Sequence` erstellen.

## Verhalten
- `DialogueManager.StartDialogue(sequence, callback)` startet eine Sequenz.
- `DialoguePanel` zeigt die aktuelle Zeile.
- Button `Weiter` zeigt die naechste Zeile.
- Beim letzten Klick wird das Panel geschlossen und der optionale Callback ausgefuehrt.

## Integration
Arthur und Bernd werden in diesem MVP nicht automatisch migriert. Spaeter koennen kleine Adapter `IDialogueTrigger` implementieren und den `DialogueManager` aufrufen, ohne die bestehenden NPC-Systeme zu ersetzen.
