using TMPro;
using UnityEngine;

public class LoginMenuController : MonoBehaviour
{
    #region Inspector

    [SerializeField] private SaveSlotSelectionPopup popup;
    [SerializeField] private TMP_Text selectedUserText;

    #endregion

    #region Public

    public void OpenSaveSelection()
    {
        Debug.Log("[Login] Öffne SaveSlot Auswahl");
        popup.Open(OnSaveSelected);
    }

    #endregion

    #region Callback

    private void OnSaveSelected(SaveSlotInfo save)
    {
        Debug.Log($"[Login] Ausgewählt: {save.Username} ({save.SaveSlotName})");

        selectedUserText.text = $"{save.Username} - {save.SaveSlotName}";

        AuthManager.Instance.SignInWithSaveSlot(save);
    }

    #endregion
}