# 🎬 Scenes

## Zweck
Enthält alle Spiel-Szenen.

## Struktur
- Boot/ → Initialisierung
- Gameplay/ → Hauptspiel
- Menus/ → UI und Navigation

## Flow
Boot → Menü → Gameplay → Menü

## Hinweis
Szenen enthalten keine komplexe Logik, sondern verbinden Systeme.

## 🔗 Verknüpfung zu Runtime

Scenes enthalten die sichtbaren Spielobjekte und deren Platzierung.  
Die eigentliche Spiellogik liegt vollständig in `Runtime/`.

### Prinzip

- Scene = Komposition (Was ist im Spiel vorhanden?)
- Runtime = Verhalten (Was machen diese Objekte?)

### Beispiel

In der `Gameplay`-Scene:
- Ein Player-Objekt wird platziert
- Ein NPC mit Collider wird platziert
- UI-Canvas wird geladen

Die Logik dazu kommt aus:
- `Runtime/Features/Player/` → Bewegung und Input
- `Runtime/Features/NPC/` → Interaktion
- `Runtime/Features/UI/` → Menüs und Anzeige

### Ablauf

Scene lädt → Objekte existieren → Runtime-Skripte übernehmen Verhalten

### Wichtig

Scenes enthalten:
- Prefabs
- Referenzen
- Positionen
- visuelle Struktur

Scenes enthalten **keine komplexe Spiellogik**.