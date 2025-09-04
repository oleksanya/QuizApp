using System.Text.Json.Serialization;

namespace Quiz.Features.Quizzes.Models
{
    public class FormStatisticsDto
    {
        [JsonPropertyName("formInfo")] public FormStatisticsFormInfo FormInfo { get; set; } = new();
        [JsonPropertyName("overview")] public FormStatisticsOverview Overview { get; set; } = new();
        [JsonPropertyName("questionStatistics")] public List<QuestionStatisticsDto> QuestionStatistics { get; set; } = new();
        [JsonPropertyName("lastUpdated")] public DateTime? LastUpdated { get; set; }
    }

    public class FormStatisticsFormInfo
    {
        [JsonPropertyName("title")] public string Title { get; set; } = string.Empty;
        [JsonPropertyName("description")] public string Description { get; set; } = string.Empty;
        [JsonPropertyName("totalQuestions")] public int TotalQuestions { get; set; }
    }

    public class FormStatisticsOverview
    {
        [JsonPropertyName("totalResponses")] public int TotalResponses { get; set; }
        [JsonPropertyName("completedResponses")] public int CompletedResponses { get; set; }
        [JsonPropertyName("completionRate")] public double CompletionRate { get; set; }
        [JsonPropertyName("averageCompletionPercentage")] public double AverageCompletionPercentage { get; set; }
    }

    public class QuestionStatisticsDto
    {
        [JsonPropertyName("position")] public int Position { get; set; }
        [JsonPropertyName("questionName")] public string QuestionName { get; set; } = string.Empty;
        [JsonPropertyName("questionType")] public string QuestionType { get; set; } = string.Empty;
        [JsonPropertyName("totalAnswers")] public int TotalAnswers { get; set; }
        [JsonPropertyName("responseRate")] public string? ResponseRate { get; set; }
        [JsonPropertyName("isRequired")] public bool IsRequired { get; set; }

        [JsonPropertyName("textAnswers")] public List<string>? TextAnswers { get; set; }
        [JsonPropertyName("averageLength")] public int? AverageLength { get; set; }

        [JsonPropertyName("optionCounts")] public Dictionary<string, int>? OptionCounts { get; set; }
        [JsonPropertyName("otherAnswers")] public List<string>? OtherAnswers { get; set; }
        [JsonPropertyName("hasOtherAnswers")] public bool? HasOtherAnswers { get; set; }
    }
}