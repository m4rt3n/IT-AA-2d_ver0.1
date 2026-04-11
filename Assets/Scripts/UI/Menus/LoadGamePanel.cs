using System.Collections.Generic;
using ITAA.Authentication;
using ITAA.Data;
using ITAA.Data.Models;
using UnityEngine;

namespace ITAA.UI.Menus
{
    public class LoadGamePanel : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform contentRoot;
        [SerializeField] private SaveSlotListItemUI itemPrefab;

        [Header("Auto Refresh")]
        [SerializeField] private bool refreshOnEnable = true;

        private readonly List<SaveSlotListItemUI> spawnedItems = new();

        private void OnEnable()
        {
            if (refreshOnEnable)
            {
                Refresh();
            }
        }

        public void Refresh()
        {
            ClearItems();

            if (DatabaseManager.Instance == null || itemPrefab == null || contentRoot == null)
            {
                Debug.LogWarning("[LoadGamePanel] Referenzen unvollständig.");
                return;
            }

            IReadOnlyList<SaveSlotData> saveSlots = DatabaseManager.Instance.GetAllSaveSlots();

            foreach (SaveSlotData saveSlot in saveSlots)
            {
                SaveSlotListItemUI item = Instantiate(itemPrefab, contentRoot);
                item.Bind(saveSlot, HandleSaveSlotSelected);
                spawnedItems.Add(item);
            }
        }

        private void HandleSaveSlotSelected(SaveSlotData saveSlot)
        {
            if (AuthManager.Instance == null)
            {
                Debug.LogError("[LoadGamePanel] AuthManager fehlt.");
                return;
            }

            AuthManager.Instance.StartGameWithSave(saveSlot);
        }

        private void ClearItems()
        {
            foreach (SaveSlotListItemUI item in spawnedItems)
            {
                if (item != null)
                {
                    Destroy(item.gameObject);
                }
            }

            spawnedItems.Clear();
        }
    }
}