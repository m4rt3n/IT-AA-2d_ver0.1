/*
 * Datei: TerminalCommand.cs
 * Zweck: Beschreibt eine geparste Terminal-Eingabe.
 * Verantwortung: Trennt Rohtext, Befehlsname, Argument und simulierten Befehlstyp.
 * Abhaengigkeiten: TerminalCommandType.
 * Verwendung: TerminalPanel uebergibt Nutzereingaben an TerminalEmulator, der daraus TerminalCommand-Instanzen erzeugt.
 */

using UnityEngine;

namespace ITAA.Features.Terminal
{
    [global::System.Serializable]
    public class TerminalCommand
    {
        [SerializeField] private string rawInput;
        [SerializeField] private string commandName;
        [SerializeField] private string argument;
        [SerializeField] private TerminalCommandType commandType;

        public string RawInput => rawInput;
        public string CommandName => commandName;
        public string Argument => argument;
        public TerminalCommandType CommandType => commandType;

        public TerminalCommand(string rawInput, string commandName, string argument, TerminalCommandType commandType)
        {
            this.rawInput = rawInput ?? string.Empty;
            this.commandName = commandName ?? string.Empty;
            this.argument = argument ?? string.Empty;
            this.commandType = commandType;
        }

        public static TerminalCommand Parse(string input)
        {
            string normalizedInput = (input ?? string.Empty).Trim();

            if (string.IsNullOrEmpty(normalizedInput))
            {
                return new TerminalCommand(string.Empty, string.Empty, string.Empty, TerminalCommandType.Unknown);
            }

            int firstSpaceIndex = normalizedInput.IndexOf(' ');
            string command = firstSpaceIndex < 0
                ? normalizedInput
                : normalizedInput.Substring(0, firstSpaceIndex);
            string remainingArgument = firstSpaceIndex < 0
                ? string.Empty
                : normalizedInput.Substring(firstSpaceIndex + 1).Trim();

            return new TerminalCommand(
                normalizedInput,
                command,
                remainingArgument,
                ResolveType(command));
        }

        private static TerminalCommandType ResolveType(string command)
        {
            string normalizedCommand = (command ?? string.Empty).Trim().ToLowerInvariant();

            switch (normalizedCommand)
            {
                case "help":
                    return TerminalCommandType.Help;
                case "ipconfig":
                    return TerminalCommandType.IpConfig;
                case "ping":
                    return TerminalCommandType.Ping;
                case "nslookup":
                    return TerminalCommandType.NsLookup;
                case "clear":
                    return TerminalCommandType.Clear;
                case "exit":
                    return TerminalCommandType.Exit;
                default:
                    return TerminalCommandType.Unknown;
            }
        }
    }
}
