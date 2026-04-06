using TMPro;
using UnityEngine;

public class PlayerNameDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshPro nameText;

    private void Start()
    {
        UpdateName();
    }

    public void UpdateName()
    {
        if (nameText == null)
        {
            Debug.LogWarning("PlayerNameDisplay: nameText ist nicht gesetzt.");
            return;
        }

        if (PlayerSession.Instance == null)
        {
            nameText.text = "Unknown";
            return;
        }

        nameText.text = PlayerSession.Instance.Username;
    }
}