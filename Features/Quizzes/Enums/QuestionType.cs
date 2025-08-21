using System.ComponentModel.DataAnnotations;

public enum QuestionType
{
    [Display(Name = "Short Answer")]
    ShortAnswer,

    [Display(Name = "Paragraph")]
    Paragraph,

    [Display(Name = "Multiple Choice")]
    MultipleChoice,

    [Display(Name = "Checkboxes")]
    Checkboxes
}