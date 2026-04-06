# 🎮 IT-AA 2D Lernspiel

> Interaktives 2D-Lernspiel zur Vorbereitung auf das Auswahlverfahren im IT-Bereich des Auswärtigen Amts

---

## 📌 Projektübersicht

Dieses Projekt ist ein praxisnahes Lernspiel, das typische IT-Szenarien spielerisch vermittelt.  
Der Fokus liegt auf realen Problemstellungen aus dem IT-Support und der Systemadministration.

Ziel ist es, den Spieler durch eine strukturierte Lernwelt zu führen – von Grundlagen bis hin zu komplexeren Aufgaben.

---

## 🎯 Zielsetzung

- Vorbereitung auf das Auswahlverfahren (mittlerer Dienst IT)
- Vermittlung von IT-Grundlagen durch Gameplay
- Simulation realer Support-Situationen
- Aufbau eines starken Portfolio-Projekts

---

## 🧩 Aktueller Entwicklungsstand

### ✅ Implementierte Features

- 🗺️ **Tilemap-basierte Spielwelt**
  - Straßen, Schnee, strukturierte Umgebung

- 🚶 **Spielerbewegung**
  - Grid-basiertes Movement (Step-System)
  - Richtungsabhängige Animation

- 🧱 **Kollisionssystem**
  - Tilemap Collider + Composite Collider
  - Grundlegende Begrenzung der Spielfläche

- 🧍 **NPC-Interaktion**
  - Trigger-System
  - Interaktion per **E-Taste**
  - Basis für Dialogsystem

- 🧭 **Startszene**
  - Einstiegspunkt ins Spiel
  - Vorbereitung für Menüführung

- 🖥️ **UI-System**
  - Menüstruktur vorhanden
  - Background-Dimming implementiert

- 🔁 **Versionierung**
  - Git + GitHub
  - Kontinuierliche Entwicklung dokumentiert

---

### ⚠️ Aktuelle Herausforderungen

- Kollisionsabstände entlang der Straßen noch nicht optimal
- Spieler kann visuell über Grenzen hinauslaufen
- UI-Menü wird teilweise nicht korrekt gerendert
- Trigger feuern zu häufig (Interaktionssystem)

---

## 🛠️ Tech Stack

| Bereich        | Technologie                |
|----------------|--------------------------|
| Engine         | Unity (2D)               |
| Sprache        | C#                       |
| Physik         | Unity 2D Physics         |
| UI             | Unity UI System          |
| Assets         | Free Pixel Art Tilesets  |
| Versionierung  | Git + GitHub             |

---

## 🎮 Gameplay-Konzept

### Beispiel-Szenario: „Kein Internet“

Der Spieler muss schrittweise eine Störung analysieren:

1. Verbindung prüfen (LAN / WLAN)
2. IP-Adresse analysieren
3. Gateway prüfen
4. DNS-Probleme erkennen

**Interaktion erfolgt über:**
- NPCs
- Umgebung (Objekte)
- Entscheidungslogik

---

## 🧠 Systemarchitektur (vereinfacht)

    PlayerController
     ├── Movement (Grid System)
     ├── Collision Detection
     └── Animation Handling

    NPCInteraction
     ├── Trigger Detection
     ├── Input Handling (E-Taste)
     └── Dialog Trigger

    StartMenuController
     ├── Menüsteuerung
     └── Background Fade

    Tilemap System
     ├── Ground Layer
     ├── Collision Layer
     └── Visual Layer

---

## 🚀 Roadmap

### 🔹 Kurzfristig
- Kollisionssystem optimieren (präzise Begrenzung)
- UI Menü vollständig funktionsfähig machen
- Interaktionssystem stabilisieren

### 🔹 Mittelfristig
- Dialogsystem erweitern
- Quest-System implementieren
- Inventar-System hinzufügen
- Skilltree integrieren
- Tag-/Nacht-Zyklus

### 🔹 Langfristig
- Backend (Datenbank für Fortschritt)
- Cloud-Speicherung
- Dynamische Dialoge (KI-Integration)
- Mobile Deployment

---

## 📦 Installation & Setup

```bash
git clone https://github.com/m4rt3n/IT-AA-2d_ver0.1.git
