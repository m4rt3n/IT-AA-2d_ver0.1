# Achievement System

## Zweck
Dieses Feature bereitet ein kleines Achievement-System fuer Lern- und Gameplay-Meilensteine vor.

## Namespace
`ITAA.Features.Achievements`

## Dateien
- `AchievementDefinition.cs`: Datenmodell fuer ein Achievement.
- `AchievementProgress.cs`: Runtime-Status eines Achievements.
- `AchievementProfile.cs`: Sammlung aller Achievement-Statuswerte.
- `AchievementManager.cs`: Unlock-, Progress- und Query-API.

## Demo-Achievements
Wenn keine eigenen Definitionen gesetzt sind, kann der Manager Demo-Achievements erzeugen:
- `first_login`
- `first_quiz`
- `network_beginner`

## MVP-Grenzen
- Keine automatische Savegame-Persistenz.
- Keine UI-Anbindung.
- Keine direkte Kopplung an Progress, Skills, Quiz oder HUD.

## Integration spaeter
- HUD kann Unlock-Events anzeigen.
- Progress/Skill/Quiz koennen Achievements ueber kleine Adapter freischalten.
- Savegame kann `AchievementProfile` persistieren.
