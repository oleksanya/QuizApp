using Microsoft.AspNetCore.Components;
using System.Text.Json;
using Quiz.Features.Quizzes.Helpers;
using Quiz.Features.Quizzes.Models;
using Quiz.Features.Quizzes.Services;

namespace Quiz.Features.Quizzes.Pages.CreateForm
{
    public partial class CreateForm : ComponentBase
    {
        [Inject] private FormService FormService { get; set; } = default!;

        protected string formTitle = "Untitled Form";
        protected string formDescription = "Form description goes here";
        protected FormModel form = new();
        protected bool isLoading = false;
        protected string message = string.Empty;
        protected bool isError = false;

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

        public async Task SaveForm()
        {
            isLoading = true;
            message = string.Empty;
            isError = false;

            try
            {

                form.Title = formTitle;
                form.Description = formDescription;


                var createFormDto = new CreateFormDto
                {
                    FormInfo = new FormInfoDto
                    {
                        Title = form.Title,
                        Description = form.Description ?? string.Empty,
                        CreatedDate = DateTime.Now,
                        TotalQuestions = form.Questions.Count
                    },
                    Questions = form.Questions.Select(q => new QuestionDto
                    {
                        Name = q.Name,
                        Type = q.Type.ToString(),
                        TypeDisplayName = q.Type.GetDisplayName(),
                        IsRequired = q.IsRequired,
                        Options = (q.Type == QuestionType.MultipleChoice || q.Type == QuestionType.Checkboxes) && q.Options.Any() ? q.Options : null,
                        HasOtherOption = q.HasOtherOption,
                        OptionsCount = (q.Type == QuestionType.MultipleChoice || q.Type == QuestionType.Checkboxes) ? q.Options.Count : 0
                    }).ToList(),
                    FormSummary = new FormSummaryDto
                    {
                        TotalQuestions = form.Questions.Count,
                        RequiredQuestions = form.Questions.Count(q => q.IsRequired),
                        MultipleChoiceQuestions = form.Questions.Count(q => q.Type == QuestionType.MultipleChoice),
                        ShortAnswerQuestions = form.Questions.Count(q => q.Type == QuestionType.ShortAnswer),
                        ParagraphQuestions = form.Questions.Count(q => q.Type == QuestionType.Paragraph),
                        CheckboxQuestions = form.Questions.Count(q => q.Type == QuestionType.Checkboxes)
                    }
                };


                var result = await FormService.SaveFormAsync(createFormDto);

                if (result?.Success == true)
                {
                    message = "Form saved successfully!";
                    isError = false;
                }
                else
                {
                    message = result?.Error ?? result?.Message ?? "Failed to save form";
                    isError = true;
                }
            }
            catch (Exception ex)
            {
                message = $"Unexpected error: {ex.Message}";
                isError = true;
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }
    }
}