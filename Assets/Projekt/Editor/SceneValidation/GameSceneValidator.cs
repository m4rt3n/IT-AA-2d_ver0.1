/*
 * Datei: GameSceneValidator.cs
 * Zweck: Prueft GameScene und BuildSettings auf zentrale MVP-Testwelt-Anforderungen.
 * Verantwortung: Meldet fehlende Kernobjekte, Tilemap-Struktur, Missing Scripts, BuildSettings, Sprite-/Collider-Probleme und wichtige Referenzen in der Console.
 * Abhaengigkeiten: UnityEditor, EditorSceneManager, Tilemaps, GameScene-Komponenten.
 * Verwendung: Menue `ITAA/Validation/Validate GameScene` oder Batchmode `ITAA.EditorTools.SceneValidation.GameSceneValidator.ValidateGameScene`.
 */

#if UNITY_EDITOR
using System.Collections.Generic;
using ITAA.Core.SceneManagement;
using ITAA.Gameplay.Scene;
using ITAA.Gameplay.World;
using ITAA.NPC.Bernd;
using ITAA.Player.Movement;
using ITAA.UI.Panels;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

namespace ITAA.EditorTools.SceneValidation
{
    public static class GameSceneValidator
    {
        private const string GameScenePath = "Assets/Projekt/Content/Scenes/GameScene.unity";
        private const string StartScenePath = "Assets/Projekt/Content/Scenes/StartScene.unity";

        [MenuItem("ITAA/Validation/Validate GameScene")]
        public static void ValidateGameScene()
        {
            if (!global::System.IO.File.Exists(GameScenePath))
            {
                Debug.LogError($"[SceneValidation] GameScene fehlt: {GameScenePath}");
                return;
            }

            EditorSceneManager.OpenScene(GameScenePath);
            int errors = 0;
            int warnings = 0;

            errors += RequireObject("_SceneRoot");
            errors += RequireObject("_Bootstrap");
            errors += RequireObject("_Systems");
            errors += RequireObject("_UI");
            errors += RequireObject("World");
            errors += RequireObject("Grid");
            errors += RequireObject("Characters");
            errors += RequireObject("Cameras");
            errors += RequireObject("Lighting");
            errors += RequireObject("GameplayCanvas");
            errors += RequireObject("HUD");
            errors += RequireObject("InteractionPrompt");
            errors += RequireObject("InteractionZone");

            errors += RequireComponent<PlayerController>("Player");
            errors += RequireComponent<PlayerSpawnPoint>("PlayerSpawn_Default");
            errors += RequireComponent<GameSceneBootstrap>("_Bootstrap");
            errors += RequireComponent<Camera>("Main Camera");
            errors += RequireComponent<SimpleCameraFollow2D>("Main Camera");
            errors += RequireComponent<EventSystem>("EventSystem");
            errors += RequireComponent<QuizPanel>("QuizPanel");
            errors += RequireComponent<BerndQuizStarter>("Bernd");
            errors += RequireComponent<SpriteRenderer>("Bernd");
            errors += RequireComponent<SpriteRenderer>("Arthur");
            errors += RequireComponent<Tilemap>("Ground_Base");
            errors += RequireComponent<Tilemap>("Ground_Details");
            errors += RequireComponent<Tilemap>("Roads");
            errors += RequireComponent<Tilemap>("Buildings");
            errors += RequireComponent<Tilemap>("Roofs");
            errors += RequireComponent<Tilemap>("Nature");
            errors += RequireComponent<Tilemap>("Props");
            errors += RequireComponent<Tilemap>("Interactables");
            errors += RequireComponent<Tilemap>("Collision");
            errors += RequireComponent<TilemapCollider2D>("Collision");
            errors += RequireComponent<CompositeCollider2D>("Collision");

            warnings += WarnIfMissingTag("Player", "Player");
            warnings += WarnIfMissingBuildScene(StartScenePath);
            warnings += WarnIfMissingBuildScene(GameScenePath);
            warnings += WarnIfCameraFollowTargetMissing();
            warnings += CountSpriteRenderersWithoutSprites();
            warnings += CountTilemapRenderersWithoutTilemap();
            warnings += CountForbiddenTilemapColliders();
            warnings += CountLargeColliders();
            warnings += WarnIfKeyObjectsOffGrid();
            warnings += WarnIfTargetsUnreachable();
            errors += CountMissingScripts();

            if (errors == 0)
            {
                Debug.Log($"[SceneValidation] GameScene OK. Warnings={warnings}");
            }
            else
            {
                Debug.LogError($"[SceneValidation] GameScene hat {errors} Fehler und {warnings} Warnungen.");
            }
        }

        private static int RequireObject(string objectName)
        {
            if (FindObjectByName(objectName) != null)
            {
                Debug.Log($"[SceneValidation] OK: {objectName}");
                return 0;
            }

            Debug.LogError($"[SceneValidation] FEHLT: {objectName}");
            return 1;
        }

        private static int RequireComponent<T>(string objectName) where T : Component
        {
            GameObject gameObject = FindObjectByName(objectName);
            if (gameObject == null)
            {
                Debug.LogError($"[SceneValidation] FEHLT: {objectName} fuer {typeof(T).Name}");
                return 1;
            }

            if (gameObject.GetComponentInChildren<T>(true) != null)
            {
                Debug.Log($"[SceneValidation] OK: {typeof(T).Name} auf {objectName}");
                return 0;
            }

            Debug.LogError($"[SceneValidation] FEHLT: {typeof(T).Name} auf {objectName}");
            return 1;
        }

        private static int WarnIfMissingTag(string objectName, string expectedTag)
        {
            GameObject gameObject = FindObjectByName(objectName);
            if (gameObject != null && gameObject.CompareTag(expectedTag))
            {
                return 0;
            }

            Debug.LogWarning($"[SceneValidation] WARNUNG: {objectName} hat nicht Tag {expectedTag}.");
            return 1;
        }

        private static int WarnIfMissingBuildScene(string path)
        {
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (scene != null && scene.path == path && scene.enabled)
                {
                    return 0;
                }
            }

            Debug.LogWarning($"[SceneValidation] WARNUNG: BuildSettings enthalten {path} nicht aktiv.");
            return 1;
        }

        private static int CountMissingScripts()
        {
            int missing = 0;
            GameObject[] objects = Object.FindObjectsByType<GameObject>(FindObjectsInactive.Include);

            for (int i = 0; i < objects.Length; i++)
            {
                int count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(objects[i]);
                if (count <= 0)
                {
                    continue;
                }

                missing += count;
                Debug.LogError($"[SceneValidation] Missing Script auf {objects[i].name}: {count}");
            }

            return missing;
        }

        private static int CountSpriteRenderersWithoutSprites()
        {
            int warnings = 0;
            SpriteRenderer[] renderers = Object.FindObjectsByType<SpriteRenderer>(FindObjectsInactive.Include);

            for (int i = 0; i < renderers.Length; i++)
            {
                SpriteRenderer renderer = renderers[i];
                if (renderer == null || renderer.sprite != null)
                {
                    continue;
                }

                warnings++;
                Debug.LogWarning($"[SceneValidation] WARNUNG: SpriteRenderer ohne Sprite auf {renderer.gameObject.name}.", renderer);
            }

            return warnings;
        }

        private static int CountTilemapRenderersWithoutTilemap()
        {
            int warnings = 0;
            TilemapRenderer[] renderers = Object.FindObjectsByType<TilemapRenderer>(FindObjectsInactive.Include);

            for (int i = 0; i < renderers.Length; i++)
            {
                TilemapRenderer renderer = renderers[i];
                if (renderer == null || renderer.GetComponent<Tilemap>() != null)
                {
                    continue;
                }

                warnings++;
                Debug.LogWarning($"[SceneValidation] WARNUNG: TilemapRenderer ohne Tilemap auf {renderer.gameObject.name}.", renderer);
            }

            return warnings;
        }

        private static int CountForbiddenTilemapColliders()
        {
            int warnings = 0;
            string[] forbiddenNames = { "Ground_Base", "Ground_Details", "Roads" };

            for (int i = 0; i < forbiddenNames.Length; i++)
            {
                GameObject gameObject = FindObjectByName(forbiddenNames[i]);
                if (gameObject == null || gameObject.GetComponent<Collider2D>() == null)
                {
                    continue;
                }

                warnings++;
                Debug.LogWarning($"[SceneValidation] WARNUNG: {forbiddenNames[i]} darf keinen Collider tragen.", gameObject);
            }

            return warnings;
        }

        private static int CountLargeColliders()
        {
            int warnings = 0;
            Collider2D[] colliders = Object.FindObjectsByType<Collider2D>(FindObjectsInactive.Include);

            for (int i = 0; i < colliders.Length; i++)
            {
                Collider2D collider = colliders[i];
                if (collider == null || collider.isTrigger)
                {
                    continue;
                }

                Bounds bounds = collider.bounds;
                bool isBoundary = collider.gameObject.name.Contains("Fence") || collider.gameObject.name.Contains("Boundary");
                if (!isBoundary && (bounds.size.x > 6f || bounds.size.y > 4f))
                {
                    warnings++;
                    Debug.LogWarning(
                        $"[SceneValidation] WARNUNG: Grosser Collider auf {collider.gameObject.name}: {bounds.size}.",
                        collider);
                }
            }

            return warnings;
        }

        private static int WarnIfCameraFollowTargetMissing()
        {
            SimpleCameraFollow2D follow = Object.FindAnyObjectByType<SimpleCameraFollow2D>(FindObjectsInactive.Include);
            if (follow == null)
            {
                return 0;
            }

            SerializedObject serializedObject = new SerializedObject(follow);
            SerializedProperty targetProperty = serializedObject.FindProperty("target");
            if (targetProperty != null && targetProperty.objectReferenceValue != null)
            {
                return 0;
            }

            Debug.LogWarning("[SceneValidation] WARNUNG: Camera Follow Target ist nicht gesetzt.");
            return 1;
        }

        private static int WarnIfKeyObjectsOffGrid()
        {
            int warnings = 0;
            warnings += WarnIfPositionDiffers("Player", new Vector2(20f, 14f));
            warnings += WarnIfPositionDiffers("PlayerSpawn_Default", new Vector2(20f, 14f));
            warnings += WarnIfPositionDiffers("Bernd", new Vector2(20f, 19f));
            warnings += WarnIfPositionDiffers("Arthur", new Vector2(20f, 8f));
            warnings += WarnIfPositionDiffers("TestItem_Diensthandy", new Vector2(7f, 14f));
            warnings += WarnIfPositionDiffers("NetworkTerminal", new Vector2(32f, 14f));
            return warnings;
        }

        private static int WarnIfPositionDiffers(string objectName, Vector2 expected)
        {
            GameObject gameObject = FindObjectByName(objectName);
            if (gameObject == null)
            {
                return 0;
            }

            Vector2 actual = gameObject.transform.position;
            if (Vector2.Distance(actual, expected) <= 0.05f)
            {
                return 0;
            }

            Debug.LogWarning($"[SceneValidation] WARNUNG: {objectName} steht bei {actual}, erwartet {expected}.", gameObject);
            return 1;
        }

        private static int WarnIfTargetsUnreachable()
        {
            Tilemap collision = FindTilemap("Collision");
            if (collision == null)
            {
                return 0;
            }

            int warnings = 0;
            Vector2Int start = new Vector2Int(20, 14);
            warnings += WarnIfUnreachable(collision, start, new Vector2Int(20, 19), "Bernd");
            warnings += WarnIfUnreachable(collision, start, new Vector2Int(7, 14), "Diensthandy");
            warnings += WarnIfUnreachable(collision, start, new Vector2Int(32, 14), "NetworkTerminal");
            return warnings;
        }

        private static int WarnIfUnreachable(Tilemap collision, Vector2Int start, Vector2Int target, string label)
        {
            if (IsReachable(collision, start, target))
            {
                return 0;
            }

            Debug.LogWarning($"[SceneValidation] WARNUNG: {label} ist vom PlayerSpawn im Collision-Grid nicht erreichbar.");
            return 1;
        }

        private static bool IsReachable(Tilemap collision, Vector2Int start, Vector2Int target)
        {
            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
            queue.Enqueue(start);
            visited.Add(start);

            Vector2Int[] directions =
            {
                Vector2Int.right,
                Vector2Int.left,
                Vector2Int.up,
                Vector2Int.down
            };

            while (queue.Count > 0)
            {
                Vector2Int current = queue.Dequeue();
                if (current == target)
                {
                    return true;
                }

                for (int i = 0; i < directions.Length; i++)
                {
                    Vector2Int next = current + directions[i];
                    if (next.x < 0 || next.x > 39 || next.y < 0 || next.y > 29 || visited.Contains(next))
                    {
                        continue;
                    }

                    if (collision.HasTile(new Vector3Int(next.x, next.y, 0)))
                    {
                        continue;
                    }

                    visited.Add(next);
                    queue.Enqueue(next);
                }
            }

            return false;
        }

        private static Tilemap FindTilemap(string objectName)
        {
            GameObject gameObject = FindObjectByName(objectName);
            return gameObject != null ? gameObject.GetComponent<Tilemap>() : null;
        }

        private static GameObject FindObjectByName(string objectName)
        {
            GameObject[] objects = Object.FindObjectsByType<GameObject>(FindObjectsInactive.Include);

            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i] != null && objects[i].name == objectName)
                {
                    return objects[i];
                }
            }

            return null;
        }
    }
}
#endif
