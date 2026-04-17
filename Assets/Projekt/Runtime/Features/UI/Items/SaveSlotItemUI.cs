/*
 * Datei: SaveSlotItemUI.cs
 * Zweck: Stellt einen einzelnen Save-Slot in der UI dar.
 * Verantwortung:
 * - Zeigt Slot-Daten an
 * - Löst Auswahl per Button aus
 * - Unterstützt belegte und leere Slots
 */

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ITAA.System.Savegame;

namespace ITAA.UI.Items
{
    public class SaveSlotItemUI : MonoBehaviour
    {
        #region Inspector

        [Header("References")]
        [SerializeField] private Button selectButton;
        [SerializeField] private TMP_Text slotNameText;
        [SerializeField] private TMP_Text sceneNameText;
        [SerializeField] private TMP_Text savedAtText;
        [SerializeField] private TMP_Text statusText;

        [Header("Optional Visuals")]
        [SerializeField] private GameObject hasDataRoot;
        [SerializeField] private GameObject emptyRoot;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color emptyColor = new Color(0.85f, 0.85f, 0.85f, 1f);

        #endregion

        #region Private Fields

        private SaveSlotEntity currentSlot;
        private Action<SaveSlotEntity> onSelected;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (selectButton == null)
            {
                selectButton = GetComponent<Button>();
            }

            if (selectButton != null)
            {
                selectButton.onClick.RemoveListener(HandleClick);
                selectButton.onClick.AddListener(HandleClick);
            }
        }

        private void OnDestroy()
        {
            if (selectButton != null)
            {
                selectButton.onClick.RemoveListener(HandleClick);
            }
        }

        #endregion

        #region Public Methods

        public void Setup(SaveSlotEntity slot, Action<SaveSlotEntity> onSelectedCallback)
        {
            currentSlot = slot;
            onSelected = onSelectedCallback;

            if (slot == null)
            {
                ApplyNullState();
                return;
            }

            bool hasData = slot.HasData;

            if (slotNameText != null)
            {
                slotNameText.text = !string.IsNullOrWhiteSpace(slot.DisplayName)
                    ? slot.DisplayName
                    : $"Slot {slot.SlotId}";
            }

            if (sceneNameText != null)
            {
                sceneNameText.text = hasData
                    ? GetSafeText(slot.SceneName, "-")
                    : "Kein Spielstand";
            }

            if (savedAtText != null)
            {
                savedAtText.text = hasData
                    ? GetSafeText(slot.SavedAtText, "-")
                    : "-";
            }

            if (statusText != null)
            {
                statusText.text = hasData ? "Belegt" : "Leer";
            }

            if (hasDataRoot != null)
            {
                hasDataRoot.SetActive(hasData);
            }

            if (emptyRoot != null)
            {
                emptyRoot.SetActive(!hasData);
            }

            if (backgroundImage != null)
            {
                backgroundImage.color = hasData ? normalColor : emptyColor;
            }

            if (selectButton != null)
            {
                selectButton.interactable = true;
            }
        }

        public SaveSlotEntity GetSlot()
        {
            return currentSlot;
        }

        #endregion

        #region Private Methods

        private void HandleClick()
        {
            if (currentSlot == null)
            {
                Debug.LogWarning($"[{nameof(SaveSlotItemUI)}] Kein SaveSlotEntity gesetzt auf {gameObject.name}.", this);
                return;
            }

            onSelected?.Invoke(currentSlot);
        }

        private void ApplyNullState()
        {
            if (slotNameText != null)
            {
                slotNameText.text = "Unbekannter Slot";
            }

            if (sceneNameText != null)
            {
                sceneNameText.text = "-";
            }

            if (savedAtText != null)
            {
                savedAtText.text = "-";
            }

            if (statusText != null)
            {
                statusText.text = "Fehler";
            }

            if (hasDataRoot != null)
            {
                hasDataRoot.SetActive(false);
            }

            if (emptyRoot != null)
            {
                emptyRoot.SetActive(true);
            }

            if (backgroundImage != null)
            {
                backgroundImage.color = emptyColor;
            }

            if (selectButton != null)
            {
                selectButton.interactable = false;
            }
        }

        private static string GetSafeText(string value, string fallback)
        {
            return string.IsNullOrWhiteSpace(value) ? fallback : value;
        }

        #endregion
    }
}