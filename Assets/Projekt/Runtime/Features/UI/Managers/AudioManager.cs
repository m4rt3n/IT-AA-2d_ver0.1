/*
 * Datei: AudioManager.cs
 * Zweck: Verwaltet Musik und Soundeffekte.
 * Verantwortung:
 *   - Hintergrundmusik abspielen
 *   - Soundeffekte abspielen
 *   - Lautstärke steuern
 *
 * Abhängigkeiten:
 *   - AudioSource
 *
 * Verwendet von:
 *   - UI
 *   - Gameplay
 */
using UnityEngine;

namespace ITAA.UI.Managers
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void PlayMusic(AudioClip clip)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }

        public void PlaySFX(AudioClip clip)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}
