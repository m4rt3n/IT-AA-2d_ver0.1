# Codex Task: Quiz Frage-Qualitaetsbewertung

## Kontext
Siehe:
- docs/quiz/QUIZ_FEATURE.md
- docs/quiz/QUIZ_ARCHITECTURE.md
- docs/quiz/QUIZ_REVIEW_MODE.md

## Aufgabe
Bereite eine kleine, optionale Qualitaetsbewertung fuer Quizfragen vor.

## Anforderungen
- Bestehende QuizSet-/QuizRunner-/QuizPanel-Struktur nicht brechen
- Keine Scene-/Prefab-Aenderungen ohne ausdrueckliche Freigabe
- Keine harte Kopplung an ChatGPT/API, Savegame oder UI erzwingen
- Eine Klasse pro Datei
- C#-Dateien mit Kopfkommentar
- README.md aktualisieren

## Erwartetes Ergebnis
- Einfaches Datenmodell oder Helper fuer Fragequalitaet
- Bewertungskriterien fuer Mindestdaten, Antworten, Erklaerung, Thema und Schwierigkeit vorbereitet
- Optionaler Anschluss fuer spaetere Review-Workflows
- Tests/Pruefung dokumentieren

## Risiken
- Finale Bewertungsregeln fuer generierte Fragen sind noch nicht entschieden
- Keine automatische Ablehnung oder Veraenderung bestehender QuizSets ohne Designentscheidung
