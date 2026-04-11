/*
 * Datei: PersistentSingleton.cs
 * Zweck: Stellt eine generische Basis für persistente Singleton-Komponenten bereit.
 * Verantwortung: Verhindert doppelte Instanzen und hält ein GameObject szenenübergreifend aktiv.
 * Abhängigkeiten: MonoBehaviour, DontDestroyOnLoad.
 * Verwendet von: Zentrale Manager wie AuthManager, DatabaseManager und PlayerSession.
 */
using UnityEngine;

namespace ITAA.Core.Runtime
{
    public abstract class PersistentSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
    }
}