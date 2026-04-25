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
- Close DevPanel

## Unity Setup
Minimal:
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
- Settings werden nur zurueckgesetzt, wenn eine `PlayerSettingsSession` vorhanden ist.
- Quiz-Draft-Persistenz existiert noch nicht; die Aktion dokumentiert den Dummy-Draft per `Debug.Log`.
