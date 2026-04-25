/*
 * Datei: UserDatabase.cs
 * Zweck: Beschreibt eine serialisierbare Sammlung lokaler Benutzer-Datensaetze.
 * Verantwortung: Haelt die Liste bekannter UserEntity-Eintraege.
 * Abhaengigkeiten: UserEntity, System.Collections.Generic.
 * Verwendung: Dient als einfacher Container fuer lokale Benutzerdateien oder Migrationsdaten.
 */

using System.Collections.Generic;

namespace ITAA.Data
{
    [System.Serializable]
    public class UserDatabase
    {
        public List<UserEntity> users = new List<UserEntity>();
    }
}
