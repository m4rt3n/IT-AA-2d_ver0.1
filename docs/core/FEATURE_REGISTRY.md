# Feature Registry: IT-AA 2D

## Ziel
Diese Datei dient als zentrale Übersicht aller Features, Systeme und Zuständigkeiten im Projekt.

## Architekturregeln
- Feature-driven Architecture
- Keine Breaking Changes
- Bestehende Systeme wiederverwenden
- Pro Klasse eine Datei
- Namespace immer mit ITAA.*
- UI, Datenmodell und Spiellogik trennen

## Bestehende Features

### Player
Namespace:
- ITAA.Player.Movement
- ITAA.Player.Session

Zuständig für:
- Spielerbewegung
- PlayerSession
- Name/Profil aus SaveSlot

Wichtige Dateien:
- PlayerController.cs
- PlayerMotor2D.cs
- PlayerSession.cs

---

### NPC
Namespace:
- ITAA.NPC.Arthur
- ITAA.NPC.Bernd

Zuständig für:
- NPC-Bewegung
- NPC-Animation
- NPC-Interaktion
- NameTag über Charakteren

Wichtige NPCs:
- Arthur
- Bernd

Routine-MVP:
- ITAA.NPC.Routines
- NpcRoutineStepType.cs
- NpcRoutineStep.cs
- NpcRoutineController.cs

Regel:
- Kein neues paralleles NPC-System bauen
- Gemeinsame Logik später extrahieren

---

### Savegame
Namespace:
- ITAA.System.Savegame

Zuständig für:
- SaveSlot-Daten
- JSON-Speicherung
- Laden/Speichern von Spielständen

Wichtige Dateien:
- SaveGameData.cs
- SaveSlotEntity.cs
- SaveSystem.cs

---

### UI
Namespace:
- ITAA.UI.*

Zuständig für:
- Menüs
- Panels
- SaveSlot-Anzeige
- QuizPanel
- SettingsPanel später

Wichtige Dateien:
- MenuManager.cs
- LoadGamePanel.cs
- SaveSlotItemUI.cs
- QuizPanel.cs

---

### Quiz
Geplanter Namespace:
- ITAA.Features.Quiz

Zuständig für:
- Fragen
- Antworten
- Schwierigkeitsgrade
- Fragetypen
- generierte Fragen
- Übernahme generierter Fragen in feste Fragenbank

---

### Settings
Namespace:
- ITAA.System.Settings

Zuständig für:
- Audio
- Video
- Input
- Gameplay-Einstellungen
- Speicherung als settings.json

Wichtige Dateien:
- SettingsData.cs
- SettingsManager.cs
- SettingsUIController.cs

Status:
- MVP implementiert
- Input-Rebinding ist als gespeicherte Tastenbasis vorbereitet, aber noch nicht an PlayerControls angebunden

---

### DevPanel
Geplanter Namespace:
- ITAA.DevTools

Zuständig für:
- Debug-Aktionen
- Testdaten erzeugen
- SaveSlots neu laden
- Settings zurücksetzen
- Quiz-Drafts erzeugen

Nur für Entwicklung gedacht.

---

### Dialogue
Geplanter Namespace:
- ITAA.Features.Dialogue

Zuständig für:
- NPC-Dialoge
- Gespräch vor/nach Quiz
- wiederverwendbare Dialoglogik für Arthur, Bernd und spätere NPCs

---

### Quest / Progress
Geplanter Namespace:
- ITAA.Features.Progress

Zuständig für:
- Aufgaben
- Lernfortschritt
- abgeschlossene Quizthemen
- Statistiken
- Spielerfortschritt

---

### Inventory / Toolbelt
Namespace:
- ITAA.Features.Inventory

Zuständig für:
- Item-Datenmodell
- Runtime-Inventar
- Toolbelt-Auswahl
- spaetere IT-Support-Werkzeuge

Wichtige Dateien:
- InventoryItemCategory.cs
- InventoryItemData.cs
- InventoryItemStack.cs
- RuntimeInventory.cs
- ToolbeltController.cs

Status:
- MVP implementiert
- Keine UI-, Savegame- oder World-Interaction-Anbindung im ersten Schritt

---

### Skill / Level
Namespace:
- ITAA.Features.Skills

Zuständig für:
- Skill-Datenmodell
- XP-Vergabe
- Level-Up-Regeln
- Runtime-Skillprofil

Wichtige Dateien:
- SkillDefinition.cs
- SkillProgress.cs
- SkillProfile.cs
- SkillRuntimeManager.cs

Status:
- MVP implementiert
- Keine direkte Kopplung an PlayerSession, ProgressManager, UI oder Savegame

---

### Achievements
Namespace:
- ITAA.Features.Achievements

Zuständig für:
- Achievement-Datenmodell
- Unlock-Status
- Fortschrittswerte
- Runtime-Unlock-Events

Wichtige Dateien:
- AchievementDefinition.cs
- AchievementProgress.cs
- AchievementProfile.cs
- AchievementManager.cs

Status:
- MVP implementiert
- Keine direkte Kopplung an Progress, Skills, HUD, UI oder Savegame
