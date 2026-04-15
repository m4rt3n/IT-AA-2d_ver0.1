/*
 * Datei: SaveSlotEntity.cs
 * Pfad: Assets/Projekt/Runtime/System/Savegame/SaveSlotEntity.cs
 * Zweck: Beschreibt einen Save-Slot in UI-freundlicher Form.
 * Verantwortung:
 * - Hält die Anzeige-Daten eines Speicherstands
 * - Wandelt SaveGameData in ein UI-Modell um
 * - Formatiert den Speicherzeitpunkt für die Darstellung
 */

using System;
using System.Globalization;

namespace ITAA.System.Savegame
{
    [Serializable]
    public class SaveSlotEntity
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
                DisplayName = string.IsNullOrWhiteSpace(data.PlayerName)
                    ? $"Slot {data.SlotId}"
                    : data.PlayerName,
                SceneName = string.IsNullOrWhiteSpace(data.SceneName)
                    ? "-"
                    : data.SceneName,
                SavedAtText = FormatSavedAt(data.SavedAtUtc),
                HasData = true
            };
        }

        private static string FormatSavedAt(string savedAtUtc)
        {
            if (string.IsNullOrWhiteSpace(savedAtUtc))
            {
                return "-";
            }

            if (DateTime.TryParse(
                savedAtUtc,
                CultureInfo.InvariantCulture,
                DateTimeStyles.RoundtripKind,
                out DateTime parsedDate))
            {
                return parsedDate.ToLocalTime().ToString("dd.MM.yyyy HH:mm");
            }

            if (DateTime.TryParse(savedAtUtc, out parsedDate))
            {
                return parsedDate.ToLocalTime().ToString("dd.MM.yyyy HH:mm");
            }

            return savedAtUtc;
        }
    }
}