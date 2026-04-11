/*
 * Datei: LoadGamePanel.cs
 * Zweck: Verwaltet die Darstellung des Load-Game-Panels und erzeugt Save-Slot-Einträge.
 * Verantwortung:
 *   - Erzeugt UI-Einträge für Save-Slots
 *   - Löscht alte Einträge vor dem Neuaufbau
 *   - Aktualisiert die Liste beim Öffnen des Panels
 *   - Unterstützt horizontale Scroll-Layouts über den Content-Container
 *
 * Abhängigkeiten:
 *   - Transform contentRoot
 *   - GameObject saveSlotItemPrefab
 *   - SaveSlotListItemUI
 *
 * Verwendet von:
 *   - MenuManager
 *   - StartMenuController
 *   - LoadGamePanel im UI
 */
using UnityEngine;

public class LoadGamePanel : MonoBehaviour
{
    #region Inspector

    [Header("References")]
    [SerializeField] private Transform contentRoot;
    [SerializeField] private GameObject saveSlotItemPrefab;

    [Header("Debug / Test")]
    [SerializeField] private int testSlotCount = 5;

    #endregion

    #region Public Methods

    /// <summary>
    /// Öffentliche Aktualisierungsmethode für externe Aufrufe.
    /// </summary>
    public void Refresh()
    {
        RefreshSlots();
    }

    #endregion

    #region Unity Methods

    private void OnEnable()
    {
        RefreshSlots();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Baut die Save-Slot-Liste neu auf.
    /// </summary>
    private void RefreshSlots()
    {
        if (contentRoot == null)
        {
            Debug.LogWarning($"[{nameof(LoadGamePanel)}] ContentRoot ist nicht zugewiesen.");
            return;
        }

        if (saveSlotItemPrefab == null)
        {
            Debug.LogWarning($"[{nameof(LoadGamePanel)}] SaveSlotItemPrefab ist nicht zugewiesen.");
            return;
        }

        ClearSlots();

        for (int i = 1; i <= testSlotCount; i++)
        {
            CreateSlot(i);
        }
    }

    /// <summary>
    /// Entfernt alle vorhandenen Slot-Elemente aus dem Content.
    /// </summary>
    private void ClearSlots()
    {
        for (int i = contentRoot.childCount - 1; i >= 0; i--)
        {
            Destroy(contentRoot.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// Erstellt einen einzelnen Slot-Eintrag.
    /// </summary>
    /// <param name="index">Slot-Nummer.</param>
    private void CreateSlot(int index)
    {
        GameObject slotObject = Instantiate(saveSlotItemPrefab, contentRoot);

        SaveSlotListItemUI slotUI = slotObject.GetComponent<SaveSlotListItemUI>();

        if (slotUI == null)
        {
            Debug.LogWarning($"[{nameof(LoadGamePanel)}] SaveSlotListItemUI fehlt auf Prefab {saveSlotItemPrefab.name}.");
            return;
        }

        slotUI.Setup(
            index,
            $"Level {index}",
            index * 100,
            index * 10
        );
    }

    #endregion
}