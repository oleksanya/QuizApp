using Quiz.Features.Quizzes.Models;
using Quiz.Features.Quizzes.Services;
using static Quiz.Features.Quizzes.Models.QuizApiModels;

namespace Quiz.Features.Quizzes.Helpers
{
    public class QuizFormState
    {
        public bool IsLoading { get; set; } = true;
        public bool IsSubmitting { get; set; } = false;
        public bool IsSubmitted { get; set; } = false;
        public string ErrorMessage { get; set; } = string.Empty;
        public string SubmitMessage { get; set; } = string.Empty;
        public bool SubmitError { get; set; } = false;
        public string FormName { get; set; } = string.Empty;
        public CreateFormDto? QuizForm { get; set; }

        public void SetLoadingError(string message)
        {
            ErrorMessage = message;
            FormName = string.Empty;
            IsLoading = false;
        }

        public void SetLoadingSuccess(CreateFormDto form)
        {
            QuizForm = form;
            FormName = form.FormInfo?.Title ?? string.Empty;
            ErrorMessage = string.Empty;
            IsLoading = false;
        }

        public void SetSubmissionResult(bool success, string message)
        {
            IsSubmitted = success;
            SubmitMessage = message;
            SubmitError = !success;
            IsSubmitting = false;
        }
    }

    public class QuizFormHelper
    {
        private readonly QuizService _quizService;

        public QuizFormHelper(QuizService quizService)
        {
            _quizService = quizService;
        }

        public async Task<bool> LoadQuizAsync(string formId, QuizFormState state)
        {
            try
            {
                state.IsLoading = true;
                state.ErrorMessage = string.Empty;

                if (string.IsNullOrEmpty(formId))
                {
                    state.SetLoadingError("Form ID is required to load the quiz.");
                    return false;
                }

                var result = await _quizService.GetFormByIdAsync(formId);

                if (result.Success && result.Data != null)
                {
                    state.SetLoadingSuccess(result.Data);
                    return true;
                }
                else
                {
                    state.SetLoadingError(result.Message ?? "Failed to load quiz.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                state.SetLoadingError($"Failed to load quiz: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SubmitQuizAsync(string formId, QuizFormState state, QuizAnswerManager answerManager)
        {
            try
            {
                state.IsSubmitting = true;
                state.SubmitMessage = string.Empty;
                state.SubmitError = false;

                if (state.QuizForm == null)
                {
                    state.SetSubmissionResult(false, "Quiz form is not loaded.");
                    return false;
                }

                var validationErrors = QuizValidationHelper.ValidateAnswers(state.QuizForm, answerManager);
                if (validationErrors.Any())
                {
                    state.SetSubmissionResult(false, $"Please complete all required questions: {string.Join(", ", validationErrors)}");
                    return false;
                }

                var finalAnswers = answerManager.PrepareSubmissionAnswers(state.QuizForm.Questions);

                var submission = new FormResponseSubmissionDto
                {
                    FormId = formId,
                    Answers = finalAnswers
                };

                var result = await _quizService.SubmitFormResponseAsync(submission);

                if (result.Success)
                {
                    state.SetSubmissionResult(true, "Quiz submitted successfully!");
                    return true;
                }
                else
                {
                    state.SetSubmissionResult(false, result.Message ?? "Failed to submit quiz. Please try again.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                state.SetSubmissionResult(false, $"An error occurred: {ex.Message}");
                return false;
            }
        }
    }
}