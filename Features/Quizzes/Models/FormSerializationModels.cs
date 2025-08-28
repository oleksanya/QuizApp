using System.Text.Json.Serialization;

namespace Quiz.Features.Quizzes.Models
{
    public class FormInfo
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public int TotalQuestions { get; set; }
    }

    public class QuestionData
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string TypeDisplayName { get; set; } = string.Empty;
        public bool IsRequired { get; set; }
        public int Position { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string>? Options { get; set; }
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool HasOtherOption { get; set; }
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int OptionsCount { get; set; }
    }

    public class FormSummary
    {
        public int TotalQuestions { get; set; }
        public int RequiredQuestions { get; set; }
        public int MultipleChoiceQuestions { get; set; }
        public int ShortAnswerQuestions { get; set; }
        public int ParagraphQuestions { get; set; }
        public int CheckboxQuestions { get; set; }
    }

    public class CompleteFormData
    {
        public FormInfo FormInfo { get; set; } = new();
        public List<QuestionData> Questions { get; set; } = new();
        public FormSummary FormSummary { get; set; } = new();
    }
}