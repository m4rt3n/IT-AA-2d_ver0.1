using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotListItemUI : MonoBehaviour
{
    #region Inspector

    [SerializeField] private TMP_Text usernameText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text lastPlayedText;
    [SerializeField] private Button button;

    #endregion

    #region Private Fields

    private SaveSlotInfo data;
    private Action<SaveSlotInfo> callback;

    #endregion

    #region Public Methods

    public void Bind(SaveSlotInfo info, Action<SaveSlotInfo> onClick)
    {
        data = info;
        callback = onClick;

        if (usernameText != null) usernameText.text = info.Username;
        if (levelText != null) levelText.text = $"Level: {info.Level}";
        if (scoreText != null) scoreText.text = $"Score: {info.Score}";
        if (lastPlayedText != null) lastPlayedText.text = info.LastPlayed;

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => callback?.Invoke(data));
        }

        Debug.Log($"[SaveSlotListItemUI] Gebunden: {info.Username}");
    }

    #endregion
}