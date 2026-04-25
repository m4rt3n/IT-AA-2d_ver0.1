# Feature: NPC Routine System

## Ziel
NPCs sollen spaeter einfache Tages- oder Ablaufplaene erhalten, ohne die bestehenden Arthur- und Bernd-Interaktionen zu brechen.

## Namespace
- ITAA.NPC.Routines

## MVP-Idee
- Datenmodell fuer Routine-Schritte
- Runtime-Komponente fuer Schrittwechsel
- optionale Zielpunkte in der Szene
- keine automatische Migration von Arthur oder Bernd im ersten Schritt

## Implementierter MVP-Stand
- `NpcRoutineStepType`: Schrittarten `Wait`, `LookDirection`, `MoveToPoint`
- `NpcRoutineStep`: serialisierbares Datenmodell fuer Inspector-Konfiguration
- `NpcRoutineController`: optionale Runtime-Komponente fuer einfache Sequenzen
- Animator-Parameter `MoveX`, `MoveY` und `IsMoving` werden nur gesetzt, wenn sie vorhanden sind
- keine automatische Scene-/Prefab-Anbindung

## Anforderungen
- Bestehende Arthur-/Bernd-Logik bleibt unveraendert
- Keine Szenen- oder Prefab-Aenderungen ohne ausdrueckliche Freigabe
- NPC-spezifische Logik bleibt in den jeweiligen NPC-Features
- Gemeinsame Routine-Logik darf nur klein und wiederverwendbar sein

## Offene Entscheidungen
- Zeitmodell: echte Uhrzeit, Spielzeit oder einfacher Sequenzmodus
- Speicherung im Savegame
- Integration mit Dialog, Quest und Scenario System
- konkrete Migration von Arthur oder Bernd
