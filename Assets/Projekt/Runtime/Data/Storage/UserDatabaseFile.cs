/*
 * Datei: UserDatabaseFile.cs
 * Zweck: Kapselt die serialisierbare Gesamtdatenstruktur für alle Benutzer.
 * Verantwortung: Hält die Liste aller Benutzer und den nächsten freien Benutzer-Index.
 * Abhängigkeiten: UserData.
 * Verwendet von: DatabaseManager für das Speichern und Laden der User-Datei.
 */
using System;
using System.Collections.Generic;
using ITAA.Data.Models;

namespace ITAA.Data.Storage
{
    [Serializable]
    public class UserDatabaseFile
    {
        public List<UserData> Users = new();
        public int NextUserId = 1;
    }
}
