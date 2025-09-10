using Quiz.Features.Quizzes.Services;
using Quiz.Common.Services;
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
        private readonly ToastService _toastService;

        public QuizFormHelper(QuizService quizService, ToastService toastService)
        {
            _quizService = quizService;
            _toastService = toastService;
        }

        public async Task<bool> LoadQuizAsync(string formId, QuizFormState state)
        {
            try
            {
                state.IsLoading = true;
                state.ErrorMessage = string.Empty;

                if (string.IsNullOrEmpty(formId))
                {
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
                    var errorMessage = result.Message ?? "Failed to load quiz. Please try again.";
                    state.SetLoadingError(errorMessage);
                    _toastService.ShowError(errorMessage, "Load Error");
                    return false;
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"Failed to load quiz: {ex.Message}";
                state.SetLoadingError(errorMessage);
                _toastService.ShowError(errorMessage, "Load Error");
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
                    var errorMessage = "Quiz form is not loaded.";
                    state.SetSubmissionResult(false, errorMessage);
                    _toastService.ShowError(errorMessage, "Submit Error");
                    return false;
                }

                var validationErrors = QuizValidationHelper.ValidateAnswers(state.QuizForm, answerManager);
                if (validationErrors.Any())
                {
                    var errorMessage = $"Please complete all required questions: {string.Join(", ", validationErrors)}";
                    _toastService.ShowError(errorMessage, "Submit Error");
                    state.SetSubmissionResult(false, errorMessage);
                    
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
                    var successMessage = "Quiz submitted successfully!";
                    state.SetSubmissionResult(true, successMessage);
                    _toastService.ShowSuccess(successMessage);
                    return true;
                }
                else
                {
                    var errorMessage = result.Message ?? "Failed to submit quiz. Please try again.";
                    state.SetSubmissionResult(false, errorMessage);
                    _toastService.ShowError(errorMessage, "Submit Error");
                    return false;
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"An error occurred: {ex.Message}";
                state.SetSubmissionResult(false, errorMessage);
                _toastService.ShowError(errorMessage, "Submit Error");
                return false;
            }
        }
    }
}