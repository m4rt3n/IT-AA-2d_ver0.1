# 🧍 NPC System

## 📖 Overview

Das **NPC-System** in *IT-AA 2D* bündelt alle nicht spielbaren Charaktere in einer klaren, modularen Struktur.
Jeder NPC folgt denselben Grundprinzipien, kann aber individuell erweitert und angepasst werden.

Das Ziel ist ein **skalierbares und wiederverwendbares System** für:

* Bewegung
* Animation
* Spielererkennung
* Interaktion
* spätere Erweiterungen wie Dialoge, Quests oder Events

---

## 🧠 Architektur

Das NPC-System folgt einer **Feature-driven Architecture**.
Jeder NPC besitzt seinen eigenen Unterordner mit seinen spezifischen Scripts und Assets.

NPC/
├── Arthur/
│   ├── ArthurAnimationController.cs
│   ├── ArthurMovementToPlayer.cs
│   ├── ArthurDetectionZone.cs
│   ├── ArthurAutoInteraction.cs
│   └── README.md
│
├── Bernd/
│   ├── BerndAnimationController.cs
│   ├── BerndMovementToPlayer.cs
│   ├── BerndDetectionZone.cs
│   ├── BerndAutoInteraction.cs
│   └── README.md
│
└── README.md

---

## 🔹 Grundprinzip

Jeder NPC besteht aus mehreren kleinen Komponenten mit klarer Verantwortung:

* **Animation Controller**
  → steuert Idle- und Walk-Animationen

* **Movement Controller**
  → bewegt den NPC optional in Richtung Spieler oder Zielpunkt

* **Detection Zone**
  → erkennt den Spieler über Trigger / Reichweite

* **Interaction Controller**
  → startet Interaktionen, Menüs oder Dialoge

Dieses Muster sorgt für:

* klare Trennung der Logik
* bessere Lesbarkeit
* einfachere Erweiterbarkeit
* Wiederverwendbarkeit für weitere NPCs

---

## ⚙️ Standard-Ablauf

### 1. Detection

Der Spieler betritt die Reichweite des NPCs.
Die Detection Zone erkennt den Spieler und setzt eine Referenz.

### 2. Movement

Je nach NPC-Verhalten kann der NPC:

* stehen bleiben
* sich zum Spieler bewegen
* auf ein Ziel reagieren

### 3. Animation

Der Animation Controller aktualisiert:

* Idle-Richtung
* Walk-Richtung
* Bewegungsstatus

### 4. Interaction

Der NPC kann:

* automatisch reagieren
* auf Eingabe warten
* ein UI / Dialog / Event starten

---

## 🎬 Animation Setup

Ein NPC-Animator sollte konsistente State-Namen verwenden, z. B.:

Base Layer

* NPC_IdleDown
* NPC_IdleUp
* NPC_IdleLeft
* NPC_IdleRight
* NPC_WalkDown
* NPC_WalkUp
* NPC_WalkLeft
* NPC_WalkRight

In der Praxis werden diese States pro NPC benannt, z. B.:

* `Arthur_IdleDown`
* `Bernd_IdleDown`

👉 Wichtig: Die State-Namen müssen exakt mit der Code-Logik übereinstimmen.

---

## 🧩 Integration in Unity

### Voraussetzungen pro NPC

* eigenes GameObject
* Animator
* Collider / Trigger für Detection
* optional Rigidbody2D für Bewegung
* zugewiesene NPC-Scripts

### Typische Bestandteile

* NPC Root Object
* Visual / Sprite Renderer
* Detection Zone
* Animator Controller
* zugehörige Script-Komponenten

---

## 🔌 Erweiterbarkeit

Das NPC-System ist als Basis für spätere Features gedacht:

* Dialogsystem
* Questgeber
* Händler
* Story-Events
* Begleiter / Follower
* unterschiedliche NPC-Verhalten
* generisches Basissystem für gemeinsame Logik

---

## ⚠️ Typische Fehler

* Animation startet nicht → State-Name stimmt nicht
* Spieler wird nicht erkannt → Trigger / Layer falsch
* Interaktion startet nicht → Referenz oder Input fehlt
* Bewegung funktioniert nicht → Ziel oder Rigidbody2D fehlt
* Menü / Dialog öffnet nicht → Verbindung zum UI-System fehlt

---

## 🚀 Roadmap

* generische NPC-Basis schaffen
* gemeinsame Interfaces / Basisklassen definieren
* Dialogsystem anbinden
* Quest-System integrieren
* NPC-Daten aus ScriptableObjects oder Datenmodellen laden
* Event-getriebene NPC-Interaktionen erweitern

---

## 🏗️ Design-Prinzipien

* Single Responsibility
* Lose Kopplung
* Wiederverwendbarkeit
* Erweiterbarkeit
* klare Feature-Trennung
* pro NPC eigene, sauber strukturierte Komponenten

---

## 📌 Hinweis

Arthur und Bernd sind konkrete Umsetzungen dieses Systems.
Langfristig kann daraus ein **gemeinsames generisches NPC-Framework** entstehen,
bei dem nur noch Verhalten, Animationen und Inhalte pro Figur ausgetauscht werden.
