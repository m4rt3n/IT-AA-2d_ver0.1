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

    private readonly List<SaveSlotListItemUI> spawnedItems = new();
    private Action<SaveSlotInfo> onSaveSelected;

    #endregion

    #region Unity

    private void Awake()
    {
        if (root != null)
        {
            root.SetActive(false);
        }

        Debug.Log("[Popup] Initialisiert.");
    }

    #endregion

    #region Public

    public void Open(Action<SaveSlotInfo> onSelected)
    {
        Debug.Log("[Popup] Öffnen");

        onSaveSelected = onSelected;

        if (root != null)
        {
            root.SetActive(true);
        }

        Refresh();
    }

    public void Close()
    {
        Debug.Log("[Popup] Schließen");

        if (root != null)
        {
            root.SetActive(false);
        }
    }

    public void Refresh()
    {
        Debug.Log("[Popup] Refresh");

        Clear();

        if (DatabaseManager.Instance == null)
        {
            Debug.LogError("[Popup] DatabaseManager fehlt!");
            return;
        }

        var list = DatabaseManager.Instance.GetAllSaveSlots();

        emptyText.gameObject.SetActive(list.Count == 0);

        foreach (var slot in list)
        {
            var item = Instantiate(itemPrefab, contentRoot);
            item.Bind(slot, OnSelected);
            spawnedItems.Add(item);
        }

        Debug.Log($"[Popup] Einträge: {spawnedItems.Count}");
    }

    #endregion

    #region Private

    private void Clear()
    {
        foreach (var item in spawnedItems)
        {
            if (item != null)
                Destroy(item.gameObject);
        }

        spawnedItems.Clear();
    }

    private void OnSelected(SaveSlotInfo slot)
    {
        Debug.Log($"[Popup] Gewählt: {slot.Username}");

        onSaveSelected?.Invoke(slot);
        Close();
    }

    #endregion
}