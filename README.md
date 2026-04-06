# 🎮 IT-AA 2D Game (Unity)

Ein 2D-Spielprojekt als Lern- und Portfolio-Plattform mit Fokus auf Softwarearchitektur, Gameplay-Systeme und Backend-Integration.

---

## 🚀 Ziel des Projekts

Dieses Projekt dient als praxisorientierte Lernplattform zur:

- Entwicklung eines strukturierten Unity-Projekts  
- Umsetzung von Clean Code & SOLID-Prinzipien  
- Aufbau eines einfachen Backends (User, Login, Datenhaltung)  
- Vorbereitung auf technische Bewerbungen (z. B. Auswärtiges Amt)  

---

## 🧩 Aktueller Stand

### 🔐 Authentifizierung & Benutzerverwaltung
- Login- und Registrierungssystem implementiert  
- Persistente Speicherung (aktuell JSON / vorbereitet für SQLite)  
- `AuthManager` verwaltet Session & Szenenwechsel  

### 🗄️ Datenhaltung
- Erste Datenbankstruktur vorhanden (`DatabaseManager`)  
- Alternative Speicherung via JSON umgesetzt  
- Vorbereitung auf SQLite Integration  

### 🧭 Menüsystem
- Startmenü (Start / Login / Navigation)  
- Login UI mit Validierung  
- UI-Manager für Steuerung der Menüs  
- Fade-In / UI-Animation vorbereitet  

### 🧍 Gameplay & Interaktion
- Player Movement mit Grid/Step-System  
- NPC-Interaktion (z. B. Arthur)  
- Trigger-System für Interaktionen  
- Basis für Dialogsystem vorhanden  

### 🧱 Welt & Kollision
- Tilemap-System im Einsatz  
- Collider + Composite Collider  
- Erste Ansätze für „unsichtbare Begrenzungen“  

---

## 🛠️ Tech Stack

- **Engine:** Unity (2D, URP)  
- **Sprache:** C#  
- **Datenhaltung:** JSON (aktuell), SQLite (geplant)  
- **Versionskontrolle:** Git / GitHub  

**Tools:**  
- Visual Studio Code  
- GitHub  

---

## 🏗️ Projektstruktur (vereinfacht)

Assets/  
├── Scripts/  
│   ├── Auth/  
│   │   └── AuthManager.cs  
│   ├── Database/  
│   │   └── DatabaseManager.cs  
│   ├── UI/  
│   │   ├── MenuManager.cs  
│   │   ├── StartMenuController.cs  
│   │   └── LoginMenuController.cs  
│   ├── Player/  
│   │   └── PlayerController.cs  
│   └── NPC/  
│       └── NPCInteraction.cs  
│  
├── Scenes/  
│   └── StartScene.unity  
│  
└── Data/  
    └── (JSON / DB Files)  

---

## 🎯 Nächste Schritte

### 🔜 Kurzfristig
- Login vollständig testen & absichern  
- UI-Flow verbessern (Transitions, Feedback)  
- Fehlerhandling erweitern  

### 🧠 Mittelfristig
- SQLite sauber integrieren (sqlite-net)  
- Dialogsystem für NPCs  
- Quest-/Event-System  

### 🔥 Langfristig
- Backend-Anbindung (API)  
- Dynamische Dialoge (z. B. ChatGPT Integration)  
- Tag-/Nacht-System (Realtime)  
- Mobile Build  

---

## 📊 Portfolio-Mehrwert

Dieses Projekt zeigt:

- Strukturierte Softwareentwicklung in Unity  
- Verständnis von Architektur (Manager, Trennung von UI/Logic)  
- Backend-Grundlagen im Game-Kontext  
- Eigenständige Problemlösung (z. B. Collision, UI, DB)  

---

## 📎 Repository

https://github.com/m4rt3n/IT-AA-2d_ver0.1  

---

## ⚠️ Hinweis

Dieses Projekt befindet sich aktiv in Entwicklung und dient primär Lern- und Demonstrationszwecken.
