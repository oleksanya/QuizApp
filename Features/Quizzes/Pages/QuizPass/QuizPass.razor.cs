using Microsoft.AspNetCore.Components;
using Quiz.Features.Quizzes.Helpers;
using Quiz.Features.Quizzes.Services;
using Quiz.Features.Quizzes.Models;
using static Quiz.Features.Quizzes.Models.QuizApiModels;

namespace Quiz.Features.Quizzes.Pages.QuizPass
{
    public partial class QuizPass : ComponentBase
    {
        [Parameter] public string FormId { get; set; } = string.Empty;
        [Inject] private QuizService? QuizService { get; set; }

        private readonly QuizFormState _state = new();
        private readonly QuizAnswerManager _answerManager = new();
        private QuizFormHelper? _formHelper;


        private bool isLoading => _state.IsLoading;
        private bool isSubmitting => _state.IsSubmitting;
        private bool isSubmitted => _state.IsSubmitted;
        private string errorMessage => _state.ErrorMessage;
        private string submitMessage => _state.SubmitMessage;
        private bool submitError => _state.SubmitError;
        private string FormName => _state.FormName;
        private CreateFormDto? quizForm => _state.QuizForm;

        protected override async Task OnInitializedAsync()
        {
            _formHelper = new QuizFormHelper(QuizService!);
            await LoadQuiz();
        }

        protected override async Task OnParametersSetAsync()
        {
            if (!string.IsNullOrEmpty(FormId))
            {
                await LoadQuiz();
            }
        }

        private async Task LoadQuiz()
        {
            var success = await _formHelper!.LoadQuizAsync(FormId, _state);
            if (success && _state.QuizForm != null)
            {
                _answerManager.InitializeAnswers(_state.QuizForm.Questions);
            }
        }

        private async Task SubmitQuiz()
        {
            var success = await _formHelper!.SubmitQuizAsync(FormId, _state, _answerManager);
        }

        private void ClearForm()
        {
            _answerManager.Clear();

            if (_state.QuizForm != null)
            {
                _answerManager.InitializeAnswers(_state.QuizForm.Questions);
            }
            
            _state.IsSubmitted = false;
            _state.SubmitMessage = string.Empty;
            _state.SubmitError = false;
        }

        private void UpdateTextAnswer(string questionId, string value) =>
            _answerManager.UpdateTextAnswer(questionId, value);

        private void UpdateRadioAnswer(string questionId, string value) =>
            _answerManager.UpdateRadioAnswer(questionId, value);

        private void UpdateCheckboxAnswer(string questionId, string option, bool isChecked) =>
            _answerManager.UpdateCheckboxAnswer(questionId, option, isChecked);

        private void UpdateCustomOtherAnswer(string questionId, string value) =>
            _answerManager.UpdateCustomOtherAnswer(questionId, value);

        private string GetTextAnswer(string questionId) =>
            _answerManager.GetTextAnswer(questionId);

        private string GetRadioAnswer(string questionId) =>
            _answerManager.GetRadioAnswer(questionId);

        private bool IsCheckboxSelected(string questionId, string option) =>
            _answerManager.IsCheckboxSelected(questionId, option);

        private string GetCustomOtherAnswer(string questionId) =>
            _answerManager.GetCustomOtherAnswer(questionId);

        private bool IsOtherSelected(string questionId, string questionType) =>
            _answerManager.IsOtherSelected(questionId, questionType);
    }
}