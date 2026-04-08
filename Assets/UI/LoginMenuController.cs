using TMPro;
using UnityEngine;

public class LoginMenuController : MonoBehaviour
{
    [SerializeField] private SaveSlotSelectionPopup popup;
    [SerializeField] private TMP_Text selectedUserText;

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

    private void OnSaveSelected(SaveSlotInfo save)
    {
        if (save == null)
        {
            Debug.LogWarning("[LoginMenuController] Kein Save ausgewählt.");
            return;
        }

        Debug.Log($"[LoginMenuController] Gewählt: {save.Username} - {save.SaveSlotName}");

        if (selectedUserText != null)
        {
            selectedUserText.text = $"{save.Username} - {save.SaveSlotName}";
        }

        if (AuthManager.Instance != null)
        {
            AuthManager.Instance.SignInWithSaveSlot(save);
        }
        else
        {
            Debug.LogError("[LoginMenuController] AuthManager fehlt.");
        }
    }
}