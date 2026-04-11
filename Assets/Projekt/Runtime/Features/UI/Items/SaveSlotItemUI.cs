/*
 * Datei: SaveSlotItemUI.cs
 * Zweck: Stellt einen einzelnen Save-Slot im Load-Game-Menü dar.
 * Verantwortung:
 *   - Zeigt Slot-Informationen im UI an
 *   - Reagiert auf Klick auf den Lade-Button
 *   - Verbindet Slot-Daten mit dem visuellen Prefab
 *
 * Abhängigkeiten:
 *   - TMPro.TextMeshProUGUI
 *   - UnityEngine.UI.Button
 *
 * Verwendet von:
 *   - LoadGamePanel
 *   - SaveSlotItem-Prefab im ScrollView-Content
 */
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotItemUI : MonoBehaviour
{
    #region Inspector

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI labelText;
    [SerializeField] private Button loadButton;

    #endregion

    #region Private Fields

    private int slotIndex;

    #endregion

    #region Public Methods

    /// <summary>
    /// Initialisiert den Save-Slot mit anzuzeigenden Daten.
    /// </summary>
    /// <param name="index">Index des Slots.</param>
    /// <param name="level">Aktuelles Level oder Szenenname.</param>
    /// <param name="score">Aktueller Score.</param>
    /// <param name="progressPercent">Spielfortschritt in Prozent.</param>
    public void Setup(int index, string level, int score, int progressPercent)
    {
        slotIndex = index;

        if (labelText != null)
        {
            labelText.text = $"Slot {index}\n{level}\nScore {score}\n{progressPercent}%";
        }
        else
        {
            Debug.LogWarning($"[{nameof(SaveSlotItemUI)}] LabelText ist nicht zugewiesen auf {gameObject.name}.");
        }

        if (loadButton != null)
        {
            loadButton.onClick.RemoveAllListeners();
            loadButton.onClick.AddListener(OnLoadClicked);
        }
        else
        {
            Debug.LogWarning($"[{nameof(SaveSlotItemUI)}] LoadButton ist nicht zugewiesen auf {gameObject.name}.");
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Wird beim Klick auf den Lade-Button ausgeführt.
    /// </summary>
    private void OnLoadClicked()
    {
        Debug.Log($"Load angeklickt: Slot {slotIndex}");

        // TODO:
        // Hier später echtes Laden über SaveSystem / DatabaseManager anschließen.
    }

    #endregion
}