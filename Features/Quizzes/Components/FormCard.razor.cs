using Microsoft.AspNetCore.Components;
using Quiz.Features.Quizzes.Models;

namespace Quiz.Features.Quizzes.Components
{
    public partial class FormCard
    {
        [Parameter] public CreateFormDto Form { get; set; } = default!;
    }
}
