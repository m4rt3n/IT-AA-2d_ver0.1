/*
 * Datei: DummySaveBootstrap.cs
 * Zweck:
 * - Erstellt beim Start einmalig einen Test-Spielstand
 * - Praktisch für Arthur -> LoadGamePanel -> Laden Workflow
 */

using UnityEngine;

namespace ITAA.System.Savegame
{
    public sealed class DummySaveBootstrap : MonoBehaviour
    {
        [SerializeField] private bool createDummySaveOnStart = true;

        private void Awake()
        {
            if (!createDummySaveOnStart)
            {
                return;
            }

            SaveSystem saveSystem = new SaveSystem();
            saveSystem.EnsureDummySaveExists();
        }
    }
}