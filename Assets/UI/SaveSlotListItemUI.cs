using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotListItemUI : MonoBehaviour
{
    [SerializeField] private TMP_Text usernameText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text lastPlayedText;
    [SerializeField] private Button button;

    private SaveSlotInfo data;
    private Action<SaveSlotInfo> callback;

    public void Bind(SaveSlotInfo info, Action<SaveSlotInfo> onClick)
    {
        data = info;
        callback = onClick;

        usernameText.text = info.Username;
        levelText.text = $"Level: {info.Level}";
        scoreText.text = $"Score: {info.Score}";
        lastPlayedText.text = info.LastPlayed;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => callback?.Invoke(data));
    }
}