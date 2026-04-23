# 📦 Assets

## Zweck
Der `Assets`-Ordner enthält alle Inhalte des Unity-Projekts – sowohl visuelle Ressourcen als auch die komplette Spiellogik.

Er ist in zwei Hauptbereiche unterteilt:
- **Content** → Spielinhalte (Grafik, Audio, Szenen)
- **Runtime** → Spiellogik und Systeme

---

## 📁 Struktur

    Assets/
    ├── _Recovery/                     # Unity interne Wiederherstellung (ignorieren)
    ├── Projekt/
    │   ├── Content/                  # Spielinhalte (keine Logik)
    │   └── Runtime/                  # Spiellogik
    │       ├── Features/
    │       │   ├── NPC/Arthur/       # Arthur NPC + Interaktion
    │       │   ├── Player/Movement/  # Player Steuerung & Bewegung
    │       │   └── UI/               # UI Panels & Manager
    │       └── System/Savegame/      # Save/Load System
    ├── Settings/                     # Projekt-Konfigurationen
    ├── ScriptableObjects/            # Daten-Assets
    ├── TextMesh Pro/                 # UI/Text-System
    └── PlayerControls.inputactions   # Input-System (New Input System)

---

## 🧩 Content vs Runtime

### 🎨 Content

Enthält alles Visuelle und Statische:

- Art → Grafiken und Animationen  
- Audio → Sounds und Musik  
- Materials → Materialien  
- Prefabs → wiederverwendbare Objekte  
- Scenes → Unity Szenen  

👉 **Keine Spiellogik**

---

### ⚙️ Runtime

Enthält die komplette Spiellogik:

#### Core (optional / erweiterbar)
- Basis-Logik (Singletons, globale Systeme)

#### Features
- **Player**
  - `PlayerController`
  - `PlayerMotor2D`
  - `PlayerInputReader`

- **NPC / Arthur**
  - `ArthurAutoInteraction`
  - `ArthurMovementToPlayer`
  - `ArthurAnimationController`
  - `ArthurNameUI`

👉 ArthurDetectionZone wurde entfernt → Logik direkt in `ArthurAutoInteraction`

- **UI**
  - `MenuManager`
  - `LoadGamePanel`
  - `SaveSlotItemUI`

#### System

- **Savegame**
  - `SaveSystem`
  - `SaveGameData`
  - `SaveSlotEntity`
  - `SavegameRuntimeSession`
  - `DummySaveBootstrap`

👉 Speicherung erfolgt als JSON über `Application.persistentDataPath`

---

## 🔗 Verbindung Content ↔ Runtime

- Content stellt Objekte bereit (Prefabs, Scenes)
- Runtime steuert deren Verhalten (Scripts)

### Beispiel: Player

- Prefab in `Content/Prefabs`
- Instanziert in Scene
- Gesteuert durch:
  - `PlayerController`
  - `PlayerMotor2D`

### Beispiel: Arthur NPC

- Objekt in Scene
- Trigger über Collider
- Verhalten über:
  - `ArthurAutoInteraction`

---

## 🎮 Gameplay Flow (aktueller Stand)

### Arthur Interaktion

1. Player betritt Trigger-Zone  
2. Arthur läuft zum Player  
3. Player wird **gelockt**  
4. **StartMenu öffnet sich**  
5. Spieler wählt „Laden“  
6. **LoadGamePanel öffnet sich**

---

### Save / Load System

- Speicherung pro Slot:
save_slot_1.json
save_slot_2.json


- Dummy Save:
- wird automatisch erstellt
- dient Testzwecken

- Ladeablauf:
1. Slot auswählen  
2. SaveGameData laden  
3. in `SavegameRuntimeSession` speichern  
4. Szene wechseln  

---

## 🧠 Architekturprinzip

Das Projekt folgt einer **Feature-basierten Architektur**:

- Struktur nach Systemen (Player, NPC, UI)
- keine zentralen „Scripts“-Ordner
- klare Verantwortlichkeiten
- lose Kopplung
- einfache Erweiterbarkeit

---

## ⚠️ Hinweise

- `_Recovery/` wird von Unity automatisch verwaltet  
- `TextMesh Pro/` ist ein externes UI-System  
- `PlayerControls.inputactions` nutzt das neue Input System  
- Savegames liegen außerhalb des Projekts (persistentDataPath)

---

## 🚧 Aktueller Stand

Bereits umgesetzt:

- Player Movement (Input + Physics getrennt)
- Arthur NPC (Movement + Interaction)
- UI System (StartMenu + LoadGamePanel)
- Save/Load System (JSON basiert)
- Dummy Save für Tests

---

## 🎯 Nächste Schritte

- Player Spawn aus SaveGameData
- Position beim Laden setzen
- Speichern im Spiel
- Weitere NPCs (z. B. „Bernd“)
- Dialogsystem / Quest-System

---

## 🎯 Ziel

Der `Assets`-Ordner ist so aufgebaut, dass:

- Logik klar getrennt ist
- Systeme modular erweitert werden können
- UI, NPC und Save-System sauber zusammenspielen
- das Projekt langfristig skalierbar bleibt