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

## Collision Setup
- Player:
  - `Rigidbody2D` Dynamic
  - `CapsuleCollider2D`
  - Rotation eingefroren
  - Interpolation aktiv
  - Collision Detection Continuous
- Welt:
  - Gebaeude, Mauern und Grenzen nutzen `BoxCollider2D`
  - Testinteraktionen nutzen Trigger-Collider
  - Deko wie Baumkronen blockiert nicht
- NPCs:
  - physischer `CapsuleCollider2D` am Root
  - separater Trigger fuer Bernds Interaktionszone

## Editor Tools
- `Assets/Projekt/Editor/SceneBuilding/GameSceneBuilder.cs`
  - baut die Szene neu
  - erzeugt kleine TestWorld-Sprite-Assets
  - aktualisiert BuildSettings ohne bestehende Eintraege zu entfernen
- `Assets/Projekt/Editor/SceneValidation/GameSceneValidator.cs`
  - prueft Kernobjekte
  - prueft MainCamera, EventSystem, PlayerSpawn, Bernd-Quiz, QuizPanel und BuildSettings
  - meldet Missing Scripts

## Inspector-Referenzen
- `GameSceneBootstrap.player` zeigt auf `Characters/Player`.
- `GameSceneBootstrap.hudController` zeigt auf `_UI/GameplayCanvas/HUD`.
- `BerndQuizStarter.quizSet` zeigt auf `Assets/Projekt/Content/Quiz/BerndIntroQuiz.asset`.
- `BerndQuizStarter.quizPanel` zeigt auf `_UI/GameplayCanvas/QuizPanel`.
- `InteractionController.promptView` zeigt auf `_UI/GameplayCanvas/InteractionPrompt`.

## Risiken / Offene Punkte
- Die Testwelt nutzt bewusst einfache farbige Sprite-Bloecke statt finaler Art-Prefabs.
- Inventory, Skills und Achievements bleiben noch ohne Savegame-Persistenz.
- Arthur ist in der GameScene als NPC-Praesenz gesetzt, aber nicht an den Startmenue-Flow gekoppelt.
- Eine finale Pause-Menue-Integration ist offen.
- Kein Layer-/Collision-Matrix-Umbau wurde vorgenommen.

## Validierung
- Unity Batchmode `GameSceneBuilder.BuildGameScene`: erfolgreich.
- Unity Batchmode `GameSceneValidator.ValidateGameScene`: `GameScene OK. Warnings=0`.
- Textsuche nach Missing Scripts (`m_Script: {fileID: 0}`): keine Treffer fuer GameScene/StartScene/Prefabs.
