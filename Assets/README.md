# 📦 Assets

## Zweck
Der `Assets`-Ordner enthält alle Inhalte des Unity-Projekts – sowohl visuelle Ressourcen als auch die komplette Spiellogik.

Er ist in zwei Hauptbereiche unterteilt:
- Content → Spielinhalte (Grafik, Audio, Szenen)
- Runtime → Spiellogik und Systeme

---

## 📁 Struktur

    Assets/
    ├── _Recovery/        # Unity interne Wiederherstellung (ignorieren)
    ├── Projekt/
    │   ├── Content/      # Spielinhalte (keine Logik)
    │   └── Runtime/      # Spiellogik
    ├── Settings/         # Projekt-Konfigurationen
    ├── ScriptableObjects/# Daten-Assets
    ├── TextMesh Pro/     # UI/Text-System
    └── PlayerControls.inputactions # Input-Konfiguration

---

## 🧩 Content vs Runtime

### 🎨 Content

Enthält alles Visuelle und Statische:

- Art → Grafiken und Animationen  
- Audio → Sounds und Musik  
- Materials → Materialien  
- Prefabs → wiederverwendbare Objekte  
- Scenes → Unity Szenen  

👉 Keine Spiellogik

---

### ⚙️ Runtime

Enthält die komplette Spiellogik:

- Core → technische Basis (Singletons, Events, SceneManagement)  
- Data → Daten und Speicherung  
- Features → Gameplay-Systeme (Player, NPC, UI, World)  
- System → globale Logik  

👉 Hier passiert das Verhalten des Spiels

---

## 🔗 Verbindung Content ↔ Runtime

- Content stellt Objekte bereit (Prefabs, Scenes)
- Runtime steuert deren Verhalten (Scripts)

### Beispiel

Ein Player:
- liegt als Prefab in `Content/Prefabs`
- wird in einer Scene platziert
- wird durch `Runtime/Features/Player/` gesteuert

---

## 🧠 Architekturprinzip

Das Projekt folgt einer **Feature-basierten Struktur**:

- Code ist nach Systemen organisiert (Player, NPC, UI)
- nicht nach Typen (kein Scripts-Ordner)
- klare Trennung von Verantwortung

---

## ⚠️ Hinweise

- `_Recovery/` wird von Unity automatisch verwaltet
- `TextMesh Pro/` ist ein externes UI-System
- `PlayerControls.inputactions` definiert Input

---

## 🎯 Ziel

Der `Assets`-Ordner ist so aufgebaut, dass:
- Logik klar getrennt ist
- Systeme modular erweitert werden können
- das Projekt skalierbar bleibt