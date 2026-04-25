# 🧍 NPC – Bernd

## 📖 Overview

**Bernd** ist ein modular aufgebauter NPC im Projekt *IT-AA 2D*.
Er nutzt ein eigenständiges Feature-Set für **Bewegung, Animation, Interaktion und Quiz-Start**.

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
├── BerndQuizStarter.cs
├── BerndNameTag.cs
├── README.md

### 🔹 Verantwortlichkeiten

* **BerndAnimationController** → Steuert Animator (Idle / Walk Richtungen)
* **BerndMovementToPlayer** → Bewegt Bernd Richtung Spieler
* **BerndDetectionZone** → Erkennt Spieler via Trigger
* **BerndAutoInteraction** → Startet Interaktion (automatisch oder per Input)
* **BerndQuizStarter** → Startet Bernds zugewiesenes `QuizSet`
* **BerndNameTag** → Zeigt Bernds Namensanzeige bei Player-Naehe wie Arthurs NameTag

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

### 5. Quiz

* `BerndQuizStarter`:

  * referenziert `BerndIntroQuiz`
  * findet oder nutzt das zugewiesene `QuizPanel`
  * startet das Quiz ohne hart codierte Fragen im UI-Code

---

## 🖼️ Sprite- und Animationsbasis

Bernd nutzt `Assets/Projekt/Content/Art/Player/character green.png`.
Die Import-Basis ist auf die Player-/Arthur-Groesse abgestimmt:

* Pixels Per Unit: `16`
* Sprite Mode: `Multiple`
* Slice-Groesse: `16 x 32`
* Pivot je Slice: Bottom Center (`0.5, 0`)
* Filter Mode: Point
* Texture Compression: None

Vorhandene Clips:

* `Bernd_IdleDown`
* `Bernd_IdleUp`
* `Bernd_IdleLeft`
* `Bernd_IdleRight`
* `Bernd_WalkDown`
* `Bernd_WalkUp`
* `Bernd_WalkLeft`
* `Bernd_WalkRight`

Der Animator nutzt `MoveX`, `MoveY` und `IsMoving`, um zwischen Idle- und Walk-BlendTree zu wechseln.

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
    * BerndQuizStarter
    * BerndNameTag

Aktuell ist Bernd bereits in `Assets/Projekt/Content/Scenes/StartScene.unity` angelegt und mit
`Assets/Projekt/Content/Quiz/BerndIntroQuiz.asset` sowie dem `QuizPanel` verbunden.
Das `NameTagCanvas` ist als Child von Bernd angelegt und wird ueber `BerndNameTag` automatisch
bei Naehe zum Player sichtbar.

---

## 🔌 Erweiterbarkeit

Das System ist vorbereitet für:

* Dialogsystem
* Quest-System
* Mehrere QuizSets pro NPC
* Unterschiedliche NPC-Verhalten (passiv / aggressiv)
* Austauschbare Animationen

---

## ⚠️ Typische Fehler

* Animation läuft nicht → State-Name falsch
* NPC bewegt sich nicht → Player nicht erkannt
* Interaktion startet nicht → Input nicht gesetzt oder Player nicht im Trigger
* Quiz oeffnet nicht → `QuizSet` oder `QuizPanel` fehlt auf `BerndQuizStarter`
* Animator Fehler → falscher Layer / Pfad

---

## 🚀 Roadmap

* Walk Animationen ergänzen
* Quiz-UI visuell ausbauen
* Dialogsystem vor/nach Quiz integrieren
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

Dieses Feature bleibt bewusst leichtgewichtig. Das eigentliche Quiz-System liegt unter
`Assets/Projekt/Runtime/Features/Quiz/` und kann spaeter auch von weiteren NPCs genutzt werden.
