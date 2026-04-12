🎮 IT-AA 2D Projekt
Ein modulares 2D-Spielprojekt in Unity mit Fokus auf sauberer Architektur, klarer Trennung von Content und Runtime sowie langfristiger Erweiterbarkeit.
---
📁 Projektstruktur
Root
Assets/ → Hauptinhalt des Projekts
Database/ → Externe Datenbank (z. B. SQLite)
Packages/ → Unity Package Manager
ProjectSettings/ → Unity Projekteinstellungen
---
🧩 Assets
Assets/  
├── Projekt/  
│   ├── Content/  
│   │   ├── Art/ → Sprites, Animationen, Tiles  
│   │   ├── Audio/ → Musik und Soundeffekte  
│   │   ├── Materials/ → Materialien  
│   │   ├── Prefabs/ → Wiederverwendbare GameObjects  
│   │   └── Scenes/ → Unity Szenen  
│   │  
│   └── Runtime/  
│       ├── Core/ → Basis-Systeme (Bootstrap, Events, Utilities)  
│       ├── Data/ → Datenmodelle und Speicherung  
│       ├── Features/ → Gameplay-Systeme (Player, NPC, UI, World)  
│       ├── System/ → Globale Manager (Game, Save, Scene)  
│       └── PersistentSingleton.cs  
│  
├── Settings/ → Input, Render, globale Settings  
└── PlayerControls.inputactions
---
🧠 Architekturprinzip
Das Projekt folgt einer klaren Trennung zwischen Content und Runtime.
Content enthält alle statischen Daten wie Grafiken, Sounds und Prefabs.  
Runtime enthält sämtliche Logik, Systeme und Code.
---
🎮 Wichtige Systeme
👤 Player
Player/  
├── Movement/ → Bewegungssystem  
├── Session/ → Spielzustand (Stats, Progress, Position)  
└── UI/ → Spieleranzeige
🤖 NPC
NPC/  
├── Interactions/ → Interaktion (Trigger, Input, Prompt)  
├── Dialogue/ → Dialogsystem (geplant)  
├── Behaviour/ → Verhalten (geplant)
🧭 UI
UI/  
├── Panels/ → Menüs (Start, LoadGame, Pause)  
├── Items/ → Listenelemente (z. B. SaveSlots)  
├── Widgets/ → UI-Komponenten (Bars, Timer etc.)
---
🔄 Gameplay-Flow
StartScene → NPC Interaktion → StartMenu → LoadGamePanel → Slot Auswahl → Gameplay
---
🧱 Code-Standards
Eine Klasse = eine Verantwortung
Feature-basierte Struktur
Trennung von Input, Logik und UI
---
🚀 Aktueller Stand
StartMenu implementiert
LoadGamePanel mit dynamischen Slots
Horizontales Scroll-System
NPC Interaktion
Grundstruktur vorhanden
---
🔮 Geplante Features
Dialogsystem
Quest-System
Inventory-System
SaveGame mit echten Daten
UI-Verbesserungen
---
🛠 Entwicklung
Repo klonen
In Unity öffnen
StartScene laden
Play drücken
---
👨‍💻 Ziel
Saubere, skalierbare Architektur für modulare Erweiterbarkeit.