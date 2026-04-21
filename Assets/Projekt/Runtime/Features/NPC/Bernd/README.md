# 🧍 NPC – Bernd

## 📖 Overview

**Bernd** ist ein modular aufgebauter NPC im Projekt *IT-AA 2D*.
Er nutzt ein eigenständiges Feature-Set für **Bewegung, Animation und Interaktion**.

Ziel ist ein wiederverwendbares NPC-System nach dem Prinzip:
**„Plug & Play NPC mit klarer Verantwortung pro Script“**

---

## 🧠 Architektur

Das Bernd-System folgt einer **Feature-driven Architecture**:

Bernd/
├── BerndAnimationController.cs
├── BerndMovementToPlayer.cs
├── BerndDetectionZone.cs
├── BerndAutoInteraction.cs
├── README.md

### 🔹 Verantwortlichkeiten

* **BerndAnimationController** → Steuert Animator (Idle / Walk Richtungen)
* **BerndMovementToPlayer** → Bewegt Bernd Richtung Spieler
* **BerndDetectionZone** → Erkennt Spieler via Trigger
* **BerndAutoInteraction** → Startet Interaktion (automatisch oder per Input)

---

## ⚙️ Funktionsweise

### 1. Detection

* Spieler betritt Trigger (`BerndDetectionZone`)
* Referenz zum Player wird gesetzt

### 2. Movement

* `BerndMovementToPlayer` bewegt NPC zum Spieler

### 3. Animation

* `BerndAnimationController` setzt:

  * Idle (Up / Down / Left / Right)
  * Walk (richtungsabhängig)

### 4. Interaction

* `BerndAutoInteraction`:

  * automatisch **oder**
  * via Input (z. B. `E`)

---

## 🎬 Animation Setup

Animator erwartet folgende States:

Base Layer

* Bernd_IdleDown
* Bernd_IdleUp
* Bernd_IdleLeft
* Bernd_IdleRight

👉 State-Namen müssen **exakt stimmen**, da sie per Code angesprochen werden.

---

## 🧩 Integration in Unity

### Voraussetzungen

* GameObject: **Bernd**
* Komponenten:

  * Animator
  * Collider (IsTrigger) für Detection
  * Scripts:

    * BerndAnimationController
    * BerndMovementToPlayer
    * BerndDetectionZone
    * BerndAutoInteraction

---

## 🔌 Erweiterbarkeit

Das System ist vorbereitet für:

* Dialogsystem
* Quest-System
* Unterschiedliche NPC-Verhalten (passiv / aggressiv)
* Austauschbare Animationen

---

## ⚠️ Typische Fehler

* Animation läuft nicht → State-Name falsch
* NPC bewegt sich nicht → Player nicht erkannt
* Interaktion startet nicht → Input nicht gesetzt
* Animator Fehler → falscher Layer / Pfad

---

## 🚀 Roadmap

* Walk Animationen ergänzen
* Dialog UI integrieren
* NPC Verhalten variabel machen
* Event-System anbinden

---

## 🏗️ Design-Prinzipien

* Single Responsibility
* Lose Kopplung
* Wiederverwendbarkeit
* Erweiterbarkeit

---

## 📌 Hinweis

Dieses Feature ist **unabhängig nutzbar** und kann leicht für weitere NPCs
(z. B. „Arthur“) adaptiert werden.
