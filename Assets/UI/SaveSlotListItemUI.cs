using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotListItemUI : MonoBehaviour
{
    #region Inspector

    [Header("Texts")]
    [SerializeField] private TMP_Text saveSlotNameText;
    [SerializeField] private TMP_Text usernameText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private TMP_Text lastPlayedText;

    [Header("UI")]
    [SerializeField] private Slider progressSlider;
    [SerializeField] private Button selectButton;

    #endregion

    #region Private Fields

    private SaveSlotInfo currentSave;
    private Action<SaveSlotInfo> onSelected;

    #endregion

    #region Public Methods

    public void Setup(SaveSlotInfo save, Action<SaveSlotInfo> onSelectCallback)
    {
        currentSave = save;
        onSelected = onSelectCallback;

        if (saveSlotNameText != null) saveSlotNameText.text = save.SaveSlotName;
        if (usernameText != null) usernameText.text = $"User: {save.Username}";
        if (levelText != null) levelText.text = $"Level: {save.Level}";
        if (scoreText != null) scoreText.text = $"Score: {save.Score}";
        if (progressText != null) progressText.text = $"Fortschritt: {save.ProgressPercent}%";
        if (lastPlayedText != null) lastPlayedText.text = $"Letztes Spiel: {FormatDate(save.LastPlayedUtc)}";

        if (progressSlider != null)
        {
            progressSlider.minValue = 0f;
            progressSlider.maxValue = 100f;
            progressSlider.value = save.ProgressPercent;
        }

        if (selectButton != null)
        {
            selectButton.onClick.RemoveAllListeners();
            selectButton.onClick.AddListener(OnClickSelect);
        }
    }

    #endregion

    #region Private Methods

    private void OnClickSelect()
    {
        onSelected?.Invoke(currentSave);
    }

    private static string FormatDate(string utcValue)
    {
        if (DateTime.TryParse(utcValue, out DateTime parsed))
        {
            return parsed.ToLocalTime().ToString("dd.MM.yyyy HH:mm");
        }

        return "-";
    }

    #endregion
}