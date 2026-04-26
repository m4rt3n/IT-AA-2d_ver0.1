/*
 * Datei: TestWorldInteractable.cs
 * Zweck: Macht einfache Testwelt-Objekte mit vorhandenen MVP-Systemen interagierbar.
 * Verantwortung: Delegiert Pickup, Terminal, Skill-XP und Achievement-Tests an bestehende Manager.
 * Abhaengigkeiten: IInteractable, RuntimeInventory, SkillRuntimeManager, AchievementManager, TerminalPanel.
 * Verwendung: Wird auf Testobjekte in GameScene gelegt; ersetzt keine spezialisierten spaeteren Adapter.
 */

using ITAA.Features.Achievements;
using ITAA.Features.Interaction;
using ITAA.Features.Inventory;
using ITAA.Features.Skills;
using ITAA.Features.Terminal;
using UnityEngine;

namespace ITAA.Gameplay.Interaction
{
    [DisallowMultipleComponent]
    public sealed class TestWorldInteractable : MonoBehaviour, IInteractable
    {
        private enum TestAction
        {
            PickupItem,
            OpenTerminal,
            UnlockAchievement,
            GrantSkillXp
        }

        [Header("Interaction")]
        [SerializeField] private string interactionPrompt = "Interagieren [E]";
        [SerializeField] private InteractionType interactionType = InteractionType.Custom;
        [SerializeField] private TestAction action = TestAction.PickupItem;
        [SerializeField] private bool disableAfterSuccessfulInteraction;

        [Header("Inventory")]
        [SerializeField] private InventoryItemData itemData = new InventoryItemData
        {
            ItemId = "diensthandy",
            DisplayName = "Diensthandy",
            Description = "Ein Testgeraet fuer IT-Support-Aufgaben.",
            Category = InventoryItemCategory.Tool,
            IsStackable = false,
            MaxStack = 1
        };
        [SerializeField] private int itemAmount = 1;

        [Header("Skills / Achievements")]
        [SerializeField] private string skillId = "networking";
        [SerializeField] private int xpAmount = 25;
        [SerializeField] private string achievementId = "network_beginner";

        [Header("Terminal")]
        [SerializeField] private TerminalPanel terminalPanel;

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs = true;

        public string InteractionPrompt => interactionPrompt;
        public InteractionType InteractionType => interactionType;

        public bool CanInteract(Transform interactor)
        {
            return isActiveAndEnabled;
        }

        public void Interact(Transform interactor)
        {
            bool success = action switch
            {
                TestAction.PickupItem => TryPickupItem(),
                TestAction.OpenTerminal => TryOpenTerminal(),
                TestAction.UnlockAchievement => TryUnlockAchievement(),
                TestAction.GrantSkillXp => TryGrantSkillXp(),
                _ => false
            };

            if (success && disableAfterSuccessfulInteraction)
            {
                gameObject.SetActive(false);
            }
        }

        private bool TryPickupItem()
        {
            RuntimeInventory inventory = FindAnyObjectByType<RuntimeInventory>(FindObjectsInactive.Include);
            if (inventory == null)
            {
                Debug.LogWarning($"[{nameof(TestWorldInteractable)}] RuntimeInventory fehlt.", this);
                return false;
            }

            bool added = inventory.AddItem(itemData, itemAmount);
            if (added)
            {
                UnlockAchievement(achievementId);
                Log($"Item aufgenommen: {itemData.DisplayName}");
            }

            return added;
        }

        private bool TryOpenTerminal()
        {
            if (terminalPanel == null)
            {
                terminalPanel = FindAnyObjectByType<TerminalPanel>(FindObjectsInactive.Include);
            }

            if (terminalPanel == null)
            {
                Debug.LogWarning($"[{nameof(TestWorldInteractable)}] TerminalPanel fehlt.", this);
                return false;
            }

            terminalPanel.Open();
            GrantSkillXp(skillId, xpAmount);
            Log("Terminal geoeffnet.");
            return true;
        }

        private bool TryUnlockAchievement()
        {
            return UnlockAchievement(achievementId);
        }

        private bool TryGrantSkillXp()
        {
            return GrantSkillXp(skillId, xpAmount);
        }

        private bool UnlockAchievement(string id)
        {
            AchievementManager achievementManager = FindAnyObjectByType<AchievementManager>(FindObjectsInactive.Include);
            if (achievementManager == null)
            {
                Debug.LogWarning($"[{nameof(TestWorldInteractable)}] AchievementManager fehlt.", this);
                return false;
            }

            achievementManager.UnlockAchievement(id);
            Log($"Achievement getriggert: {id}");
            return true;
        }

        private bool GrantSkillXp(string id, int amount)
        {
            SkillRuntimeManager skillManager = FindAnyObjectByType<SkillRuntimeManager>(FindObjectsInactive.Include);
            if (skillManager == null)
            {
                Debug.LogWarning($"[{nameof(TestWorldInteractable)}] SkillRuntimeManager fehlt.", this);
                return false;
            }

            skillManager.AddXp(id, amount);
            Log($"Skill-XP vergeben: {id} +{amount}");
            return true;
        }

        private void Log(string message)
        {
            if (!enableDebugLogs)
            {
                return;
            }

            Debug.Log($"[GameScene] {message}", this);
        }
    }
}
