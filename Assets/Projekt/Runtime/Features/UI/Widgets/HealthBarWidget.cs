/*
 * Datei: HealthBarWidget.cs
 * Zweck: Zeigt den aktuellen Lebenswert als UI-Leiste an.
 * Verantwortung:
 *   - Aktualisiert die Füllmenge der Health-Bar
 *   - Optional Textanzeige
 *
 * Abhängigkeiten:
 *   - UnityEngine.UI.Image
 *
 * Verwendet von:
 *   - Player
 *   - UI Canvas
 */
using UnityEngine;
using UnityEngine.UI;

namespace ITAA.UI.Widgets
{
    public class HealthBarWidget : MonoBehaviour
    {
        [SerializeField] private Image fillImage;

        public void SetHealth(float current, float max)
        {
            if (fillImage == null) return;

            float value = Mathf.Clamp01(current / max);
            fillImage.fillAmount = value;
        }
    }
}
