using System;

namespace ITAA.Data.Models
{
    [Serializable]
    public class SaveSlotData
    {
        public int SaveSlotId;
        public int UserId;
        public string Username;
        public string SaveSlotName;
        public int Level;
        public int Score;
        public int ProgressPercent;

        public SaveSlotData Clone()
        {
            return new SaveSlotData
            {
                SaveSlotId = SaveSlotId,
                UserId = UserId,
                Username = Username,
                SaveSlotName = SaveSlotName,
                Level = Level,
                Score = Score,
                ProgressPercent = ProgressPercent
            };
        }
    }
}