/*
 * Datei: LoadGamePanel.cs
 * Zweck: Erstellt die sichtbare Liste aller verfügbaren Spielstände.
 * Verantwortung: Lädt SaveSlots aus dem DatabaseManager, instanziiert UI-Items und startet den gewählten Spielstand.
 * Abhängigkeiten: AuthManager, DatabaseManager, SaveSlotData, SaveSlotListItemUI.
 * Verwendet von: LoadGamePanel im Hauptmenü.
 */

using System.Collections.Generic;
using ITAA.Authentication;
using ITAA.Data;
using ITAA.Data.Models;
using ITAA.UI.Widgets;
using UnityEngine;

namespace ITAA.UI.Menus
{
    public class LoadGamePanel : MonoBehaviour
    {
        #region Inspector

        [Header("References")]
        [SerializeField] private Transform contentRoot;
        [SerializeField] private SaveSlotListItemUI itemPrefab;

        [Header("Auto Refresh")]
        [SerializeField] private bool refreshOnEnable = true;

        #endregion

        #region Private Fields

        private readonly List<SaveSlotListItemUI> spawnedItems = new();

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            if (refreshOnEnable)
            {
                Refresh();
            }
        }

        #endregion

        #region Public Methods

        public void Refresh()
        {
            ClearItems();

            if (DatabaseManager.Instance == null || contentRoot == null || itemPrefab == null)
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

        #endregion

        #region Private Methods

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

        #endregion
    }
}