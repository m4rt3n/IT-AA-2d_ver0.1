/*
 * Datei: TerminalCommandType.cs
 * Zweck: Definiert die unterstuetzten simulierten Terminal-Befehle.
 * Verantwortung: Stellt eine stabile Befehlsauswahl fuer Parser, Emulator und UI bereit.
 * Abhaengigkeiten: Keine.
 * Verwendung: Wird von TerminalCommand und TerminalEmulator genutzt, um Eingaben ohne echte OS- oder Netzwerkzugriffe einzuordnen.
 */

namespace ITAA.Features.Terminal
{
    public enum TerminalCommandType
    {
        Unknown = 0,
        Help = 1,
        IpConfig = 2,
        Ping = 3,
        NsLookup = 4,
        Clear = 5,
        Exit = 6
    }
}
