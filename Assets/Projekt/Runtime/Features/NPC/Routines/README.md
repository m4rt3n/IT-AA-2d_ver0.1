# NPC Routine System

## Zweck
Das Routine-System ist ein kleines, optionales MVP fuer einfache NPC-Ablaufplaene. Es ist nicht automatisch mit Arthur oder Bernd verbunden und veraendert deren bestehende Interaktionslogik nicht.

## Namespace
`ITAA.NPC.Routines`

## Dateien
- `NpcRoutineStepType.cs`: Schrittarten fuer Routinen.
- `NpcRoutineStep.cs`: Serialisierbares Datenmodell fuer einen Routine-Schritt.
- `NpcRoutineController.cs`: Runtime-Komponente fuer einfache Schrittsequenzen.

## Unterstuetzte Schritte
- `Wait`: NPC bleibt stehen und wartet.
- `LookDirection`: NPC setzt eine Blickrichtung.
- `MoveToPoint`: NPC bewegt sich zu einem optionalen Ziel-Transform.

## Integration
Der Controller kann spaeter auf einen NPC oder ein Testobjekt gesetzt werden. Zielpunkte werden ueber Inspector-Referenzen zugewiesen. Ohne konfigurierte Schritte startet keine Routine.

## Grenzen des MVP
- Keine Savegame-Persistenz.
- Kein Tageszeit-System.
- Keine automatische Migration von Arthur oder Bernd.
- Keine Szenen- oder Prefab-Anbindung in diesem Task.
