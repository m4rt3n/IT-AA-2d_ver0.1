# Codex Task: Quiz FreeText Bewertung verbessern

## Kontext
Siehe:
- docs/quiz/QUIZ_FEATURE.md
- docs/quiz/QUIZ_ARCHITECTURE.md
- docs/quiz/QUIZ_TODO.md

## Aufgabe
Verbessere die Freitext-Bewertung im Quiz-System als kleinen, kontrollierten Schritt.

## Anforderungen
- Bestehende QuizSet-/QuizRunner-/QuizPanel-Struktur nicht brechen
- Keine Scene-/Prefab-Aenderungen ohne explizite Freigabe
- UI und Bewertungslogik getrennt halten
- Eine Klasse pro Datei
- C#-Dateien mit Kopfkommentar
- README.md aktualisieren

## Erwartetes Ergebnis
- robustere Freitext-Normalisierung
- einfache Fuzzy-/Synonym-Bewertung, wenn passend
- Tests/Pruefung dokumentieren

## Risiken
- Keine falschen Antworten zu grosszuegig als richtig werten
- Bestehende Multiple-Choice-Bewertung nicht veraendern
- Datenmodell nur erweitern, wenn wirklich noetig
