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
