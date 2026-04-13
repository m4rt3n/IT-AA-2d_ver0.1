/*
 * Datei: NPCInteraction.cs
 * Zweck: Ermöglicht die direkte Interaktion eines NPCs mit dem Spieler per Taste.
 * Verantwortung: Erkennt den Spieler im Triggerbereich und öffnet bei Interaktion das Startmenü.
 * Abhängigkeiten: IInteractable, MenuManager, 2D-Trigger-Collider.
 * Verwendet von: Interagierbare NPCs in der Szene.
 */
// Datei: Assets/Projekt/Runtime/Features/NPC/Interactions/NPCInteraction.cs

using ITAA.NPC.Interfaces;
using ITAA.UI.Managers;
using UnityEngine;

namespace ITAA.NPC.Interactions
{
    public class NPCInteraction : MonoBehaviour, IInteractable
    {
        #region Inspector

        [Header("Interaction")]
        [SerializeField] private KeyCode interactKey = KeyCode.E;
        [SerializeField] private string playerTag = "Player";

        [Header("References")]
        [SerializeField] private MenuManager menuManager;

        #endregion

        #region Private Fields

        private bool playerInRange;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (menuManager == null)
            {
                menuManager = FindAnyObjectByType<MenuManager>();
            }
        }

        private void Update()
        {
            if (playerInRange && Input.GetKeyDown(interactKey))
            {
                Interact();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(playerTag))
            {
                playerInRange = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(playerTag))
            {
                playerInRange = false;
            }
        }

        #endregion

        #region Public Methods

        public void Interact()
        {
            if (menuManager == null)
            {
                Debug.LogWarning($"[{nameof(NPCInteraction)}] Kein {nameof(MenuManager)} gefunden.");
                return;
            }

            menuManager.ShowStartMenu();
        }

        #endregion
    }
}