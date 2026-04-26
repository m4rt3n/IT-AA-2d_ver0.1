# DevTools / DevPanel

## Zweck
Das DevPanel ist ein optionales Entwicklerwerkzeug fuer schnelle Debug- und Testaktionen im Play Mode. Es ist kein Gameplay-Pflichtsystem und ersetzt keine bestehenden Systeme.

## Dateien
- `DevPanelController.cs` enthaelt die Button-Aktionen.
- `DevPanelBootstrap.cs` kann ein einfaches Runtime-Panel erzeugen und per `F12` toggeln.

## Funktionen
- Reload SaveSlots
- Generate Dummy Saves
- Reset Settings
- Generate Dummy Quiz Draft
- Print Player Session
- Print Current Scene
- Print Feature Managers
- Grant Demo Skill XP
- Unlock Demo Achievement
- Close DevPanel

## Unity Setup
Automatisch:
1. `StartSceneFeatureBootstrap` erzeugt in der `StartScene` bei Bedarf ein Runtime-Objekt `StartSceneRuntimeFeatures`.
2. Darauf wird `DevPanelBootstrap` hinzugefuegt, wenn noch kein DevPanel vorhanden ist.
3. Play Mode starten und `F12` druecken.

Manueller Minimalfall:
1. Leeres GameObject `DevPanelBootstrap` in die StartScene legen.
2. `DevPanelBootstrap` hinzufuegen.
3. `Create Runtime Panel` aktiv lassen.
4. Play Mode starten und `F12` druecken.

Manuelles Panel:
1. Eigenes UI-Panel anlegen.
2. `DevPanelController` auf das Panel setzen.
3. Buttons im Inspector zuweisen.
4. Button-OnClick kann direkt die oeffentlichen Methoden des Controllers verwenden.

## Verhalten
Das Panel arbeitet defensiv:
- Fehlende Systeme erzeugen `Debug.LogWarning`.
- SaveSlots werden ueber das bestehende `SaveSystem` geschrieben und ueber `LoadGamePanel.ReloadSlots()` aktualisiert.
- Settings werden ueber den zentralen `SettingsManager` zurueckgesetzt.
- Vorbereitete Feature-Manager koennen per Log geprueft werden.
- Demo-Aktionen fuer Skills und Achievements nutzen nur die vorhandenen MVP-Demo-IDs.
- Quiz-Draft-Persistenz existiert noch nicht; die Aktion dokumentiert den Dummy-Draft per `Debug.Log`.
