using Microsoft.AspNetCore.Components;
using static Quiz.Features.Quizzes.Models.QuizApiModels;

namespace Quiz.Features.Quizzes.Components
{
    public partial class FormCard
    {
        [Parameter] public CreateFormDto Form { get; set; } = default!;
    }
}
