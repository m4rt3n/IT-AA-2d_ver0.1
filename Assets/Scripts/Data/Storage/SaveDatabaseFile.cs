using System;
using System.Collections.Generic;
using ITAA.Data.Models;

namespace ITAA.Data.Storage
{
    [Serializable]
    public class SaveDatabaseFile
    {
        public List<SaveSlotData> SaveSlots = new();
        public int NextSaveSlotId = 1;
    }
}