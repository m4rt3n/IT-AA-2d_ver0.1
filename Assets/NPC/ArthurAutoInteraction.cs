using UnityEngine;

public class ArthurAutoInteraction : MonoBehaviour
{
    [SerializeField] private float triggerDelay = 0.25f;

    private MenuManager menuManager;
    private bool triggered;

    private void Start()
    {
        menuManager = FindAnyObjectByType<MenuManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered || !other.CompareTag("Player"))
        {
            return;
        }

        triggered = true;
        Invoke(nameof(OpenMenu), triggerDelay);
    }

    private void OpenMenu()
    {
        Debug.Log("[ArthurAutoInteraction] Öffne Login-Menü.");

        if (menuManager != null)
        {
            menuManager.ShowLoginMenu();
        }
    }
}