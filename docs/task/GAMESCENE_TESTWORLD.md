# GameScene Testwelt

## Ziel
Die `GameScene` ist die erste spielbare MVP-Testwelt fuer die vorhandenen Feature-Systeme. Sie wird nicht per manueller YAML-Bearbeitung gepflegt, sondern ueber das Editor-Werkzeug `GameSceneBuilder` erzeugt.

## Szene
- Pfad: `Assets/Projekt/Content/Scenes/GameScene.unity`
- Build Settings:
  1. `Assets/Projekt/Content/Scenes/StartScene.unity`
  2. `Assets/Projekt/Content/Scenes/GameScene.unity`
- StartScene bleibt Einstiegspunkt.
- `LoadGamePanel` nutzt den SceneName aus dem SaveSlot und faellt bei fehlendem SceneName auf `GameScene` zurueck.

## Hierarchie
- `_SceneRoot`
- `_Bootstrap`
- `_Systems`
- `_UI`
- `World`
  - `Ground`
  - `Roads`
  - `Buildings`
  - `Nature`
  - `Walls`
  - `Interactables`
- `Characters`
  - `Player`
  - `Arthur`
  - `Bernd`
- `Cameras`
- `Lighting`

## Verwendete Systeme
- Player: `PlayerController`, `PlayerInputReader`, `PlayerMotor2D`, `PlayerNameTag`
- Save/Session: `SavegameRuntimeSession`, `PlayerSession`, `SaveSystem`
- Runtime-Systeme: `GameSystemsBootstrap`, `SettingsManager`, `RuntimeInventory`, `ToolbeltController`, `SkillRuntimeManager`, `AchievementManager`, `ProgressManager`, `ScenarioManager`, `DevPanelBootstrap`
- UI: `HudView`, `HudController`, `QuizPanel`, `TerminalPanel`, `InteractionPromptView`
- Interaktion: `InteractionDetector`, `InteractionController`, `BerndInteractableAdapter`, `TestWorldInteractable`
- NPC: `BerndQuizStarter`, `BerndDetectionZone`, `BerndMovementToPlayer`, `BerndAnimationController`, `ArthurAnimationController`
- Kamera: `SimpleCameraFollow2D`

## Testpunkte
- Bernd startet `BerndIntroQuiz.asset` per Interaktion.
- `Diensthandy` wird ueber `RuntimeInventory` aufgenommen und triggert ein Achievement.
- `NetworkTerminal` oeffnet das vorhandene `TerminalPanel` und gibt Netzwerk-XP.
- `AchievementTrigger` loest ein Demo-Achievement aus.
- DevPanel ist per `F12` erreichbar.
- Settings sind ueber `GameSystemsBootstrap` verfuegbar.

## Verwendete Sprites / Tiles
- Tileset: `Assets/Projekt/Content/Art/Floors/cloud_tileset.png`
- Tile-Assets: `Assets/Projekt/Content/Art/Tiles/cloud_tileset_*.asset`
- Ground:
  - `SnowGround_Tilemap` nutzt `cloud_tileset_0.asset` als Schnee-/Boden-Tile.
- Wege:
  - `MainRoad_Tilemap` nutzt `cloud_tileset_164.asset`.
  - `NorthPath_Tilemap` nutzt `cloud_tileset_177.asset`.
  - `NpcPlaza_Tilemap` nutzt `cloud_tileset_99.asset`.
- Gebaeude:
  - `BlueHouse`, `OrangeHouse` und `Depot` werden aus Sliced-SpriteRenderern zusammengesetzt.
  - Genutzte Sprites: `cloud_tileset_105`, `cloud_tileset_187`, `cloud_tileset_191`, `cloud_tileset_207`, `cloud_tileset_211`, `cloud_tileset_238`.
- Natur / Deko:
  - Baeume nutzen `cloud_tileset_72`.
  - Blumen-/Dekoflaechen nutzen `cloud_tileset_487`.
- Mauern / Barrieren:
  - Sichtbare Zaeune nutzen `cloud_tileset_422` und `cloud_tileset_424`.
- Interactables:
  - Diensthandy: `cloud_tileset_474`
  - NetworkTerminal: `cloud_tileset_464`
  - AchievementTrigger: `cloud_tileset_501`
- Player/NPC:
  - `Assets/Projekt/Content/Art/Player/character.png`
  - `Assets/Projekt/Content/Art/Player/character blue.png`
  - `Assets/Projekt/Content/Art/Player/character green.png`

## Collision Setup
- Player:
  - `Rigidbody2D` Dynamic
  - `CapsuleCollider2D`
  - Rotation eingefroren
  - Interpolation aktiv
  - Collision Detection Continuous
- Welt:
  - Gebaeude, Mauern und Grenzen nutzen `BoxCollider2D`
  - Gebaeude-Collider liegen auf dem Root und sind auf die sichtbaren Hauskoerper begrenzt
  - Baum-Collider blockieren nur den unteren Stamm-/Standbereich
  - Testinteraktionen nutzen Trigger-Collider
  - Deko wie Baumkronen blockiert nicht
- NPCs:
  - physischer `CapsuleCollider2D` am Root
  - separater Trigger fuer Bernds Interaktionszone

## Editor Tools
- `Assets/Projekt/Editor/SceneBuilding/GameSceneBuilder.cs`
  - baut die Szene neu
  - baut die sichtbare Welt aus vorhandenen Tileset-Sprites und Tile-Assets
  - `RebuildVisualWorld()` ersetzt nur den World-Bereich der bestehenden GameScene
  - `ReplacePlaceholderSprites()` erzeugt die spritebasierte Testwelt neu
  - `ValidateWorldSprites()` meldet SpriteRenderer ohne Sprite
  - erzeugt nur noch ein kleines Fallback-TestWorld-Sprite, falls ein erwartetes Tileset-Sprite fehlt
  - aktualisiert BuildSettings ohne bestehende Eintraege zu entfernen
- `Assets/Projekt/Editor/SceneValidation/GameSceneValidator.cs`
  - prueft Kernobjekte
  - prueft MainCamera, EventSystem, PlayerSpawn, Bernd-Quiz, QuizPanel und BuildSettings
  - prueft `SnowGround_Tilemap` und `MainRoad_Tilemap`
  - prueft SpriteRenderer ohne Sprite
  - warnt vor auffaellig grossen Nicht-Trigger-Collidern
  - meldet Missing Scripts

## Inspector-Referenzen
- `GameSceneBootstrap.player` zeigt auf `Characters/Player`.
- `GameSceneBootstrap.hudController` zeigt auf `_UI/GameplayCanvas/HUD`.
- `BerndQuizStarter.quizSet` zeigt auf `Assets/Projekt/Content/Quiz/BerndIntroQuiz.asset`.
- `BerndQuizStarter.quizPanel` zeigt auf `_UI/GameplayCanvas/QuizPanel`.
- `InteractionController.promptView` zeigt auf `_UI/GameplayCanvas/InteractionPrompt`.

## Risiken / Offene Punkte
- Es gibt weiterhin keine fertigen World-Prefabs fuer Haeuser, Zaeune oder Baeume; die Testwelt setzt diese deshalb aus vorhandenen Tileset-Sprites zusammen.
- `Assets/Projekt/Content/Art/TestWorld/testworld_square.png` bleibt als klar benannter Fallback-Platzhalter erhalten, wird aber nur verwendet, wenn ein erwartetes Tileset-Sprite fehlt.
- Inventory, Skills und Achievements bleiben noch ohne Savegame-Persistenz.
- Arthur ist in der GameScene als NPC-Praesenz gesetzt, aber nicht an den Startmenue-Flow gekoppelt.
- Eine finale Pause-Menue-Integration ist offen.
- Kein Layer-/Collision-Matrix-Umbau wurde vorgenommen.

## Validierung
- Unity Batchmode `GameSceneBuilder.BuildGameScene`: erfolgreich.
- Unity Batchmode `GameSceneBuilder.RebuildVisualWorld`: erfolgreich.
- Unity Batchmode `GameSceneValidator.ValidateGameScene`: `GameScene OK. Warnings=0`.
- Textsuche nach Missing Scripts (`m_Script: {fileID: 0}`): keine Treffer fuer GameScene/StartScene/Prefabs.
