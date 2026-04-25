# Feature: Skill / Level System

## Ziel
Ein leichtgewichtiges Skill- und Level-System fuer spaetere Lern- und Gameplay-Fortschritte vorbereiten.

## Namespace
- ITAA.Features.Skills

## MVP-Idee
- Datenmodell fuer Skills
- Runtime-Profil fuer XP und Level
- einfache XP-Vergabe
- keine automatische Savegame-, UI- oder Quest-Anbindung im ersten Schritt

## Implementierter MVP-Stand
- `SkillDefinition`: Skill-Datenmodell mit XP-Kosten und Growth-Factor
- `SkillProgress`: Runtime-Level, aktuelle XP und Total-XP
- `SkillProfile`: Sammlung aller Skill-Fortschritte
- `SkillRuntimeManager`: Skill-Definitionen, XP-Vergabe, Level-Up-Events
- Demo-Skills: `networking`, `support`, `terminal`
- keine automatische Savegame-, UI-, PlayerSession- oder ProgressManager-Anbindung

## Anforderungen
- Bestehende PlayerSession und Progress-Systeme nicht brechen
- Keine Scene-/Prefab-Aenderungen ohne Freigabe
- Skill-Logik von UI trennen

## Offene Entscheidungen
- Persistenz im Savegame
- Skill-Daten als ScriptableObject, JSON oder Runtime-Daten
- Integration mit Quiz, Quest und Scenario System
- UI-Darstellung fuer Skill-Fortschritt
