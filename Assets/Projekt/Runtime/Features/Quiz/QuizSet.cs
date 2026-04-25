/*
 * Datei: QuizSet.cs
 * Zweck: Definiert ein wiederverwendbares Quiz-Set als Unity-Datenasset.
 * Verantwortung: Buendelt Quiz-ID, Anzeigename und eine Liste serialisierter Fragen.
 * Abhaengigkeiten: QuizQuestion, ScriptableObject.
 * Verwendung: Wird von NPCs wie Bernd referenziert, damit Quizfragen nicht im UI-Code liegen.
 */

using System.Collections.Generic;
using UnityEngine;

namespace ITAA.Quiz
{
    [CreateAssetMenu(fileName = "QuizSet", menuName = "IT-AA/Quiz/Quiz Set")]
    public class QuizSet : ScriptableObject
    {
        public string QuizId;
        public string DisplayName;
        public List<QuizQuestion> Questions = new();

        public bool HasQuestions()
        {
            return Questions != null && Questions.Count > 0;
        }
    }
}
