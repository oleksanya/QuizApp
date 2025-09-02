using Quiz.Features.Quizzes.Models;
using static Quiz.Features.Quizzes.Models.QuizApiModels;

namespace Quiz.Features.Quizzes.Helpers
{
    public class QuizAnswerManager
    {
        private readonly Dictionary<string, object?> _userAnswers = new();
        private readonly Dictionary<string, string> _customOtherAnswers = new();

        public void InitializeAnswers(List<QuestionDto> questions)
        {
            _userAnswers.Clear();
            _customOtherAnswers.Clear();
            
            foreach (var question in questions)
            {
                _userAnswers[question.Id] = question.Type switch
                {
                    "Checkboxes" => new List<string>(),
                    _ => string.Empty
                };
                
                if (question.HasOtherOption)
                {
                    _customOtherAnswers[question.Id] = string.Empty;
                }
            }
        }

        public void UpdateTextAnswer(string questionId, string value)
        {
            _userAnswers[questionId] = value;
        }

        public void UpdateRadioAnswer(string questionId, string value)
        {
            _userAnswers[questionId] = value;
            
            if (value != "Other" && _customOtherAnswers.ContainsKey(questionId))
            {
                _customOtherAnswers[questionId] = string.Empty;
            }
        }

        public void UpdateCheckboxAnswer(string questionId, string option, bool isChecked)
        {
            if (_userAnswers[questionId] is not List<string> selectedOptions)
            {
                selectedOptions = new List<string>();
                _userAnswers[questionId] = selectedOptions;
            }

            if (isChecked && !selectedOptions.Contains(option))
            {
                selectedOptions.Add(option);
            }
            else if (!isChecked && selectedOptions.Contains(option))
            {
                selectedOptions.Remove(option);
                
                if (option == "Other" && _customOtherAnswers.ContainsKey(questionId))
                {
                    _customOtherAnswers[questionId] = string.Empty;
                }
            }
        }

        public void UpdateCustomOtherAnswer(string questionId, string value)
        {
            _customOtherAnswers[questionId] = value;
        }

        public string GetTextAnswer(string questionId)
        {
            return _userAnswers.TryGetValue(questionId, out var answer) ? answer?.ToString() ?? string.Empty : string.Empty;
        }

        public string GetRadioAnswer(string questionId)
        {
            return _userAnswers.TryGetValue(questionId, out var answer) ? answer?.ToString() ?? string.Empty : string.Empty;
        }

        public bool IsCheckboxSelected(string questionId, string option)
        {
            return _userAnswers.TryGetValue(questionId, out var answer) && 
                   answer is List<string> selectedOptions && 
                   selectedOptions.Contains(option);
        }

        public string GetCustomOtherAnswer(string questionId)
        {
            return _customOtherAnswers.TryGetValue(questionId, out var customAnswer) ? customAnswer : string.Empty;
        }

        public bool IsOtherSelected(string questionId, string questionType)
        {
            return questionType switch
            {
                "MultipleChoice" => GetRadioAnswer(questionId) == "Other",
                "Checkboxes" => IsCheckboxSelected(questionId, "Other"),
                _ => false
            };
        }

        public Dictionary<string, object?> PrepareSubmissionAnswers(List<QuestionDto> questions)
        {
            var finalAnswers = new Dictionary<string, object?>();

            foreach (var kvp in _userAnswers)
            {
                var questionId = kvp.Key;
                var answer = kvp.Value;
                
                var question = questions.FirstOrDefault(q => q.Id == questionId);
                if (question == null) continue;

                if (question.Type == "MultipleChoice" && answer?.ToString() == "Other" && question.HasOtherOption)
                {
                    finalAnswers[questionId] = GetCustomOtherAnswer(questionId);
                }

                else if (question.Type == "Checkboxes" && answer is List<string> checkboxAnswers && question.HasOtherOption)
                {
                    var processedAnswers = new List<string>();
                    foreach (var checkAnswer in checkboxAnswers)
                    {
                        if (checkAnswer == "Other")
                        {
                            var customAnswer = GetCustomOtherAnswer(questionId);
                            if (!string.IsNullOrWhiteSpace(customAnswer))
                            {
                                processedAnswers.Add(customAnswer);
                            }
                        }
                        else
                        {
                            processedAnswers.Add(checkAnswer);
                        }
                    }
                    finalAnswers[questionId] = processedAnswers;
                }
                else
                {
                    finalAnswers[questionId] = answer;
                }
            }

            return finalAnswers;
        }

        public void Clear()
        {
            _userAnswers.Clear();
            _customOtherAnswers.Clear();
        }
    }
}