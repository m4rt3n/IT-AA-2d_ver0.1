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

    #region Private

    private List<SaveSlotListItemUI> items = new();
    private Action<SaveSlotInfo> callback;

    #endregion

    public void Open(Action<SaveSlotInfo> onSelected)
    {
        callback = onSelected;
        root.SetActive(true);

        Refresh();
    }

    public void Close()
    {
        root.SetActive(false);
    }

    private void Refresh()
    {
        Clear();

        var list = DatabaseManager.Instance.GetAllSaveSlots();

        emptyText.gameObject.SetActive(list.Count == 0);

        foreach (var slot in list)
        {
            var item = Instantiate(itemPrefab, contentRoot);
            item.Bind(slot, OnSelected);
            items.Add(item);
        }
    }

    private void Clear()
    {
        foreach (var i in items)
            Destroy(i.gameObject);

        items.Clear();
    }

    private void OnSelected(SaveSlotInfo save)
    {
        callback?.Invoke(save);
        Close();
    }
}