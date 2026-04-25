# TODO: Quiz-System

## Phase 1: Grundlagen
- QuizQuestion Datenmodell erstellen
- QuizAnswerOption Datenmodell erstellen
- Enums erstellen:
  - QuizDifficulty
  - QuizQuestionType
  - QuizQuestionSource
- StaticQuestionBank mit festen Demo-Fragen erstellen

## Phase 2: UI
- QuizPanel erweitern
- ButtonClick-Fragen anzeigen
- MultipleChoice-Fragen anzeigen
- FreeText-Fragen anzeigen
- Ergebnis/Erklärung anzeigen

## Phase 3: Bernd Integration
- BerndQuizStarter nutzt QuizManager
- Bernd startet Quiz mit Topic/Difficulty
- Bestehendes Bernd-System nicht brechen

## Phase 4: Generierte Fragen als Draft
- GeneratedQuestionDraftBank erstellen
- DummyQuestionGenerator erstellen
- Button "Neue Frage generieren"
- Button "In feste Fragen übernehmen"

## Phase 5: Persistenz
- Draft-Fragen als JSON speichern
- Übernommene Fragen als JSON speichern
- später Import/Export vorbereiten

## Phase 6: Echte ChatGPT/API-Anbindung
- Interface IQuizQuestionGenerator erstellen
- ChatGptQuestionGenerator vorbereiten
- API-Key extern konfigurieren
- Fehlerbehandlung einbauen