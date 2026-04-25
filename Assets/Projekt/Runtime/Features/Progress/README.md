# Quest / Progress System

## Zweck
Das Progress-System haelt Lernfortschritt und Quest-Zustaende im Speicher. Es ist ein MVP ohne Savegame-Persistenz und ohne harte Kopplung an Bernd, QuizPanel oder konkrete UI.

## Dateien
- `QuestDefinition.cs` beschreibt eine Quest als Unity-Asset.
- `QuestProgress.cs` speichert den Fortschritt einer Quest.
- `ProgressProfile.cs` buendelt Quests, Quiz-Statistik und Themenfortschritt.
- `ProgressManager.cs` startet, erhoeht und beendet Quests.
- `QuizProgressReporter.cs` meldet Quiz-Ergebnisse an den ProgressManager.

## Demo-Quests
Wenn `Create Demo Quest Definitions` aktiv ist, erzeugt der `ProgressManager` zur Laufzeit:
- `talk_to_bernd`
- `answer_3_dns_questions`
- `complete_easy_quiz`

## Unity Setup
1. GameObject `ProgressManager` in die Szene legen.
2. `ProgressManager` hinzufuegen.
3. Optional eigene `QuestDefinition`-Assets ueber `Create > IT-AA > Progress > Quest Definition` erstellen und zuweisen.
4. Optional `QuizProgressReporter` neben Quiz-Komponenten platzieren und den `ProgressManager` zuweisen.

## Verhalten
- `StartQuest(string questId)` startet eine Quest.
- `AddQuestProgress(string questId, int amount)` erhoeht Fortschritt.
- `CompleteQuest(string questId)` schliesst eine Quest ab.
- `ReportQuizAnswer(topic, isCorrect)` aktualisiert Quiz-Statistik.
- `ReportQuizCompleted(...)` markiert Quiz-Abschluss und kann Demo-Quests abschliessen.

## Savegame
Persistenz ist vorbereitet, aber noch nicht angebunden. `ProgressProfile` ist serialisierbar und kann spaeter in `SaveGameData` oder eine eigene Progress-Datei uebernommen werden.
