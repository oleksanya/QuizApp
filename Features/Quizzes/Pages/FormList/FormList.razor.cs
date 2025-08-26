using Microsoft.AspNetCore.Components;
using Quiz.Features.Quizzes.Services;
using static Quiz.Features.Quizzes.Models.QuizApiModels;

namespace Quiz.Features.Quizzes.Pages.FormList
{
    public partial class FormList
    {
        [Inject] private QuizService QuizService { get; set; } = default!;

        private List<CreateFormDto>? forms;
        private bool isLoading = true;
        private string? errorMessage;

        protected override async Task OnInitializedAsync()
        {
            await LoadForms();
        }

        private async Task LoadForms()
        {
            try
            {
                isLoading = true;
                errorMessage = null;

                var result = await QuizService.GetFormsAsync();

                if (result?.Success == true && result.Data != null)
                {
                    forms = result.Data;
                }
                else
                {
                    errorMessage = result?.Message ?? "Failed to load forms";
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Error loading forms: {ex.Message}";
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }
    }
}
