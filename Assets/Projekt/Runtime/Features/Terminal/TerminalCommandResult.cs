/*
 * Datei: TerminalCommandResult.cs
 * Zweck: Beschreibt das Ergebnis eines simulierten Terminal-Befehls.
 * Verantwortung: Kapselt Ausgabetext und UI-Aktionen wie Clear oder Exit.
 * Abhaengigkeiten: TerminalCommand.
 * Verwendung: TerminalEmulator erzeugt Resultate, TerminalPanel rendert sie und reagiert auf Clear/Exit.
 */

using UnityEngine;

namespace ITAA.Features.Terminal
{
    [global::System.Serializable]
    public class TerminalCommandResult
    {
        [SerializeField] private TerminalCommand command;
        [SerializeField] private string output;
        [SerializeField] private bool isKnownCommand;
        [SerializeField] private bool shouldClearOutput;
        [SerializeField] private bool shouldClosePanel;

        public TerminalCommand Command => command;
        public string Output => output;
        public bool IsKnownCommand => isKnownCommand;
        public bool ShouldClearOutput => shouldClearOutput;
        public bool ShouldClosePanel => shouldClosePanel;

        public TerminalCommandResult(
            TerminalCommand command,
            string output,
            bool isKnownCommand,
            bool shouldClearOutput = false,
            bool shouldClosePanel = false)
        {
            this.command = command;
            this.output = output ?? string.Empty;
            this.isKnownCommand = isKnownCommand;
            this.shouldClearOutput = shouldClearOutput;
            this.shouldClosePanel = shouldClosePanel;
        }
    }
}
