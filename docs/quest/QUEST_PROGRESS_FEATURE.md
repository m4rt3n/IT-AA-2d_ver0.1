# Feature: Quest und Progress System

## Ziel
Ein System für Lernfortschritt, Aufgaben und Quiz-Ergebnisse.

Das Projekt soll nicht nur einzelne Fragen stellen, sondern Fortschritt messbar machen.

## Namespace
ITAA.Features.Progress

## Aufgaben-Beispiele
- Sprich mit Bernd
- Beantworte 3 DNS-Fragen
- Bestehe Easy Quiz
- Erreiche 5 richtige Antworten
- Schließe Thema Netzwerk ab

## Fortschrittsdaten
Pro Spieler sollen gespeichert werden:
- abgeschlossene Quests
- Quiz-Ergebnisse
- richtig/falsch Statistik
- Fortschritt pro Thema
- letzter Lernstand

## Komponenten

### QuestDefinition
Beschreibt eine Aufgabe.

### QuestProgress
Speichert aktuellen Fortschritt.

### ProgressProfile
Speichert gesamten Lernfortschritt des Spielers.

### ProgressManager
Verwaltet Fortschritt.

### QuizProgressReporter
Meldet Quiz-Ergebnisse an ProgressManager.

## Integration
- QuizManager meldet Ergebnis
- BerndQuizStarter kann Quest starten/abschließen
- SaveSystem speichert später ProgressProfile

## MVP
- Fortschritt nur im Speicher halten
- Debug.Log-Ausgabe bei Fortschritt
- JSON-Speicherung später vorbereiten

## Aktueller Quiz-Themenfortschritt
- `ProgressProfile.TopicProgress` sammelt Antworten und richtige Antworten pro Thema.
- `QuizProgressReporter` kann von `QuizPanel` optional genutzt werden.
- Das HUD kann den Fortschritt ueber `QuizTopicProgressFormatter` als kurzen Text anzeigen.
