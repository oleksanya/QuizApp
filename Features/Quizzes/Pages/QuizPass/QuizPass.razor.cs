using Microsoft.AspNetCore.Components;
using Quiz.Features.Quizzes.Models;
using Quiz.Features.Quizzes.Services;
using static Quiz.Features.Quizzes.Models.QuizApiModels;

namespace Quiz.Features.Quizzes.Pages.QuizPass
{
    public partial class QuizPass : ComponentBase
    {
        [Parameter] public string FormId { get; set; } = string.Empty;
        [Inject] private QuizService? QuizService { get; set; }

        private bool isLoading = true;
        private string errorMessage = string.Empty;
        private string FormName = string.Empty;
        private CreateFormDto? quizForm;

        protected override async Task OnInitializedAsync()
        {
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
            try
            {
                isLoading = true;
                errorMessage = string.Empty;
                StateHasChanged();

                if (string.IsNullOrEmpty(FormId))
                {
                    errorMessage = "Form ID is required to load the quiz.";
                    return;
                }

                if (FormId == "temp-id")
                {
                    errorMessage = "This quiz hasn't been saved yet.";
                    return;
                }

                var result = QuizService != null
                    ? await QuizService.GetFormByIdAsync(FormId)
                    : null;

                if (result?.Success == true && result.Data != null)
                {
                    quizForm = result.Data;
                    FormName = quizForm.FormInfo?.Title ?? string.Empty;
                }
                else
                {
                    errorMessage = result?.Message ?? "Failed to load quiz.";
                    FormName = string.Empty;
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Failed to load quiz: {ex.Message}";
                FormName = string.Empty;
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }
    }
}