/*
 * Datei: SettingsData.cs
 * Zweck: Speichert alle serialisierbaren Spieleinstellungen fuer Audio, Video, Input und Gameplay.
 * Verantwortung: Stellt ein reines Datenmodell mit Defaultwerten bereit, das vom SettingsManager als JSON gespeichert wird.
 * Abhaengigkeiten: System.Serializable, Unity JsonUtility-kompatible Felder.
 * Verwendung: Wird von SettingsManager geladen, gespeichert und an UI-/Runtime-Systeme weitergegeben.
 */

using System;

namespace ITAA.System.Settings
{
    [Serializable]
    public class SettingsData
    {
        public float MasterVolume = 1f;
        public float MusicVolume = 0.8f;
        public float SfxVolume = 0.8f;

        public bool Fullscreen = true;
        public int ResolutionWidth = 1920;
        public int ResolutionHeight = 1080;
        public bool VSync = true;

        public string InteractKey = "E";
        public string MoveUpKey = "W";
        public string MoveDownKey = "S";
        public string MoveLeftKey = "A";
        public string MoveRightKey = "D";

        public string TextSpeed = "normal";
        public bool ShowTutorials = true;

        public static SettingsData CreateDefault()
        {
            return new SettingsData();
        }

        public void Sanitize()
        {
            MasterVolume = Clamp01(MasterVolume);
            MusicVolume = Clamp01(MusicVolume);
            SfxVolume = Clamp01(SfxVolume);

            if (ResolutionWidth <= 0)
            {
                ResolutionWidth = 1920;
            }

            if (ResolutionHeight <= 0)
            {
                ResolutionHeight = 1080;
            }

            if (string.IsNullOrWhiteSpace(InteractKey))
            {
                InteractKey = "E";
            }

            if (string.IsNullOrWhiteSpace(MoveUpKey))
            {
                MoveUpKey = "W";
            }

            if (string.IsNullOrWhiteSpace(MoveDownKey))
            {
                MoveDownKey = "S";
            }

            if (string.IsNullOrWhiteSpace(MoveLeftKey))
            {
                MoveLeftKey = "A";
            }

            if (string.IsNullOrWhiteSpace(MoveRightKey))
            {
                MoveRightKey = "D";
            }

            if (string.IsNullOrWhiteSpace(TextSpeed))
            {
                TextSpeed = "normal";
            }
        }

        private static float Clamp01(float value)
        {
            if (value < 0f)
            {
                return 0f;
            }

            if (value > 1f)
            {
                return 1f;
            }

            return value;
        }
    }
}
