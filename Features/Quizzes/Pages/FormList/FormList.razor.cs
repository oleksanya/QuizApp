using Microsoft.AspNetCore.Components;

namespace Quiz.Features.Quizzes.Pages.FormList
{
    public partial class FormList
    {
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        private void CreateNewForm()
        {
            Navigation.NavigateTo("/create-form");
        }
    }
}
