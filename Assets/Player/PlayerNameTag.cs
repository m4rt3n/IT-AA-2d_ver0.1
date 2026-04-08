using TMPro;
using UnityEngine;

public class PlayerNameTag : MonoBehaviour
{
    [SerializeField] private TMP_Text playerNameText;

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (playerNameText == null)
        {
            Debug.LogWarning("[PlayerNameTag] TMP_Text Referenz fehlt.");
            return;
        }

        if (PlayerSession.Instance == null || !PlayerSession.Instance.IsLoggedIn)
        {
            playerNameText.text = "Gast";
            Debug.Log("[PlayerNameTag] Keine aktive Session.");
            return;
        }

        playerNameText.text = PlayerSession.Instance.Username;
        Debug.Log($"[PlayerNameTag] Name gesetzt: {PlayerSession.Instance.Username}");
    }
}