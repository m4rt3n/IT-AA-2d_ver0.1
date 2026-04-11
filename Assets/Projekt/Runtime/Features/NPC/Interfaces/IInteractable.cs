/*
 * Datei: IInteractable.cs
 * Zweck: Definiert eine einheitliche Schnittstelle für interagierbare Objekte.
 * Verantwortung: Erzwingt eine Interact-Methode für NPCs oder andere Interaktionsziele.
 * Abhängigkeiten: Keine.
 * Verwendet von: NPCInteraction und späteren Interaktionssystemen.
 */
 namespace ITAA.NPC.Interfaces
{
    public interface IInteractable
    {
        void Interact();
    }
}