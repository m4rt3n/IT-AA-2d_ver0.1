# Quiz Architektur

## Namespace
ITAA.Features.Quiz

## Prinzipien
- Feature-driven Architecture
- Keine Breaking Changes
- Bestehendes Bernd-System wiederverwenden
- Quiz-NPCs sollen später erweiterbar sein
- Daten und UI strikt trennen

## Hauptkomponenten

### QuizQuestion
Datenmodell für eine einzelne Frage.

### QuizAnswerOption
Datenmodell für Antwortoptionen.

### QuizDifficulty
Enum für Schwierigkeitsgrade.

### QuizQuestionType
Enum für Fragetypen.

### QuizTopic
Themenkennung für Fragen.

### StaticQuestionBank
Liefert feste Fragen.

### GeneratedQuestionDraftBank
Speichert generierte Fragen als Entwürfe.

### QuizSession
Verwaltet laufendes Quiz:
- aktuelle Frage
- Punktestand
- Fortschritt
- Auswertung

### QuizTextAnswerEvaluator
Bewertet Freitextantworten:
- Normalisierung
- Vergleich mit akzeptierten Antworten
- vorsichtiges Fuzzy-Matching bei kleinen Tippfehlern

### QuizManager
Zentrale Steuerung:
- Quiz starten
- Frage laden
- Antwort prüfen
- nächste Frage anzeigen

### QuizPanel
UI für:
- Frage anzeigen
- Antwortbuttons anzeigen
- Freitextfeld anzeigen
- Ergebnis anzeigen
- Button: GeneratedQuestion übernehmen

### QuizQuestionPromoter
Übernimmt generierte Fragen aus DraftBank in StaticQuestionBank.
