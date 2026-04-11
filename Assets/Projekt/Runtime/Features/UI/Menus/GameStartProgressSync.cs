/*
 * Datei: GameStartProgressSync.cs
 * Zweck: Synchronisiert geladene Save-Daten beim Szenenstart mit UI und Speicherung.
 * Verantwortung: Zeigt Werte aus der PlayerSession an und schreibt sie bei Bedarf zurück in die Datenhaltung.
 * Abhängigkeiten: PlayerSession, DatabaseManager, TMP_Text.
 * Verwendet von: Gameplay- oder Status-UI nach dem Laden eines Spielstands.
 */

using ITAA.Data;
using ITAA.Player.Session;
using TMPro;
using UnityEngine;

namespace ITAA.UI.Menus
{
    public class GameStartProgressSync : MonoBehaviour
    {
        #region Inspector

        [Header("Optional UI Targets")]
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text progressText;
        [SerializeField] private TMP_Text slotNameText;

        #endregion

        #region Unity Methods

        private void Start()
        {
            if (PlayerSession.Instance == null || !PlayerSession.Instance.HasSaveLoaded)
            {
                Debug.LogWarning("[GameStartProgressSync] Kein geladener Save vorhanden.");
                return;
            }

            UpdateTexts();

            if (DatabaseManager.Instance != null)
            {
                DatabaseManager.Instance.SaveProgress(
                    PlayerSession.Instance.SaveSlotId,
                    PlayerSession.Instance.Level,
                    PlayerSession.Instance.Score,
                    PlayerSession.Instance.ProgressPercent);
            }
        }

        #endregion

        #region Private Methods

        private void UpdateTexts()
        {
            if (slotNameText != null)
            {
                slotNameText.text = PlayerSession.Instance.SaveSlotName;
            }

            if (levelText != null)
            {
                levelText.text = $"Level: {PlayerSession.Instance.Level}";
            }

            if (scoreText != null)
            {
                scoreText.text = $"Score: {PlayerSession.Instance.Score}";
            }

            if (progressText != null)
            {
                progressText.text = $"Fortschritt: {PlayerSession.Instance.ProgressPercent}%";
            }
        }

        #endregion
    }
}