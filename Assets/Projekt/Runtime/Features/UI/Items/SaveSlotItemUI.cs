/*
 * Datei: SaveSlotListItemUI.cs
 * Pfad: Assets/Projekt/Runtime/Features/UI/Items/SaveSlotListItemUI.cs
 * Zweck: Steuert die Darstellung eines einzelnen Save-Slot-Eintrags in der UI-Liste.
 * Verantwortung:
 * - Übernimmt ein SaveSlotEntity und zeigt dessen Daten an
 * - Aktiviert oder deaktiviert den Button je nach Slot-Zustand
 * - Meldet Klicks an den aufrufenden Panel-Controller zurück
 */

using System;
using ITAA.System.Savegame;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ITAA.UI.Items
{
    public class SaveSlotListItemUI : MonoBehaviour
    {
        [Header("Texts")]
        [SerializeField] private TMP_Text displayNameText;
        [SerializeField] private TMP_Text sceneNameText;
        [SerializeField] private TMP_Text savedAtText;

        [Header("Interaction")]
        [SerializeField] private Button selectButton;

        private SaveSlotEntity currentSlot;
        private Action<SaveSlotEntity> onSelected;

        public void Setup(SaveSlotEntity slot, Action<SaveSlotEntity> onSelectedCallback)
        {
            currentSlot = slot;
            onSelected = onSelectedCallback;

            RefreshView();
            BindButton();
        }

        private void RefreshView()
        {
            if (currentSlot == null)
            {
                SetText(displayNameText, "Unbekannter Slot");
                SetText(sceneNameText, "Szene: -");
                SetText(savedAtText, "Gespeichert: -");

                if (selectButton != null)
                {
                    selectButton.interactable = false;
                }

                return;
            }

            SetText(displayNameText, currentSlot.DisplayName);
            SetText(sceneNameText, $"Szene: {currentSlot.SceneName}");
            SetText(savedAtText, $"Gespeichert: {currentSlot.SavedAtText}");

            if (selectButton != null)
            {
                selectButton.interactable = currentSlot.HasData;
            }
        }

        private void BindButton()
        {
            if (selectButton == null)
            {
                return;
            }

            selectButton.onClick.RemoveAllListeners();
            selectButton.onClick.AddListener(HandleSelected);
        }

        private void HandleSelected()
        {
            if (currentSlot == null || !currentSlot.HasData)
            {
                return;
            }

            onSelected?.Invoke(currentSlot);
        }

        private void SetText(TMP_Text target, string value)
        {
            if (target != null)
            {
                target.text = value;
            }
        }
    }
}