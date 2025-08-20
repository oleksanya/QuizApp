public class QuestionModel
{
    public string Name { get; set; } = string.Empty;
    public QuestionType Type { get; set; } = QuestionType.MultipleChoice;
    public List<string> Options { get; set; } = new();
    public bool IsRequired { get; set; }
    public bool HasOtherOption { get; set; } = false;
 }