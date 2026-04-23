/*
 * Datei: SaveSlotEntity.cs
 * Zweck: Leichtgewichtiges UI-Modell für Save-Slots im LoadGamePanel.
 */

using System;

namespace ITAA.System.Savegame
{
    [Serializable]
    public class SaveSlotEntity
    {
        public int SlotId;
        public string DisplayName;
        public string PlayerName;
        public string SceneName;
        public string SavedAtText;
        public int Level;
        public int Score;
        public bool HasData;

        public static SaveSlotEntity CreateEmpty(int slotId)
        {
            return new SaveSlotEntity
            {
                SlotId = slotId,
                DisplayName = $"Slot {slotId}",
                PlayerName = "-",
                SceneName = "-",
                SavedAtText = "Leer",
                Level = 0,
                Score = 0,
                HasData = false
            };
        }

        public static SaveSlotEntity FromSaveData(SaveGameData data)
        {
            if (data == null)
            {
                return null;
            }

            return new SaveSlotEntity
            {
                SlotId = data.SlotId,
                DisplayName = string.IsNullOrWhiteSpace(data.DisplayName) ? $"Slot {data.SlotId}" : data.DisplayName,
                PlayerName = string.IsNullOrWhiteSpace(data.PlayerName) ? "-" : data.PlayerName,
                SceneName = string.IsNullOrWhiteSpace(data.SceneName) ? "-" : data.SceneName,
                SavedAtText = string.IsNullOrWhiteSpace(data.SavedAtText) ? "-" : data.SavedAtText,
                Level = data.Level,
                Score = data.Score,
                HasData = data.HasData
            };
        }
    }
}