/*
 * Datei: SaveSystem.cs
 * Zweck:
 * - Speichert und lädt Spielstände als JSON
 * - Liefert Slot-Daten für das LoadGamePanel
 * - Erstellt bei Bedarf einen Dummy-Spielstand für Tests
 */

using System;
using System.Collections.Generic;
using System.IO;
using ITAA.Core.SceneManagement;
using UnityEngine;

namespace ITAA.System.Savegame
{
    public class SaveSystem
    {
        private const string FilePrefix = "save_slot_";
        private const string FileExtension = ".json";

        private static string SaveDirectory =>
            Path.Combine(Application.persistentDataPath, "Savegames");

        public SaveSystem()
        {
            EnsureDirectoryExists();
        }

        public IReadOnlyList<SaveSlotEntity> GetAllSlots(int slotCount)
        {
            List<SaveSlotEntity> slots = new List<SaveSlotEntity>(slotCount);

            for (int i = 1; i <= slotCount; i++)
            {
                SaveGameData data = Load(i);

                if (data == null || !data.HasData)
                {
                    slots.Add(SaveSlotEntity.CreateEmpty(i));
                    continue;
                }

                slots.Add(SaveSlotEntity.FromSaveData(data));
            }

            return slots;
        }

        public SaveGameData Load(int slotId)
        {
            string path = GetSlotPath(slotId);

            if (!File.Exists(path))
            {
                return null;
            }

            try
            {
                string json = File.ReadAllText(path);

                if (string.IsNullOrWhiteSpace(json))
                {
                    return null;
                }

                return JsonUtility.FromJson<SaveGameData>(json);
            }
            catch (Exception exception)
            {
                Debug.LogError(
                    $"[{nameof(SaveSystem)}] Fehler beim Laden von Slot {slotId}: {exception.Message}"
                );
                return null;
            }
        }

        public void Save(int slotId, SaveGameData data)
        {
            if (data == null)
            {
                Debug.LogWarning($"[{nameof(SaveSystem)}] Save aufgerufen mit null-Daten.");
                return;
            }

            EnsureDirectoryExists();

            data.SlotId = slotId;
            data.HasData = true;

            string path = GetSlotPath(slotId);

            try
            {
                string json = JsonUtility.ToJson(data, true);
                File.WriteAllText(path, json);
            }
            catch (Exception exception)
            {
                Debug.LogError(
                    $"[{nameof(SaveSystem)}] Fehler beim Speichern von Slot {slotId}: {exception.Message}"
                );
            }
        }

        public void Delete(int slotId)
        {
            string path = GetSlotPath(slotId);

            if (!File.Exists(path))
            {
                return;
            }

            try
            {
                File.Delete(path);
            }
            catch (Exception exception)
            {
                Debug.LogError(
                    $"[{nameof(SaveSystem)}] Fehler beim Löschen von Slot {slotId}: {exception.Message}"
                );
            }
        }

        public void EnsureDummySaveExists()
        {
            SaveGameData existing = Load(1);

            if (existing != null && existing.HasData)
            {
                EnsureExistingDummySceneIsValid(existing);
                return;
            }

            SaveGameData dummy = new SaveGameData
            {
                SlotId = 1,
                DisplayName = "Testslot Arthur",
                PlayerName = "Martin",
                SceneName = SceneNames.GameScene,
                SavedAtText = DateTime.Now.ToString("dd.MM.yyyy HH:mm"),
                Level = 3,
                Score = 1200,
                HasData = true
            };

            dummy.SetPlayerPosition(new Vector3(2f, 1f, 0f));

            Save(1, dummy);

            Debug.Log(
                $"[{nameof(SaveSystem)}] Dummy-Spielstand in Slot 1 erstellt: {GetSlotPath(1)}"
            );
        }

        private void EnsureExistingDummySceneIsValid(SaveGameData existing)
        {
            if (existing == null)
            {
                return;
            }

            if (existing.SceneName == SceneNames.GameScene)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(existing.SceneName) && existing.SceneName != SceneNames.StartScene)
            {
                return;
            }

            existing.SceneName = SceneNames.GameScene;
            Save(existing.SlotId > 0 ? existing.SlotId : 1, existing);
        }

        private static string GetSlotPath(int slotId)
        {
            return Path.Combine(SaveDirectory, $"{FilePrefix}{slotId}{FileExtension}");
        }

        private static void EnsureDirectoryExists()
        {
            if (!Directory.Exists(SaveDirectory))
            {
                Directory.CreateDirectory(SaveDirectory);
            }
        }
    }
}
