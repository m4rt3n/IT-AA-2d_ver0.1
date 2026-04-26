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

### QuizDifficultyPerformance
Beschreibt beantwortete Fragen, richtige Antworten, aktuelle Schwierigkeit und Trefferquote.

### QuizDifficultyEvaluator
Empfiehlt optional die nächste Schwierigkeit anhand der bisherigen Quizleistung.

### QuizTopicProgressFormatter
Formatiert vorhandenen Themenfortschritt fuer HUD- oder UI-Anzeigen.

### QuizQuestionQualityEvaluator
Prueft einzelne Fragen auf Mindestdaten, Antwortmodell, Thema, Schwierigkeit und Erklaerung.

### QuizQuestionQualityReport
Beschreibt Score, Hinweise und Nutzbarkeit einer geprueften Frage.

### QuizQuestionQualityIssue
Beschreibt einen einzelnen Hinweis aus der Qualitaetspruefung.

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
- optionale Difficulty-Empfehlung

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
- optionalen ProgressReporter informieren
- Button: GeneratedQuestion übernehmen

### QuizQuestionPromoter
Übernimmt generierte Fragen aus DraftBank in StaticQuestionBank.
