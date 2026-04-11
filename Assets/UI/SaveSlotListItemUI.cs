using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotListItemUI : MonoBehaviour
{
    #region Inspector

    [SerializeField] private TMP_Text labelText;
    [SerializeField] private Button loadButton;

    #endregion

    #region Private Fields

    private SaveSlotInfo currentSave;
    private Action<SaveSlotInfo> onLoadClicked;

    #endregion

    #region Public Methods

    public void Setup(SaveSlotInfo save, Action<SaveSlotInfo> onLoadCallback)
    {
        currentSave = save;
        onLoadClicked = onLoadCallback;

        if (labelText != null)
        {
            labelText.text =
                $"{save.SaveSlotName} | " +
                $"Level {save.Level} | " +
                $"Score {save.Score} | " +
                $"{save.ProgressPercent}%";
        }

        if (loadButton != null)
        {
            loadButton.onClick.RemoveAllListeners();
            loadButton.onClick.AddListener(OnClickLoad);
        }
    }

    #endregion

    #region Private Methods

    private void OnClickLoad()
    {
        onLoadClicked?.Invoke(currentSave);
    }

    #endregion
}