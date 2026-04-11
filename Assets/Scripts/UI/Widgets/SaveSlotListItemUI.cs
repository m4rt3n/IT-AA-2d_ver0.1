using System;
using ITAA.Data.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ITAA.UI
{
    public class SaveSlotListItemUI : MonoBehaviour
    {
        [Header("Texts")]
        [SerializeField] private TMP_Text slotNameText;
        [SerializeField] private TMP_Text userText;
        [SerializeField] private TMP_Text progressText;
        [SerializeField] private TMP_Text statsText;

        [Header("Button")]
        [SerializeField] private Button selectButton;

        private SaveSlotData saveSlot;
        private Action<SaveSlotData> onSelected;

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

        private void Select()
        {
            onSelected?.Invoke(saveSlot);
        }
    }
}