/*
 * Datei: SavegameRuntimeSession.cs
 * Zweck:
 * - Hält den zuletzt ausgewählten Spielstand zwischen Scene-Wechseln
 * - Dient als zentrale Quelle für geladenen Save-State
 * - Grundlage für Player-Spawn / Restore nach Scene-Wechsel
 */

using UnityEngine;

namespace ITAA.System.Savegame
{
    public sealed class SavegameRuntimeSession : MonoBehaviour
    {
        public static SavegameRuntimeSession Instance { get; private set; }

        public SaveGameData CurrentSave { get; private set; }

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs = true;

        #region Unity Methods

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (enableDebugLogs)
            {
                Debug.Log($"[{nameof(SavegameRuntimeSession)}] Initialized.", this);
            }
        }

        #endregion

        #region Public API

        public void SetCurrentSave(SaveGameData saveData)
        {
            CurrentSave = saveData;

            if (enableDebugLogs)
            {
                if (saveData != null)
                {
                    Debug.Log(
                        $"[{nameof(SavegameRuntimeSession)}] Save gesetzt: Slot {saveData.SlotId}, Scene '{saveData.SceneName}'.",
                        this
                    );
                }
                else
                {
                    Debug.LogWarning($"[{nameof(SavegameRuntimeSession)}] Save gesetzt = NULL.", this);
                }
            }
        }

        public SaveGameData GetCurrentSave()
        {
            return CurrentSave;
        }

        public bool HasSave()
        {
            return CurrentSave != null && CurrentSave.HasData;
        }

        public void Clear()
        {
            CurrentSave = null;

            if (enableDebugLogs)
            {
                Debug.Log($"[{nameof(SavegameRuntimeSession)}] Save gelöscht.", this);
            }
        }

        #endregion
    }
}