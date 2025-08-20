using Microsoft.AspNetCore.Components;
using System.Text.Json;
using Quiz.Features.Quizzes.Helpers;
using Quiz.Features.Quizzes.Models;

namespace Quiz.Features.Quizzes.Pages.CreateForm
{
    public partial class CreateForm : ComponentBase
    {
        protected string formTitle = "Untitled Form";
        protected string formDescription = "Form description goes here";
        protected FormModel form = new();

        protected override void OnInitialized()
        {
            if (form.Questions.Count == 0)
            {
                AddQuestion();
            }
        }

        protected void AddQuestion()
        {
            form.Questions.Add(new QuestionModel
            {
                Name = "Untitled Question",
                Options = new() { "Option 1" }
            });
        }

        protected void RemoveQuestion(QuestionModel q)
        {
            if (form.Questions.Count > 1)
            {
                form.Questions.Remove(q);
            }
        }

        public void SaveForm()
        {
            form.Title = formTitle;
            form.Description = formDescription;

            var formInfo = new FormInfo
            {
                Id = Guid.NewGuid().ToString(),
                Title = form.Title,
                Description = form.Description,
                CreatedDate = DateTime.Now,
                TotalQuestions = form.Questions.Count
            };

            var questions = form.Questions.Select(q => new QuestionData
            {
                Name = q.Name,
                Type = q.Type.ToString(),
                TypeDisplayName = q.Type.GetDisplayName(),
                IsRequired = q.IsRequired,
                Options = (q.Type == QuestionType.MultipleChoice || q.Type == QuestionType.Checkboxes) && q.Options.Any() ? q.Options : null,
                HasOtherOption = q.HasOtherOption,
                OptionsCount = (q.Type == QuestionType.MultipleChoice || q.Type == QuestionType.Checkboxes) ? q.Options.Count : 0
            }).ToList();

            var formSummary = new FormSummary
            {
                TotalQuestions = form.Questions.Count,
                RequiredQuestions = form.Questions.Count(q => q.IsRequired),
                MultipleChoiceQuestions = form.Questions.Count(q => q.Type == QuestionType.MultipleChoice),
                ShortAnswerQuestions = form.Questions.Count(q => q.Type == QuestionType.ShortAnswer),
                ParagraphQuestions = form.Questions.Count(q => q.Type == QuestionType.Paragraph),
                CheckboxQuestions = form.Questions.Count(q => q.Type == QuestionType.Checkboxes)
            };

            var completeFormData = new CompleteFormData
            {
                FormInfo = formInfo,
                Questions = questions,
                FormSummary = formSummary
            };

            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var jsonString = JsonSerializer.Serialize(completeFormData, jsonOptions);

            Console.WriteLine("Complete Form Data:");
            Console.WriteLine(jsonString);
        }
    }
}