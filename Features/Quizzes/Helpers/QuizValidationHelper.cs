using Quiz.Features.Quizzes.Models;
using static Quiz.Features.Quizzes.Models.QuizApiModels;

namespace Quiz.Features.Quizzes.Helpers
{
    public class QuizValidationHelper
    {
        public static List<string> ValidateAnswers(CreateFormDto quizForm, QuizAnswerManager answerManager)
        {
            var errors = new List<string>();

            if (quizForm.Questions == null) return errors;

            foreach (var question in quizForm.Questions.Where(q => q.IsRequired))
            {
                var hasValidAnswer = question.Type switch
                {
                    "ShortAnswer" or "Paragraph" => !string.IsNullOrWhiteSpace(answerManager.GetTextAnswer(question.Id)),
                    "MultipleChoice" => ValidateMultipleChoiceAnswer(question, answerManager),
                    "Checkboxes" => ValidateCheckboxAnswer(question, answerManager),
                    _ => false
                };

                if (!hasValidAnswer)
                {
                    errors.Add($"Question {question.Position + 1}");
                }
            }

            return errors;
        }

        private static bool ValidateMultipleChoiceAnswer(QuestionDto question, QuizAnswerManager answerManager)
        {
            var answer = answerManager.GetRadioAnswer(question.Id);
            if (string.IsNullOrEmpty(answer)) return false;
            
            if (answer == "Other" && question.HasOtherOption)
            {
                return !string.IsNullOrWhiteSpace(answerManager.GetCustomOtherAnswer(question.Id));
            }
            
            return true;
        }

        private static bool ValidateCheckboxAnswer(QuestionDto question, QuizAnswerManager answerManager)
        {
            var hasSelectedOptions = false;
            if (question.Options != null)
            {
                hasSelectedOptions = question.Options.Any(option => answerManager.IsCheckboxSelected(question.Id, option));
            }
            
            if (!hasSelectedOptions) return false;
            
            if (answerManager.IsCheckboxSelected(question.Id, "Other") && question.HasOtherOption)
            {
                return !string.IsNullOrWhiteSpace(answerManager.GetCustomOtherAnswer(question.Id));
            }
            
            return true;
        }
    }
}