/*
 * Datei: GameSceneBootstrap.cs
 * Zweck: Verbindet die GameScene beim Start mit vorhandenen Session-, Player- und UI-Systemen.
 * Verantwortung: Setzt Player-Spawn, stellt Fallback-Sessiondaten bereit, aktualisiert NameTags/HUD und aktiviert optionale Runtime-Systeme.
 * Abhaengigkeiten: GameSystemsBootstrap, PlayerSession, SavegameRuntimeSession, PlayerNameTag, HudController, Unity SceneManagement.
 * Verwendung: Liegt auf `_Bootstrap` in der GameScene; enthaelt keine Spiellogik, nur sichere Szenenverdrahtung.
 */

using ITAA.Core.SceneManagement;
using ITAA.Features.HUD;
using ITAA.Player.Session;
using ITAA.Player.UI;
using ITAA.System.Savegame;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ITAA.Gameplay.Scene
{
    [DisallowMultipleComponent]
    public sealed class GameSceneBootstrap : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform player;
        [SerializeField] private HudController hudController;

        [Header("Spawn")]
        [SerializeField] private string spawnId = "Default";
        [SerializeField] private bool preferSavePosition = true;

        [Header("Fallback")]
        [SerializeField] private string fallbackPlayerName = "Player";

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs = true;

        private void Start()
        {
            GameSystemsBootstrap.EnsureForScene(SceneManager.GetActiveScene());
            EnsureRuntimeSession();
            EnsurePlayerSession();
            ResolveReferences();
            ApplyPlayerSpawn();
            RefreshPlayerNameTags();
            RefreshHud();
        }

        public void ResetPlayerToSpawn()
        {
            ResolveReferences();
            MovePlayer(ResolveSpawnPosition());
            Log("Player auf Spawn zurueckgesetzt.");
        }

        private void ResolveReferences()
        {
            if (player == null)
            {
                GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
                player = playerObject != null ? playerObject.transform : null;
            }

            if (hudController == null)
            {
                hudController = FindAnyObjectByType<HudController>(FindObjectsInactive.Include);
            }
        }

        private void EnsureRuntimeSession()
        {
            if (SavegameRuntimeSession.Instance != null)
            {
                return;
            }

            GameObject sessionObject = new GameObject(nameof(SavegameRuntimeSession));
            sessionObject.AddComponent<SavegameRuntimeSession>();
            Log("Fallback SavegameRuntimeSession erzeugt.");
        }

        private void EnsurePlayerSession()
        {
            if (PlayerSession.Instance == null)
            {
                GameObject sessionObject = new GameObject("PlayerSessionManager");
                sessionObject.AddComponent<PlayerSession>();
                Log("Fallback PlayerSession erzeugt.");
            }

            SavegameRuntimeSession runtimeSession = SavegameRuntimeSession.Instance;
            if (runtimeSession != null && runtimeSession.HasSave())
            {
                PlayerSession.Instance.ApplySaveGameData(runtimeSession.GetCurrentSave());
                return;
            }

            SaveGameData fallbackSave = new SaveGameData
            {
                SlotId = 0,
                DisplayName = "Editor Test",
                PlayerName = fallbackPlayerName,
                SceneName = SceneNames.GameScene,
                Level = 1,
                Score = 0,
                HasData = true
            };

            fallbackSave.SetPlayerPosition(ResolveSpawnPosition());
            runtimeSession?.SetCurrentSave(fallbackSave);
            PlayerSession.Instance.ApplySaveGameData(fallbackSave);
            Log("Fallback-Testdaten fuer direkten GameScene-Start gesetzt.");
        }

        private void ApplyPlayerSpawn()
        {
            if (player == null)
            {
                Debug.LogWarning($"[{nameof(GameSceneBootstrap)}] Player fehlt.", this);
                return;
            }

            MovePlayer(ResolveSpawnPosition());
        }

        private Vector3 ResolveSpawnPosition()
        {
            SavegameRuntimeSession runtimeSession = SavegameRuntimeSession.Instance;
            SaveGameData saveData = runtimeSession != null ? runtimeSession.GetCurrentSave() : null;

            if (preferSavePosition && saveData != null && saveData.HasData)
            {
                Vector3 savedPosition = saveData.GetPlayerPosition();

                if (savedPosition.sqrMagnitude > 0.0001f)
                {
                    return savedPosition;
                }
            }

            PlayerSpawnPoint spawnPoint = FindSpawnPoint();
            return spawnPoint != null ? spawnPoint.transform.position : Vector3.zero;
        }

        private PlayerSpawnPoint FindSpawnPoint()
        {
            PlayerSpawnPoint[] spawnPoints = FindObjectsByType<PlayerSpawnPoint>(FindObjectsInactive.Include);

            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if (spawnPoints[i] != null && spawnPoints[i].SpawnId == spawnId)
                {
                    return spawnPoints[i];
                }
            }

            return spawnPoints.Length > 0 ? spawnPoints[0] : null;
        }

        private void MovePlayer(Vector3 position)
        {
            if (player == null)
            {
                return;
            }

            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.position = position;
                rb.linearVelocity = Vector2.zero;
            }

            player.position = position;
        }

        private void RefreshPlayerNameTags()
        {
            PlayerNameTag[] nameTags = FindObjectsByType<PlayerNameTag>(FindObjectsInactive.Include);

            for (int i = 0; i < nameTags.Length; i++)
            {
                nameTags[i].UpdateName();
            }
        }

        private void RefreshHud()
        {
            if (hudController == null)
            {
                return;
            }

            hudController.SetCurrentObjective("Sprich mit Bernd oder pruefe das Terminal.");
            hudController.SetCurrentTopic("Netzwerk");
            hudController.RefreshHud();
        }

        private void Log(string message)
        {
            if (!enableDebugLogs)
            {
                return;
            }

            Debug.Log($"[GameScene] {message}", this);
        }
    }
}
