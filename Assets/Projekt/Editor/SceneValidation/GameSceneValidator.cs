/*
 * Datei: GameSceneValidator.cs
 * Zweck: Prueft GameScene und BuildSettings auf zentrale MVP-Testwelt-Anforderungen.
 * Verantwortung: Meldet fehlende Kernobjekte, Missing Scripts, BuildSettings, Sprite-/Collider-Probleme und wichtige Referenzen in der Console.
 * Abhaengigkeiten: UnityEditor, EditorSceneManager, GameScene-Komponenten.
 * Verwendung: Menue `ITAA/Validation/Validate GameScene` oder Batchmode `ITAA.EditorTools.SceneValidation.GameSceneValidator.ValidateGameScene`.
 */

#if UNITY_EDITOR
using ITAA.Core.SceneManagement;
using ITAA.Gameplay.Scene;
using ITAA.NPC.Bernd;
using ITAA.Player.Movement;
using ITAA.UI.Panels;
using UnityEditor.SceneManagement;
using UnityEditor;
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
            errors += RequireObject("Characters");
            errors += RequireObject("Cameras");
            errors += RequireObject("Lighting");

            errors += RequireComponent<PlayerController>("Player");
            errors += RequireComponent<PlayerSpawnPoint>("PlayerSpawn_Default");
            errors += RequireComponent<GameSceneBootstrap>("_Bootstrap");
            errors += RequireComponent<Camera>("Main Camera");
            errors += RequireComponent<EventSystem>("EventSystem");
            errors += RequireComponent<QuizPanel>("QuizPanel");
            errors += RequireComponent<BerndQuizStarter>("Bernd");
            errors += RequireComponent<SpriteRenderer>("Bernd");
            errors += RequireComponent<SpriteRenderer>("Arthur");
            errors += RequireComponent<Tilemap>("SnowGround_Tilemap");
            errors += RequireComponent<Tilemap>("MainRoad_Tilemap");

            warnings += WarnIfMissingTag("Player", "Player");
            warnings += WarnIfMissingBuildScene(StartScenePath);
            warnings += WarnIfMissingBuildScene(GameScenePath);
            warnings += CountSpriteRenderersWithoutSprites();
            warnings += CountLargeColliders();
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
