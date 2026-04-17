/*
 * Datei: NPCInteraction.cs
 * Zweck: Öffnet das Menü nur dann, wenn der Spieler mit dem NPC interagiert.
 * Verantwortung:
 *   - Erkennt, ob der Spieler in Reichweite ist
 *   - Wartet auf die Interaktionstaste
 *   - Öffnet dann das Startmenü über den MenuManager
 *
 * Abhängigkeiten:
 *   - MenuManager
 *   - Collider2D als Trigger
 *   - Player-Tag auf dem Spieler
 */

using ITAA.UI.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ITAA.Features.NPC.Interactions
{
    public class NPCInteraction : MonoBehaviour
    {
        #region Inspector

        [Header("Interaction")]
        [SerializeField] private Key interactKey = Key.E;

        [Header("References")]
        [SerializeField] private MenuManager menuManager;

        #endregion

        #region Fields

        private bool playerInRange;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (menuManager == null)
            {
                menuManager = FindAnyObjectByType<MenuManager>();
            }

            Debug.Log($"[{nameof(NPCInteraction)}] Awake auf '{gameObject.name}'. MenuManager gefunden: {menuManager != null}");
        }

        private void Update()
        {
            if (!playerInRange)
            {
                return;
            }

            if (menuManager == null)
            {
                Debug.LogWarning($"[{nameof(NPCInteraction)}] Kein MenuManager gefunden.");
                return;
            }

            if (menuManager.IsOpen)
            {
                return;
            }

            if (Keyboard.current != null && Keyboard.current[interactKey].wasPressedThisFrame)
            {
                Debug.Log($"[{nameof(NPCInteraction)}] Interaktionstaste '{interactKey}' gedrückt. Öffne Startmenü.");
                menuManager.ShowStartMenu();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log($"[{nameof(NPCInteraction)}] Trigger Enter mit: {other.name}, Tag: {other.tag}");

            if (other.CompareTag("Player"))
            {
                playerInRange = true;
                Debug.Log($"[{nameof(NPCInteraction)}] Spieler ist in Reichweite.");
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Debug.Log($"[{nameof(NPCInteraction)}] Trigger Exit mit: {other.name}, Tag: {other.tag}");

            if (other.CompareTag("Player"))
            {
                playerInRange = false;
                Debug.Log($"[{nameof(NPCInteraction)}] Spieler hat Reichweite verlassen.");
            }
        }

        #endregion
    }
}