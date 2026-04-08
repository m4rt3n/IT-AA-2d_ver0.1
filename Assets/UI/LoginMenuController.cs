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
        Debug.Log("[Login] Öffne Auswahl");

        popup.Open(OnSaveSelected);
    }

    #endregion

    #region Callback

    private void OnSaveSelected(SaveSlotInfo save)
    {
        Debug.Log($"[Login] Spieler gewählt: {save.Username}");

        selectedUserText.text = save.Username;
    }

    #endregion
}