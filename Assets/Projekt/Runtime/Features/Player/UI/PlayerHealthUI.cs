/*
 * Datei: PlayerHealthUI.cs
 * Zweck: Zeigt die aktuellen Lebenspunkte des Spielers an.
 * Verantwortung:
 *   - Aktualisiert Health-Bar und Text
 *
 * Abhängigkeiten:
 *   - UnityEngine.UI.Image
 *   - TMPro
 *
 * Verwendet von:
 *   - Player UI Canvas
 */
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI healthText;

    public void SetHealth(int current, int max)
    {
        float value = Mathf.Clamp01((float)current / max);

        if (fillImage != null)
            fillImage.fillAmount = value;

        if (healthText != null)
            healthText.text = $"{current} / {max}";
    }
}