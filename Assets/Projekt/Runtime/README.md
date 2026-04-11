# ⚙️ Runtime

## Zweck
Enthält die komplette Spiellogik zur Laufzeit.

## Struktur
- Core/ → technische Basis
- Data/ → Daten und Speicherung
- Features/ → Gameplay-Systeme
- System/ → globale Systeme

## Architektur
Feature-basierter Ansatz:
- Code ist nach Systemen organisiert
- klare Trennung von Verantwortung

## Einstieg
Beginne mit:
- Features/Player/
- Core/

## 🔗 Verknüpfung zu Scenes

Die Runtime enthält die komplette Spiellogik und wird von Objekten in den Unity-Szenen genutzt.

### Prinzip

- Scenes erstellen und platzieren Objekte
- Runtime definiert deren Verhalten

### Beispiel

Ein Player in der Scene:
- GameObject in `Scenes/Gameplay`
- enthält Komponenten wie `PlayerController`

Diese kommen aus:
- `Runtime/Features/Player/`

Ein NPC:
- wird in der Scene platziert
- nutzt `NPCInteraction` aus der Runtime

Ein Menü:
- liegt in einer Scene oder Canvas
- wird durch `UI`-System gesteuert

### Ablauf

Scene lädt  
→ GameObjects werden instanziiert  
→ Runtime-Skripte initialisieren  
→ Spiel beginnt  

### Rolle der Systeme

- Core → Initialisierung und globale Logik  
- Features → Gameplay (Player, NPC, UI)  
- Data → Speicherung und Zustand  

### Wichtig

Runtime enthält:
- Logik
- Regeln
- Zustände
- Datenfluss

Runtime kennt keine festen Szenenlayouts, sondern arbeitet generisch mit den Objekten aus den Scenes.