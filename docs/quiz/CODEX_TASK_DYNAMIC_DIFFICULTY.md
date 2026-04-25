# Codex Task: Quiz Schwierigkeit dynamisch anpassen

## Kontext
Siehe:
- docs/quiz/QUIZ_FEATURE.md
- docs/quiz/QUIZ_ARCHITECTURE.md

## Aufgabe
Bereite eine kleine, optionale Logik fuer dynamische Quiz-Schwierigkeit vor.

## Anforderungen
- Bestehende QuizSet-/QuizRunner-/QuizPanel-Struktur nicht brechen
- Keine Scene-/Prefab-Aenderungen ohne explizite Freigabe
- Multiple-Choice- und FreeText-Bewertung nicht veraendern
- Eine Klasse pro Datei
- C#-Dateien mit Kopfkommentar
- README.md aktualisieren

## Erwartetes Ergebnis
- einfaches Difficulty-Modell oder Helper
- Bewertung anhand bisheriger Quizleistung vorbereitet
- Tests/Pruefung dokumentieren

## Risiken
- Keine automatische Umstellung bestehender QuizSets ohne Designentscheidung
- Keine harte Kopplung an Progress, Skills oder Savegame
