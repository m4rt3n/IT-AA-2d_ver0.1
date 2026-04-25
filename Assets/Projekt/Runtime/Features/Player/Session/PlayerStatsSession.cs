/*
 * Datei: PlayerStatsSession.cs
 * Zweck: Speichert aktuelle Spielerwerte wie Health und Stamina.
 * Verantwortung:
 *   - Verwaltung von Lebenspunkten
 *   - Verwaltung von Ausdauer
 *
 * Verwendet von:
 *   - Player
 *   - PlayerHealthUI
 */
using UnityEngine;

namespace ITAA.Player.Session
{
    public class PlayerStatsSession : MonoBehaviour
    {
        public int MaxHealth = 100;
        public int CurrentHealth = 100;

        public float MaxStamina = 100f;
        public float CurrentStamina = 100f;

        public void TakeDamage(int amount)
        {
            CurrentHealth = Mathf.Max(CurrentHealth - amount, 0);
        }

        public void Heal(int amount)
        {
            CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);
        }
    }
}
