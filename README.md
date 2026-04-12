import pypandoc

content = """# IT-AA 2D -- Unity Game

## 📖 Overview

**IT-AA 2D** ist ein modular aufgebautes 2D-Spielprojekt auf Basis von
Unity.\\
Ziel ist der Aufbau eines skalierbaren Game-Frameworks mit klarer
Architektur, das sich sowohl für Lernzwecke als auch für komplexe
Spielsysteme eignet.

Der Fokus liegt auf:
- sauberer Code-Struktur\\
- Feature-basierter Entwicklung\\
- einfacher Erweiterbarkeit (z. B. NPCs, UI, Quiz-Systeme)

------------------------------------------------------------------------

## 🧠 Architektur

Das Projekt folgt einer **Feature-driven Architecture**.\\
Jedes System ist in sich abgeschlossen und unabhängig erweiterbar.

Wichtige Prinzipien:
- Separation of Concerns\\
- Single Responsibility\\
- Lose Kopplung\\
- Wiederverwendbarkeit\\
- Skalierbarkeit

------------------------------------------------------------------------

## 📂 Projektstruktur

Assets/Projekt/Runtime

### Core

Basis-Systeme wie Manager, Utilities und globale Logik.

### Features

#### Player

- Movement (Bewegung, Input, Physik)
- Session (Spielerzustand, Fortschritt, Position)
- UI (Anzeige von Spielerinfos)

#### NPC

- Behaviour (Verhalten)
- Detection (Spieler erkennen)
- Interaction (Interaktionen & Events)

#### UI

- Menüs (Start, Login, Game UI)
- Widgets (wiederverwendbare UI-Komponenten)

### Data

- Models (Datenstrukturen)
- Storage (Speicherung, Save/Load)

### Systems

Übergreifende Systeme wie GameFlow oder globale Logik.

------------------------------------------------------------------------

## 🎮 Kernfeatures

### Player System

- 2D Bewegungssystem
- Sprint & Steuerung
- Session-Handling (Position, Fortschritt)

### NPC System

- Spielererkennung über Trigger
- Automatische Interaktionen
- Erweiterbar für Dialoge und KI

### UI System

- Menüverwaltung
- Dynamische UI-Elemente
- Zustandssteuerung

### Daten & Speicherung

- Speicherung von Spielständen
- Benutzerverwaltung
- Vorbereitung für Datenbanken

------------------------------------------------------------------------

## 🔄 Game Flow

StartScene\\
→ Menü / Login\\
→ Spieler spawnt\\
→ Bewegung & Exploration\\
→ NPC erkennt Spieler\\
→ Interaktion wird ausgelöst\\
→ UI reagiert\\
→ Fortschritt wird gespeichert

------------------------------------------------------------------------

## 🚀 Ziel des Projekts

- Aufbau eines modularen Game-Frameworks\\
- Integration dynamischer Inhalte (z. B. generierte Fragen)\\
- Grundlage für komplexe Spielsysteme

Geplante Erweiterungen:
- Quest-System\\
- Dialogsystem\\
- Fortschrittssystem\\
- Multiplayer / Online-Funktionen

------------------------------------------------------------------------

## 🧪 Entwicklungsstatus

- Architektur steht\\
- Player Movement implementiert\\
- NPC-System im Aufbau\\
- UI-System in Entwicklung\\
- Datenstruktur vorbereitet

------------------------------------------------------------------------

## 🛠️ Technologien

- Unity (2D)\\
- C#\\
- Modulare Architektur

Geplant:
- Datenbank (z. B. SQLite)\\
- KI / LLM Integration

------------------------------------------------------------------------

## 📦 Installation

Repository klonen und im Unity Hub öffnen.\\
StartScene laden und Spiel starten.

------------------------------------------------------------------------

## 📌 Roadmap

### 🧱 Phase 1 -- Foundation

- Architektur: 🟢 Fertig\\
- Projektstruktur: 🟢 Fertig\\
- Core: 🟡 Teilweise\\
- Scene Setup: 🟢 Fertig\\
- Basis UI: 🟡 In Arbeit

### 🧍 Phase 2 -- Player System

- Movement: 🟢 Fertig\\
- Animation: 🟡 In Arbeit\\
- Sprint / States: 🟡 In Arbeit\\
- Session System: 🟡 Teilweise\\
- UI: 🔴 Offen

### 🤖 Phase 3 -- NPC System

- Detection: 🟡 In Arbeit\\
- Auto Interaction: 🟡 Teilweise\\
- Movement / AI: 🔴 Offen\\
- Interaction System: 🟡 In Arbeit\\
- Dialog System: 🔴 Offen

### 🧩 Phase 4 -- UI System

- Menu Manager: 🟡 In Arbeit\\
- UI States: 🔴 Offen\\
- Widgets: 🔴 Offen\\
- HUD: 🔴 Offen

### 💾 Phase 5 -- Data & Persistence

- Data Models: 🟡 Teilweise\\
- Save System: 🔴 Offen\\
- Load System: 🔴 Offen\\
- Save Slots: 🔴 Offen\\
- DB Integration: 🔴 Offen

### 🧠 Phase 6 -- Gameplay Systems

- Quiz System: 🔴 Offen\\
- Level System: 🔴 Offen\\
- Reward System: 🔴 Offen\\
- Event System: 🔴 Offen

### 🤖 Phase 7 -- AI & Dynamic Content

- LLM Integration: 🔴 Offen\\
- Adaptive Difficulty: 🔴 Offen\\
- Advanced NPC AI: 🔴 Offen

### 📱 Phase 8 -- Polishing & Deployment

- UI Polish: 🔴 Offen\\
- Performance: 🔴 Offen\\
- Sound: 🔴 Offen\\
- Mobile Build: 🔴 Offen\\
- Testing: 🔴 Offen

------------------------------------------------------------------------

## 📊 Gesamtfortschritt

- Foundation: 80%\\
- Player System: 60%\\
- NPC System: 40%\\
- UI System: 30%\\
- Data System: 20%\\
- Gameplay Systems: 0%\\
- AI / LLM: 0%\\
- Polishing: 0%

------------------------------------------------------------------------

## 🚀 Nächste Schritte

1. NPC Interaction stabilisieren\\
2. UI System fertigstellen (Menu + HUD)\\
3. Save/Load implementieren\\
4. Grundlegendes Gameplay (Quiz-System) starten

------------------------------------------------------------------------

## 📋 GitHub Project Board (Kanban)

### Spalten
- Backlog  
- Ready  
- In Progress  
- Review / Testing  
- Done  

### Labels
- feature, bug, refactor, architecture  
- ui, npc, player, data, system  
- high-priority, low-priority  

### Beispiel-Tasks

#### Backlog
- NPC Movement / AI entwickeln  
- Dialogsystem erstellen  
- Save/Load System bauen  

#### Ready
- NPC Interaction stabilisieren  
- Menu Manager fixen  

#### In Progress
- Player Movement Feinschliff  
- UI Menü Anzeige Fix  

#### Review / Testing
- Collision System testen  
- UI Verhalten prüfen  

#### Done
- Grundstruktur erstellt  
- Player Movement Basis  
- StartScene Setup  

------------------------------------------------------------------------

## 🤝 Contribution

Beiträge sind willkommen.

Richtlinien:
- Feature-basiert arbeiten\\
- Klare Namensgebung verwenden\\
- Ein Script = eine Verantwortung\\
- Dokumentation pro Feature

------------------------------------------------------------------------

## 📄 Lizenz

Dieses Projekt dient aktuell Lern- und Entwicklungszwecken.\\
Lizenzdetails folgen.
"""

output_path = "/mnt/data/README_complete.md"
pypandoc.convert_text(content, 'md', format='md', outputfile=output_path, extra_args=['--standalone'])

output_path