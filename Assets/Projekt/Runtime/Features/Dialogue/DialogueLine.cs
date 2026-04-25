/*
 * Datei: DialogueLine.cs
 * Zweck: Beschreibt eine einzelne Zeile innerhalb einer Dialogsequenz.
 * Verantwortung: Haelt Sprecher, Text und optionale Integrations-IDs fuer spaetere Portrait-/Audio-Erweiterungen.
 * Abhaengigkeiten: System.Serializable.
 * Verwendung: Wird in DialogueSequence serialisiert und vom DialogueManager an das DialoguePanel uebergeben.
 */

using System;

namespace ITAA.Features.Dialogue
{
    [Serializable]
    public class DialogueLine
    {
        public string SpeakerName;
        public string Text;
        public string PortraitId;
        public string AudioCueId;

        public DialogueLine()
        {
        }

        public DialogueLine(string speakerName, string text)
        {
            SpeakerName = speakerName;
            Text = text;
        }

        public bool HasText()
        {
            return !string.IsNullOrWhiteSpace(Text);
        }
    }
}
