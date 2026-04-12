/*
 * Datei: ProgressBarWidget.cs
 * Zweck: Zeigt einen allgemeinen Fortschritt als UI-Leiste an.
 * Verantwortung:
 *   - Setzt Fortschritt visuell
 *
 * Abhängigkeiten:
 *   - UnityEngine.UI.Image
 */
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarWidget : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    public void SetProgress(float value)
    {
        if (fillImage == null) return;

        fillImage.fillAmount = Mathf.Clamp01(value);
    }
}