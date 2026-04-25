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
  - Richtungsbasierte Idle-/Walk-Animationen mit gemerkter Blickrichtung
- 🧑‍🏫 **NPC Bernd + Quiz**
  - Bernd ist in `StartScene` als NPC mit Sprite, Animator, Trigger und Namensanzeige angelegt
  - Namensanzeige nutzt wie Arthur ein World-Space-`NameTagCanvas` und wird nur bei Player-Naehe eingeblendet
  - Sprite-Import ist auf Player-/Arthur-Basis angepasst (`16 PPU`, `16x32` Slices, Bottom-Center Pivot, Point Filter)
  - Idle- und Walk-Basisclips sind im Bernd Animator Controller eingebunden
  - Interaktion per **E** startet ein lokales, erweiterbares Quiz
  - Quiz-Fragen liegen als `QuizSet`-Datenmodell unter `Assets/Projekt/Content/Quiz/`
  - Quiz-UI wird ueber `QuizPanel` geoeffnet und enthaelt keine hart codierten Fragen
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
│ │ │ ├── BerndQuizStarter
│ │ │ └── BerndNameTag
│ │ ├── Quiz/
│ │ │ ├── QuizSet
│ │ │ ├── QuizQuestion
│ │ │ ├── QuizAnswerOption
│ │ │ ├── QuizRunner
│ │ │ └── QuizResult
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
