/*
 * Datei: TerminalEmulator.cs
 * Zweck: Fuehrt simulierte Terminal-Befehle fuer IT-Minispiele aus.
 * Verantwortung: Parst Eingaben, liefert deterministische Lern-Ausgaben und vermeidet echte OS- oder Netzwerkzugriffe.
 * Abhaengigkeiten: TerminalCommand, TerminalCommandResult, TerminalCommandType.
 * Verwendung: Wird vom TerminalPanel oder spaeteren Szenario-Adaptern genutzt, um Befehle rein lokal zu simulieren.
 */

namespace ITAA.Features.Terminal
{
    public class TerminalEmulator
    {
        private const string DefaultPingTarget = "8.8.8.8";
        private const string DefaultLookupTarget = "example.local";

        public TerminalCommandResult Execute(string rawInput)
        {
            return Execute(TerminalCommand.Parse(rawInput));
        }

        public TerminalCommandResult Execute(TerminalCommand command)
        {
            if (command == null)
            {
                return Unknown(null, string.Empty);
            }

            switch (command.CommandType)
            {
                case TerminalCommandType.Help:
                    return Known(command, BuildHelpOutput());
                case TerminalCommandType.IpConfig:
                    return Known(command, BuildIpConfigOutput());
                case TerminalCommandType.Ping:
                    return Known(command, BuildPingOutput(command.Argument));
                case TerminalCommandType.NsLookup:
                    return Known(command, BuildNsLookupOutput(command.Argument));
                case TerminalCommandType.Clear:
                    return new TerminalCommandResult(command, string.Empty, true, true, false);
                case TerminalCommandType.Exit:
                    return new TerminalCommandResult(command, "Terminal geschlossen.", true, false, true);
                default:
                    return Unknown(command, command.CommandName);
            }
        }

        private static TerminalCommandResult Known(TerminalCommand command, string output)
        {
            return new TerminalCommandResult(command, output, true);
        }

        private static TerminalCommandResult Unknown(TerminalCommand command, string commandName)
        {
            if (string.IsNullOrEmpty(commandName))
            {
                return new TerminalCommandResult(command, "Gib help ein, um verfuegbare Befehle zu sehen.", false);
            }

            return new TerminalCommandResult(
                command,
                $"'{commandName}' ist kein bekannter simulierter Befehl. Nutze help.",
                false);
        }

        private static string BuildHelpOutput()
        {
            return "Verfuegbare Befehle:\n" +
                   "  help              Zeigt diese Hilfe\n" +
                   "  ipconfig          Zeigt simulierte IP-Konfiguration\n" +
                   "  ping <ziel>       Prueft simulierte Erreichbarkeit\n" +
                   "  nslookup <name>   Prueft simulierte DNS-Aufloesung\n" +
                   "  clear             Leert die Ausgabe\n" +
                   "  exit              Schliesst das Terminal";
        }

        private static string BuildIpConfigOutput()
        {
            return "Windows-IP-Konfiguration\n\n" +
                   "Ethernet-Adapter Support-LAN:\n" +
                   "   IPv4 Address. . . . . . . . . . . : 192.168.1.42\n" +
                   "   Subnet Mask . . . . . . . . . . . : 255.255.255.0\n" +
                   "   Default Gateway . . . . . . . . . : 192.168.1.1";
        }

        private static string BuildPingOutput(string argument)
        {
            string target = string.IsNullOrEmpty(argument) ? DefaultPingTarget : argument.Trim();

            if (IsUnreachableTarget(target))
            {
                return $"Pinging {target} with 32 bytes of data:\n" +
                       "Request timed out.\n" +
                       "Request timed out.\n\n" +
                       $"Ping statistics for {target}:\n" +
                       "    Packets: Sent = 2, Received = 0, Lost = 2 (100% loss)";
            }

            return $"Pinging {target} with 32 bytes of data:\n" +
                   $"Reply from {target}: bytes=32 time=12ms TTL=57\n" +
                   $"Reply from {target}: bytes=32 time=11ms TTL=57\n\n" +
                   $"Ping statistics for {target}:\n" +
                   "    Packets: Sent = 2, Received = 2, Lost = 0 (0% loss)";
        }

        private static string BuildNsLookupOutput(string argument)
        {
            string target = string.IsNullOrEmpty(argument) ? DefaultLookupTarget : argument.Trim();

            if (IsUnknownDnsTarget(target))
            {
                return "Server:  router.local\n" +
                       "Address: 192.168.1.1\n\n" +
                       $"*** router.local can't find {target}: Non-existent domain";
            }

            return "Server:  router.local\n" +
                   "Address: 192.168.1.1\n\n" +
                   $"Name:    {target}\n" +
                   "Address: 192.168.1.10";
        }

        private static bool IsUnreachableTarget(string target)
        {
            string normalizedTarget = (target ?? string.Empty).ToLowerInvariant();

            return normalizedTarget.Contains("timeout") ||
                   normalizedTarget.Contains("offline") ||
                   normalizedTarget == "0.0.0.0";
        }

        private static bool IsUnknownDnsTarget(string target)
        {
            string normalizedTarget = (target ?? string.Empty).ToLowerInvariant();

            return normalizedTarget.Contains("unknown") ||
                   normalizedTarget.Contains("invalid");
        }
    }
}
