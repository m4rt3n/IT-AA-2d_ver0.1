/*
 * Datei: SaveSystem.cs
 * Pfad: Assets/Projekt/Runtime/System/Savegame/SaveSystem.cs
 * Zweck: Verwaltet Laden, Speichern und Auflisten von Spielständen.
 * Verantwortung:
 * - Lädt SaveGameData aus der Persistenz
 * - Baut daraus UI-taugliche SaveSlotEntity-Einträge
 * - Liefert alle Slots für das LoadGamePanel
 */

using System.Collections.Generic;
using UnityEngine;

namespace ITAA.System.Savegame
{
    public class SaveSystem
    {
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
                        SavedAtText = "-",
                        HasData = false
                    });

                    continue;
                }

                SaveSlotEntity slot = SaveSlotEntity.FromSaveData(data);

                if (slot.SlotId <= 0)
                {
                    slot.SlotId = i;
                }

                slots.Add(slot);
            }

            return slots;
        }

        public SaveGameData Load(int slotId)
        {
            string key = GetSlotKey(slotId);

            if (!PlayerPrefs.HasKey(key))
            {
                return null;
            }

            string json = PlayerPrefs.GetString(key);

            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }

            return JsonUtility.FromJson<SaveGameData>(json);
        }

        public void Save(SaveGameData data)
        {
            if (data == null || data.SlotId <= 0)
            {
                Debug.LogError("SaveSystem.Save: Ungültige Speicherdaten.");
                return;
            }

            string key = GetSlotKey(data.SlotId);
            string json = JsonUtility.ToJson(data);

            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }

        public void Delete(int slotId)
        {
            string key = GetSlotKey(slotId);

            if (PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.DeleteKey(key);
                PlayerPrefs.Save();
            }
        }

        private string GetSlotKey(int slotId)
        {
            return $"save_slot_{slotId}";
        }
    }
}