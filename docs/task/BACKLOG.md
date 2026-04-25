# BACKLOG (Future Tasks)

## Ziel
Sammlung aller geplanten Features, die später umgesetzt werden sollen.

## Regeln
- Kein aktiver Task hier
- Nur Ideen / geplante Erweiterungen
- Werden später nach NEXT verschoben
- Backlog-Items werden nicht umgesetzt, solange `docs/task/NEXT.md` aktive Tasks enthält
- Wenn `NEXT.md` leer ist, wird der höchste priorisierte passende Backlog-Task nach `NEXT.md` verschoben
- Beim Verschieben muss der Task in `NEXT.md` eine konkrete Task-Datei und optional eine Feature-Doku referenzieren
- Verschobene Tasks werden aus dem Backlog entfernt oder eindeutig als `Moved to NEXT` markiert

---

## Progress-Workflow

### Task nach NEXT verschieben
Wenn `docs/task/NEXT.md` leer ist:
1. Den wichtigsten Backlog-Eintrag auswählen.
2. Prüfen, ob eine passende `CODEX_TASK_*.md` Datei existiert.
3. Falls keine Task-Datei existiert:
   - zuerst eine Task-Datei unter dem passenden docs-Feature-Ordner erstellen.
   - noch keine Runtime-Implementierung beginnen.
4. Den Task in `NEXT.md` mit Ziel, Task-Datei, Abhängigkeiten und Status `Not Started` eintragen.
5. Den Backlog-Eintrag entfernen oder mit `Moved to NEXT` markieren.
6. Danach stoppen und den Nutzer informieren.

### Blockierte Tasks
Blockierte Tasks gehören nicht in den Backlog zurück.
Sie bleiben in `NEXT.md`, bis der Blocker gelöst oder der Nutzer die Priorität ändert.

## Gameplay Features

---

## Quiz Erweiterungen
- Themen-Fortschritt visualisieren
- Frage-Qualitätsbewertung

---

## Scenario Erweiterungen
- Mehrstufige Szenarien
- Zufällige Fehlerursachen
- Zeitbasierte Aufgaben
- Multiple Lösungswege

---

## UI / UX
- Animationen im UI
- Bessere Transitions
- Mobile Layout Vorbereitung
- Controller Support

---

## Dev / Tools
- Testdaten Generator erweitern
- Logging System
- Performance Overlay
- Error Tracking

---

## AI / ChatGPT Integration
- echte Fragegenerierung
- Prompt Templates
- Qualitätsfilter
- Review-Workflow verbessern

---

## Audio / Immersion
- Soundeffekte für Aktionen
- Hintergrundmusik
- UI Sounds
- Audio Feedback für Quiz

---

## Networking / Advanced (optional)
- Online Highscore
- Sync von Lernfortschritt
- Cloud Save

---

## Hinweis
Items aus dem Backlog werden nur umgesetzt, wenn:
- NEXT leer ist
- oder Priorität angepasst wird
