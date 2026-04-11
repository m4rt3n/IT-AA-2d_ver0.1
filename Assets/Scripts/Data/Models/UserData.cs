/*
 * Datei: UserData.cs
 * Zweck: Reines Datenmodell für einen Benutzer.
 * Verantwortung: Speichert benutzerbezogene Basisdaten wie ID, Name und Passwort.
 * Abhängigkeiten: Keine Unity-Komponenten erforderlich.
 * Verwendet von: DatabaseManager und spätere Authentifizierungslogik.
 */
 using System;

namespace ITAA.Data.Models
{
    [Serializable]
    public class UserData
    {
        public int UserId;
        public string Username;
        public string Password;

        public UserData Clone()
        {
            return new UserData
            {
                UserId = UserId,
                Username = Username,
                Password = Password
            };
        }
    }
}