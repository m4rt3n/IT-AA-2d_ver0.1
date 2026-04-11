/*
 * Datei: SaveSlotListItemUI.cs
 * Zweck: Stellt einen einzelnen Save-Slot als Listeneintrag im LoadGamePanel dar.
 * Verantwortung:
 *   - Anzeige von Slot-Daten
 *   - Reaktion auf Load-Button
 *   - Verbindung UI ↔ Slot-Daten
 *
 * Abhängigkeiten:
 *   - TMPro.TextMeshProUGUI
 *   - UnityEngine.UI.Button
 *
 * Verwendet von:
 *   - LoadGamePanel
 *   - SaveSlotItem Prefab
 */
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotListItemUI : MonoBehaviour
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

    public void Setup(int index, string level, int score, int progressPercent)
    {
        slotIndex = index;

        if (labelText != null)
        {
            labelText.text = $"Slot {index} | {level} | Score {score} | {progressPercent}%";
        }
        else
        {
            Debug.LogWarning($"[{nameof(SaveSlotListItemUI)}] LabelText fehlt auf {name}");
        }

        if (loadButton != null)
        {
            loadButton.onClick.RemoveAllListeners();
            loadButton.onClick.AddListener(OnLoadClicked);
        }
        else
        {
            Debug.LogWarning($"[{nameof(SaveSlotListItemUI)}] LoadButton fehlt auf {name}");
        }
    }

    #endregion

    #region Private Methods

    private void OnLoadClicked()
    {
        Debug.Log($"Load Slot: {slotIndex}");

        // TODO: echtes Laden
    }

    #endregion
}