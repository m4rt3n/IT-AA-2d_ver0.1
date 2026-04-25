/*
 * Datei: IInteractable.cs
 * Zweck: Definiert die gemeinsame Schnittstelle fuer Welt-Interaktionen.
 * Verantwortung: Beschreibt Prompt, Typ, Verfuegbarkeit und Ausloesung einer Interaktion.
 * Abhaengigkeiten: InteractionType, Unity Transform.
 * Verwendung: Wird von NPCs, Objekten, Terminals oder Tueren implementiert, die vom InteractionDetector erkannt werden sollen.
 */

using UnityEngine;

namespace ITAA.Features.Interaction
{
    public interface IInteractable
    {
        string InteractionPrompt { get; }
        InteractionType InteractionType { get; }

        bool CanInteract(Transform interactor);
        void Interact(Transform interactor);
    }
}
