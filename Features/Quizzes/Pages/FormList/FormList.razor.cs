using Microsoft.AspNetCore.Components;
using Quiz.Features.Quizzes.Services;
using Quiz.Common.Services;
using static Quiz.Features.Quizzes.Models.QuizApiModels;

namespace Quiz.Features.Quizzes.Pages.FormList
{
    public partial class FormList
    {
        [Inject] private QuizService QuizService { get; set; } = default!;
        [Inject] private ToastService ToastService { get; set; } = default!;

        private List<CreateFormDto>? forms;
        private bool isLoading = true;

        protected override async Task OnInitializedAsync()
        {
            await LoadForms();
        }

        private async Task LoadForms()
        {
            try
            {
                isLoading = true;

                var result = await QuizService.GetFormsAsync();

                if (result?.Success == true && result.Data != null)
                {
                    forms = result.Data;
                }
                else
                {
                    var errorMsg = result?.Message ?? "Failed to load forms";
                    ToastService.ShowError(errorMsg, "Load Error");
                }
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Error loading forms: {ex.Message}", "Unexpected Error");
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }
    }
}
