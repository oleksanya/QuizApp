using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using static Quiz.Features.Quizzes.Models.QuizApiModels;

namespace Quiz.Features.Quizzes.Components
{
    public partial class FormCard : ComponentBase
    {
        [Parameter] public CreateFormDto Form { get; set; } = default!;
        [Inject] private IJSRuntime JS { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;

        private bool showContextMenu = false;

        private void ToggleContextMenu()
        {
            showContextMenu = !showContextMenu;
        }

        private void CloseContextMenu()
        {
            showContextMenu = false;
        }

        private async Task CopyQuizLinkToClipboard()
        {
            try
            {
                var baseUrl = Navigation.BaseUri.TrimEnd('/');
                var formId = Form.Id ?? "temp-id";
                var quizLink = $"{baseUrl}/quiz-pass/{formId}";
                
                if (string.IsNullOrEmpty(quizLink))
                {
                    return;
                }

                await JS.InvokeVoidAsync("copyToClipboard", quizLink);
            }
            finally
            {
                CloseContextMenu();
            }
        }
    }
}