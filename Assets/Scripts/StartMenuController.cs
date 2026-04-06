using UnityEngine;

public class StartMenuController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject backgroundDim;

    [Header("Player")]
    [SerializeField] private PlayerController playerController;

    private bool isOpen = false;

    private void Start()
    {
        if (loginPanel != null)
        {
            loginPanel.SetActive(false);
        }

        if (backgroundDim != null)
        {
            backgroundDim.SetActive(false);
        }

        isOpen = false;
    }

    public void OpenMenu()
    {
        if (isOpen)
            return;

        isOpen = true;

        if (loginPanel != null)
        {
            loginPanel.SetActive(true);
        }

        if (backgroundDim != null)
        {
            backgroundDim.SetActive(true);
        }

        if (playerController != null)
        {
            playerController.SetPlayerControlEnabled(false);
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseMenu()
    {
        if (!isOpen)
            return;

        isOpen = false;

        if (loginPanel != null)
        {
            loginPanel.SetActive(false);
        }

        if (backgroundDim != null)
        {
            backgroundDim.SetActive(false);
        }

        if (playerController != null)
        {
            playerController.SetPlayerControlEnabled(true);
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ToggleMenu()
    {
        if (isOpen)
            CloseMenu();
        else
            OpenMenu();
    }

    public void OnClickSignIn()
    {
        Debug.Log("Sign In clicked");
    }

    public void OnClickLogin()
    {
        Debug.Log("Login clicked");
    }

    public void OnClickGuest()
    {
        Debug.Log("Guest clicked");
        CloseMenu();
    }

    public bool IsOpen()
    {
        return isOpen;
    }
}