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
    [SerializeField] private Button selectButton;

    #endregion

    #region Private

    private SaveSlotInfo data;
    private Action<SaveSlotInfo> onSelected;

    #endregion

    #region Public

    public void Bind(SaveSlotInfo info, Action<SaveSlotInfo> callback)
    {
        data = info;
        onSelected = callback;

        usernameText.text = info.Username;
        levelText.text = "Level: " + info.Level;
        scoreText.text = "Score: " + info.Score;
        lastPlayedText.text = info.LastPlayed;

        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(OnClicked);

        Debug.Log($"[SaveSlotItem] Gebunden: {info.Username}");
    }

    #endregion

    #region Events

    private void OnClicked()
    {
        Debug.Log($"[SaveSlotItem] Klick auf {data.Username}");
        onSelected?.Invoke(data);
    }

    #endregion
}