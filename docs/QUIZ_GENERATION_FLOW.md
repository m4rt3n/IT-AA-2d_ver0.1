# Quiz Generation Flow

## Ziel
Fragen sollen später durch ChatGPT/API erzeugt werden können.

## MVP ohne echte API
Für die erste Umsetzung wird die Generierung simuliert:
- Button "Neue Frage generieren"
- System erzeugt Dummy-Frage anhand von Topic + Difficulty + QuestionType
- Frage wird in GeneratedQuestionDraftBank gespeichert
- UI zeigt die Frage als Entwurf an

## Spätere echte API
Eine spätere Klasse ChatGptQuestionGenerator kann angebunden werden.

## Ablauf

1. User wählt:
   - Topic
   - Difficulty
   - QuestionType

2. User klickt:
   - "Frage generieren"

3. System erstellt GeneratedDraft:
   - source = GeneratedDraft
   - approved = false

4. User prüft Frage im UI.

5. User klickt:
   - "In feste Fragen übernehmen"

6. QuizQuestionPromoter:
   - setzt source = Static
   - setzt approved = true
   - speichert Frage in StaticQuestionBank

## Wichtig
- Keine automatische Übernahme generierter Fragen
- Übernahme nur per Button
- Generierte Fragen sollen editierbar vorbereitet werden
- API-Key niemals hardcoden