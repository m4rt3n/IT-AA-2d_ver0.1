/*
 * Datei: SceneLoaderManager.cs
 * Zweck: Verwaltet das Laden von Szenen.
 * Verantwortung:
 *   - Szenenwechsel
 *   - Vorbereitung für Ladebildschirm
 *
 * Abhängigkeiten:
 *   - UnityEngine.SceneManagement
 *
 * Verwendet von:
 *   - GameManager
 *   - UI
 */
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ITAA.UI.Managers
{
    public class SceneLoaderManager : MonoBehaviour
    {
        public static SceneLoaderManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
