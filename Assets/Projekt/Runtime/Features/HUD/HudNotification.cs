/*
 * Datei: HudNotification.cs
 * Zweck: Beschreibt eine kurze HUD-Meldung.
 * Verantwortung: Haelt Nachrichtentext, Anzeigedauer und optionale Kategorie fuer spaetere UI-Darstellung.
 * Abhaengigkeiten: System.Serializable.
 * Verwendung: Wird vom HudController an die HudView uebergeben.
 */

using System;

namespace ITAA.Features.HUD
{
    [Serializable]
    public class HudNotification
    {
        public string Message;
        public float DurationSeconds = 3f;
        public string Category;

        public HudNotification()
        {
        }

        public HudNotification(string message, float durationSeconds = 3f, string category = "")
        {
            Message = message;
            DurationSeconds = durationSeconds;
            Category = category;
        }

        public bool HasMessage()
        {
            return !string.IsNullOrWhiteSpace(Message);
        }
    }
}
