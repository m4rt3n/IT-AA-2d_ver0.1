using UnityEngine;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject backgroundDim;
    [SerializeField] private PlayerController playerController;

    private bool isOpen = false;

    private void Start()
    {
        loginPanel.SetActive(false);
        backgroundDim.SetActive(false);
    }

    public void OpenMenu()
    {
        Debug.Log("OpenMenu called");

        isOpen = true;

        loginPanel.SetActive(true);
        backgroundDim.SetActive(true);

        if (playerController != null)
            playerController.SetPlayerControlEnabled(false);
    }

    public void CloseMenu()
    {
        isOpen = false;

        loginPanel.SetActive(false);
        backgroundDim.SetActive(false);

        if (playerController != null)
            playerController.SetPlayerControlEnabled(true);
    }
}