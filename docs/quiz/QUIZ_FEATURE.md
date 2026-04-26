# Feature: Erweiterbares Quiz-System

## Ziel
Das Quiz-System soll mehrere Fragetypen, Schwierigkeitsgrade und Quellen unterstützen.

## Fragetypen
- ButtonClick: einfache Auswahl über Buttons
- MultipleChoice: mehrere Antwortoptionen, eine oder mehrere richtige Antworten
- FreeText: freie Texteingabe mit normalisierter Auswertung und vorsichtigem Fuzzy-Matching

## Aktueller FreeText-MVP
- `QuizQuestion.AcceptedTextAnswers` speichert akzeptierte Freitextantworten
- `QuizTextAnswerEvaluator` normalisiert Eingaben, entfernt Akzente und vereinheitlicht Leerzeichen/Trennzeichen
- optionales Fuzzy-Matching ist ueber `AllowFuzzyTextMatch` und `MaxTextAnswerDistance` begrenzt
- `QuizRunner.AnswerCurrentQuestion(string)` wertet Freitext aus
- `QuizPanel` zeigt fuer Fragen ohne Antwortoptionen und mit akzeptierten Textantworten ein Eingabefeld
- bestehende Multiple-Choice-Bewertung ueber Antwortindex bleibt unveraendert

## Schwierigkeitsgrade
- Easy
- Medium
- Hard
- Expert

## Aktueller Dynamic-Difficulty-MVP
- `QuizDifficulty` stellt feste Difficulty-Werte bereit.
- `QuizQuestion.Difficulty` kann pro Frage gepflegt werden; bestehende Fragen fallen auf `Medium` zurueck.
- `QuizDifficultyPerformance` beschreibt beantwortete Fragen, richtige Antworten und aktuelle Schwierigkeit.
- `QuizDifficultyEvaluator` empfiehlt optional anhand der Trefferquote eine naechste Schwierigkeit.
- `QuizRunner` zaehlt beantwortete und richtige Fragen mit und stellt `RecommendedNextDifficulty` bereit.
- Bestehende QuizSets werden nicht automatisch gefiltert oder umsortiert.

## Aktueller Themenfortschritt-MVP
- `ProgressProfile` sammelt Quiz-Antworten pro Thema ueber `TopicProgress`.
- `QuizPanel` meldet Antworten und Quizabschluss optional an `QuizProgressReporter`, wenn dieser vorhanden ist.
- `QuizTopicProgressFormatter` erstellt kurze HUD-/UI-Texte wie `DNS: 2 / 3 (67%)`.
- `HudController` zeigt fuer das aktuelle Thema vorhandenen Fortschritt im bestehenden Topic-Feld an.
- `GameSystemsBootstrap` erzeugt in `StartScene` und `GameScene` bei Bedarf einen `QuizProgressReporter`.
- Savegame-Persistenz und finale Themenfortschritts-UX bleiben offen.

## Aktueller Fragequalitaets-MVP
- `QuizQuestionQualityEvaluator` prueft einzelne Fragen defensiv.
- `QuizQuestionQualityReport` liefert Score, Hinweise und `IsUsable`.
- Geprueft werden Mindestdaten, Thema, Schwierigkeit, Antwortmodell, leere/doppelte Antworten und Erklaerung.
- Die Pruefung veraendert keine Frage und lehnt keine QuizSets automatisch ab.
- Review Mode, Draft-Banks und DevTools koennen den Evaluator spaeter optional nutzen.

## Fragequellen
1. StaticQuestionBank
   - feste Fragen im Projekt
   - zuerst hardcoded oder lokal als JSON
2. GeneratedQuestionDrafts
   - Fragen, die später durch ChatGPT/API erzeugt werden
   - werden zunächst als Entwurf gespeichert
   - können per Button in die feste Fragenbank übernommen werden

## Themen
Beispiele:
- IT-Support
- Netzwerk
- DHCP/DNS
- Windows
- Linux
- Auswärtiges Amt Vorbereitung
- Konsularisches Grundwissen
- Englisch
