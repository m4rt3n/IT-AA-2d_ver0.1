/*
 * Datei: NpcRoutineStepType.cs
 * Zweck: Definiert die unterstuetzten Schrittarten fuer einfache NPC-Routinen.
 * Verantwortung: Typisiert Warte-, Blickrichtungs- und Bewegungs-Schritte fuer den NpcRoutineController.
 * Abhaengigkeiten: Keine.
 * Verwendung: Wird von NpcRoutineStep genutzt, um das Verhalten eines Routine-Schritts festzulegen.
 */

namespace ITAA.NPC.Routines
{
    public enum NpcRoutineStepType
    {
        Wait = 0,
        LookDirection = 1,
        MoveToPoint = 2
    }
}
