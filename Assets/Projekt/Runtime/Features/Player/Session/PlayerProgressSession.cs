/*
 * Datei: PlayerProgressSession.cs
 * Zweck: Speichert Fortschritt des Spielers.
 * Verantwortung:
 *   - Level
 *   - Erfahrungspunkte
 *
 * Verwendet von:
 *   - UI (Level Anzeige)
 *   - SaveGameSystem
 */
using UnityEngine;

namespace ITAA.Player.Session
{
    public class PlayerProgressSession : MonoBehaviour
    {
        public int Level = 1;
        public float CurrentXP = 0f;
        public float MaxXP = 100f;

        public void AddXP(float amount)
        {
            CurrentXP += amount;

            if (CurrentXP >= MaxXP)
            {
                LevelUp();
            }
        }

        private void LevelUp()
        {
            Level++;
            CurrentXP = 0f;
            MaxXP *= 1.2f;
        }
    }
}
