/*
 * Datei: SettingsManager.cs
 * Zweck: Verwaltet globale Spieleinstellungen und deren Persistenz als JSON.
 * Verantwortung: Laedt, speichert, setzt Defaultwerte und wendet Audio-/Video-Basiseinstellungen zur Laufzeit an.
 * Abhaengigkeiten: PersistentSingleton, SettingsData, Application.persistentDataPath, JsonUtility, Screen, QualitySettings.
 * Verwendung: Wird als zentraler Zugriffspunkt fuer UI, DevTools und spaetere Runtime-Systeme genutzt.
 */

using System;
using System.IO;
using ITAA.Core.Runtime;
using UnityEngine;

namespace ITAA.System.Settings
{
    public class SettingsManager : PersistentSingleton<SettingsManager>
    {
        private const string SettingsFileName = "settings.json";

        [Header("Startup")]
        [SerializeField] private bool loadOnAwake = true;
        [SerializeField] private bool applyOnAwake = true;

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs;

        public event Action<SettingsData> SettingsChanged;

        public SettingsData CurrentSettings { get; private set; } = SettingsData.CreateDefault();

        public static string SettingsFilePath =>
            Path.Combine(Application.persistentDataPath, SettingsFileName);

        protected override void Awake()
        {
            base.Awake();

            if (Instance != this)
            {
                return;
            }

            if (loadOnAwake)
            {
                LoadSettings();
            }

            if (applyOnAwake)
            {
                ApplySettings();
            }
        }

        public static SettingsManager GetOrCreate()
        {
            if (Instance != null)
            {
                return Instance;
            }

            SettingsManager existing = FindAnyObjectByType<SettingsManager>(FindObjectsInactive.Include);
            if (existing != null)
            {
                return existing;
            }

            GameObject managerObject = new GameObject(nameof(SettingsManager));
            return managerObject.AddComponent<SettingsManager>();
        }

        public SettingsData GetSettings()
        {
            EnsureSettings();
            return CurrentSettings;
        }

        public float GetMasterVolume()
        {
            EnsureSettings();
            return CurrentSettings.MasterVolume;
        }

        public float GetMusicVolume()
        {
            EnsureSettings();
            return CurrentSettings.MusicVolume;
        }

        public float GetSfxVolume()
        {
            EnsureSettings();
            return CurrentSettings.SfxVolume;
        }

        public void SetMasterVolume(float value, bool saveImmediately = true)
        {
            EnsureSettings();
            CurrentSettings.MasterVolume = Mathf.Clamp01(value);
            ApplyAudioSettings();
            NotifyChanged();
            SaveIfRequested(saveImmediately);
        }

        public void SetMusicVolume(float value, bool saveImmediately = true)
        {
            EnsureSettings();
            CurrentSettings.MusicVolume = Mathf.Clamp01(value);
            NotifyChanged();
            SaveIfRequested(saveImmediately);
        }

        public void SetSfxVolume(float value, bool saveImmediately = true)
        {
            EnsureSettings();
            CurrentSettings.SfxVolume = Mathf.Clamp01(value);
            NotifyChanged();
            SaveIfRequested(saveImmediately);
        }

        public void SetVideoSettings(bool fullscreen, int width, int height, bool vSync, bool saveImmediately = true)
        {
            EnsureSettings();
            CurrentSettings.Fullscreen = fullscreen;
            CurrentSettings.ResolutionWidth = Mathf.Max(1, width);
            CurrentSettings.ResolutionHeight = Mathf.Max(1, height);
            CurrentSettings.VSync = vSync;

            ApplyVideoSettings();
            NotifyChanged();
            SaveIfRequested(saveImmediately);
        }

        public void SetInputKeys(
            string interactKey,
            string moveUpKey,
            string moveDownKey,
            string moveLeftKey,
            string moveRightKey,
            bool saveImmediately = true)
        {
            EnsureSettings();
            CurrentSettings.InteractKey = ResolveText(interactKey, "E");
            CurrentSettings.MoveUpKey = ResolveText(moveUpKey, "W");
            CurrentSettings.MoveDownKey = ResolveText(moveDownKey, "S");
            CurrentSettings.MoveLeftKey = ResolveText(moveLeftKey, "A");
            CurrentSettings.MoveRightKey = ResolveText(moveRightKey, "D");

            NotifyChanged();
            SaveIfRequested(saveImmediately);
        }

        public void SetGameplaySettings(string textSpeed, bool showTutorials, bool saveImmediately = true)
        {
            EnsureSettings();
            CurrentSettings.TextSpeed = ResolveText(textSpeed, "normal");
            CurrentSettings.ShowTutorials = showTutorials;

            NotifyChanged();
            SaveIfRequested(saveImmediately);
        }

        public void LoadSettings()
        {
            try
            {
                if (!File.Exists(SettingsFilePath))
                {
                    CurrentSettings = SettingsData.CreateDefault();
                    SaveSettings();
                    return;
                }

                string json = File.ReadAllText(SettingsFilePath);
                CurrentSettings = string.IsNullOrWhiteSpace(json)
                    ? SettingsData.CreateDefault()
                    : JsonUtility.FromJson<SettingsData>(json);

                EnsureSettings();
                CurrentSettings.Sanitize();
                SaveSettings();
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"[{nameof(SettingsManager)}] Settings konnten nicht geladen werden: {exception.Message}", this);
                CurrentSettings = SettingsData.CreateDefault();
            }

            NotifyChanged();
        }

        public void SaveSettings()
        {
            EnsureSettings();
            CurrentSettings.Sanitize();

            try
            {
                Directory.CreateDirectory(Application.persistentDataPath);
                string json = JsonUtility.ToJson(CurrentSettings, true);
                File.WriteAllText(SettingsFilePath, json);

                if (enableDebugLogs)
                {
                    Debug.Log($"[{nameof(SettingsManager)}] Settings gespeichert: {SettingsFilePath}", this);
                }
            }
            catch (Exception exception)
            {
                Debug.LogError($"[{nameof(SettingsManager)}] Settings konnten nicht gespeichert werden: {exception.Message}", this);
            }
        }

        public void ResetToDefaults(bool saveImmediately = true)
        {
            CurrentSettings = SettingsData.CreateDefault();
            ApplySettings();
            NotifyChanged();
            SaveIfRequested(saveImmediately);
        }

        public void ApplySettings()
        {
            EnsureSettings();
            CurrentSettings.Sanitize();
            ApplyAudioSettings();
            ApplyVideoSettings();
        }

        private void ApplyAudioSettings()
        {
            AudioListener.volume = CurrentSettings != null
                ? Mathf.Clamp01(CurrentSettings.MasterVolume)
                : 1f;
        }

        private void ApplyVideoSettings()
        {
            if (CurrentSettings == null)
            {
                return;
            }

            QualitySettings.vSyncCount = CurrentSettings.VSync ? 1 : 0;

            if (CurrentSettings.ResolutionWidth > 0 && CurrentSettings.ResolutionHeight > 0)
            {
                Screen.SetResolution(
                    CurrentSettings.ResolutionWidth,
                    CurrentSettings.ResolutionHeight,
                    CurrentSettings.Fullscreen);
            }
        }

        private void EnsureSettings()
        {
            if (CurrentSettings == null)
            {
                CurrentSettings = SettingsData.CreateDefault();
            }
        }

        private void NotifyChanged()
        {
            SettingsChanged?.Invoke(CurrentSettings);
        }

        private void SaveIfRequested(bool saveImmediately)
        {
            if (saveImmediately)
            {
                SaveSettings();
            }
        }

        private static string ResolveText(string value, string fallback)
        {
            return string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();
        }
    }
}
