# HUD System

## Zweck
Das HUD zeigt zentrale Laufzeitinformationen, ohne direkt das SaveSystem zu lesen. Es ist als optionales MVP fuer StartScene und spaetere Gameplay-Szenen vorbereitet.

## Dateien
- `HudController.cs` sammelt Daten aus vorhandenen Runtime-Systemen.
- `HudView.cs` stellt Texte und Meldungen dar.
- `HudNotification.cs` beschreibt kurze Info-Meldungen.

## Anzeigen
- Spielername
- aktuelles Ziel
- Quizpunkte
- aktuelles Thema
- kurze Systemmeldung

## Unity Setup
1. UI-GameObject in einem Canvas anlegen.
2. `HudView` hinzufuegen.
3. Optional `Create Missing Ui` aktiv lassen, damit eine einfache MVP-UI erzeugt wird.
4. `HudController` auf dasselbe GameObject setzen.
5. Optional `ProgressManager` zuweisen. Falls keiner vorhanden ist, zeigt das HUD `Quiz: 0 / 0`.

## Verhalten
- `HudController.RefreshHud()` aktualisiert alle Werte.
- `SetCurrentObjective(...)` setzt das aktuelle Ziel.
- `SetCurrentTopic(...)` setzt das aktuelle Thema.
- `SetQuizScore(...)` setzt Quizpunkte manuell.
- `ShowNotification(...)` zeigt eine kurze Meldung.

## Integration
Das HUD nutzt `PlayerSession`, falls vorhanden. Progress- und Quizdaten werden nur angebunden, wenn ein `ProgressManager` existiert. Dadurch bleibt das HUD optional und verursacht keine harte Kopplung an Bernd, SaveSystem oder QuizPanel.
