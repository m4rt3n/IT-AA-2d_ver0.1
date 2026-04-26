# Feature: Quiz Review Mode

## Ziel
Generierte Quizfragen sollen geprüft, bearbeitet und anschließend in die feste Fragenstruktur übernommen werden können.

## Kontext
Das Quiz-System unterstützt später:
- feste Fragen
- generierte Fragen
- manuelle Übernahme generierter Fragen

## Review Mode Funktionen

### Anzeigen
- generierte Frage anzeigen
- Thema anzeigen
- Schwierigkeit anzeigen
- Fragetyp anzeigen
- Antwortoptionen anzeigen
- Erklärung anzeigen

### Bearbeiten
- Fragetext ändern
- Antwortoptionen ändern
- richtige Antwort markieren
- Erklärung ändern
- Schwierigkeit ändern
- Thema ändern

### Aktionen
- Frage übernehmen
- Frage verwerfen
- nächste generierte Frage anzeigen
- alle Drafts anzeigen

## Datenfluss

1. DummyQuestionGenerator oder ChatGptQuestionGenerator erzeugt Frage
2. Frage wird als Draft gespeichert
3. ReviewPanel zeigt Frage
4. User prüft Frage
5. User klickt "Übernehmen"
6. QuizQuestionPromoter setzt:
   - source = Static
   - approved = true
7. Frage wird in feste Fragenbank übernommen

## MVP
Für erste Umsetzung reicht:
- Draft-Frage anzeigen
- Button "Übernehmen"
- Button "Verwerfen"
- keine vollständige Editierfunktion nötig
- optionaler Qualitaetsbericht ueber `QuizQuestionQualityEvaluator`

## Spätere Erweiterung
- echtes Bearbeiten im UI
- JSON Export
- JSON Import
- Validierung doppelter IDs
- erweiterte Qualitätsprüfung
- Themenfilter
