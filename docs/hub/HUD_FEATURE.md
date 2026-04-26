# Feature: HUD System

## Ziel
Ein zentrales HUD zeigt dem Spieler wichtige Informationen während des Spiels.

## Namespace
ITAA.Features.HUD

## Anzeigen
- Spielername
- aktuelles Ziel
- Queststatus
- Quizpunkte
- aktuelles Thema
- kurze Systemmeldungen

## UI-Elemente
- PlayerNameText
- CurrentObjectiveText
- QuizScoreText
- NotificationText
- TopicText

## Anforderungen
- HUD darf nicht direkt SaveSystem lesen
- HUD bekommt Daten über Controller/Events
- HUD soll optional ein-/ausblendbar sein
- HUD muss mit StartScene funktionieren

## MVP
- Spielername anzeigen
- aktuelles Ziel anzeigen
- Quizpunkte anzeigen
- einfache Nachricht anzeigen
- Themenfortschritt kann optional im Topic-Feld angezeigt werden, wenn `ProgressManager` Quizdaten fuer das aktuelle Thema enthaelt

## Spätere Erweiterungen
- kleine Icons
- Fortschrittsbalken
- Quest-Tracker
- Animationen für neue Meldungen
