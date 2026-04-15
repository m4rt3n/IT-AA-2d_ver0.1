/*
 * Datei: SaveGameData.cs
 * Pfad: Assets/Projekt/Runtime/System/Savegame/SaveGameData.cs
 * Zweck: Enthält die rohen Speicherdaten eines Spielstands.
 * Verantwortung:
 * - Speichert Slot-ID, Spielername, Szene und Zeitstempel
 * - Dient als Serialisierungsmodell für Save/Load
 */

using System;

namespace ITAA.System.Savegame
{
    [Serializable]
    public class SaveGameData
    {
        public int SlotId;
        public string PlayerName;
        public string SceneName;
        public string SavedAtUtc;
    }
}