/*
 * Datei: SaveGameManager.cs
 * Zweck: Verwaltet das Speichern und Laden von Spielständen.
 * Verantwortung:
 *   - Speichern von Daten
 *   - Laden von Daten
 *   - Verwaltung von SaveSlots
 *
 * Abhängigkeiten:
 *   - PlayerPrefs / Datei-System (später erweiterbar)
 *
 * Verwendet von:
 *   - LoadGamePanel
 *   - Gameplay
 */
using UnityEngine;

namespace ITAA.UI.Managers
{
    public class SaveGameManager : MonoBehaviour
    {
        public static SaveGameManager Instance { get; private set; }

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

        public void Save(int slotIndex)
        {
            Debug.Log($"Speichern in Slot {slotIndex}");

            PlayerPrefs.SetInt($"Slot_{slotIndex}_Score", Random.Range(0, 1000));
            PlayerPrefs.Save();
        }

        public void Load(int slotIndex)
        {
            Debug.Log($"Lade Slot {slotIndex}");

            int score = PlayerPrefs.GetInt($"Slot_{slotIndex}_Score", 0);
            Debug.Log($"Geladener Score: {score}");
        }
    }
}
