/*
 * Datei: AchievementDefinition.cs
 * Zweck: Beschreibt ein einzelnes Achievement als leichtgewichtiges Datenmodell.
 * Verantwortung: Haelt ID, Titel, Beschreibung, Kategorie, Punkte und Sichtbarkeitsregeln.
 * Abhaengigkeiten: System.Serializable, UnityEngine.
 * Verwendung: Wird vom AchievementManager registriert und fuer Unlock-/Query-Logik genutzt.
 */

using System;
using UnityEngine;

namespace ITAA.Features.Achievements
{
    [Serializable]
    public class AchievementDefinition
    {
        public string AchievementId = "achievement_id";
        public string Title = "Achievement";
        [TextArea] public string Description;
        public string Category = "General";
        [Min(0)] public int Points = 10;
        public bool IsHiddenUntilUnlocked;

        public bool HasValidId()
        {
            return !string.IsNullOrWhiteSpace(AchievementId);
        }
    }
}
