# Codex Task: Quest und Progress System implementieren

## Ziel
Implementiere ein einfaches Quest-/Progress-System.

## Kontext
Bitte vorher lesen:
- docs/FEATURE_REGISTRY.md
- docs/quest/QUEST_PROGRESS_FEATURE.md

## Anforderungen
- Namespace: ITAA.Features.Progress
- Keine Breaking Changes
- Quiz-System vorbereiten
- SaveSystem später integrierbar machen

## Zu erstellen
- QuestDefinition.cs
- QuestProgress.cs
- ProgressProfile.cs
- ProgressManager.cs
- QuizProgressReporter.cs

## MVP-Funktionen
- Quest starten
- Quest-Fortschritt erhöhen
- Quest abschließen
- Quiz-Ergebnis melden
- Fortschritt per Debug.Log anzeigen

## Beispiel-Quests
- talk_to_bernd
- answer_3_dns_questions
- complete_easy_quiz

## Bestehende Dateien prüfen/anpassen
- BerndQuizStarter.cs
- QuizPanel.cs
- QuizManager.cs falls vorhanden
- SaveGameData.cs später nur vorbereiten, nicht zwingend ändern

## Wichtig
- Keine harte Kopplung an Bernd
- Keine harte Kopplung an konkrete UI
- Vollständige Dateien liefern
- Header-Kommentare ergänzen