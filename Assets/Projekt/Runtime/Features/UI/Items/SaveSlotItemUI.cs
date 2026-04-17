/*
 * Datei: SaveSlotItemUI.cs
 * Zweck: Stellt einen einzelnen Save-Slot in der UI dar.
 * Verantwortung:
 * - Zeigt Slot-Daten an
 * - Löst Auswahl per Button aus
 * - Unterstützt belegte und leere Slots
 * - Verdrahtet sich bei Bedarf teilweise automatisch
 */

using System;
using ITAA.System.Savegame;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
            currentSlot = slot;
            onSelected = onSelectedCallback;

            if (slot == null)
            {
                ApplyNullState();
                return;
            }

            bool hasData = slot.HasData;

            SetText(slotNameText,
                !string.IsNullOrWhiteSpace(slot.DisplayName)
                    ? slot.DisplayName
                    : $"Slot {slot.SlotId}");

            SetText(sceneNameText,
                hasData
                    ? GetSafeText(slot.SceneName, "-")
                    : "Kein Spielstand");

            SetText(savedAtText,
                hasData
                    ? GetSafeText(slot.SavedAtText, "-")
                    : "-");

            SetText(statusText, hasData ? "Belegt" : "Leer");

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

            if (enableDebugLogs)
            {
                Debug.Log($"[{nameof(SaveSlotItemUI)}] Setup Slot {slot.SlotId} | HasData={slot.HasData}", this);
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
                Debug.LogWarning($"[{nameof(SaveSlotItemUI)}] Kein SaveSlotEntity gesetzt auf '{gameObject.name}'.", this);
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
            SetText(slotNameText, "Unbekannter Slot");
            SetText(sceneNameText, "-");
            SetText(savedAtText, "-");
            SetText(statusText, "Fehler");

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

            TMP_Text[] texts = GetComponentsInChildren<TMP_Text>(true);

            if (slotNameText == null)
            {
                slotNameText = FindTextByName(texts, "SlotNameText", "LabelText", "TitleText", "NameText");
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

            if (enableDebugLogs)
            {
                Debug.Log(
                    $"[{nameof(SaveSlotItemUI)}] ResolveReferences | " +
                    $"Button={(selectButton != null)} | " +
                    $"SlotName={(slotNameText != null)} | " +
                    $"SceneName={(sceneNameText != null)} | " +
                    $"SavedAt={(savedAtText != null)} | " +
                    $"Status={(statusText != null)}",
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

            return texts[0];
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

        #endregion
    }
}