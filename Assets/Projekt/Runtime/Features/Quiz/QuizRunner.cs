/*
 * Datei: QuizRunner.cs
 * Zweck: Fuehrt ein QuizSet zustandsbasiert durch.
 * Verantwortung: Verwaltet aktuelle Frage, Antwortpruefung und Fortschritt im Quiz.
 * Abhaengigkeiten: QuizSet, QuizQuestion, QuizResult.
 * Verwendung: Wird von QuizPanel genutzt, damit UI und Quizlogik getrennt bleiben.
 */

namespace ITAA.Quiz
{
    public class QuizRunner
    {
        private readonly QuizSet quizSet;
        private int currentQuestionIndex;

        public QuizRunner(QuizSet quizSet)
        {
            this.quizSet = quizSet;
            currentQuestionIndex = 0;
        }

        public int CurrentQuestionIndex => currentQuestionIndex;
        public int QuestionCount => quizSet != null && quizSet.Questions != null ? quizSet.Questions.Count : 0;
        public bool HasQuestions => quizSet != null && quizSet.HasQuestions();

        public QuizQuestion GetCurrentQuestion()
        {
            if (!HasQuestions || currentQuestionIndex < 0 || currentQuestionIndex >= QuestionCount)
            {
                return null;
            }

            return quizSet.Questions[currentQuestionIndex];
        }

        public QuizResult AnswerCurrentQuestion(int selectedAnswerIndex)
        {
            QuizQuestion question = GetCurrentQuestion();
            bool isCorrect = question != null &&
                             question.HasValidAnswerIndex() &&
                             selectedAnswerIndex == question.CorrectAnswerIndex;

            return new QuizResult(question, selectedAnswerIndex, isCorrect);
        }

        public bool MoveNext()
        {
            if (!HasQuestions)
            {
                return false;
            }

            if (currentQuestionIndex + 1 >= QuestionCount)
            {
                return false;
            }

            currentQuestionIndex++;
            return true;
        }
    }
}
