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
// Datei: Assets/Projekt/Runtime/Features/UI/Items/SaveSlotListItemUI.cs

using ITAA.System.Savegame;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ITAA.UI.Items
{
    public class SaveSlotListItemUI : MonoBehaviour
    {
        #region Inspector

        [Header("Texts")]
        [SerializeField] private TMP_Text slotTitleText;
        [SerializeField] private TMP_Text sceneNameText;
        [SerializeField] private TMP_Text savedAtText;

        [Header("Buttons")]
        [SerializeField] private Button selectButton;

        #endregion

        #region Private Fields

        private SaveSlotEntity slotData;
        private UnityAction<SaveSlotEntity> onSelected;

        #endregion

        #region Public Methods
public class SaveSlotEntity
{
    public int slotId;
    public string displayName;
    public string sceneName;
    public string saveTime;
}
        public void Setup(SaveSlotEntity data, UnityAction<SaveSlotEntity> onSelectedCallback)
        {
            slotData = data;
            onSelected = onSelectedCallback;

            if (slotTitleText != null)
            {
                slotTitleText.text = data != null ? data.displayName : "Unbekannter Slot";
            }

            if (sceneNameText != null)
            {
                sceneNameText.text = data != null ? $"Szene: {data.sceneName}" : "Szene: -";
            }

            if (savedAtText != null)
            {
                savedAtText.text = data != null ? $"Gespeichert: {data.savedAtText}" : "Gespeichert: -";
            }

            if (selectButton != null)
            {
                selectButton.onClick.RemoveAllListeners();
                selectButton.onClick.AddListener(HandleClicked);
            }
        }

        #endregion

        #region Private Methods

        private void HandleClicked()
        {
            onSelected?.Invoke(slotData);
        }

        #endregion
    }
}