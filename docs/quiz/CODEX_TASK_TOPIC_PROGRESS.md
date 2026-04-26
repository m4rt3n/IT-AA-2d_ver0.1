# Codex Task: Quiz Themen-Fortschritt visualisieren

## Kontext
Siehe:
- docs/quiz/QUIZ_FEATURE.md
- docs/quiz/QUIZ_ARCHITECTURE.md
- docs/quest/QUEST_PROGRESS_FEATURE.md
- docs/hub/HUD_FEATURE.md

## Aufgabe
Bereite eine kleine, optionale Visualisierung fuer Quiz-Themenfortschritt vor.

## Anforderungen
- Bestehende QuizSet-/QuizRunner-/QuizPanel-Struktur nicht brechen
- Keine Scene-/Prefab-Aenderungen ohne ausdrueckliche Freigabe
- Keine harte Kopplung an Savegame oder Skills
- Bestehendes Progress- und HUD-System defensiv nutzen
- Eine Klasse pro Datei
- C#-Dateien mit Kopfkommentar
- README.md aktualisieren

## Erwartetes Ergebnis
- Einfaches Datenmodell oder Helper fuer Themenfortschritt
- Optionaler UI-/HUD-Anknuepfungspunkt vorbereitet
- Keine automatische Umstellung bestehender QuizSets ohne Designentscheidung
- Tests/Pruefung dokumentieren

## Risiken
- Finale UX fuer Themenfortschritt ist noch nicht entschieden
- Savegame-Persistenz fuer Themenfortschritt ist noch offen
