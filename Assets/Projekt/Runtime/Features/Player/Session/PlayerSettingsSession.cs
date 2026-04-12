/*
 * Datei: PlayerSettingsSession.cs
 * Zweck: Speichert individuelle Einstellungen des Spielers.
 * Verantwortung:
 *   - Lautstärke
 *   - Sensitivität
 *
 * Verwendet von:
 *   - SettingsMenu
 */
using UnityEngine;

public class PlayerSettingsSession : MonoBehaviour
{
    public float MusicVolume = 1f;
    public float SFXVolume = 1f;
    public float MouseSensitivity = 1f;

    public void ApplySettings()
    {
        AudioListener.volume = MusicVolume;
    }
}