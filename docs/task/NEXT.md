# NEXT (High Priority Tasks)

## Ziel
Diese Datei enthält die **nächsten Aufgaben**, die Codex umsetzen soll.

## Regeln
- Immer nur 1 Task gleichzeitig aktiv
- Tasks müssen klar referenzieren:
  - Feature-Doku
  - CODEX_TASK_*.md Datei
- Reihenfolge = Priorität
- Codex arbeitet immer nur den ersten Task in dieser Datei ab
- Keine Tasks überspringen, außer ein Task ist ausdrücklich als blockiert markiert und der Nutzer gibt die Freigabe
- Nach jedem erledigten oder blockierten Task stoppen und Ergebnis liefern
- Keine automatischen Commits

---

## Progress-Workflow

### Erfolgreicher Task
Wenn der erste Task vollständig umgesetzt und geprüft wurde:
1. Task aus `docs/task/NEXT.md` entfernen.
2. Task mit Datum in `docs/task/DONE.md` eintragen.
3. Kurz dokumentieren:
   - Aufgabe
   - betroffene Systeme/Dateien
   - Ergebnis
   - Tests/Prüfung
   - offene Follow-Ups
4. Falls `NEXT.md` danach leer ist:
   - den nächsten priorisierten Task aus `docs/task/BACKLOG.md` nach `NEXT.md` verschieben.
   - den verschobenen Task im Backlog als verschoben entfernen oder markieren.

### Blockierter Task
Wenn der erste Task nicht umgesetzt werden kann:
1. Task in `NEXT.md` stehen lassen.
2. Status auf `Blocked` setzen.
3. Blocker direkt beim Task dokumentieren:
   - Grund
   - betroffene Dateien/Systeme
   - benötigte Entscheidung oder fehlende Information
   - mögliche Alternativen
4. Nicht automatisch zum nächsten Task wechseln.
5. Auf Nutzerentscheidung warten.

### NEXT leer
Wenn keine aktiven Tasks mehr in `NEXT.md` stehen:
1. `docs/task/BACKLOG.md` prüfen.
2. Den höchsten priorisierten passenden Task nach `NEXT.md` verschieben.
3. Task mit Feature-Doku und Task-Datei referenzieren.
4. Danach stoppen und den neuen nächsten Task melden.

## Aktuelle Tasks

### 1. Settings System (MEDIUM)
- Ziel: Zentrales Settings-System fuer Audio, Video, Input und Gameplay vorbereiten
- Task-Datei:
  - docs/settings/CODEX_TASK_SETTINGS.md
- Feature-Doku:
  - docs/settings/FEATURE_SETTINGS.md
- Abhängigkeiten:
  - UI System optional
  - Application.persistentDataPath
- Status:
  - Not Started

---

## Nächster Schritt für Codex

Beim Start:

1. Lies:
   - docs/core/FEATURE_REGISTRY.md
   - docs/core/AI_CONTEXT.md

2. Arbeite Task #1 vollständig ab

3. Danach:
   - Ergebnis liefern
   - Task-Status nach dem Progress-Workflow dokumentieren
   - stoppen

---

## Hinweis
Wenn ein Task blockiert ist:
- Grund dokumentieren
- Task in NEXT lassen
- Status auf `Blocked` setzen
- nicht automatisch den nächsten Task beginnen
