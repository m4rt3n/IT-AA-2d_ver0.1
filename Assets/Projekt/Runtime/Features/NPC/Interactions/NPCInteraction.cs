/*
 * Datei: NPCInteraction.cs
 * Zweck: Ermöglicht die direkte Interaktion eines NPCs mit dem Spieler per Taste.
 * Verantwortung: Erkennt den Spieler im Triggerbereich und öffnet bei Interaktion das Startmenü.
 * Abhängigkeiten: IInteractable, MenuManager, 2D-Trigger-Collider.
 * Verwendet von: Interagierbare NPCs in der Szene.
 */
 using ITAA.NPC.Interfaces;
using ITAA.UI.Managers;
using UnityEngine;

namespace ITAA.NPC.Interactions
{
    public class NPCInteraction : MonoBehaviour, IInteractable
    {
        [Header("Interaction")]
        [SerializeField] private KeyCode interactKey = KeyCode.E;
        [SerializeField] private string playerTag = "Player";

        [Header("References")]
        [SerializeField] private MenuManager menuManager;

        private bool playerInRange;

        private void Start()
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

        public void Interact()
        {
            if (menuManager != null)
            {
                menuManager.ShowStartMenu();
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
    }
}