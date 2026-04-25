/*
 * Datei: InteractionType.cs
 * Zweck: Beschreibt die Art einer Welt-Interaktion.
 * Verantwortung: Ermoeglicht UI, Logik und spaeteren Systemen eine einfache Einordnung interaktiver Ziele.
 * Abhaengigkeiten: Keine.
 * Verwendung: Wird von IInteractable-Implementierungen und InteractionPromptView genutzt.
 */

namespace ITAA.Features.Interaction
{
    public enum InteractionType
    {
        Talk,
        Quiz,
        Door,
        Terminal,
        Pickup,
        Hint,
        Custom
    }
}
