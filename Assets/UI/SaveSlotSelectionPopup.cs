using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotSelectionPopup : MonoBehaviour
{
    #region Inspector

    [SerializeField] private MenuManager menuManager;

    [Header("List")]
    [SerializeField] private Transform contentRoot;
    [SerializeField] private SaveSlotListItemUI saveSlotItemPrefab;

    [Header("Header / Feedback")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text infoText;

    [Header("Actions")]
    [SerializeField] private Button createNewSlotButton;
    [SerializeField] private Button backButton;

    #endregion

    #region Private Fields

    private string currentUsername = string.Empty;
    private readonly List<SaveSlotListItemUI> spawnedItems = new List<SaveSlotListItemUI>();

    #endregion

    #region Public Methods

    public void OpenForUser(string username)
    {
        currentUsername = username;

        if (titleText != null)
        {
            titleText.text = $"SaveSlots für {username}";
        }

        Refresh();
    }

    public void OnClickCreateNewSlot()
    {
        if (string.IsNullOrWhiteSpace(currentUsername))
        {
            SetInfo("Kein Benutzer aktiv.");
            return;
        }

        if (DatabaseManager.Instance == null)
        {
            SetInfo("DatabaseManager fehlt.");
            return;
        }

        SaveSlotInfo newSlot = DatabaseManager.Instance.CreateNewSaveSlot(currentUsername);

        if (newSlot == null)
        {
            SetInfo("SaveSlot konnte nicht erstellt werden.");
            return;
        }

        SetInfo($"Neuer SaveSlot erstellt: {newSlot.SaveSlotName}");
        Refresh();
    }

    public void OnClickBack()
    {
        menuManager?.ShowLoginMenu();
    }

    #endregion

    #region Private Methods

    private void Refresh()
    {
        ClearList();

        if (DatabaseManager.Instance == null)
        {
            SetInfo("DatabaseManager fehlt.");
            return;
        }

        List<SaveSlotInfo> saves = DatabaseManager.Instance.GetSaveSlotsByUsername(currentUsername);

        if (saves.Count == 0)
        {
            SetInfo("Keine SaveSlots vorhanden.");
            return;
        }

        SetInfo("Wähle einen Spielstand aus.");

        foreach (SaveSlotInfo save in saves)
        {
            SaveSlotListItemUI item = Instantiate(saveSlotItemPrefab, contentRoot);
            item.Setup(save, OnSaveSelected);
            spawnedItems.Add(item);
        }
    }

    private void OnSaveSelected(SaveSlotInfo selectedSave)
    {
        if (selectedSave == null)
        {
            SetInfo("Ungültiger SaveSlot.");
            return;
        }

        AuthManager.Instance?.StartGameWithSave(selectedSave);
    }

    private void ClearList()
    {
        for (int i = 0; i < spawnedItems.Count; i++)
        {
            if (spawnedItems[i] != null)
            {
                Destroy(spawnedItems[i].gameObject);
            }
        }

        spawnedItems.Clear();
    }

    private void SetInfo(string message)
    {
        if (infoText != null)
        {
            infoText.text = message;
        }

        Debug.Log($"[SaveSlotSelectionPopup] {message}");
    }

    #endregion
}