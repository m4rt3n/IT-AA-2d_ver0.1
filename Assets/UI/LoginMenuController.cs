using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginMenuController : MonoBehaviour
{
    #region Inspector

    [Header("Popup")]
    [SerializeField] private SaveSlotSelectionPopup popup;

    [Header("UI")]
    [SerializeField] private TMP_Text selectedUserText;
    [SerializeField] private Button continueButton;

    #endregion

    #region Private Fields

    private SaveSlotInfo selectedSave;

    #endregion

    #region Unity Methods

    private void Start()
    {
        ResetSelection();
    }

    private void OnEnable()
    {
        ResetSelection();
    }

    #endregion

    #region Public Methods

    public void OpenSaveSelection()
    {
        Debug.Log("[LoginMenuController] Öffne SaveSlot-Auswahl.");

        if (popup == null)
        {
            Debug.LogError("[LoginMenuController] Popup Referenz fehlt.");
            return;
        }

        popup.Open(OnSaveSelected);
    }

    public void ContinueWithSelectedSave()
    {
        Debug.Log("[LoginMenuController] Fortsetzen mit ausgewähltem Save.");

        if (selectedSave == null)
        {
            Debug.LogWarning("[LoginMenuController] Kein Save ausgewählt.");
            return;
        }

        if (AuthManager.Instance == null)
        {
            Debug.LogError("[LoginMenuController] AuthManager fehlt.");
            return;
        }

        AuthManager.Instance.SignInWithSaveSlot(selectedSave);
    }

    #endregion

    #region Private Methods

    private void OnSaveSelected(SaveSlotInfo save)
    {
        if (save == null)
        {
            Debug.LogWarning("[LoginMenuController] Kein Save ausgewählt.");
            return;
        }

        selectedSave = save;

        Debug.Log($"[LoginMenuController] Gewählt: {save.Username} - {save.SaveSlotName}");

        if (selectedUserText != null)
        {
            selectedUserText.text = $"{save.Username} - {save.SaveSlotName} | Level {save.Level} | Score {save.Score}";
        }

        if (continueButton != null)
        {
            continueButton.interactable = true;
        }
    }

    private void ResetSelection()
    {
        selectedSave = null;

        if (selectedUserText != null)
        {
            selectedUserText.text = "Kein Spielstand ausgewählt";
        }

        if (continueButton != null)
        {
            continueButton.interactable = false;
        }
    }

    #endregion
}