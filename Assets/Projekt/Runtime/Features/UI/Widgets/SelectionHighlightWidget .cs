/*
 * Datei: SelectionHighlightWidget.cs
 * Zweck: Hebt ein UI-Element visuell hervor.
 * Verantwortung:
 *   - Aktiviert / deaktiviert Highlight
 *
 * Abhängigkeiten:
 *   - UnityEngine.UI.Image
 */
using UnityEngine;
using UnityEngine.UI;

public class SelectionHighlightWidget : MonoBehaviour
{
    [SerializeField] private Image highlightImage;

    public void SetSelected(bool selected)
    {
        if (highlightImage != null)
        {
            highlightImage.enabled = selected;
        }
    }
}