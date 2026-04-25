/*
 * Datei: QuizAnswerOption.cs
 * Zweck: Beschreibt eine einzelne Antwortmoeglichkeit fuer eine Quizfrage.
 * Verantwortung: Haelt den anzeigbaren Antworttext als serialisierbares Datenmodell.
 * Abhaengigkeiten: System.Serializable.
 * Verwendung: Wird von QuizQuestion genutzt, damit Antwortoptionen datengetrieben gepflegt werden koennen.
 */

using System;

namespace ITAA.Quiz
{
    [Serializable]
    public class QuizAnswerOption
    {
        public string Text;
    }
}
