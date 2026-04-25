/*
 * Datei: UserEntity.cs
 * Zweck: Beschreibt einen serialisierbaren Benutzer-Datensatz fuer lokale Datenhaltung.
 * Verantwortung: Haelt User-ID, Benutzernamen, Passwortdaten und zuletzt verwendeten SaveSlot.
 * Abhaengigkeiten: System.Serializable.
 * Verwendung: Wird von UserDatabase als einzelner Benutzer-Eintrag gespeichert.
 */

using System;

namespace ITAA.Data
{
    [Serializable]
    public class UserEntity
    {
        public int Id;
        public string Username;
        public string PasswordHash;
        public string PasswordSalt;
        public int LastUsedSaveSlotId;
    }
}
