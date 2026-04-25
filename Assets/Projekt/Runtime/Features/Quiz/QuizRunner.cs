/*
 * Datei: QuizRunner.cs
 * Zweck: Fuehrt ein QuizSet zustandsbasiert durch.
 * Verantwortung: Verwaltet aktuelle Frage, Multiple-Choice- und Freitext-Antwortpruefung sowie Fortschritt im Quiz.
 * Abhaengigkeiten: QuizSet, QuizQuestion, QuizResult, QuizTextAnswerEvaluator.
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

        public QuizResult AnswerCurrentQuestion(string textAnswer)
        {
            QuizQuestion question = GetCurrentQuestion();
            bool isCorrect = false;

            if (question != null && question.HasAcceptedTextAnswers())
            {
                for (int i = 0; i < question.AcceptedTextAnswers.Count; i++)
                {
                    string acceptedAnswer = question.AcceptedTextAnswers[i];

                    if (QuizTextAnswerEvaluator.IsAnswerAccepted(
                            textAnswer,
                            acceptedAnswer,
                            question.AllowFuzzyTextMatch,
                            question.MaxTextAnswerDistance))
                    {
                        isCorrect = true;
                        break;
                    }
                }
            }

            return new QuizResult(question, -1, textAnswer, isCorrect);
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
