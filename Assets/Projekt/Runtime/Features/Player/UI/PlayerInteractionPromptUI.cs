/*
 * Datei: PlayerInteractionPromptUI.cs
 * Zweck: Zeigt Interaktionshinweise für den Spieler an.
 * Verantwortung:
 *   - Ein-/Ausblenden von Prompt
 *   - Anzeige von Text (z. B. "Drücke E")
 */
using UnityEngine;
using TMPro;

public class PlayerInteractionPromptUI : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private TextMeshProUGUI promptText;

    public void Show(string text)
    {
        if (root != null)
            root.SetActive(true);

        if (promptText != null)
            promptText.text = text;
    }

    public void Hide()
    {
        if (root != null)
            root.SetActive(false);
    }
}