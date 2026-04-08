using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SaveSlotSelectionPopup : MonoBehaviour
{
    #region Inspector

    [SerializeField] private GameObject root;
    [SerializeField] private Transform contentRoot;
    [SerializeField] private SaveSlotListItemUI itemPrefab;
    [SerializeField] private TMP_Text emptyText;

    #endregion

    #region Private Fields

    private readonly List<SaveSlotListItemUI> items = new List<SaveSlotListItemUI>();
    private Action<SaveSlotInfo> callback;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        if (root != null)
        {
            root.SetActive(false);
        }

        Debug.Log("[SaveSlotSelectionPopup] Initialisiert.");
    }

    #endregion

    #region Public Methods

    public void Open(Action<SaveSlotInfo> onSelected)
    {
        callback = onSelected;

        if (root != null)
        {
            root.SetActive(true);
        }

        Refresh();
    }

    public void Close()
    {
        if (root != null)
        {
            root.SetActive(false);
        }
    }

    #endregion

    #region Private Methods

    private void Refresh()
    {
        Clear();

        if (DatabaseManager.Instance == null)
        {
            Debug.LogError("[SaveSlotSelectionPopup] DatabaseManager fehlt.");
            return;
        }

        List<SaveSlotInfo> list = DatabaseManager.Instance.GetAllSaveSlots();

        if (emptyText != null)
        {
            emptyText.gameObject.SetActive(list.Count == 0);
        }

        foreach (SaveSlotInfo slot in list)
        {
            SaveSlotListItemUI item = Instantiate(itemPrefab, contentRoot);
            item.Bind(slot, OnSelected);
            items.Add(item);
        }

        Debug.Log($"[SaveSlotSelectionPopup] Einträge aufgebaut: {items.Count}");
    }

    private void Clear()
    {
        foreach (SaveSlotListItemUI item in items)
        {
            if (item != null)
            {
                Destroy(item.gameObject);
            }
        }

        items.Clear();
    }

    private void OnSelected(SaveSlotInfo save)
    {
        Debug.Log($"[SaveSlotSelectionPopup] Gewählt: {save.Username}");
        callback?.Invoke(save);
        Close();
    }

    #endregion
}