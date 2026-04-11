using TMPro;
using UnityEngine;

public class PlayerNameDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text progressText;

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (PlayerSession.Instance == null || !PlayerSession.Instance.IsLoggedIn)
        {
            if (nameText != null) nameText.text = "Gast";
            if (progressText != null) progressText.text = string.Empty;

            Debug.Log("[PlayerNameDisplay] Keine aktive Session.");
            return;
        }

        if (nameText != null)
        {
            nameText.text = PlayerSession.Instance.Username;
        }

        if (progressText != null)
        {
            progressText.text = $"Level {PlayerSession.Instance.Level} | Score {PlayerSession.Instance.Score}";
        }

        Debug.Log($"[PlayerNameDisplay] Anzeige aktualisiert: {PlayerSession.Instance.Username}");
    }
}