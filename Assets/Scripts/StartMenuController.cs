using UnityEngine;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] private GameObject loginPanel;

    private void Start()
    {
        if (loginPanel != null)
        {
            loginPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            OpenMenu();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            CloseMenu();
        }
    }

    public void OpenMenu()
    {
        if (loginPanel != null)
        {
            loginPanel.SetActive(true);
        }
    }

    public void CloseMenu()
    {
        if (loginPanel != null)
        {
            loginPanel.SetActive(false);
        }
    }
}