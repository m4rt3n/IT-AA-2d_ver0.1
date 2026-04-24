/*
 * Datei: PlayerNameDisplay.cs
 * Zweck: Zeigt den Namen des aktuell geladenen Spielers in einem UI-Textfeld an.
 * Verantwortung: Liest den Namen aus der PlayerSession oder dem Runtime-Save und setzt ihn in ein TMP_Text-Element.
 * Abhängigkeiten: PlayerSession, TMP_Text.
 * Verwendet von: UI-Elemente wie HUD oder Menüanzeigen mit Spielername.
 */

using ITAA.Player.Session;
using TMPro;
using UnityEngine;

namespace ITAA.Player.UI
{
    public class PlayerNameDisplay : MonoBehaviour
    {
        #region Inspector

        [Header("References")]
        [SerializeField] private TMP_Text targetText;

        [Header("Fallback")]
        [SerializeField] private string fallbackName = "Spieler";

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (targetText == null)
            {
                targetText = GetComponent<TMP_Text>();
            }
        }

        private void Start()
        {
            UpdateName();
        }

        #endregion

        #region Public Methods

        public void UpdateName()
        {
            if (targetText == null)
            {
                return;
            }

            string playerName = PlayerSession.Instance != null
                ? PlayerSession.Instance.GetResolvedPlayerName(fallbackName)
                : fallbackName;

            targetText.text = playerName;
        }

        #endregion
    }
}
