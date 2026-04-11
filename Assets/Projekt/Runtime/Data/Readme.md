# Data

## Zweck
Verwaltung aller persistierten Daten (User, Savegames).

## Inhalt
- DatabaseManager.cs: zentrale Datenzugriffslogik
- Models/: Datenstrukturen
- Storage/: Speicherung (Dateien / DB)

## Verantwortlichkeit
Speichern und Laden aller Spielzustände.

## Abhängigkeiten
- Core (Singletons)
- Player (für Save-Daten)

## Hinweise
Keine Game-Logik → nur Datenhandling.