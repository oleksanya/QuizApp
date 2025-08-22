using Microsoft.AspNetCore.Components;
using Quiz.Features.Quizzes.Models;
using Quiz.Features.Quizzes.Services;

namespace Quiz.Features.Quizzes.Pages.FormList
{
    public partial class FormList
    {
        [Inject] private FormService FormService { get; set; } = default!;

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

                var result = await FormService.GetFormsAsync();

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
