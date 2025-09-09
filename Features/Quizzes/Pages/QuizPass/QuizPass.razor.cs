using Microsoft.AspNetCore.Components;
using Quiz.Features.Quizzes.Helpers;
using Quiz.Features.Quizzes.Services;
using Quiz.Common.Services;

namespace Quiz.Features.Quizzes.Pages.QuizPass
{
    public partial class QuizPass : ComponentBase
    {
        [Parameter] public string FormId { get; set; } = string.Empty;
        [Inject] private QuizService? QuizService { get; set; }
        [Inject] private ToastService ToastService { get; set; } = default!;

        private readonly QuizFormState _state = new();
        private readonly QuizAnswerManager _answerManager = new();
        private QuizFormHelper? _formHelper;

        protected override async Task OnInitializedAsync()
        {
            _formHelper = new QuizFormHelper(QuizService!, ToastService);
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
    }
}