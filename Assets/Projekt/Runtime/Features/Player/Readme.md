# 🧍 Player System

## Zweck
Verwaltet alle spielerbezogenen Funktionen wie Bewegung, Session und UI.

## Struktur

- Movement/
  - PlayerController.cs → Steuerung der Bewegung

- Session/
  - PlayerSession.cs → Spielerstatus und Daten

- UI/
  - PlayerNameDisplay.cs → Anzeige des Spielernamens
  - PlayerNameTag.cs → Name über dem Spieler

## Verantwortlichkeiten

- Input verarbeiten
- Bewegung steuern
- Spielerzustand verwalten
- UI-Elemente aktualisieren

## Flow

Input → Movement → Session → UI

## Einstieg

Startpunkt: `PlayerController.cs`