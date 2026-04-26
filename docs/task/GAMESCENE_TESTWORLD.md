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
  - `Grid`
    - `Ground_Base`
    - `Ground_Details`
    - `Roads`
    - `Buildings`
    - `Nature`
    - `Props`
    - `Collision`
  - `PlayerSpawn_Default`
  - `RuntimeInteractables`
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

## Koordinatenlayout
- Tile Size: 1 Unity Unit = 1 Tile
- Map Size: 40 x 30 Tiles
- Koordinatensystem: `(0,0)` unten links, X nach rechts, Y nach oben
- Zentraler Platz: X 15-25, Y 10-18
- PlayerSpawn und Player: `(20,14)`
- Hauptweg horizontal: X 0-39, Y 14
- Hauptweg vertikal: X 20, Y 0-29
- Quiz-Zone: X 17-23, Y 20-26
- Bernd: `(20,19)`
- Inventory-Zone: X 5-10, Y 12-17
- Diensthandy: `(7,14)`
- Skill-Zone: X 30-35, Y 12-17
- NetworkTerminal: `(32,14)`
- Sued-Ausgang: X 18-22, Y 0-4
- Arthur: `(20,8)`
- AchievementTrigger: `(22,16)`

## Verwendete Sprites / Tiles
- Source Tileset: `Assets/Projekt/Content/Art/Floors/winter_town_tileset.png`
- Clean Tileset: `Assets/Projekt/Content/Art/TestWorld/WinterTownTileset_Clean_32.png`
- Mapping: `Assets/Projekt/Content/Tiles/TestWorld/tile_mapping.json`
- Tile-Assets: `Assets/Projekt/Content/Tiles/TestWorld/<Kategorie>/<tile_name>.asset`
- Unity-ready Art-Atlas: `Assets/Projekt/Content/Art/Tiles/WinterTownTileset_Clean_32.png`
- Art-Atlas-Mapping: `Assets/Projekt/Content/Art/Tiles/WinterTownTileset_Clean_32.mapping.json`
- Separate grosse Props: `Assets/Projekt/Content/Art/Props/TestWorld/`
- Import Settings:
  - Sprite Mode: Multiple
  - Pixels Per Unit: 32
  - Filter Mode: Point
  - Compression: None
  - Slice: 32x32 Grid, Pivot Center
- Kategorien:
  - `Ground`
  - `Roads`
  - `Walls`
  - `Buildings`
  - `Roofs`
  - `Nature`
  - `Props`
  - `Interactables`
  - `Water`
  - `Details`
- Ground:
  - `Ground_Base` mischt `ground_snow_01`, `ground_snow_02` und `ground_snow_detail_01`.
  - `Ground_Details` nutzt `snow_shadow_01` sparsam als Schnee-/Detailvariante.
- Wege:
  - `Roads` nutzt `road_cobble_h`, `road_cobble_v`, `road_cobble_center`, `road_cobble_cross` und `road_cobble_edge_top`.
- Gebaeude:
  - `Buildings` nutzt `building_wall_stone_01`, `building_door_wood` und vorbereitete Building-Varianten.
  - `Roofs` nutzt `roof_blue_center` fuer das Quiz-Haus.
- Natur / Deko:
  - `Nature` nutzt `tree_pine_01`, `tree_pine_02`, `bush_snow_01`, `flower_snow_01`, `rock_snow_01`.
- Mauern / Barrieren:
  - `Walls` stellt `wall_stone_h`, `wall_stone_v`, `wall_stone_corner`, `fence_wood_h`, `fence_wood_v`, `fence_wood_gate` bereit.
  - `Collision` nutzt `wall_stone_h` als blockierendes Tile.
- Interactables:
  - Diensthandy: `item_marker_01`
  - NetworkTerminal: `terminal_01`
  - AchievementTrigger / NPC Marker: `npc_marker_quiz`
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
  - Blockierende Zellen liegen ausschliesslich auf der `Collision`-Tilemap.
  - `Collision` nutzt `TilemapCollider2D`, `CompositeCollider2D` und einen statischen `Rigidbody2D`.
  - Gebaeude, Boundary, harte Props und der Sued-Ausgang werden dort blockierend markiert.
  - `Ground_Base`, `Ground_Details` und `Roads` haben keine Collider.
  - Testinteraktionen nutzen Trigger-Collider
  - Deko und kleine Markierungen blockieren nicht
- NPCs:
  - physischer `CapsuleCollider2D` am Root
  - separater Trigger fuer Bernds Interaktionszone

## Sorting
- `Ground_Base`: Order 0
- `Roads`: Order 1
- `Ground_Details`: Order 2
- `Props`: Order 4
- `Buildings`: Order 5
- `Nature`: Order 6
- Player/NPC: Order 10
- `Collision`: Order 20, bleibt als Debug-/Collision-Layer sichtbar
- UI bleibt Screen Space Canvas

## Editor Tools
- `Assets/Projekt/Editor/SceneBuilding/GameSceneBuilder.cs`
  - baut die Szene neu
  - `RebuildCleanWinterTownTileset()` erzeugt `WinterTownTileset_Clean_32.png`, Tile-Assets und `tile_mapping.json`
  - baut die sichtbare Welt als strukturiertes 40x30-Tilemap-Layout
  - `RebuildStructuredWinterHub()` baut nur `World` neu und positioniert Player, Bernd, Arthur, Kamera, Spawn, Item und Terminal
  - `RebuildVisualWorld()` ersetzt nur den World-Bereich der bestehenden GameScene
  - `ReplacePlaceholderSprites()` erzeugt die spritebasierte Testwelt neu
  - `ValidateWorldSprites()` meldet SpriteRenderer ohne Sprite
  - erzeugt nur noch ein kleines Fallback-TestWorld-Sprite, falls ein erwartetes Tileset-Sprite fehlt
  - aktualisiert BuildSettings ohne bestehende Eintraege zu entfernen
- `Assets/Projekt/Editor/SceneBuilding/WinterTownTilesetCleaner.cs`
  - extrahiert aus dem gelabelten Preview-Bild einen label-freien 1024x1024-Art-Atlas im 32x32-Raster
  - schreibt `WinterTownTileset_Clean_32.mapping.json` mit Grid-Koordinaten, Kategorien, Collision-Flags und Sorting-Orders
  - exportiert grosse Props separat unter `Assets/Projekt/Content/Art/Props/TestWorld/`
  - setzt Unity-Importwerte fuer Point-Filter, 32 PPU, unkomprimierte Texturen und Sprite-Slicing
- `Assets/Projekt/Editor/SceneValidation/GameSceneValidator.cs`
  - prueft Kernobjekte
  - prueft MainCamera, EventSystem, PlayerSpawn, Bernd-Quiz, QuizPanel und BuildSettings
  - prueft die Pflicht-Tilemaps `Ground_Base`, `Ground_Details`, `Roads`, `Buildings`, `Nature`, `Props`, `Collision`
  - prueft Camera-Follow-Target, Bernd-InteractionZone und grobe Erreichbarkeit von Bernd, Diensthandy und Terminal
  - prueft SpriteRenderer ohne Sprite
  - prueft TilemapRenderer ohne Tilemap
  - prueft, dass `Ground_Base`, `Ground_Details` und `Roads` keine Collider tragen
  - warnt vor auffaellig grossen Nicht-Trigger-Collidern
  - meldet Missing Scripts

## Inspector-Referenzen
- `GameSceneBootstrap.player` zeigt auf `Characters/Player`.
- `GameSceneBootstrap.hudController` zeigt auf `_UI/GameplayCanvas/HUD`.
- `BerndQuizStarter.quizSet` zeigt auf `Assets/Projekt/Content/Quiz/BerndIntroQuiz.asset`.
- `BerndQuizStarter.quizPanel` zeigt auf `_UI/GameplayCanvas/QuizPanel`.
- `InteractionController.promptView` zeigt auf `_UI/GameplayCanvas/InteractionPrompt`.

## Risiken / Offene Punkte
- Das gelabelte Source-Tileset wird nicht direkt in der Szene genutzt; die Szene referenziert die daraus erzeugten sauberen 32x32-Tile-Assets.
- Der neue Art-Atlas unter `Assets/Projekt/Content/Art/Tiles/` ist fuer Unity-Slicing und weitere Tile-Erstellung vorbereitet; die bestehende GameScene nutzt weiterhin die bereits erzeugten Tile-Assets unter `Assets/Projekt/Content/Tiles/TestWorld/`.
- `Assets/Projekt/Content/Art/TestWorld/testworld_square.png` bleibt als klar benannter Fallback-Platzhalter erhalten, wird aber nur verwendet, wenn ein erwartetes Tileset-Sprite fehlt.
- Einige Kategorien wie `npc_marker_*`, `player_spawn_marker` und `fence_wood_gate` nutzen bestpassende vorhandene Winter-Town-Icons/Props, weil keine eindeutig benannten Spezialtiles existieren.
- Inventory, Skills und Achievements bleiben noch ohne Savegame-Persistenz.
- Arthur ist in der GameScene als NPC-Praesenz gesetzt, aber nicht an den Startmenue-Flow gekoppelt.
- Eine finale Pause-Menue-Integration ist offen.
- Kein Layer-/Collision-Matrix-Umbau wurde vorgenommen.

## Validierung
- Unity Batchmode `GameSceneBuilder.BuildGameScene`: erfolgreich.
- Unity Batchmode `GameSceneBuilder.RebuildVisualWorld`: erfolgreich.
- Unity Batchmode `GameSceneBuilder.RebuildStructuredWinterHub`: erfolgreich.
- Unity Batchmode `GameSceneBuilder.RebuildCleanWinterTownTileset`: erfolgreich.
- Unity Batchmode `WinterTownTilesetCleaner.CreateCleanTileset`: erfolgreich.
- Unity Batchmode `GameSceneValidator.ValidateGameScene`: `GameScene OK. Warnings=0`.
- Textsuche nach Missing Scripts (`m_Script: {fileID: 0}`): keine Treffer fuer GameScene/StartScene/Prefabs.
