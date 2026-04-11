using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadGamePanel : MonoBehaviour
{
    #region Inspector

    [Header("UI References")]
    [SerializeField] private Transform contentRoot;
    [SerializeField] private GameObject slotItemPrefab;
    [SerializeField] private TMP_Text infoText;

    [Header("Scene")]
    [SerializeField] private string gameSceneName = "GameScene";

    [Header("Navigation")]
    [SerializeField] private GameObject parentPanelToHide;

    #endregion

    #region Unity

    private void OnEnable()
    {
        LoadSlots();
    }

    #endregion

    #region Loading

    private void LoadSlots()
    {
        ClearList();

        if (contentRoot == null)
        {
            SetInfo("Content Root fehlt.");
            return;
        }

        if (slotItemPrefab == null)
        {
            SetInfo("Slot Item Prefab fehlt.");
            return;
        }

        if (DatabaseManager.Instance == null)
        {
            SetInfo("Keine Datenbank vorhanden.");
            return;
        }

        List<SaveSlotInfo> saves = DatabaseManager.Instance.GetAllSaveSlots();

        if (saves == null || saves.Count == 0)
        {
            SetInfo("Keine Spielstände gefunden.");
            return;
        }

        SetInfo("Wähle einen Spielstand.");

        foreach (SaveSlotInfo save in saves)
        {
            GameObject entry = Instantiate(slotItemPrefab, contentRoot);

            TMP_Text text = entry.GetComponentInChildren<TMP_Text>(true);
            Button btn = entry.GetComponentInChildren<Button>(true);

            if (text != null)
            {
                text.text =
                    $"{save.SaveSlotName}\n" +
                    $"Level {save.Level}  |  Score {save.Score}\n" +
                    $"Fortschritt: {save.ProgressPercent}%";
            }

            if (btn != null)
            {
                SaveSlotInfo capturedSave = save;
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => LoadSelected(capturedSave));
            }
            else
            {
                Debug.LogWarning("[LoadGamePanel] Kein Button im Slot-Prefab gefunden.");
            }
        }
    }

    private void LoadSelected(SaveSlotInfo save)
    {
        if (save == null)
        {
            SetInfo("Ungültiger Spielstand.");
            return;
        }

        if (PlayerSession.Instance == null)
        {
            SetInfo("Session-System fehlt.");
            return;
        }

        PlayerSession.Instance.SetSession(save);

        if (parentPanelToHide != null)
        {
            parentPanelToHide.SetActive(false);
        }

        Debug.Log($"[LoadGamePanel] Lade Spielstand: {save.SaveSlotName}");
        SceneManager.LoadScene(gameSceneName);
    }

    #endregion

    #region Helpers

    private void ClearList()
    {
        if (contentRoot == null)
        {
            return;
        }

        for (int i = contentRoot.childCount - 1; i >= 0; i--)
        {
            Destroy(contentRoot.GetChild(i).gameObject);
        }
    }

    private void SetInfo(string msg)
    {
        if (infoText != null)
        {
            infoText.text = msg;
        }

        Debug.Log($"[LoadGamePanel] {msg}");
    }

    #endregion
}