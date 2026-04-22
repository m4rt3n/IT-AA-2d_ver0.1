/*
 * Datei: ArthurNameUI.cs
 * Zweck:
 *   Zeigt den Namen von Arthur im UI an und blendet ihn wieder aus.
 *
 * Verantwortung:
 *   - Text setzen
 *   - UI-Root aktivieren/deaktivieren
 *
 * Wichtig:
 *   - Diese Version nutzt KEINE Bildschirmpositions-Berechnung
 *   - Dadurch entstehen keine WorldToScreenPoint-/Frustum-Probleme
 *
 * Verwendung:
 *   - Wird von ArthurAutoInteraction angesprochen
 *   - ShowName("Arthur")
 *   - Hide()
 */

using TMPro;
using UnityEngine;

namespace ITAA.NPC.Arthur
{
    public sealed class ArthurNameUI : MonoBehaviour
    {
        #region Inspector

        [Header("References")]
        [SerializeField] private GameObject root;
        [SerializeField] private TextMeshProUGUI nameText;

        [Header("Defaults")]
        [SerializeField] private string defaultName = "Arthur";

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs = false;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            HideImmediate();
        }

        #endregion

        #region Public API

        public void Show()
        {
            ShowName(defaultName);
        }

        public void ShowName(string displayName)
        {
            if (nameText != null)
            {
                nameText.text = string.IsNullOrWhiteSpace(displayName) ? defaultName : displayName;
            }
            else if (enableDebugLogs)
            {
                Debug.LogWarning($"[{nameof(ArthurNameUI)}] nameText ist nicht gesetzt.", this);
            }

            if (root != null)
            {
                root.SetActive(true);
            }
            else if (enableDebugLogs)
            {
                Debug.LogWarning($"[{nameof(ArthurNameUI)}] root ist nicht gesetzt.", this);
            }

            if (enableDebugLogs)
            {
                Debug.Log($"[{nameof(ArthurNameUI)}] ShowName -> {displayName}", this);
            }
        }

        public void Hide()
        {
            if (root != null)
            {
                root.SetActive(false);
            }

            if (enableDebugLogs)
            {
                Debug.Log($"[{nameof(ArthurNameUI)}] Hide", this);
            }
        }

        public void HideImmediate()
        {
            if (root != null)
            {
                root.SetActive(false);
            }
        }

        public bool IsVisible()
        {
            return root != null && root.activeSelf;
        }

        #endregion
    }
}