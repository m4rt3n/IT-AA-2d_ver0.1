/*
 * Datei: SaveSlotItemUI.cs
 * Zweck: Stellt einen einzelnen Save-Slot in der UI dar und bereitet dessen Daten für kleine sowie große Slot-Ansichten auf.
 * Verantwortung:
 * - Zeigt Save-Slot-Daten strukturiert und lesbar an
 * - Setzt die vier Inspector-Textfelder fuer Slotname, Szene, Zeitstempel und Status
 * - Löst Auswahl/Laden per Button oder Klick auf die gesamte Slot-Karte aus
 * - Hebt den aktuell sichtbaren großen Slot visuell als aktive Auswahl hervor
 * - Unterscheidet sauber zwischen belegten und leeren Slots
 * Abhängigkeiten:
 * - ITAA.System.Savegame.SaveSlotEntity
 * - TMPro
 * - Unity UI / EventSystem
 * Verwendung:
 * - Wird vom LoadGamePanel für den großen aktuell ausgewählten Slot verwendet
 * - Kann auch für kleinere Listen-/Kartenansichten wiederverwendet werden
 */

using System;
using ITAA.System.Savegame;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ITAA.UI.Items
{
    public class SaveSlotItemUI : MonoBehaviour, IPointerClickHandler
    {
        #region Inspector

        [Header("References")]
        [SerializeField] private Button selectButton;
        [SerializeField] private TMP_Text slotNameText;
        [SerializeField] private TMP_Text sceneNameText;
        [SerializeField] private TMP_Text savedAtText;
        [SerializeField] private TMP_Text statusText;

        [Header("Optional Extra Text")]
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text actionButtonLabelText;

        [Header("Optional Visuals")]
        [SerializeField] private GameObject hasDataRoot;
        [SerializeField] private GameObject emptyRoot;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Outline selectionOutline;
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color emptyColor = new Color(0.85f, 0.85f, 0.85f, 1f);
        [SerializeField] private Color selectedColor = new Color(0.96f, 0.97f, 1f, 1f);
        [SerializeField] private Color selectedOutlineColor = new Color(0.96f, 0.72f, 0.2f, 1f);

        [Header("Text Colors")]
        [SerializeField] private Color titleColor = new Color(0.11f, 0.15f, 0.24f, 1f);
        [SerializeField] private Color infoColor = new Color(0.19f, 0.24f, 0.32f, 1f);
        [SerializeField] private Color accentColor = new Color(0.1f, 0.56f, 0.27f, 1f);
        [SerializeField] private Color emptyInfoColor = new Color(0.43f, 0.47f, 0.53f, 1f);

        [Header("Debug")]
        [SerializeField] private bool autoResolveReferences = true;
        [SerializeField] private bool enableDebugLogs = false;

        #endregion

        #region Private Fields

        private SaveSlotEntity currentSlot;
        private Action<SaveSlotEntity> onSelected;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            ResolveReferences();

            if (selectButton != null)
            {
                selectButton.onClick.RemoveListener(HandleClick);
                selectButton.onClick.AddListener(HandleClick);
            }
            else if (enableDebugLogs)
            {
                Debug.LogWarning($"[{nameof(SaveSlotItemUI)}] Kein Button gefunden auf '{gameObject.name}'.", this);
            }
        }

        private void OnDestroy()
        {
            if (selectButton != null)
            {
                selectButton.onClick.RemoveListener(HandleClick);
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (autoResolveReferences)
            {
                ResolveReferences();
            }
        }
#endif

        #endregion

        #region Public Methods

        public void Setup(SaveSlotEntity slot, Action<SaveSlotEntity> onSelectedCallback)
        {
            ResolveReferences();

            currentSlot = slot;
            onSelected = onSelectedCallback;

            if (slot == null)
            {
                ApplyNullState();
                return;
            }

            bool hasData = slot.HasData;

            string displayName = GetSafeText(slot.DisplayName, $"Slot {slot.SlotId}");
            string playerName = GetSafeText(slot.PlayerName, displayName);
            string sceneName = GetSafeText(slot.SceneName, "-");
            string savedAt = GetSafeText(slot.SavedAtText, hasData ? "-" : "Leer");
            string status = hasData ? "Belegt" : "Leer";

            SetText(slotNameText, displayName);
            SetText(titleText, playerName);
            SetText(sceneNameText, sceneName);
            SetText(savedAtText, savedAt);
            SetText(statusText, status);

            ApplyTextColors(hasData);

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
                backgroundImage.color = hasData ? selectedColor : emptyColor;
            }

            EnsureSelectionOutline();

            if (selectionOutline != null)
            {
                selectionOutline.enabled = true;
                selectionOutline.effectColor = hasData ? selectedOutlineColor : emptyInfoColor;
                selectionOutline.effectDistance = hasData ? new Vector2(3f, -3f) : new Vector2(2f, -2f);
            }

            SetText(actionButtonLabelText, hasData ? "Laden" : "Leer");

            if (actionButtonLabelText != null)
            {
                actionButtonLabelText.color = hasData ? titleColor : emptyInfoColor;
            }

            if (selectButton != null)
            {
                selectButton.interactable = hasData;
            }

            if (enableDebugLogs)
            {
                Debug.Log(
                    $"[{nameof(SaveSlotItemUI)}] Setup Slot {slot.SlotId} | HasData={slot.HasData}",
                    this);
            }
        }

        public SaveSlotEntity GetSlot()
        {
            return currentSlot;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData == null)
            {
                return;
            }

            HandleClick();
        }

        #endregion

        #region Private Methods

        private void HandleClick()
        {
            if (currentSlot == null)
            {
                if (enableDebugLogs)
                {
                    Debug.Log($"[{nameof(SaveSlotItemUI)}] Klick ohne SaveSlotEntity auf '{gameObject.name}' ignoriert.", this);
                }

                return;
            }

            if (!currentSlot.HasData)
            {
                if (enableDebugLogs)
                {
                    Debug.Log($"[{nameof(SaveSlotItemUI)}] Slot {currentSlot.SlotId} ist leer und wird nicht geladen.", this);
                }

                return;
            }

            if (enableDebugLogs)
            {
                Debug.Log($"[{nameof(SaveSlotItemUI)}] Slot gewählt: {currentSlot.SlotId}", this);
            }

            onSelected?.Invoke(currentSlot);
        }

        private void ApplyNullState()
        {
            ResolveReferences();

            SetText(slotNameText, "Leerer Spielstand");
            SetText(titleText, "Spieler");
            SetText(sceneNameText, "-");
            SetText(savedAtText, "-");
            SetText(statusText, "Nicht belegt");

            ApplyTextColors(false);

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

            EnsureSelectionOutline();

            if (selectionOutline != null)
            {
                selectionOutline.enabled = true;
                selectionOutline.effectColor = emptyInfoColor;
                selectionOutline.effectDistance = new Vector2(2f, -2f);
            }

            if (selectButton != null)
            {
                selectButton.interactable = false;
            }

            SetText(actionButtonLabelText, "Leer");

            if (actionButtonLabelText != null)
            {
                actionButtonLabelText.color = emptyInfoColor;
            }
        }

        private void ResolveReferences()
        {
            if (!autoResolveReferences)
            {
                return;
            }

            if (selectButton == null)
            {
                selectButton = GetComponent<Button>();

                if (selectButton == null)
                {
                    selectButton = GetComponentInChildren<Button>(true);
                }
            }

            if (backgroundImage == null)
            {
                backgroundImage = GetComponent<Image>();
            }

            if (selectionOutline == null)
            {
                selectionOutline = GetComponent<Outline>();
            }

            TMP_Text[] texts = GetComponentsInChildren<TMP_Text>(true);

            if (slotNameText == null)
            {
                slotNameText = FindTextByName(texts, "SlotNameText", "LabelText", "SlotTitleText");
            }

            if (titleText == null)
            {
                titleText = FindTextByName(texts, "TitleText", "PlayerNameText", "NameText");
            }

            if (sceneNameText == null)
            {
                sceneNameText = FindTextByName(texts, "SceneNameText", "SceneText");
            }

            if (savedAtText == null)
            {
                savedAtText = FindTextByName(texts, "SavedAtText", "DateText", "TimeText");
            }

            if (statusText == null)
            {
                statusText = FindTextByName(texts, "StatusText", "InfoText");
            }

            if (actionButtonLabelText == null && selectButton != null)
            {
                actionButtonLabelText = selectButton.GetComponentInChildren<TMP_Text>(true);
            }

            if (enableDebugLogs)
            {
                Debug.Log(
                    $"[{nameof(SaveSlotItemUI)}] ResolveReferences | " +
                    $"Button={(selectButton != null)} | " +
                    $"SlotName={(slotNameText != null)} | " +
                    $"Title={(titleText != null)} | " +
                    $"SceneName={(sceneNameText != null)} | " +
                    $"SavedAt={(savedAtText != null)} | " +
                    $"Status={(statusText != null)} | " +
                    $"ActionLabel={(actionButtonLabelText != null)} | " +
                    $"Outline={(selectionOutline != null)}",
                    this
                );
            }
        }

        private static TMP_Text FindTextByName(TMP_Text[] texts, params string[] candidateNames)
        {
            if (texts == null || texts.Length == 0)
            {
                return null;
            }

            for (int i = 0; i < candidateNames.Length; i++)
            {
                string candidate = candidateNames[i];

                for (int j = 0; j < texts.Length; j++)
                {
                    TMP_Text text = texts[j];

                    if (text != null && text.name.Equals(candidate, StringComparison.OrdinalIgnoreCase))
                    {
                        return text;
                    }
                }
            }

            return null;
        }

        private static void SetText(TMP_Text target, string value)
        {
            if (target != null)
            {
                target.text = value;
            }
        }

        private static string GetSafeText(string value, string fallback)
        {
            return string.IsNullOrWhiteSpace(value) ? fallback : value;
        }

        private void ApplyTextColors(bool hasData)
        {
            Color resolvedTitleColor = hasData ? titleColor : emptyInfoColor;
            Color resolvedInfoColor = hasData ? infoColor : emptyInfoColor;

            SetTextColor(titleText, resolvedTitleColor);
            SetTextColor(slotNameText, resolvedTitleColor);
            SetTextColor(sceneNameText, resolvedInfoColor);
            SetTextColor(savedAtText, resolvedInfoColor);
            SetTextColor(statusText, hasData ? accentColor : emptyInfoColor);
        }

        private void EnsureSelectionOutline()
        {
            if (selectionOutline != null)
            {
                return;
            }

            selectionOutline = GetComponent<Outline>();

            if (selectionOutline == null)
            {
                selectionOutline = gameObject.AddComponent<Outline>();
            }
        }

        private static void SetTextColor(TMP_Text target, Color color)
        {
            if (target != null)
            {
                target.color = color;
            }
        }

        #endregion
    }
}
