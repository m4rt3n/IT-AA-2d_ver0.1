/*
 * Datei: PlayerLevelUI.cs
 * Zweck: Zeigt Level und Fortschritt des Spielers an.
 * Verantwortung:
 *   - Anzeige von Level
 *   - Anzeige von XP Fortschritt
 */
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace ITAA.Player.UI
{
    public class PlayerLevelUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Image xpBar;

        public void SetLevel(int level, float xpProgress)
        {
            if (levelText != null)
                levelText.text = $"Level {level}";

            if (xpBar != null)
                xpBar.fillAmount = Mathf.Clamp01(xpProgress);
        }
    }
}
