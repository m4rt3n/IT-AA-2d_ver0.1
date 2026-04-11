using ITAA.Data;
using ITAA.Player.Session;
using TMPro;
using UnityEngine;

namespace ITAA.UI.Menus
{
    public class GameStartProgressSync : MonoBehaviour
    {
        [Header("Optional UI Targets")]
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text progressText;
        [SerializeField] private TMP_Text slotNameText;

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
    }
}