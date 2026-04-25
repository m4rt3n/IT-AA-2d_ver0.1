# 🧍 NPC – Arthur

## 📖 Overview

**Arthur** ist ein interaktiver NPC im Projekt *IT-AA 2D*.
Er dient als **zentrale Interaktionsfigur** und verbindet Movement, Animation und UI (z. B. Menü / Dialog).

Im Gegensatz zu Bernd liegt der Fokus stärker auf:

* **Spieler-Interaktion**
* **UI / Menü-Trigger**
* **steuerbarer Kommunikation**

---

## 🧠 Architektur

Arthur folgt ebenfalls der **Feature-driven Architecture**, ist aber stärker auf Interaktion ausgelegt:

Arthur/
├── ArthurAnimationController.cs
├── ArthurMovementToPlayer.cs
├── ArthurDetectionZone.cs
├── ArthurAutoInteraction.cs
├── README.md

---

## 🔹 Verantwortlichkeiten

* **ArthurAnimationController**
  → Steuert Animator (Idle / Walk Richtungen)

* **ArthurMovementToPlayer**
  → Bewegt Arthur optional zum Spieler

* **ArthurDetectionZone**
  → Erkennt Spieler über Trigger

* **ArthurAutoInteraction**
  → Startet Interaktion (z. B. Menü öffnen)
  → Oeffnet das Startmenue in der aktuellen `StartScene` automatisch ueber `openMenuWhenArthurReachesPlayer`
  → Laesst `interactAction` dort bewusst leer; fuer spaetere manuelle Interaktion ist `Player/Interact` aus `Assets/PlayerControls.inputactions` vorgesehen

---

## ⚙️ Funktionsweise

### 1. Detection

* Spieler betritt Trigger (`ArthurDetectionZone`)
* Spieler wird als Target gesetzt

### 2. Optional Movement

* Arthur kann sich zum Spieler bewegen (abhängig vom Setup)

### 3. Animation

* `ArthurAnimationController` setzt:

  * Idle (Up / Down / Left / Right)
  * Walk (richtungsabhängig)

### 4. Interaction (Core Feature)

* `ArthurAutoInteraction`:

  * prüft Spieler in Reichweite
  * oeffnet das Startmenue aktuell automatisch, wenn `openMenuWhenArthurReachesPlayer` aktiv ist
  * kann optional auf Input warten, wenn spaeter eine `InputActionReference` gesetzt wird
  * öffnet z. B.:

    * StartMenu
    * Dialog
    * UI Panel

👉 **Arthur ist dein primärer „Entry Point“ für Gameplay-Interaktionen**

### Aktueller StartScene-Input-Stand

In `Assets/Projekt/Content/Scenes/StartScene.unity` bleibt `ArthurAutoInteraction.interactAction` bewusst leer.
Arthur nutzt dort den Auto-Open-Flow:

1. Player betritt Arthurs Trigger.
2. Arthur bewegt sich zum Player.
3. `openMenuWhenArthurReachesPlayer` oeffnet das Startmenue.

Fuer eine spaetere manuelle Interaktion soll die Action `Player/Interact` aus `Assets/PlayerControls.inputactions` verwendet werden.
Diese Action ist aktuell auf `E` und Gamepad `buttonNorth` gebunden.

---

## 🎬 Animation Setup

Aktueller Stand in `Assets/Projekt/Content/Scenes/StartScene.unity`:

* Arthur nutzt die vorhandenen Animator-States:
  * `Arthur_Idle`
  * `Arthur_Walk`
* Richtung und Bewegung werden ueber diese Parameter gesteuert:
  * `MoveX`
  * `MoveY`
  * `IsMoving`
* `ArthurAnimationController` kann weiterhin richtungsbasierte State-Namen verwenden, wenn der Animator spaeter passende States erhaelt.

Optional vorbereitete richtungsbasierte States:

Base Layer

* Arthur_IdleDown
* Arthur_IdleUp
* Arthur_IdleLeft
* Arthur_IdleRight
* Arthur_WalkDown
* Arthur_WalkUp
* Arthur_WalkLeft
* Arthur_WalkRight

👉 In der aktuellen `StartScene` muessen nur `Arthur_Idle` und `Arthur_Walk` vorhanden sein.

---

## 🧩 Integration in Unity

### Voraussetzungen

* GameObject: **Arthur**
* Komponenten:

  * Animator
  * Collider (IsTrigger)
  * Rigidbody2D (optional für Bewegung)
  * Scripts:

    * ArthurAnimationController
    * ArthurMovementToPlayer
    * ArthurDetectionZone
    * ArthurAutoInteraction

---

## 🔌 Verbindung mit UI

Arthur kann direkt mit deinem UI-System gekoppelt werden:

Beispiel:

* `MenuManager.ShowStartMenu()`
* `LoadGamePanel öffnen`
* Dialogsystem starten

👉 Damit wird Arthur zum **Game Flow Controller**

---

## ⚠️ Typische Fehler

* Animation funktioniert nicht → State-Name falsch
* Menü öffnet nicht → Referenz im Script fehlt
* Spieler wird nicht erkannt → Trigger falsch eingestellt
* Input funktioniert nicht → neues Input System nicht korrekt verwendet

---

## 🚀 Roadmap

* Dialogsystem integrieren
* Quest-System anbinden
* NPC Entscheidungen / Verhalten erweitern
* Interaction UI verbessern

---

## 🏗️ Design-Prinzipien

* Single Responsibility
* Klare Trennung von UI & Logik
* Event-basierte Interaktion (zukünftig)
* Erweiterbarkeit für komplexe NPC-Systeme

---

## 📌 Hinweis

Arthur ist als **zentraler NPC für Interaktionen** gedacht und bildet die Basis
für komplexere Systeme wie Dialoge, Quests und Story-Events.
