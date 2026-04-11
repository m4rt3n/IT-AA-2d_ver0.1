# 🎮 IT-AA 2D Game

Ein modulares 2D-Spielprojekt auf Basis von Unity mit Fokus auf:

* sauberer Architektur (SOLID)
* skalierbaren Systemen
* Kombination aus Gameplay und Backend

---

## 🚀 Features

* 🔐 Authentifizierungssystem (Login / User)
* 💾 Datenbank-Anbindung (lokal / erweiterbar)
* 🧍 Spielersteuerung (Movement + Animation)
* 🤖 NPC-Interaktion (Trigger + Menüsystem)
* 🧭 UI-System (Startmenü, Ingame-Menüs)
* 🌍 Welt & Kollision (Tilemap-basiert)

---

## 📁 Projektstruktur

```
Assets/
├── Player/         # Spieler-System (Movement, Animation, Input)
├── NPC/            # NPC-Systeme und Interaktionen
├── UI/             # Globale UI-Systeme
├── Data/           # Datenmodelle und Speicherung
├── Core/           # Basis-Systeme (Manager, Utilities)
├── Authentication/ # Login / Benutzerverwaltung
├── Scenes/         # Unity Szenen
├── Art/            # Grafiken und Animationen
├── Prefabs/        # Wiederverwendbare Objekte
└── Settings/       # Konfigurationen

Database/           # Datenbank / externe Speicherung
Packages/           # Unity Package Manager
ProjectSettings/    # Unity Einstellungen
```

---

## 🧩 Architekturprinzip

Das Projekt folgt einer **Feature-basierten Struktur**:

* Code ist nach **Funktion (Feature)** organisiert, nicht nach Typ
* Jedes System ist **isoliert und erweiterbar**
* Fokus auf **lose Kopplung und klare Verantwortlichkeiten**

**Vorteile:**

* bessere Skalierbarkeit
* einfachere Wartung
* klare Systemtrennung

---

## 🔁 System-Flow

```
Input
  ↓
Player (Movement, Animation)
  ↓
NPC Interaction
  ↓
UI (Menüs, Dialoge)
  ↓
Data (Speichern / Laden)
```

---

## 🧠 Wichtige Systeme

### 🧍 Player

* Bewegung
* Animation
* Input-Verarbeitung

### 🤖 NPC

* Trigger-basierte Interaktion
* Menü-Auslösung

### 🧭 UI

* Menüverwaltung
* Anzeige von Spielzuständen

### 💾 Data

* Speicherung von Spielständen
* Benutzerdaten

### 🔐 Authentication

* Login / Registrierung
* Nutzerverwaltung

---

## 🏁 Einstieg für Entwickler

1. Projekt in Unity öffnen

2. Szene starten: **StartScene**

3. Einstieg in den Code:

* Player → `PlayerController`
* NPC → `NPCInteraction`
* UI → `MenuManager`

---

## 🧱 Code-Standards

### Script-Header (Pflicht)

```
/*
 * Datei:
 * Modul:
 * Zweck:
 * Verantwortung:
 * Abhängigkeiten:
 * Verwendung:
 */
```

---

### Struktur im Code

```
#region Inspector
#endregion

#region Unity Methods
#endregion

#region Private Methods
#endregion
```

---

## 🔧 Technologien

* Unity (2D)
* C#
* SQLite (optional)

---

## 📈 Roadmap

* [ ] Dialogsystem (LLM-Integration)
* [ ] Quest-System
* [ ] Save/Load-System erweitern
* [ ] Mobile Version
* [ ] Backend-Anbindung (API)

---

## 📌 Ziel des Projekts

Dieses Projekt dient als:

* Lernplattform für Game Development
* Testumgebung für Architekturkonzepte
* Grundlage für ein skalierbares Spielsystem

---

## 👤 Autor

Martin
Ingenieur / IT-Support / Entwickler

---

## 📄 Lizenz

Noch nicht definiert
