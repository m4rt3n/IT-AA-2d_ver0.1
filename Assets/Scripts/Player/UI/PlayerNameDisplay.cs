using ITAA.Player.Session;
using TMPro;
using UnityEngine;

namespace ITAA.Player.UI
{
    public class PlayerNameDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text targetText;
        [SerializeField] private string fallbackName = "Spieler";

        private void Start()
        {
            if (targetText == null)
            {
                targetText = GetComponent<TMP_Text>();
            }

            if (targetText == null)
            {
                return;
            }

            string playerName = fallbackName;

            if (PlayerSession.Instance != null && !string.IsNullOrWhiteSpace(PlayerSession.Instance.Username))
            {
                playerName = PlayerSession.Instance.Username;
            }

            targetText.text = playerName;
        }
    }
}