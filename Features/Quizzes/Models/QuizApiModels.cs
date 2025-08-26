using System.Text.Json.Serialization;

namespace Quiz.Features.Quizzes.Models
{
    public class QuizApiModels
    {
        public class DeleteFormResponse
        {
            [JsonPropertyName("deletedId")]
            public string? DeletedId { get; set; }
        }

        public class CreateFormDto
        {
            [JsonPropertyName("_id")]
            public string? DatabaseId { get; set; }

            [JsonPropertyName("formInfo")]
            public FormInfoDto FormInfo { get; set; } = new();

            [JsonPropertyName("questions")]
            public List<QuestionDto> Questions { get; set; } = new();

            [JsonPropertyName("formSummary")]
            public FormSummaryDto FormSummary { get; set; } = new();

            [JsonPropertyName("createdAt")]
            public DateTime? CreatedAt { get; set; }
        }

        public class FormInfoDto
        {
            [JsonPropertyName("_id")]
            public string? DatabaseId { get; set; }

            [JsonPropertyName("title")]
            public string Title { get; set; } = string.Empty;

            [JsonPropertyName("description")]
            public string Description { get; set; } = string.Empty;

            [JsonPropertyName("createdDate")]
            public DateTime CreatedDate { get; set; }

            [JsonPropertyName("totalQuestions")]
            public int TotalQuestions { get; set; }
        }

        public class QuestionDto
        {
            [JsonPropertyName("_id")]
            public string? DatabaseId { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; } = string.Empty;

            [JsonPropertyName("type")]
            public string Type { get; set; } = string.Empty;

            [JsonPropertyName("typeDisplayName")]
            public string TypeDisplayName { get; set; } = string.Empty;

            [JsonPropertyName("isRequired")]
            public bool IsRequired { get; set; }

            [JsonPropertyName("options")]
            public List<string>? Options { get; set; }

            [JsonPropertyName("hasOtherOption")]
            public bool HasOtherOption { get; set; } = false;

            [JsonPropertyName("optionsCount")]
            public int OptionsCount { get; set; } = 0;
        }

        public class FormSummaryDto
        {
            [JsonPropertyName("_id")]
            public string? DatabaseId { get; set; }

            [JsonPropertyName("totalQuestions")]
            public int TotalQuestions { get; set; }

            [JsonPropertyName("requiredQuestions")]
            public int RequiredQuestions { get; set; }

            [JsonPropertyName("multipleChoiceQuestions")]
            public int MultipleChoiceQuestions { get; set; }

            [JsonPropertyName("shortAnswerQuestions")]
            public int ShortAnswerQuestions { get; set; }

            [JsonPropertyName("paragraphQuestions")]
            public int ParagraphQuestions { get; set; }

            [JsonPropertyName("checkboxQuestions")]
            public int CheckboxQuestions { get; set; }
        }
    }
}
