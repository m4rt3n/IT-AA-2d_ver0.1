/*
 * Datei: QuestProgress.cs
 * Zweck: Speichert den Laufzeitfortschritt einer einzelnen Quest.
 * Verantwortung: Verwaltet Quest-ID, aktuellen Wert, Zielwert und Abschlusszustand.
 * Abhaengigkeiten: QuestDefinition, System.Serializable.
 * Verwendung: Wird innerhalb von ProgressProfile gespeichert und vom ProgressManager aktualisiert.
 */

using System;
using UnityEngine;

namespace ITAA.Features.Progress
{
    [Serializable]
    public class QuestProgress
    {
        public string QuestId;
        public int CurrentValue;
        public int TargetValue = 1;
        public bool IsCompleted;

        public QuestProgress()
        {
        }

        public QuestProgress(QuestDefinition definition)
        {
            QuestId = definition != null ? definition.QuestId : string.Empty;
            TargetValue = definition != null ? definition.GetSafeTargetValue() : 1;
            CurrentValue = 0;
            IsCompleted = false;
        }

        public float GetProgress01()
        {
            return Mathf.Clamp01(TargetValue <= 0 ? 0f : (float)CurrentValue / TargetValue);
        }

        public void AddProgress(int amount)
        {
            if (IsCompleted)
            {
                return;
            }

            CurrentValue = Mathf.Clamp(CurrentValue + Mathf.Max(0, amount), 0, Mathf.Max(1, TargetValue));

            if (CurrentValue >= TargetValue)
            {
                IsCompleted = true;
            }
        }

        public void Complete()
        {
            CurrentValue = Mathf.Max(CurrentValue, TargetValue);
            IsCompleted = true;
        }
    }
}
