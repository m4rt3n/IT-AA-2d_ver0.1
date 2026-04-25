# IT-AA 2D – Unity Game

<p align="center">
  <img src="https://img.shields.io/badge/Unity-2D-black?logo=unity&style=for-the-badge" />
  <img src="https://img.shields.io/badge/C%23-Game%20Dev-blue?logo=csharp&style=for-the-badge" />
  <img src="https://img.shields.io/badge/Status-In%20Development-yellow?style=for-the-badge" />
  <img src="https://img.shields.io/badge/Architecture-Feature--Based-green?style=for-the-badge" />
</p>

<p align="center">
  <b>Modulares 2D-Game-Framework mit sauberer Architektur & skalierbarem Design</b>
</p>

---

## 📸 Screenshots

<p align="center">
 <img width="45%" alt="image" src="https://github.com/user-attachments/assets/f9c9c1ed-5d41-4121-8219-51f510543883" />
</p>

---

## 📖 Overview

**IT-AA 2D** ist ein modular aufgebautes Unity-2D-Projekt mit Fokus auf:

- **saubere Architektur**
- **klare Systemtrennung**
- **skalierbare Erweiterbarkeit**

Das Projekt dient als **Framework + Lernplattform**, insbesondere für strukturierte Game-Entwicklung und IT-nahe Szenarien.

---

## ✨ Features (aktueller Stand)

- 🧍 **Player Movement System**
  - Input → Controller → Motor (sauber getrennt)
- 🤖 **NPC System (Arthur)**
  - Auto-Approach zum Player
  - Trigger-basierte Interaktion
  - Oeffnet das Startmenue aktuell automatisch ueber `openMenuWhenArthurReachesPlayer`
  - `ArthurAutoInteraction.interactAction` bleibt in der `StartScene` bewusst leer; fuer spaetere manuelle Interaktion ist `Player/Interact` aus `Assets/PlayerControls.inputactions` vorgesehen
  - Animator nutzt in der `StartScene` die vorhandenen Blend-Tree-States `Arthur_Idle` und `Arthur_Walk`; Richtung wird ueber `MoveX`, `MoveY` und `IsMoving` gesteuert
- 🧭 **NPC Routine System (MVP)**
  - Neues optionales Routine-Feature unter `Assets/Projekt/Runtime/Features/NPC/Routines/`
  - Bietet einfache Schrittsequenzen fuer Warten, Blickrichtung und Bewegung zu Zielpunkten
  - Bleibt bewusst unabhaengig von Arthur und Bernd, damit bestehende Interaktionen stabil bleiben
  - Noch keine Scene-/Prefab-Anbindung und keine Savegame-Persistenz
- 🧑‍🏫 **NPC Bernd + Quiz**
  - Bernd ist in `StartScene` als NPC mit Sprite, Animator, Trigger und Namensanzeige angelegt
  - Namensanzeige nutzt wie Arthur ein World-Space-`NameTagCanvas` und wird nur bei Player-Naehe eingeblendet
  - Sprite-Import ist auf Player-/Arthur-Basis angepasst (`16 PPU`, `16x32` Slices, Bottom-Center Pivot, Point Filter)
  - Idle- und Walk-Basisclips sind im Bernd Animator Controller eingebunden
  - Interaktion per **E** startet ein lokales, erweiterbares Quiz
  - Quiz-Fragen liegen als `QuizSet`-Datenmodell unter `Assets/Projekt/Content/Quiz/`
  - Quiz-UI wird ueber `QuizPanel` geoeffnet und enthaelt keine hart codierten Fragen
  - Freitextfragen koennen akzeptierte Antworten mit Normalisierung und vorsichtigem Fuzzy-Matching auswerten
- 🤝 **World Interaction System (MVP)**
  - Neues Feature unter `Assets/Projekt/Runtime/Features/Interaction/`
  - Erkennt interaktive Ziele ueber `IInteractable`
  - Zeigt optional einen Prompt wie `E druecken`
  - Loest Interaktionen ueber Unity Input System oder E-Fallback aus
  - Bernd besitzt einen optionalen `BerndInteractableAdapter`, der an die bestehende Quiz-Logik delegiert
  - Arthur bleibt vorerst unveraendert, bis der Menue-Flow sauber migriert werden kann
- 🧭 **Scenario System (MVP)**
  - Neues Feature unter `Assets/Projekt/Runtime/Features/Scenarios/`
  - Buendelt IT-Lernsituationen als datengetriebene `ScenarioDefinition`
  - Verwaltet Schritte und Fortschritt ueber `ScenarioManager`
  - Enthaltenes Demo-Szenario: `no_internet_basic` / `Kein Internet`
  - HUD, Quiz, Dialog und Savegame bleiben entkoppelt und koennen spaeter andocken
- 🛠️ **DevPanel (MVP)**
  - Neues optionales Entwicklerwerkzeug unter `Assets/Projekt/Runtime/Features/DevTools/`
  - Runtime-Panel kann per `F12` geoeffnet werden
  - Bietet Debug-Aktionen fuer SaveSlots, Dummy-Saves, Settings, Quiz-Drafts, PlayerSession und aktuelle Szene
  - Nutzt bestehende Systeme defensiv und meldet fehlende Abhaengigkeiten per `Debug.LogWarning`
- 📚 **Knowledge Base (MVP)**
  - Neues Lexikon-Feature unter `Assets/Projekt/Runtime/Features/KnowledgeBase/`
  - Enthält Demo-Artikel zu DNS, DHCP, Gateway, VPN und OSI-Modell
  - UI-Panel trennt Artikel-Daten, Suche und Anzeige
  - Quiz- und Szenario-Integration ist ueber Artikel-IDs vorbereitet
- 💬 **Dialogue System (MVP)**
  - Neues Dialog-Feature unter `Assets/Projekt/Runtime/Features/Dialogue/`
  - Dialogdaten liegen in `DialogueSequence` und `DialogueLine`
  - `DialogueManager` startet Dialoge und fuehrt optional einen Abschluss-Callback aus
  - `DialoguePanel` kann seine einfache MVP-UI selbst erzeugen
  - Arthur/Bernd sind vorbereitet, aber noch nicht automatisch migriert
- 📈 **Quest / Progress System (MVP)**
  - Neues Fortschritts-Feature unter `Assets/Projekt/Runtime/Features/Progress/`
  - Verwaltet Quests und Quiz-Statistiken im Speicher
  - Demo-Quests: `talk_to_bernd`, `answer_3_dns_questions`, `complete_easy_quiz`
  - `QuizProgressReporter` bereitet die spätere Quiz-Anbindung vor
  - Savegame-Persistenz ist vorbereitet, aber noch nicht aktiv angebunden
- 🎒 **Inventory / Toolbelt (MVP)**
  - Neues Inventar-Feature unter `Assets/Projekt/Runtime/Features/Inventory/`
  - Bietet Item-Datenmodell, Item-Stacks und Runtime-Inventar
  - `ToolbeltController` verwaltet Slot-Zuweisung, Auswahl und Use-Events
  - Bleibt ohne harte UI-, Savegame- oder World-Interaction-Kopplung
- 📊 **Skill / Level System (MVP)**
  - Neues Skill-Feature unter `Assets/Projekt/Runtime/Features/Skills/`
  - Verwaltet Skill-Definitionen, XP, Level und Level-Up-Events im Speicher
  - Enthaltene Demo-Skills: `networking`, `support`, `terminal`
  - Bleibt ohne harte Kopplung an PlayerSession, ProgressManager, UI oder Savegame
- 🏆 **Achievement System (MVP)**
  - Neues Achievement-Feature unter `Assets/Projekt/Runtime/Features/Achievements/`
  - Verwaltet Achievement-Definitionen, Unlock-Status und Fortschritt im Speicher
  - Enthaltene Demo-Achievements: `first_login`, `first_quiz`, `network_beginner`
  - Bleibt ohne harte Kopplung an Progress, Skills, HUD, UI oder Savegame
- 🧭 **HUD System (MVP)**
  - Neues HUD-Feature unter `Assets/Projekt/Runtime/Features/HUD/`
  - Zeigt Spielername, aktuelles Ziel, Quizpunkte, Thema und kurze Meldungen
  - Nutzt `PlayerSession` und optional `ProgressManager`
  - Liest nicht direkt aus dem SaveSystem und bleibt dadurch optional einsetzbar
- 🖥️ **IT Terminal Minigames (MVP)**
  - Neues Terminal-Feature unter `Assets/Projekt/Runtime/Features/Terminal/`
  - Simuliert IT-Support-Befehle wie `help`, `ipconfig`, `ping`, `nslookup`, `clear` und `exit`
  - Fuehrt keine echten OS-Befehle aus und nutzt keine echten Netzwerkzugriffe
  - Trennt Befehlsdaten, Emulator-Logik und UI-Panel
  - `TerminalPanel` kann eine einfache MVP-UI selbst erzeugen oder per Inspector verdrahtet werden
- ⚙️ **Settings System (MVP)**
  - Neues globales Settings-System unter `Assets/Projekt/Runtime/System/Settings/`
  - Speichert Audio-, Video-, Input- und Gameplay-Basiswerte als `settings.json`
  - Nutzt `Application.persistentDataPath` und legt bei fehlender Datei Defaultwerte an
  - `SettingsManager` stellt zentrale Getter, Reset, Apply und Save/Load bereit
  - `SettingsUIController` kann optionale Slider, Toggles, Dropdowns und Inputfelder per Inspector anbinden
  - DevPanel-Reset nutzt jetzt den zentralen `SettingsManager`
- 🧩 **UI System**
  - MenuManager (zentrale Steuerung)
  - StartMenu + LoadGamePanel
  - Leere Panel-Platzhalter wurden als minimale `BasePanel`-Ableitungen bereinigt (`DialoguePanel`, `InventoryPanel`, `QuestLogPanel`)
  - Einfache Daten-, Player-Session-, Player-UI- und Widget-Klassen wurden auf passende `ITAA.*` Namespaces und Kopfkommentare gebracht
  - Close im LoadGamePanel führt sauber zurück ins Startmenü
  - Großer Save-Slot zeigt Dummy-/Save-Daten jetzt strukturiert an
  - Klick auf die große Slot-Karte oder den Button lädt belegte Slots direkt
  - Aktiver großer Slot ist farblich hervorgehoben und besser lesbar
  - SaveSlotItem verwendet getrennte TMP-Felder: `SlotNameText`, `TitleText`, `SceneNameText`, `SavedAtText`, `StatusText`
- 💾 **Save/Load System**
  - JSON-basiert
  - Slot-System
  - Dummy Save für Tests
  - Dummy-Saves verweisen aktuell auf `StartScene`, da derzeit nur diese Laufzeit-Szene in den Build Settings aktiv ist
  - Vor Spielstart Prüfung und Initialisierung benötigter Dateien
  - Ladebalken mit Prozentanzeige vor Szenenstart
- 🔁 **Runtime Session**
  - Übergabe von Save-Daten zwischen Szenen
- 🏷️ **Player Name Tag**
  - Spielername wird aus dem geladenen SaveSlot übernommen
  - Anzeige direkt über dem Player
- 🎮 **Menü-Flow**
  - Arthur → StartMenu → LoadGamePanel → Scene Load

---

## 🧠 Architektur

Feature-driven Architecture:

- Systeme sind **isoliert**
- klare Verantwortlichkeiten
- minimale Abhängigkeiten

**Prinzipien:**

- Separation of Concerns  
- Single Responsibility  
- Lose Kopplung  
- Wiederverwendbarkeit  
- Skalierbarkeit  

---


## 📂 Projektstruktur
Assets/
├── Projekt/
│ ├── Content/
│ │ ├── Art/
│ │ ├── Audio/
│ │ ├── Materials/
│ │ ├── Prefabs/
│ │ └── Scenes/
│ │
│ └── Runtime/
│ ├── Core/
│ │ └── SceneManagement/
│ │ └── SceneNames
│ ├── Features/
│ │ ├── Player/
│ │ │ └── Movement/
│ │ │ ├── PlayerController
│ │ │ ├── PlayerMotor2D
│ │ │ └── PlayerInputReader
│ │ │
│ │ ├── NPC/
│ │ │ └── Arthur/
│ │ │ ├── ArthurAutoInteraction
│ │ │ ├── ArthurMovementToPlayer
│ │ │ ├── ArthurAnimationController
│ │ │ └── ArthurNameUI
│ │ │ └── Bernd/
│ │ │ ├── BerndAutoInteraction
│ │ │ ├── BerndMovementToPlayer
│ │ │ ├── BerndAnimationController
│ │ │ ├── BerndInteractableAdapter
│ │ │ ├── BerndQuizStarter
│ │ │ └── BerndNameTag
│ │ │ └── Routines/
│ │ │ ├── NpcRoutineStepType
│ │ │ ├── NpcRoutineStep
│ │ │ └── NpcRoutineController
│ │ ├── Quiz/
│ │ │ ├── QuizSet
│ │ │ ├── QuizQuestion
│ │ │ ├── QuizAnswerOption
│ │ │ ├── QuizTextAnswerEvaluator
│ │ │ ├── QuizRunner
│ │ │ └── QuizResult
│ │ ├── Scenarios/
│ │ │ ├── ScenarioDefinition
│ │ │ ├── ScenarioStep
│ │ │ ├── ScenarioProgress
│ │ │ ├── ScenarioManager
│ │ │ └── ScenarioStatus
│ │ ├── DevTools/
│ │ │ ├── DevPanelController
│ │ │ └── DevPanelBootstrap
│ │ ├── KnowledgeBase/
│ │ │ ├── KnowledgeArticle
│ │ │ ├── KnowledgeBaseRepository
│ │ │ ├── KnowledgeBasePanel
│ │ │ ├── KnowledgeArticleListItemUI
│ │ │ └── KnowledgeTopic
│ │ ├── Dialogue/
│ │ │ ├── DialogueLine
│ │ │ ├── DialogueSequence
│ │ │ ├── DialogueManager
│ │ │ ├── DialoguePanel
│ │ │ └── IDialogueTrigger
│ │ ├── Progress/
│ │ │ ├── QuestDefinition
│ │ │ ├── QuestProgress
│ │ │ ├── ProgressProfile
│ │ │ ├── ProgressManager
│ │ │ └── QuizProgressReporter
│ │ ├── Inventory/
│ │ │ ├── InventoryItemCategory
│ │ │ ├── InventoryItemData
│ │ │ ├── InventoryItemStack
│ │ │ ├── RuntimeInventory
│ │ │ └── ToolbeltController
│ │ ├── Skills/
│ │ │ ├── SkillDefinition
│ │ │ ├── SkillProgress
│ │ │ ├── SkillProfile
│ │ │ └── SkillRuntimeManager
│ │ ├── Achievements/
│ │ │ ├── AchievementDefinition
│ │ │ ├── AchievementProgress
│ │ │ ├── AchievementProfile
│ │ │ └── AchievementManager
│ │ ├── HUD/
│ │ │ ├── HudController
│ │ │ ├── HudView
│ │ │ └── HudNotification
│ │ ├── Terminal/
│ │ │ ├── TerminalCommand
│ │ │ ├── TerminalCommandResult
│ │ │ ├── TerminalCommandType
│ │ │ ├── TerminalEmulator
│ │ │ └── TerminalPanel
│ │ │
│ │ └── UI/
│ │ ├── Managers/
│ │ │ └── MenuManager
│ │ └── Panels/
│ │ ├── LoadGamePanel
│ │ └── QuizPanel
│ │
│ └── System/
│ └── Savegame/
│ ├── SaveSystem
│ ├── SaveGameData
│ ├── SaveSlotEntity
│ ├── SavegameRuntimeSession
│ └── DummySaveBootstrap
│ └── Settings/
│ ├── SettingsData
│ ├── SettingsManager
│ └── SettingsUIController
│
├── Settings/
├── ScriptableObjects/
└── PlayerControls.inputactions


---

## 🎮 Game Flow (aktuell implementiert)

### Einstieg

StartScene  
→ StartMenu wird automatisch geöffnet  

Hinweis: Kurzfristig ist `StartScene` die zentrale Laufzeit-Szene für Menü und geladenen Dummy-Spielstand. Eine separate `GameScene` ist als späterer Portfolio-Ausbau vorgesehen.

---

### Arthur Interaktion

1. Player betritt Trigger  
2. Arthur läuft zum Player  
3. Player wird **gelockt**  
4. **StartMenu öffnet sich**

Aktueller Input-Stand:
- Arthur nutzt in der `StartScene` den automatischen Menue-Flow ueber `openMenuWhenArthurReachesPlayer`.
- `ArthurAutoInteraction.interactAction` bleibt dort bewusst leer, damit keine doppelte Eingabelogik entsteht.
- Fuer eine spaetere manuelle Wieder-Interaktion ist `Player/Interact` aus `Assets/PlayerControls.inputactions` vorgesehen.

Aktueller Animator-Stand:
- Die Szene verwendet fuer Arthur die vorhandenen States `Arthur_Idle` und `Arthur_Walk`.
- Richtung und Bewegung laufen ueber die Animator-Parameter `MoveX`, `MoveY` und `IsMoving`.
- Richtungsbasierte Einzel-State-Namen wie `Arthur_IdleDown` bleiben als spaetere Erweiterung im Code vorbereitet, werden in der aktuellen `StartScene` aber nicht als Inspector-State verwendet.

---

### Bernd Quiz-Interaktion

1. Player betritt Bernds Trigger  
2. Bernd kann zum Player laufen und richtet seine Animation aus  
3. Player drueckt **E**  
4. `BerndQuizStarter` oeffnet das `QuizPanel` mit `BerndIntroQuiz`  
5. Antworten werden durch `QuizRunner` gegen das `QuizSet` ausgewertet  

---

### Menü-Flow

StartMenu  
→ Spieler wählt **„Laden“**  
→ LoadGamePanel öffnet sich mit **Slot 1** als Standardauswahl  
→ Pfeile wechseln sauber zwischen **Slot 1 / 2 / 3**  
→ Großer Slot zeigt **Slot, Spielername, Szene, Datum, Status, Level und Score**  
→ Aktiver Slot ist mit **Highlight/Outline** sichtbar ausgewählt  
→ Klick auf den großen Slot oder **„Laden“** startet den belegten Spielstand  
→ Close führt zurück ins **StartMenu**  

---

### Load Flow

1. Slots werden geladen (JSON)
2. Dummy Save wird automatisch erstellt (falls leer)
3. Spieler wählt Slot
4. SaveGameData wird geladen
5. Daten werden in `SavegameRuntimeSession` gespeichert
6. Die im Save hinterlegte Szene wird geladen

---

## 💾 Save System

- Speicherort:
Application.persistentDataPath/Savegames/


- Format:

save_slot_1.json
save_slot_2.json


- Inhalt:
- PlayerName
- SceneName
- Position
- Level
- Score

Aktueller Dummy-Stand:
- `SceneName` wird zentral über `SceneNames.StartScene` gesetzt
- Bereits vorhandene Dummy-Saves mit altem `GameScene`-Wert werden auf `StartScene` migriert
- Dadurch bleiben Dummy-Save, Auth-Startszene und Build Settings kurzfristig synchron

---

## ⚙️ Settings System

- Speicherort:
Application.persistentDataPath/settings.json

- Inhalt:
- MasterVolume, MusicVolume, SfxVolume
- Fullscreen, ResolutionWidth, ResolutionHeight, VSync
- InteractKey, MoveUpKey, MoveDownKey, MoveLeftKey, MoveRightKey
- TextSpeed, ShowTutorials

Aktueller MVP-Stand:
- `SettingsManager` laedt und speichert Settings zentral als JSON
- fehlende oder defekte Settings fallen auf Defaultwerte zurueck
- Audio-Mastervolume und Video-Basiswerte werden runtime angewendet
- `SettingsUIController` ist vorbereitet fuer Inspector-verdrahtete Settings-UI
- echte Input-Rebinding-Anbindung an `PlayerControls.inputactions` ist noch offen

---

## 🚀 Roadmap (aktualisiert)

### 🧱 Phase 1 – Foundation
- 🟢 Architektur
- 🟢 Struktur
- 🟢 Core

### 🧍 Phase 2 – Player
- 🟢 Movement
- 🟡 Animation Finetuning
- 🔴 State Machine

### 🤖 Phase 3 – NPC
- 🟢 Arthur Movement + Interaction
- 🟡 Erweiterte Interaktion (Dialogsystem)
- 🔴 AI / Verhalten

### 🧩 Phase 4 – UI
- 🟢 MenuManager
- 🟢 LoadGamePanel (funktional)
- 🟡 UX / Navigation verbessern
- 🔴 HUD

### 💾 Phase 5 – Data
- 🟢 Save/Load Basis
- 🟡 Runtime Session
- 🔴 Persistenz erweitern (AutoSave etc.)

### 🎯 Phase 6 – Gameplay
- 🔴 Player Spawn aus Save
- 🔴 Weltzustand laden
- 🔴 Fortschritt speichern

---

## 📋 GitHub Workflow

Backlog → Ready → In Progress → Review → Done  

---

## 🛠️ Tech Stack

- Unity (2D)
- C#
- Unity Input System
- JSON (Save System)

**Geplant:**
- SQLite
- LLM Integration

---

## 📦 Installation

1. Repository klonen  
2. In Unity Hub öffnen  
3. StartScene laden  
4. Play drücken  

---

## 🤝 Contribution

Beiträge sind willkommen.

**Guidelines:**
- Feature-basiert entwickeln  
- Ein Script = eine Verantwortung  
- Klare Namensgebung  
- Saubere Trennung von Logik  

---

## 📊 Status

| Bereich        | Fortschritt |
|----------------|------------|
| Foundation     | 95%        |
| Player         | 70%        |
| NPC            | 60%        |
| UI             | 75%        |
| Data           | 60%        |
| Gameplay       | 20%        |

---

## 📄 Lizenz

Dieses Projekt dient aktuell Lern- und Entwicklungszwecken.  
Lizenzdetails folgen.  

---

## ⭐ Vision

Ein **sauberes, skalierbares Unity-Framework**, das als Grundlage dient für:

- Lernprojekte  
- Game Prototypen  
- IT-/Support-Simulationen  
- KI-gestützte Spielsysteme  
