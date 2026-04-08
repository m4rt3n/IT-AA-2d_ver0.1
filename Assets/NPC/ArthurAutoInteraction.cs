using UnityEngine;

public class ArthurAutoInteraction : MonoBehaviour
{
    #region Inspector

    [SerializeField] private float triggerDelay = 0.25f;

    #endregion

    #region Private Fields

    private MenuManager menuManager;
    private bool triggered;

    #endregion

    #region Unity Methods

    private void Start()
    {
        menuManager = FindAnyObjectByType<MenuManager>();
    }

    #endregion

    #region Public API

    public void TriggerAutoInteraction(GameObject player)
    {
        if (triggered)
        {
            Debug.Log("[ArthurAutoInteraction] Bereits ausgelöst.");
            return;
        }

        if (player == null)
        {
            Debug.LogWarning("[ArthurAutoInteraction] Player ist null.");
        }
        else
        {
            Debug.Log($"[ArthurAutoInteraction] Trigger durch: {player.name}");
        }

        triggered = true;
        Invoke(nameof(OpenMenu), triggerDelay);
    }

    #endregion

    #region Private Methods

    private void OpenMenu()
    {
        Debug.Log("[ArthurAutoInteraction] Öffne Login-Menü.");

        if (menuManager != null)
        {
            menuManager.ShowLoginMenu();
        }
        else
        {
            Debug.LogError("[ArthurAutoInteraction] MenuManager fehlt.");
        }
    }

    #endregion
}