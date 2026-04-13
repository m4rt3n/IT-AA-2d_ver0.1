using System;

namespace ITAA.System.Savegame
{
    [Serializable]
    public class SaveGameData
    {
        public int SlotId;
        public string PlayerName;
        public string SceneName;
        public float PlayerPositionX;
        public float PlayerPositionY;
        public float PlayerPositionZ;
        public string SavedAtUtc;
        public int PlayerLevel;
        public int PlayTimeSeconds;
    }
}