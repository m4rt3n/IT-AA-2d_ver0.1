/*
 * Datei: SaveSlotListItemUI.cs
 * Zweck: Steuert die Darstellung eines einzelnen SaveSlot-Eintrags in der UI-Liste.
 * Verantwortung: Befüllt Texte, verdrahtet den Lade-Button und meldet den ausgewählten SaveSlot zurück.
 * Abhängigkeiten: SaveSlotData, TMP_Text, Button.
 * Verwendet von: Prefab für Einträge im LoadGamePanel.
 */

using System;
using ITAA.Data.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ITAA.UI.Widgets
{
    public class SaveSlotListItemUI : MonoBehaviour
    {
        #region Inspector

        [Header("Texts")]
        [SerializeField] private TMP_Text slotNameText;
        [SerializeField] private TMP_Text userText;
        [SerializeField] private TMP_Text progressText;
        [SerializeField] private TMP_Text statsText;

        [Header("Button")]
        [SerializeField] private Button selectButton;

        #endregion

        #region Private Fields

        private SaveSlotData saveSlot;
        private Action<SaveSlotData> onSelected;

        #endregion

        #region Public Methods

        public void Bind(SaveSlotData data, Action<SaveSlotData> callback)
        {
            saveSlot = data;
            onSelected = callback;

            if (slotNameText != null)
            {
                slotNameText.text = data.SaveSlotName;
            }

            if (userText != null)
            {
                userText.text = data.Username;
            }

            if (progressText != null)
            {
                progressText.text = $"Fortschritt: {data.ProgressPercent}%";
            }

            if (statsText != null)
            {
                statsText.text = $"Level {data.Level} | Score {data.Score}";
            }

            if (selectButton != null)
            {
                selectButton.onClick.RemoveAllListeners();
                selectButton.onClick.AddListener(Select);
            }
        }

        #endregion

        #region Private Methods

        private void Select()
        {
            onSelected?.Invoke(saveSlot);
        }

        #endregion
    }
}