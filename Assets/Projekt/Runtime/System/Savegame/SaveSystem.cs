// Datei: Assets/Projekt/Runtime/System/Savegame/SaveSystem.cs

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ITAA.System.Savegame
{
    public class SaveSystem
    {
        private const string FilePrefix = "save_slot_";
        private const string FileExtension = ".json";

        public IReadOnlyList<SaveSlotEntity> GetAllSlots(int slotCount)
        {
            List<SaveSlotEntity> slots = new List<SaveSlotEntity>(slotCount);

            for (int i = 1; i <= slotCount; i++)
            {
                SaveGameData data = Load(i);

                if (data == null)
                {
                    slots.Add(new SaveSlotEntity
                    {
                        SlotId = i,
                        DisplayName = $"Slot {i}",
                        SceneName = "-",
                        SavedAtText = "Leer",
                        HasData = false
                    });

                    continue;
                }

                slots.Add(SaveSlotEntity.FromSaveData(data));
            }

            return slots;
        }

        public void Save(SaveGameData data)
        {
            if (data == null)
            {
                Debug.LogWarning($"[{nameof(SaveSystem)}] SaveGameData ist null.");
                return;
            }

            if (data.SlotId <= 0)
            {
                Debug.LogWarning($"[{nameof(SaveSystem)}] Ungültige Slot-ID: {data.SlotId}");
                return;
            }

            if (string.IsNullOrWhiteSpace(data.SavedAtUtc))
            {
                data.SavedAtUtc = DateTime.UtcNow.ToString("O");
            }

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(GetPath(data.SlotId), json);
        }

        public SaveGameData Load(int slotId)
        {
            string path = GetPath(slotId);

            if (!File.Exists(path))
            {
                return null;
            }

            string json = File.ReadAllText(path);

            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }

            return JsonUtility.FromJson<SaveGameData>(json);
        }

        public bool HasSave(int slotId)
        {
            return File.Exists(GetPath(slotId));
        }

        public void Delete(int slotId)
        {
            string path = GetPath(slotId);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        private static string GetPath(int slotId)
        {
            return Path.Combine(Application.persistentDataPath, $"{FilePrefix}{slotId}{FileExtension}");
        }
    }
}