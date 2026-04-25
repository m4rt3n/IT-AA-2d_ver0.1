# Skill / Level System

## Zweck
Dieses Feature bereitet ein leichtgewichtiges Skill- und Level-System fuer Lern- und Gameplay-Fortschritte vor.

## Namespace
`ITAA.Features.Skills`

## Dateien
- `SkillDefinition.cs`: Datenmodell fuer einen Skill.
- `SkillProgress.cs`: Runtime-Fortschritt eines Skills.
- `SkillProfile.cs`: Sammlung aller Skill-Fortschritte.
- `SkillRuntimeManager.cs`: XP-Vergabe, Level-Up-Regeln und Events.

## Demo-Skills
Wenn keine eigenen Definitionen gesetzt sind, kann der Manager Demo-Skills erzeugen:
- `networking`
- `support`
- `terminal`

## MVP-Grenzen
- Keine automatische Savegame-Persistenz.
- Keine UI-Anbindung.
- Keine direkte Kopplung an `ProgressManager`, `PlayerSession`, Quiz oder Scenarios.

## Integration spaeter
- Quiz kann XP fuer Themen-Skills melden.
- Quest/Progress kann auf Level-Ups reagieren.
- Savegame kann `SkillProfile` persistieren.
