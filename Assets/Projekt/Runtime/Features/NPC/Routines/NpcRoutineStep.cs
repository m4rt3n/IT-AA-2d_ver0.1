/*
 * Datei: NpcRoutineStep.cs
 * Zweck: Beschreibt einen einzelnen Schritt einer einfachen NPC-Routine.
 * Verantwortung: Haelt serialisierbare Daten fuer Schrittart, Zielpunkt, Dauer, Tempo und Blickrichtung.
 * Abhaengigkeiten: NpcRoutineStepType, Unity Transform und Vector2.
 * Verwendung: Wird im Inspector als Element der NpcRoutineController-Schrittliste konfiguriert.
 */

using System;
using UnityEngine;

namespace ITAA.NPC.Routines
{
    [Serializable]
    public class NpcRoutineStep
    {
        [Header("Step")]
        public NpcRoutineStepType StepType = NpcRoutineStepType.Wait;
        public string StepName = "Routine Step";

        [Header("Timing")]
        [Min(0f)] public float DurationSeconds = 1f;

        [Header("Movement")]
        public Transform TargetPoint;
        [Min(0.01f)] public float MoveSpeed = 1.5f;
        [Min(0.01f)] public float StopDistance = 0.05f;

        [Header("Look")]
        public Vector2 LookDirection = Vector2.down;

        public bool HasTargetPoint()
        {
            return TargetPoint != null;
        }

        public Vector2 GetSafeLookDirection(Vector2 fallbackDirection)
        {
            if (LookDirection.sqrMagnitude <= 0.0001f)
            {
                return fallbackDirection.sqrMagnitude > 0.0001f
                    ? fallbackDirection.normalized
                    : Vector2.down;
            }

            return LookDirection.normalized;
        }
    }
}
