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

### [2026-04-26] Zeitbasierte Aufgaben
- Beschreibung:
  - Optionale zeitbasierte Aufgaben und Timerstatus fuer Szenarien vorbereitet.
- Betroffene Systeme:
  - `Assets/Projekt/Runtime/Features/Scenarios/`
  - `README.md`
  - `docs/scenarios/SCENARIO_SYSTEM_FEATURE.md`
  - `docs/scenarios/CODEX_TASK_TIME_BASED_TASKS.md`
  - `docs/task/NEXT.md`
  - `docs/task/BACKLOG.md`
  - `docs/task/DONE.md`
- Wichtige Änderungen:
  - `ScenarioTimeLimit` als serialisierbares Zeitlimit fuer Szenarien und Schritte ergaenzt.
  - `ScenarioTimerState` als Laufzeitstatus fuer aktive Timer ergaenzt.
  - `ScenarioDefinition` und `ScenarioStep` koennen optionale Zeitlimits halten.
  - `ScenarioProgress` haelt Timerstatus fuer spaetere UI-/Savegame-Anbindung.
  - `ScenarioManager` tickt aktive Timer, meldet `TimerChanged` und `TimerTimedOut` und kann optional bei Timeout failen.
  - Demo-Szenario enthaelt ein 180-Sekunden-Zeitlimit fuer den DNS/DHCP-Pruefschritt.
  - `Multiple Loesungswege` als naechster Task aus dem Backlog nach `NEXT.md` verschoben.
- Tests / Prüfung:
  - Statische Pruefung und Unity-Kompilationspruefung siehe Abschlussbericht.
- Risiken / Follow-Ups:
  - Keine Scene-/Prefab-Aenderungen.
  - Keine Savegame-Persistenz fuer laufende Timer.
  - Keine HUD-/UI-Anzeige und keine finale Pause-/Menuelogik fuer Timer.
- Commit:
  - Vorschlag: `feat: prepare scenario time limits`

### [2026-04-26] Zufällige Fehlerursachen
- Beschreibung:
  - Optionale zufaellige Fehlerursachen fuer Szenarien vorbereitet.
- Betroffene Systeme:
  - `Assets/Projekt/Runtime/Features/Scenarios/`
  - `README.md`
  - `docs/scenarios/SCENARIO_SYSTEM_FEATURE.md`
  - `docs/scenarios/CODEX_TASK_RANDOM_FAILURE_CAUSES.md`
  - `docs/task/NEXT.md`
  - `docs/task/DONE.md`
- Wichtige Änderungen:
  - `ScenarioFailureCause` als serialisierbares Ursachenmodell ergaenzt.
  - `ScenarioFailureCauseSelector` fuer Zufallsauswahl und deterministische Auswahl per Index oder ID ergaenzt.
  - `ScenarioDefinition` kann optionale `FailureCauses` halten und Ursachen per ID finden.
  - `ScenarioProgress` speichert die aktive `ActiveFailureCauseId`.
  - `ScenarioManager` waehlt beim Start optional eine aktive Fehlerursache und stellt `ActiveFailureCause` bereit.
  - Demo-Szenario enthaelt DNS-, DHCP/Gateway- und Gateway-Erreichbarkeits-Ursachen.
- Tests / Prüfung:
  - Statische Pruefung und Unity-Kompilationspruefung siehe Abschlussbericht.
- Risiken / Follow-Ups:
  - Keine Scene-/Prefab-Aenderungen.
  - Keine Savegame-Persistenz fuer aktive Ursachen.
  - Keine HUD-/UI-Anzeige und keine finale Gameplay-UX fuer Ursachenhinweise.
- Commit:
  - Vorschlag: `feat: prepare scenario failure causes`

### [2026-04-26] Mehrstufige Szenarien
- Beschreibung:
  - Bestehendes Scenario-System um optionale Mehrschritt-Metadaten und Abschluss-APIs erweitert.
- Betroffene Systeme:
  - `Assets/Projekt/Runtime/Features/Scenarios/`
  - `README.md`
  - `docs/scenarios/SCENARIO_SYSTEM_FEATURE.md`
  - `docs/scenarios/CODEX_TASK_RANDOM_FAILURE_CAUSES.md`
  - `docs/task/NEXT.md`
  - `docs/task/BACKLOG.md`
  - `docs/task/DONE.md`
- Wichtige Änderungen:
  - `ScenarioStepType` fuer Objective-, Dialogue-, Quiz-, Task- und Checkpoint-Schritte ergaenzt.
  - `ScenarioStep` um Step-Typ, Completion-Key, ProgressQuestId und manuelle Completion-Hinweise erweitert.
  - `ScenarioDefinition` kann Schritte per StepId, Completion-Key, QuizId oder DialogueId finden.
  - `ScenarioProgress` kann spezifische und optionale Schritte abschliessen und `Progress01` berechnen.
  - `ScenarioManager` stellt Abschlussmethoden fuer StepId, Completion-Key, LinkedQuiz und LinkedDialogue bereit.
  - Demo-Szenario bleibt lauffaehig und nutzt die neuen Metadaten defensiv.
  - `Zufällige Fehlerursachen` als naechster Task aus dem Backlog nach `NEXT.md` verschoben.
- Tests / Prüfung:
  - Statische Pruefung und Kompilationspruefung siehe Abschlussbericht.
- Risiken / Follow-Ups:
  - Keine Scene-/Prefab-Anbindung.
  - HUD-, Quiz-, Dialog- und Progress-Adapter rufen die neuen Methoden noch nicht automatisch auf.
  - Savegame-Persistenz fuer Szenariofortschritt bleibt offen.
- Commit:
  - Vorschlag: `feat: extend multi-step scenarios`

### [2026-04-26] Quiz Frage-Qualitaetsbewertung
- Beschreibung:
  - Optionale Qualitaetsbewertung fuer einzelne Quizfragen vorbereitet.
- Betroffene Systeme:
  - `Assets/Projekt/Runtime/Features/Quiz/`
  - `README.md`
  - `docs/quiz/QUIZ_FEATURE.md`
  - `docs/quiz/QUIZ_ARCHITECTURE.md`
  - `docs/quiz/QUIZ_REVIEW_MODE.md`
  - `docs/scenarios/CODEX_TASK_MULTI_STEP_SCENARIOS.md`
  - `docs/task/NEXT.md`
  - `docs/task/BACKLOG.md`
  - `docs/task/DONE.md`
- Wichtige Änderungen:
  - `QuizQuestionQualitySeverity`, `QuizQuestionQualityIssue`, `QuizQuestionQualityReport` und `QuizQuestionQualityEvaluator` ergaenzt.
  - Evaluator prueft Fragetext, Thema, Schwierigkeit, Antwortmodell, leere/doppelte Antworten und Erklaerung.
  - Pruefung erzeugt nur Score und Hinweise; bestehende Fragen oder QuizSets werden nicht automatisch veraendert.
  - Review-Mode-Doku um optionalen Qualitaetsbericht ergaenzt.
  - `Mehrstufige Szenarien` als naechster Task aus dem Backlog nach `NEXT.md` verschoben.
- Tests / Prüfung:
  - Statische Pruefung und Kompilationspruefung siehe Abschlussbericht.
- Risiken / Follow-Ups:
  - Finale Bewertungsregeln fuer generierte Fragen muessen spaeter mit echten Drafts kalibriert werden.
  - Noch keine Review-UI fuer die Anzeige der Quality-Issues.
- Commit:
  - Vorschlag: `feat: add quiz question quality evaluator`

### [2026-04-26] Quiz Themen-Fortschritt visualisieren
- Beschreibung:
  - Optionalen Quiz-Themenfortschritt fuer Progress- und HUD-System vorbereitet.
- Betroffene Systeme:
  - `Assets/Projekt/Runtime/Features/Quiz/`
  - `Assets/Projekt/Runtime/Features/Progress/`
  - `Assets/Projekt/Runtime/Features/HUD/HudController.cs`
  - `Assets/Projekt/Runtime/Features/UI/Panels/QuizPanel.cs`
  - `Assets/Projekt/Runtime/Core/SceneManagement/StartSceneFeatureBootstrap.cs`
  - `README.md`
  - `docs/quiz/QUIZ_FEATURE.md`
  - `docs/quiz/QUIZ_ARCHITECTURE.md`
  - `docs/quest/QUEST_PROGRESS_FEATURE.md`
  - `docs/hub/HUD_FEATURE.md`
  - `docs/quiz/CODEX_TASK_QUESTION_QUALITY.md`
  - `docs/task/NEXT.md`
  - `docs/task/BACKLOG.md`
  - `docs/task/DONE.md`
- Wichtige Änderungen:
  - `TopicProgress` in eigene Datei ausgelagert.
  - `ProgressProfile.GetTopicProgress` ergaenzt.
  - `QuizTopicProgressFormatter` fuer kurze HUD-/UI-Texte erstellt.
  - `HudController` zeigt vorhandenen Themenfortschritt im Topic-Feld an.
  - `QuizPanel` meldet Antworten und Quizabschluss optional an `QuizProgressReporter`.
  - `StartSceneFeatureBootstrap` erzeugt in der `StartScene` bei Bedarf einen `QuizProgressReporter`.
  - `Quiz Frage-Qualitaetsbewertung` als naechster Task aus dem Backlog nach `NEXT.md` verschoben.
- Tests / Prüfung:
  - Statische Pruefung und Kompilationspruefung siehe Abschlussbericht.
- Risiken / Follow-Ups:
  - Finale Themenfortschritts-UX ist weiterhin offen.
  - Savegame-Persistenz fuer Themenfortschritt ist noch nicht angebunden.
  - Bestehende QuizSets benoetigen gepflegte `Topic`-Werte fuer aussagekraeftige Themenanzeige.
- Commit:
  - Vorschlag: `feat: show quiz topic progress`

### [2026-04-26] Quiz Dynamic Difficulty
- Beschreibung:
  - Optionale Difficulty-Empfehlung fuer Quizrunden vorbereitet.
- Betroffene Systeme:
  - `Assets/Projekt/Runtime/Features/Quiz/`
  - `README.md`
  - `docs/quiz/QUIZ_FEATURE.md`
  - `docs/quiz/QUIZ_ARCHITECTURE.md`
  - `docs/quiz/CODEX_TASK_TOPIC_PROGRESS.md`
  - `docs/task/NEXT.md`
  - `docs/task/BACKLOG.md`
  - `docs/task/DONE.md`
- Wichtige Änderungen:
  - `QuizDifficulty`, `QuizDifficultyPerformance` und `QuizDifficultyEvaluator` ergaenzt.
  - `QuizQuestion` um optionale Metadaten `QuestionId`, `Topic` und `Difficulty` erweitert.
  - `QuizRunner` zaehlt beantwortete und richtige Fragen und stellt `RecommendedNextDifficulty` bereit.
  - Multiple-Choice- und FreeText-Bewertung bleiben inhaltlich unveraendert.
  - `Quiz Themen-Fortschritt visualisieren` als naechster Task aus dem Backlog nach `NEXT.md` verschoben.
- Tests / Prüfung:
  - Statische Pruefung und Kompilationspruefung siehe Abschlussbericht.
- Risiken / Follow-Ups:
  - Bestehende QuizSets werden nicht automatisch nach Schwierigkeit gefiltert oder umsortiert.
  - Konkrete UI-/Progress-Anbindung fuer Difficulty bleibt ein spaeterer Integrationsschritt.
- Commit:
  - Vorschlag: `feat: prepare quiz dynamic difficulty`

### [2026-04-26] StartScene Feature Integration
- Beschreibung:
  - Vorbereitete MVP-Features defensiv fuer die `StartScene` erreichbar gemacht.
- Betroffene Systeme:
  - `Assets/Projekt/Runtime/Core/SceneManagement/StartSceneFeatureBootstrap.cs`
  - `Assets/Projekt/Runtime/Features/DevTools/`
  - `README.md`
  - `docs/task/DONE.md`
- Wichtige Änderungen:
  - `StartSceneFeatureBootstrap` erzeugt bei Bedarf `StartSceneRuntimeFeatures`.
  - Optionale Manager werden in der `StartScene` initialisiert: Settings, Progress, Scenario, Achievements, Skills, RuntimeInventory, Toolbelt und DevPanel.
  - DevPanel um Debug-Aktionen fuer Feature-Manager, Demo-Skill-XP und Demo-Achievement erweitert.
  - Keine Szene-, Prefab-, Arthur-, Bernd- oder Animator-Controller-Referenzen manuell umverdrahtet.
- Tests / Prüfung:
  - Statische Pruefung und Kompilationspruefung siehe Abschlussbericht.
- Risiken / Follow-Ups:
  - NPC Routines bleiben absichtlich nicht an Arthur oder Bernd angebunden.
  - Konkrete UI-Panels fuer Achievements, Skills und Inventory sind weiterhin manuelle Folgearbeit.
  - Savegame-Persistenz fuer neue Profile bleibt offen.
- Commit:
  - Vorschlag: `feat: integrate prepared features in start scene`

### [2026-04-25] Quiz FreeText Bewertung
- Beschreibung:
  - Freitext-Bewertung im Quiz-System robuster vorbereitet.
- Betroffene Systeme:
  - `Assets/Projekt/Runtime/Features/Quiz/`
  - `Assets/Projekt/Runtime/Features/UI/Panels/QuizPanel.cs`
  - `README.md`
  - `docs/quiz/QUIZ_FEATURE.md`
  - `docs/quiz/QUIZ_ARCHITECTURE.md`
  - `docs/task/NEXT.md`
  - `docs/task/BACKLOG.md`
  - `docs/task/DONE.md`
- Wichtige Änderungen:
  - `QuizTextAnswerEvaluator` erstellt.
  - `QuizQuestion` um akzeptierte Freitextantworten und Fuzzy-Grenzen erweitert.
  - `QuizRunner.AnswerCurrentQuestion(string)` ergaenzt, ohne den Multiple-Choice-Pfad zu veraendern.
  - `QuizResult` kann optional die Freitextantwort tragen.
  - `QuizPanel` zeigt fuer reine Freitextfragen optional ein Eingabefeld und einen Submit-Button.
  - `Quiz Dynamic Difficulty` als naechster Task aus dem Backlog nach `NEXT.md` vorbereitet.
- Tests / Prüfung:
  - Statische Pruefung der Namespaces und Dateien.
  - `git diff --check` ohne Whitespace-Fehler; nur bestehende Line-Ending-Warnungen.
  - Unity Batchmode Script-Compilation erfolgreich: `Tundra build success`.
- Risiken / Follow-Ups:
  - Freitextfragen benoetigen gepflegte `AcceptedTextAnswers` in QuizSet-Daten.
  - Fuzzy-Matching ist bewusst konservativ und sollte mit echten Fragen getestet werden.
  - Noch keine Editor-Tools fuer Synonympflege.
- Commit:
  - Vorschlag: `feat: improve quiz free text evaluation`

### [2026-04-25] Achievement System
- Beschreibung:
  - Kleines Achievement-System fuer Lern- und Gameplay-Meilensteine als MVP vorbereitet.
- Betroffene Systeme:
  - `Assets/Projekt/Runtime/Features/Achievements/`
  - `README.md`
  - `docs/core/FEATURE_REGISTRY.md`
  - `docs/achievements/ACHIEVEMENT_FEATURE.md`
  - `docs/task/NEXT.md`
  - `docs/task/BACKLOG.md`
  - `docs/task/DONE.md`
- Wichtige Änderungen:
  - `AchievementDefinition`, `AchievementProgress`, `AchievementProfile` und `AchievementManager` erstellt.
  - Unlock-, Progress- und Query-API vorbereitet.
  - Demo-Achievements `first_login`, `first_quiz` und `network_beginner` vorbereitet.
  - Keine direkte Kopplung an Progress, Skills, HUD, UI oder Savegame eingebaut.
  - `Quiz FreeText Bewertung` als naechster Task aus dem Backlog nach `NEXT.md` vorbereitet.
- Tests / Prüfung:
  - Statische Pruefung der Namespaces und Dateien.
  - `git diff --check` ohne Whitespace-Fehler; nur bestehende Line-Ending-Warnungen.
  - Unity Batchmode Script-Compilation erfolgreich: `Tundra build success`.
- Risiken / Follow-Ups:
  - Noch keine Scene-/Prefab-Anbindung.
  - Keine Savegame-Persistenz.
  - HUD-, Progress-, Skill- und Quiz-Anbindung sollten spaeter kontrolliert ueber Adapter erfolgen.
- Commit:
  - Vorschlag: `feat: add achievement system`

### [2026-04-25] Skill / Level System
- Beschreibung:
  - Leichtgewichtiges Skill- und Level-System als MVP vorbereitet.
- Betroffene Systeme:
  - `Assets/Projekt/Runtime/Features/Skills/`
  - `README.md`
  - `docs/core/FEATURE_REGISTRY.md`
  - `docs/skills/SKILL_LEVEL_FEATURE.md`
  - `docs/task/NEXT.md`
  - `docs/task/BACKLOG.md`
  - `docs/task/DONE.md`
- Wichtige Änderungen:
  - `SkillDefinition`, `SkillProgress`, `SkillProfile` und `SkillRuntimeManager` erstellt.
  - XP-Vergabe, Level-Up-Regeln und Level-Up-Events vorbereitet.
  - Demo-Skills `networking`, `support` und `terminal` vorbereitet.
  - Keine direkte Kopplung an PlayerSession, ProgressManager, UI oder Savegame eingebaut.
  - `Achievement System` als naechster Task aus dem Backlog nach `NEXT.md` vorbereitet.
- Tests / Prüfung:
  - Statische Pruefung der Namespaces und Dateien.
  - `git diff --check` ohne Whitespace-Fehler; nur bestehende Line-Ending-Warnungen.
  - Unity Batchmode Script-Compilation erfolgreich: `Tundra build success`.
- Risiken / Follow-Ups:
  - Noch keine Scene-/Prefab-Anbindung.
  - Keine Savegame-Persistenz.
  - Quiz-, Quest- und Scenario-Anbindung sollten spaeter kontrolliert ueber Adapter erfolgen.
- Commit:
  - Vorschlag: `feat: add skill level system`

### [2026-04-25] Inventory / Toolbelt
- Beschreibung:
  - Leichtgewichtiges Inventar- und Toolbelt-System als MVP vorbereitet.
- Betroffene Systeme:
  - `Assets/Projekt/Runtime/Features/Inventory/`
  - `README.md`
  - `docs/core/FEATURE_REGISTRY.md`
  - `docs/inventory/INVENTORY_TOOLBELT_FEATURE.md`
  - `docs/task/NEXT.md`
  - `docs/task/BACKLOG.md`
  - `docs/task/DONE.md`
- Wichtige Änderungen:
  - `InventoryItemCategory`, `InventoryItemData`, `InventoryItemStack`, `RuntimeInventory` und `ToolbeltController` erstellt.
  - Runtime-Inventar unterstuetzt Add/Remove/Contains/Clear und meldet Aenderungen per Event.
  - Toolbelt verwaltet Slot-Zuweisungen, Auswahl und Use-Events.
  - Keine harte Kopplung an UI, Savegame oder World Interaction eingebaut.
  - `Skill / Level System` als naechster Task aus dem Backlog nach `NEXT.md` vorbereitet.
- Tests / Prüfung:
  - Statische Pruefung der Namespaces und Dateien.
  - `git diff --check` ohne Whitespace-Fehler; nur bestehende Line-Ending-Warnungen.
  - Unity Batchmode Script-Compilation erfolgreich: `Tundra build success`.
- Risiken / Follow-Ups:
  - Noch keine Scene-/Prefab-Anbindung.
  - Keine Savegame-Persistenz.
  - Kein Pickup-Adapter fuer World Interaction; dieser sollte spaeter kontrolliert ergaenzt werden.
- Commit:
  - Vorschlag: `feat: add inventory toolbelt system`

### [2026-04-25] NPC Routine System
- Beschreibung:
  - Einfaches optionales Routine-System fuer NPC-Ablaufplaene als MVP vorbereitet.
- Betroffene Systeme:
  - `Assets/Projekt/Runtime/Features/NPC/Routines/`
  - `Assets/Projekt/Runtime/Features/NPC/Readme.md`
  - `README.md`
  - `docs/core/FEATURE_REGISTRY.md`
  - `docs/npc/NPC_ROUTINE_FEATURE.md`
  - `docs/task/NEXT.md`
  - `docs/task/BACKLOG.md`
  - `docs/task/DONE.md`
- Wichtige Änderungen:
  - `NpcRoutineStepType`, `NpcRoutineStep` und `NpcRoutineController` erstellt.
  - Routine-Schritte fuer `Wait`, `LookDirection` und `MoveToPoint` vorbereitet.
  - Animator-Parameter `MoveX`, `MoveY` und `IsMoving` werden defensiv nur gesetzt, wenn vorhanden.
  - Arthur und Bernd wurden nicht migriert oder veraendert.
  - `Inventory / Toolbelt` als naechster Task aus dem Backlog nach `NEXT.md` vorbereitet.
- Tests / Prüfung:
  - Statische Pruefung der Namespaces und Dateien.
  - `git diff --check` ohne Whitespace-Fehler; nur bestehende Line-Ending-Warnungen.
  - Unity Batchmode Script-Compilation erfolgreich: `Tundra build success`.
- Risiken / Follow-Ups:
  - Noch keine Scene-/Prefab-Anbindung.
  - Keine Savegame-Persistenz.
  - Konkrete Arthur-/Bernd-Anbindung sollte spaeter nur kontrolliert erfolgen, damit keine doppelte Bewegungslogik entsteht.
- Commit:
  - Vorschlag: `feat: add npc routine system`

### [2026-04-25] Settings System
- Beschreibung:
  - Zentrales Settings-System fuer Audio, Video, Input und Gameplay als MVP umgesetzt.
- Betroffene Systeme:
  - `Assets/Projekt/Runtime/System/Settings/`
  - `Assets/Projekt/Runtime/Features/DevTools/DevPanelController.cs`
  - `README.md`
  - `docs/core/FEATURE_REGISTRY.md`
  - `docs/settings/SETTINGS_TODO.md`
  - `docs/task/NEXT.md`
  - `docs/task/DONE.md`
- Wichtige Änderungen:
  - `SettingsData`, `SettingsManager` und `SettingsUIController` erstellt.
  - Settings werden als `settings.json` unter `Application.persistentDataPath` gespeichert.
  - Defaultwerte, Sanitizing, Reset, Save/Load und Runtime-Apply fuer Audio-/Video-Basiswerte umgesetzt.
  - `SettingsUIController` kann optionale Slider, Toggles, Dropdowns und Inputfelder per Inspector anbinden.
  - DevPanel-Reset nutzt jetzt den zentralen `SettingsManager`.
- Tests / Prüfung:
  - `git diff --check` ohne Whitespace-Fehler; nur bestehende Line-Ending-Warnung.
  - Unity Batchmode Script-Compilation erfolgreich: `Tundra build success`.
- Risiken / Follow-Ups:
  - Noch keine Scene-/Prefab-Anbindung.
  - Music-/SFX-Volume werden gespeichert, aber noch nicht an konkrete AudioMixer/AudioSource-Gruppen angebunden.
  - Input-Rebinding speichert Tastenwerte, ist aber noch nicht an `PlayerControls.inputactions` gekoppelt.
- Commit:
  - Vorschlag: `feat: add settings system`

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
