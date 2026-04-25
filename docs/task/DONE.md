# DONE (Completed Tasks)

## Ziel
Dokumentation aller abgeschlossenen Tasks.

## Regeln
- Jeder Task wird nach Abschluss hier eingetragen
- Kurzbeschreibung + Datum
- Optional: Commit Referenz
- Nur erfolgreich abgeschlossene Tasks eintragen
- Blockierte Tasks bleiben in `docs/task/NEXT.md`
- Ein DONE-Eintrag muss genug Kontext enthalten, damit der Projektstand später nachvollziehbar ist

---

## Progress-Workflow

Ein Task wird hier eingetragen, wenn:
- die Umsetzung abgeschlossen ist
- die geforderte Prüfung durchgeführt wurde
- bekannte Rest-Risiken dokumentiert sind
- der Task aus `docs/task/NEXT.md` entfernt wurde

Codex soll pro abgeschlossenem Task genau einen Eintrag unter `Einträge` ergänzen.

Pflichtangaben:
- Datum
- Task-Name
- Beschreibung
- Betroffene Systeme/Dateien
- Wichtige Änderungen
- Tests/Prüfung
- Risiken / Follow-Ups

Optional:
- Commit-Hash oder Commit-Message, wenn der Nutzer einen Commit ausführt

## Format

### [YYYY-MM-DD] Task Name
- Beschreibung:
- Betroffene Systeme:
- Wichtige Änderungen:
- Tests / Prüfung:
- Risiken / Follow-Ups:
- Commit:

---

## Einträge

### [2026-04-25] IT Terminal Minigames
- Beschreibung:
  - Simuliertes Terminal fuer IT-Support-Minispiele als MVP umgesetzt.
- Betroffene Systeme:
  - `Assets/Projekt/Runtime/Features/Terminal/`
  - `README.md`
  - `docs/task/NEXT.md`
  - `docs/task/DONE.md`
- Wichtige Änderungen:
  - `TerminalCommandType`, `TerminalCommand`, `TerminalCommandResult`, `TerminalEmulator` und `TerminalPanel` erstellt.
  - MVP-Befehle `help`, `ipconfig`, `ping`, `nslookup`, `clear` und `exit` umgesetzt.
  - Terminal-Ausgaben sind rein simuliert; es werden keine OS-Befehle oder Netzwerkzugriffe ausgefuehrt.
  - `TerminalPanel` trennt UI von Logik und kann fehlende UI-Referenzen zur Laufzeit erzeugen.
  - `Settings System` als naechster vorhandener CODEX-Task in `NEXT.md` eingetragen.
- Tests / Prüfung:
  - Statische Pruefung der Namespaces und Dateien.
  - `git diff --check` ohne Whitespace-Fehler; nur bestehende Line-Ending-Warnungen.
  - Unity Batchmode Script-Compilation erfolgreich: `Tundra build success`.
- Risiken / Follow-Ups:
  - Noch keine Scene-/Prefab-Anbindung.
  - Keine Scenario-Bridge; diese ist als spaeterer Integrationsschritt vorgesehen.
  - Styling ist MVP und sollte spaeter an die finale UI angepasst werden.
- Commit:
  - Vorschlag: `feat: add terminal minigames`

---

### [2026-04-25] HUD System
- Beschreibung:
  - Einfaches, optionales HUD-System als MVP umgesetzt.
- Betroffene Systeme:
  - `Assets/Projekt/Runtime/Features/HUD/`
  - `README.md`
  - `docs/task/NEXT.md`
  - `docs/task/DONE.md`
- Wichtige Änderungen:
  - `HudController`, `HudView` und `HudNotification` erstellt.
  - HUD zeigt Spielername, aktuelles Ziel, Quizpunkte, Thema und kurze Meldungen.
  - `PlayerSession` wird verwendet, falls vorhanden; `ProgressManager` wird optional fuer Quizpunkte genutzt.
  - HUD liest nicht direkt aus dem SaveSystem und bleibt optional fuer StartScene nutzbar.
  - `IT Terminal Minigames` als naechster vorhandener CODEX-Task in `NEXT.md` eingetragen.
- Tests / Prüfung:
  - Statische Pruefung der Namespaces und Dateien.
  - `git diff --check` ohne Whitespace-Fehler; nur bestehende Line-Ending-Warnungen.
  - Unity Batchmode Assembly-Reload ohne `error CS`, `warning CS` oder `Scripts have compiler errors`.
- Risiken / Follow-Ups:
  - Noch keine Scene-/Prefab-Anbindung.
  - `refreshEveryFrame` ist optional; langfristig sind Events sauberer.
  - Visuelles Styling ist MVP und sollte spaeter an das finale UI angepasst werden.
- Commit:
  - Vorschlag: `feat: add hud system`

---

### [2026-04-25] Quest / Progress System
- Beschreibung:
  - Einfaches Quest- und Lernfortschrittssystem als MVP umgesetzt.
- Betroffene Systeme:
  - `Assets/Projekt/Runtime/Features/Progress/`
  - `README.md`
  - `docs/task/NEXT.md`
  - `docs/task/DONE.md`
- Wichtige Änderungen:
  - `QuestDefinition`, `QuestProgress`, `ProgressProfile`, `ProgressManager` und `QuizProgressReporter` erstellt.
  - Demo-Quests `talk_to_bernd`, `answer_3_dns_questions` und `complete_easy_quiz` als Runtime-Fallback vorbereitet.
  - Quiz-Ergebnis-Meldung ueber `QuizProgressReporter` vorbereitet, ohne `QuizPanel` oder Bernd hart zu koppeln.
  - Fortschritt bleibt im Speicher; SaveSystem-Persistenz ist vorbereitet, aber noch nicht angebunden.
  - `HUD System` als naechster vorhandener CODEX-Task in `NEXT.md` eingetragen.
- Tests / Prüfung:
  - Statische Pruefung der Namespaces und Dateien.
  - `git diff --check` ohne Whitespace-Fehler; nur bestehende Line-Ending-Warnungen.
  - Unity Batchmode Script-Compilation nach Namespace-Korrektur erfolgreich: `Tundra build success`.
- Risiken / Follow-Ups:
  - Noch keine Scene-/Prefab-Anbindung.
  - Keine automatische Meldung aus `QuizPanel`; dafuer ist spaeter ein kleiner Adapter oder Event sinnvoll.
  - Savegame-Persistenz fuer `ProgressProfile` ist noch offen.
- Commit:
  - Vorschlag: `feat: add quest progress system`

---

### [2026-04-25] Dialogue System
- Beschreibung:
  - Wiederverwendbares MVP-Dialogsystem fuer NPCs und spaetere Szenario-/Quiz-Integrationen umgesetzt.
- Betroffene Systeme:
  - `Assets/Projekt/Runtime/Features/Dialogue/`
  - `README.md`
  - `docs/task/NEXT.md`
  - `docs/task/DONE.md`
- Wichtige Änderungen:
  - `DialogueLine`, `DialogueSequence`, `DialogueManager`, `DialoguePanel` und `IDialogueTrigger` erstellt.
  - Dialogdaten sind als `DialogueSequence`-Asset vorbereitet.
  - `DialogueManager.StartDialogue(sequence, callback)` startet Dialoge und ruft nach Ende optional einen Callback auf.
  - `DialoguePanel` zeigt Sprecher/Text und kann eine einfache MVP-UI selbst erzeugen.
  - Arthur und Bernd wurden bewusst nicht automatisch migriert, damit bestehende Interaktionen stabil bleiben.
  - `Quest / Progress System` als naechster vorhandener CODEX-Task in `NEXT.md` eingetragen.
- Tests / Prüfung:
  - Statische Pruefung der Namespaces und Dateien.
  - `git diff --check` ohne Whitespace-Fehler; nur bestehende Line-Ending-Warnungen.
  - Unity Batchmode Assembly-Reload ohne `error CS`, `warning CS` oder `Scripts have compiler errors`.
- Risiken / Follow-Ups:
  - Noch keine Scene-/Prefab-Anbindung.
  - Es existiert weiterhin ein alter UI-Platzhalter `ITAA.UI.Panels.DialoguePanel`; wegen anderem Namespace kein Compile-Konflikt, aber spaeter sollte die Projekt-Doku diese Trennung klar halten.
  - NPC-Adapter fuer Arthur/Bernd sind spaetere kontrollierte Integrationsschritte.
- Commit:
  - Vorschlag: `feat: add dialogue system`

---

### [2026-04-25] Knowledge Base
- Beschreibung:
  - Einfaches internes IT-Lexikon als MVP umgesetzt.
- Betroffene Systeme:
  - `Assets/Projekt/Runtime/Features/KnowledgeBase/`
  - `README.md`
  - `docs/task/NEXT.md`
  - `docs/task/DONE.md`
- Wichtige Änderungen:
  - `KnowledgeArticle`, `KnowledgeTopic`, `KnowledgeBaseRepository`, `KnowledgeBasePanel` und `KnowledgeArticleListItemUI` erstellt.
  - Demo-Artikel fuer DNS, DHCP, Gateway, VPN und OSI-Modell ergaenzt.
  - UI und Datenmodell getrennt; Suche nach Titel und Oeffnen per Artikel-ID vorbereitet.
  - Quiz-/Szenario-Integration ueber `RelatedQuizTopic`, `RelatedScenarioId` und `OpenArticleById` vorbereitet.
  - `Dialogue System` als naechster vorhandener CODEX-Task in `NEXT.md` eingetragen.
- Tests / Prüfung:
  - Statische Pruefung der Namespaces und Dateien.
  - `git diff --check` ohne Whitespace-Fehler; nur bestehende Line-Ending-Warnungen.
  - Unity Batchmode Script-Compilation erfolgreich: `Tundra build success`.
- Risiken / Follow-Ups:
  - Noch keine Scene-/Prefab-Anbindung.
  - Runtime-UI ist MVP und sollte spaeter visuell in das bestehende UI-Design integriert werden.
  - Repository nutzt feste Demo-Daten; JSON-/ScriptableObject-Daten koennen spaeter folgen.
- Commit:
  - Vorschlag: `feat: add knowledge base`

---

### [2026-04-25] Bernd Interaction Adapter
- Beschreibung:
  - Bernd optional an das World Interaction System angebunden, ohne bestehende Bernd-Logik zu entfernen.
- Betroffene Systeme:
  - `Assets/Projekt/Runtime/Features/NPC/Bernd/`
  - `Assets/Projekt/Runtime/Features/Interaction/README.md`
  - `README.md`
  - `docs/task/NEXT.md`
  - `docs/task/DONE.md`
- Wichtige Änderungen:
  - `BerndInteractableAdapter` erstellt.
  - Adapter implementiert `IInteractable` und delegiert bevorzugt an `BerndAutoInteraction.StartInteraction()`.
  - Fallback auf `BerndQuizStarter.StartQuiz()` vorbereitet, falls `BerndAutoInteraction` fehlt.
  - Bestehende Bernd-Komponenten wurden nicht entfernt oder ersetzt.
  - `Knowledge Base` als naechster Task in `NEXT.md` eingetragen.
- Tests / Prüfung:
  - Statische Pruefung der Namespaces und Dateien.
  - `git diff --check` ohne Whitespace-Fehler; nur bestehende Line-Ending-Warnungen.
  - Unity Batchmode Assembly-Reload ohne `error CS`, `warning CS` oder `Scripts have compiler errors`.
- Risiken / Follow-Ups:
  - Noch keine Scene-/Prefab-Anbindung.
  - Wenn `BerndAutoInteraction` und `InteractionController` gleichzeitig auf `E` reagieren, muss die alte direkte E-Logik spaeter kontrolliert deaktiviert werden.
- Commit:
  - Vorschlag: `feat: add Bernd interaction adapter`

---

### [2026-04-25] DevPanel
- Beschreibung:
  - Optionales Entwickler-Panel fuer Debug- und Testfunktionen umgesetzt.
- Betroffene Systeme:
  - `Assets/Projekt/Runtime/Features/DevTools/`
  - `README.md`
  - `docs/task/NEXT.md`
  - `docs/task/DONE.md`
- Wichtige Änderungen:
  - `DevPanelController` mit Aktionen fuer SaveSlot-Reload, Dummy-Saves, Settings-Reset, Quiz-Draft-Log, PlayerSession-Log, Szenen-Log und Close erstellt.
  - `DevPanelBootstrap` erstellt, das bei Bedarf ein Runtime-Panel erzeugt und per `F12` toggelt.
  - Bestehende Systeme werden defensiv verwendet; fehlende Systeme melden `Debug.LogWarning`.
  - Naechster Task nach Nutzerreihenfolge als `Bernd Interaction Adapter` in `NEXT.md` eingetragen.
- Tests / Prüfung:
  - Statische Pruefung der Namespaces und Dateien.
  - `git diff --check` ohne Whitespace-Fehler; nur bestehende Line-Ending-Warnungen.
  - Unity Batchmode Script-Compilation nach Korrektur ohne `error CS`/`warning CS` und ohne `Scripts have compiler errors`.
- Risiken / Follow-Ups:
  - Noch keine Scene-/Prefab-Anbindung.
  - Runtime-Panel ist bewusst schlicht und fuer Entwicklung gedacht.
  - Quiz-Draft-Persistenz existiert noch nicht und wird nur per Log vorbereitet.
- Commit:
  - Vorschlag: `feat: add dev panel tools`

---

### [2026-04-25] Scenario System
- Beschreibung:
  - Eigenstaendiges MVP-System fuer IT-Lernszenarien umgesetzt.
- Betroffene Systeme:
  - `Assets/Projekt/Runtime/Features/Scenarios/`
  - `README.md`
  - `docs/task/NEXT.md`
  - `docs/task/DONE.md`
- Wichtige Änderungen:
  - `ScenarioDefinition`, `ScenarioStep`, `ScenarioProgress`, `ScenarioManager` und `ScenarioStatus` erstellt.
  - Demo-Szenario `no_internet_basic` / `Kein Internet` als Runtime-Fallback im `ScenarioManager` vorbereitet.
  - Datenmodell und Laufzeitlogik bewusst von UI, NPC, Quiz, Dialog und Savegame entkoppelt.
  - `DevPanel` als naechster Task in `NEXT.md` nachgerueckt.
- Tests / Prüfung:
  - Statische Pruefung der Namespaces und Dateien.
  - `git diff --check` ohne Whitespace-Fehler; nur bestehende Line-Ending-Warnungen.
  - Unity Batchmode Script-Compilation ohne `error CS`/`warning CS`, Prozess mit Return Code 0 beendet.
- Risiken / Follow-Ups:
  - Noch keine Scene-/Prefab-Anbindung.
  - HUD, Quiz, Dialog und Savegame-Progress muessen spaeter kontrolliert andocken.
- Commit:
  - Vorschlag: `feat: add scenario system`

---

### [2026-04-25] World Interaction System
- Beschreibung:
  - Wiederverwendbares MVP-System fuer Welt-Interaktionen vorbereitet.
- Betroffene Systeme:
  - `Assets/Projekt/Runtime/Features/Interaction/`
  - `README.md`
- Wichtige Änderungen:
  - `IInteractable`, `InteractionType`, `InteractionDetector`, `InteractionController` und `InteractionPromptView` erstellt.
  - Feature-README mit Setup- und Integrationshinweisen ergänzt.
  - Bestehende Arthur-/Bernd-Interaktionen bewusst nicht migriert, damit keine doppelte Eingabelogik entsteht.
- Tests / Prüfung:
  - Statische Prüfung der Namespaces und Dateien.
  - `git diff --check` ohne Fehler.
  - Unity Batchmode Script-Compilation erfolgreich: `Tundra build success`.
- Risiken / Follow-Ups:
  - Noch keine Scene-/Prefab-Anbindung.
  - Bernd Adapter als kontrollierter nächster Integrationsschritt sinnvoll.
- Commit:
  - Vorschlag: `feat: add world interaction system`

---

### [2026-04-25] Projektstruktur + Codex Docs
- Beschreibung:
  - Einführung vollständiger Codex-Dokumentationsstruktur
- Betroffene Systeme:
  - docs/
- Wichtige Änderungen:
  - AI_CONTEXT
  - FEATURE_REGISTRY
  - Settings/Quiz/DevPanel/Dialogue/Quest Docs
- Follow-Up:
  - Implementierung durch Codex

---

### (Neue Tasks hier eintragen)
