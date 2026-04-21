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
  * wartet optional auf Input (z. B. `E`)
  * öffnet z. B.:

    * StartMenu
    * Dialog
    * UI Panel

👉 **Arthur ist dein primärer „Entry Point“ für Gameplay-Interaktionen**

---

## 🎬 Animation Setup

Animator erwartet folgende States:

Base Layer

* Arthur_IdleDown
* Arthur_IdleUp
* Arthur_IdleLeft
* Arthur_IdleRight
* Arthur_WalkDown
* Arthur_WalkUp
* Arthur_WalkLeft
* Arthur_WalkRight

👉 State-Namen müssen exakt stimmen (String-basiert im Code)

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
