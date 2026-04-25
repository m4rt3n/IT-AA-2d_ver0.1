# Feature: Achievement System

## Ziel
Ein kleines Achievement-System fuer Lern- und Gameplay-Meilensteine vorbereiten.

## Namespace
- ITAA.Features.Achievements

## MVP-Idee
- Datenmodell fuer Achievements
- Runtime-Status fuer gesperrt/freigeschaltet
- einfache Unlock-API
- keine automatische UI-, Savegame- oder Quest-Anbindung im ersten Schritt

## Implementierter MVP-Stand
- `AchievementDefinition`: Achievement-Datenmodell
- `AchievementProgress`: Runtime-Status und optionaler Fortschritt
- `AchievementProfile`: Sammlung aller Achievement-Statuswerte
- `AchievementManager`: Unlock-, Progress- und Query-API
- Demo-Achievements: `first_login`, `first_quiz`, `network_beginner`
- keine automatische UI-, Savegame-, Progress-, Skill- oder Quest-Anbindung

## Anforderungen
- Bestehende Progress-, Skill- und Savegame-Systeme nicht brechen
- UI und Datenmodell getrennt halten
- Keine Scene-/Prefab-Aenderungen ohne Freigabe

## Offene Entscheidungen
- Persistenz im Savegame
- Achievement-Daten als ScriptableObject, JSON oder Runtime-Daten
- Integration mit HUD, Quiz, Skill und Quest System
- konkrete Achievement-UI und Notification-Flow
