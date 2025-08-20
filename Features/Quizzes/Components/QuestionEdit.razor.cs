using Microsoft.AspNetCore.Components;

namespace Quiz.Features.Quizzes.Components
{
    public partial class QuestionEdit : ComponentBase
    {
        [Parameter] public QuestionModel Question { get; set; } = default!;
        [Parameter] public EventCallback OnRemove { get; set; }
        
        public void AddOption()
        {
            Question.Options.Add($"Option {Question.Options.Count + 1}");
        }

        public void AddOtherOption()
        {
            if (!Question.HasOtherOption)
            {
                Question.Options.Add("Other");
                Question.HasOtherOption = true;
            }
        }

        public void RemoveOption(int index)
        {
            if (index >= 0 && index < Question.Options.Count)
            {
                if (Question.Options[index] == "Other")
                {
                    Question.HasOtherOption = false;
                }
                Question.Options.RemoveAt(index);
            }
        }
    }
}