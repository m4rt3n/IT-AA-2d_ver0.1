/*
 * Datei: SaveSlotInfo.cs
 * Zweck: Beschreibt kompakte Metadaten eines SaveSlots.
 * Verantwortung: Haelt Anzeige- und Fortschrittsdaten fuer SaveSlot-Uebersichten.
 * Abhaengigkeiten: System.Serializable.
 * Verwendung: Kann von Save-/Load-UI oder Datenhaltung fuer Slot-Listen genutzt werden.
 */

using System;

namespace ITAA.System.Savegame
{
    [Serializable]
    public class SaveSlotInfo
    {
        #region Fields

        public int SaveSlotId;
        public int UserId;

        public string Username;
        public string SaveSlotName;

        public int Level;
        public int Score;
        public int ProgressPercent;

        public string LastPlayedUtc;

        #endregion
    }
}
