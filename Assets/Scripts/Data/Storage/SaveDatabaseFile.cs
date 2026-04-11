/*
 * Datei: SaveDatabaseFile.cs
 * Zweck: Kapselt die serialisierbare Gesamtdatenstruktur für alle Spielstände.
 * Verantwortung: Hält die Liste aller SaveSlots und den nächsten freien SaveSlot-Index.
 * Abhängigkeiten: SaveSlotData.
 * Verwendet von: DatabaseManager für das Speichern und Laden der Save-Datei.
 */
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