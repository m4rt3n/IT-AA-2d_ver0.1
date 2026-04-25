/*
 * Datei: IDialogueTrigger.cs
 * Zweck: Definiert eine schlanke Schnittstelle fuer Objekte, die Dialoge starten koennen.
 * Verantwortung: Liefert eine DialogueSequence und optional eine Methode zum Ausloesen des Dialogstarts.
 * Abhaengigkeiten: DialogueSequence, Unity Transform.
 * Verwendung: Kann spaeter von NPC-Adaptern fuer Arthur, Bernd oder Szenario-Objekten implementiert werden.
 */

using UnityEngine;

namespace ITAA.Features.Dialogue
{
    public interface IDialogueTrigger
    {
        DialogueSequence DialogueSequence { get; }

        bool CanStartDialogue(Transform interactor);
        void StartDialogue(Transform interactor);
    }
}
