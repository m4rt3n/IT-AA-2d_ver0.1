/*
 * Datei: DialogueSequence.cs
 * Zweck: Definiert eine wiederverwendbare Liste von Dialogzeilen als Datenasset.
 * Verantwortung: Buendelt Sequence-ID, Anzeigename und serialisierte DialogueLines.
 * Abhaengigkeiten: DialogueLine, ScriptableObject, System.Collections.Generic.
 * Verwendung: Wird von DialogueManager oder IDialogueTrigger-Implementierungen referenziert.
 */

using System.Collections.Generic;
using UnityEngine;

namespace ITAA.Features.Dialogue
{
    [CreateAssetMenu(fileName = "DialogueSequence", menuName = "IT-AA/Dialogue/Dialogue Sequence")]
    public class DialogueSequence : ScriptableObject
    {
        public string SequenceId;
        public string DisplayName;
        public List<DialogueLine> Lines = new();

        public bool HasLines()
        {
            return Lines != null && Lines.Count > 0;
        }

        public DialogueLine GetLineAt(int index)
        {
            if (Lines == null || index < 0 || index >= Lines.Count)
            {
                return null;
            }

            return Lines[index];
        }
    }
}
