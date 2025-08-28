using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Quiz.Features.Quizzes.Helpers;
using Quiz.Features.Quizzes.Services;
using static Quiz.Features.Quizzes.Models.QuizApiModels;

namespace Quiz.Features.Quizzes.Pages.CreateForm
{
    public partial class CreateForm : ComponentBase
    {
        [Inject] private QuizService QuizService { get; set; } = default!;
        [Inject] private IJSRuntime JS { get; set; } = default!;

        private DotNetObjectReference<CreateForm>? _dotNetRef;
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

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await InitSortable();
            }
        }

        private async Task InitSortable()
        {
            try
            {
                _dotNetRef ??= DotNetObjectReference.Create(this);
                await JS.InvokeVoidAsync("sortableInit", "questions-list", ".drag-handle", _dotNetRef);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("[DragDrop][NET] Failed to initialize sortable", ex);
            }
        }

        private void RecalculatePositions()
        {
            for (int i = 0; i < form.Questions.Count; i++)
            {
                form.Questions[i].Position = i;
            }
        }

        protected void AddQuestion()
        {
            var q = new QuestionModel
            {
                Name = "Untitled Question",
                Options = new() { "Option 1" },
                Position = form.Questions.Count
            };
            form.Questions.Add(q);

            RecalculatePositions();
            StateHasChanged();
        }

        protected void RemoveQuestion(QuestionModel q)
        {
            if (form.Questions.Count > 1)
            {
                form.Questions.Remove(q);
                RecalculatePositions();
                StateHasChanged();
            }
        }

        public async Task SaveForm()
        {
            isLoading = true;
            message = string.Empty;
            isError = false;

            try
            {
                await SyncPositionsWithDOM();

          

                var createFormDto = new CreateFormDto
                {
                    FormInfo = new FormInfoDto
                    {
                        Title = form.Title,
                        Description = form.Description ?? string.Empty,
                        CreatedDate = DateTime.Now,
                        TotalQuestions = form.Questions.Count
                    },
                    Questions = form.Questions.OrderBy(q => q.Position).Select(q => new QuestionDto
                    {
                        Name = q.Name,
                        Type = q.Type.ToString(),
                        TypeDisplayName = q.Type.GetDisplayName(),
                        IsRequired = q.IsRequired,
                        Options = (q.Type == QuestionType.MultipleChoice || q.Type == QuestionType.Checkboxes) && q.Options.Any() ? q.Options : null,
                        HasOtherOption = q.HasOtherOption,
                        OptionsCount = (q.Type == QuestionType.MultipleChoice || q.Type == QuestionType.Checkboxes) ? q.Options.Count : 0,
                        Position = q.Position,
                        Id = q.Id.ToString()
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
      
                var result = await QuizService.SaveFormAsync(createFormDto);

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
            }
        }

        private async Task SyncPositionsWithDOM()
        {
            try
            {
                var domOrder = await JS.InvokeAsync<string[]>("getDOMQuestionOrder", "questions-list");

                var questionMap = form.Questions.ToDictionary(q => q.Id.ToString(), q => q);

                for (int i = 0; i < domOrder.Length; i++)
                {
                    if (questionMap.TryGetValue(domOrder[i], out var q))
                    {
                        q.Position = i;
                    }
                }
            }
            catch
            {
                RecalculatePositions();
            }
        }
    }
}