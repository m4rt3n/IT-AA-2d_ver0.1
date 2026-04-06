using TMPro;
using UnityEngine;

public class PlayerNameTag : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;

    private void Start()
    {
        if (nameText == null)
        {
            Debug.LogWarning("PlayerNameTag: nameText ist nicht gesetzt.");
            return;
        }

        if (PlayerSession.Instance != null && PlayerSession.Instance.IsLoggedIn)
        {
            nameText.text = PlayerSession.Instance.Username;
        }
        else
        {
            nameText.text = "Gast";
        }
    }
}