/*
 * Datei: TopicProgress.cs
 * Zweck: Speichert Quiz-Fortschritt fuer ein einzelnes Thema.
 * Verantwortung: Haelt Thema, Antwortanzahl, richtige Antworten und berechnet die Trefferquote.
 * Abhaengigkeiten: System.Serializable, UnityEngine.Mathf.
 * Verwendung: Wird von ProgressProfile fuer themenbezogene Quiz-Statistiken genutzt.
 */

using System;
using UnityEngine;

namespace ITAA.Features.Progress
{
    [Serializable]
    public class TopicProgress
    {
        public string Topic;
        public int Answers;
        public int CorrectAnswers;

        public TopicProgress()
        {
        }

        public TopicProgress(string topic)
        {
            Topic = topic;
        }

        public float GetAccuracy01()
        {
            return Answers <= 0 ? 0f : Mathf.Clamp01((float)CorrectAnswers / Answers);
        }
    }
}
