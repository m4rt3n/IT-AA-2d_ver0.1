# Codex Task: Erweiterbares Quiz-System

## Projekt
Unity 2D IT-AA

## Ziel
Implementiere ein erweiterbares Quiz-System für Bernd.

## Kontextdateien
Bitte vorher lesen:
- docs/quiz/QUIZ_FEATURE.md
- docs/quiz/QUIZ_ARCHITECTURE.md
- docs/quiz/QUIZ_DATA_MODEL.md
- docs/quiz/QUIZ_GENERATION_FLOW.md
- docs/quiz/QUIZ_TODO.md

## Anforderungen
- Namespace: ITAA.Features.Quiz
- Keine Breaking Changes
- Bestehendes Bernd-System wiederverwenden
- Bestehendes QuizPanel berücksichtigen
- Keine echte ChatGPT/API-Anbindung in Phase 1
- API nur über Interface vorbereiten

## Umzusetzen: Phase 1 bis Phase 4

### Dateien erstellen
- QuizQuestion.cs
- QuizAnswerOption.cs
- QuizDifficulty.cs
- QuizQuestionType.cs
- QuizQuestionSource.cs
- StaticQuestionBank.cs
- GeneratedQuestionDraftBank.cs
- DummyQuestionGenerator.cs
- QuizQuestionPromoter.cs
- QuizSession.cs
- QuizManager.cs

### Bestehende Dateien prüfen/anpassen
- BerndQuizStarter.cs
- QuizPanel.cs
- MenuManager.cs falls nötig

## Erwartetes Verhalten
- Bernd kann ein Quiz starten
- Fragen haben Schwierigkeitsgrad
- Fragen haben Typ:
  - ButtonClick
  - MultipleChoice
  - FreeText
- feste Demo-Fragen funktionieren
- generierte Dummy-Fragen werden als Draft gespeichert
- Draft-Fragen können per Button übernommen werden

## Wichtige Regeln
- Kein neues NPC-System bauen
- Keine hart verdrahtete UI-Logik im Datenmodell
- JSON-Persistenz vorbereiten, aber nicht zwingend vollständig in Phase 1
- Alle neuen Klassen mit Header-Kommentar
- Vollständige Dateien liefern, keine Snippets