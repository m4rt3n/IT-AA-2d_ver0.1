using System;

namespace ITAA.System.Savegame
{
    [Serializable] public class SaveSlotEntity
    {
        public int SlotId;
        public string DisplayName;
        public string SceneName;
        public string SavedAtText;
        public bool HasData;

        public static SaveSlotEntity FromSaveData(SaveGameData data)
        {
            if (data == null)
            {
                return new SaveSlotEntity
                {
                    SlotId = 0,
                    DisplayName = "Leerer Slot",
                    SceneName = "-",
                    SavedAtText = "-",
                    HasData = false
                };
            }

            return new SaveSlotEntity
            {
                SlotId = data.SlotId,
                DisplayName = string.IsNullOrWhiteSpace(data.PlayerName) ? $"Slot {data.SlotId}" : data.PlayerName,
                SceneName = string.IsNullOrWhiteSpace(data.SceneName) ? "-" : data.SceneName,
                SavedAtText = string.IsNullOrWhiteSpace(data.SavedAtUtc) ? "-" : data.SavedAtUtc,
                HasData = true
            };
        }
    }
}