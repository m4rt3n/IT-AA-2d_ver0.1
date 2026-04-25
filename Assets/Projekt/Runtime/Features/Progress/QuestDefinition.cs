/*
 * Datei: QuestDefinition.cs
 * Zweck: Beschreibt eine einzelne Aufgabe im Lernfortschrittssystem.
 * Verantwortung: Haelt Quest-ID, Titel, Beschreibung, Thema und Zielwert als datengetriebenes Unity-Asset.
 * Abhaengigkeiten: ScriptableObject.
 * Verwendung: Wird vom ProgressManager genutzt, um QuestProgress-Eintraege zu starten und auszuwerten.
 */

using UnityEngine;

namespace ITAA.Features.Progress
{
    [CreateAssetMenu(fileName = "QuestDefinition", menuName = "IT-AA/Progress/Quest Definition")]
    public class QuestDefinition : ScriptableObject
    {
        public string QuestId;
        public string Title;
        public string Description;
        public string Topic;
        public int TargetValue = 1;

        public bool HasValidId()
        {
            return !string.IsNullOrWhiteSpace(QuestId);
        }

        public int GetSafeTargetValue()
        {
            return Mathf.Max(1, TargetValue);
        }
    }
}
