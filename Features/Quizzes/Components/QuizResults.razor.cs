using Microsoft.AspNetCore.Components;
using MudBlazor;
using Quiz.Features.Quizzes.Models;
using Quiz.Features.Quizzes.Services;
using Quiz.Common.Services;

namespace Quiz.Features.Quizzes.Components
{
    public partial class QuizResults
    {
        [Parameter] public string? FormId { get; set; }
        [Inject] private QuizService QuizService { get; set; } = default!;
        [Inject] private ToastService ToastService { get; set; } = default!;

        private bool isLoading;
        private FormStatisticsDto? Stats;

        private readonly Dictionary<int, List<string>> _loadedAnswers = new();
        private readonly Dictionary<int, bool> _isLoadingAnswers = new();
        private readonly Dictionary<int, bool> _hasMoreAnswers = new();

        protected override async Task OnParametersSetAsync()
        {
            if (string.IsNullOrWhiteSpace(FormId))
            {
                ToastService.ShowError("Form ID is missing", "Load Error");
                return;
            }

            isLoading = true;
            try
            {
                var result = await QuizService.GetFormStatisticsAsync(FormId);

                if (result.Success && result.Data is not null)
                {
                    Stats = result.Data;

                    foreach (var q in Stats.QuestionStatistics.Where(q => q.QuestionType == "ShortAnswer" || q.QuestionType == "Paragraph"))
                    {
                        _hasMoreAnswers[q.Position] = q.TotalAnswers > 0;

                        if (q.TotalAnswers > 0)
                        {
                            await LoadAnswersAsync(q.Position);
                        }
                    }
                }
                else
                {

                    ToastService.ShowError("Failed to load statistics", "Load Error");
                }
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Failed to load quiz statistics: {ex.Message}", "Load Error");
            }
            finally
            {
                isLoading = false;
            }
        }

        private static Dictionary<string, int> BuildChoiceCounts(QuestionStatisticsDto q)
        {
            var counts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            if (q.OptionCounts != null)
            {
                foreach (var kv in q.OptionCounts)
                    counts[kv.Key] = kv.Value;
            }

            if (q.OtherAnswers != null)
            {
                foreach (var text in q.OtherAnswers)
                {
                    if (string.IsNullOrWhiteSpace(text)) continue;
                    counts[text] = counts.TryGetValue(text, out var c) ? c + 1 : 1;
                }
            }

            return counts;
        }

        private static bool HasChoiceData(QuestionStatisticsDto q)
        {
            var counts = BuildChoiceCounts(q);
            return counts.Count > 0;
        }

        private static double[] ToValues(QuestionStatisticsDto q) => BuildChoiceCounts(q).Values.Select(v => (double)v).ToArray();

        private static string[] ToLabels(QuestionStatisticsDto q) => BuildChoiceCounts(q).Keys.ToArray();


        private List<string> GetLoadedAnswers(int position)
        {
            return _loadedAnswers.TryGetValue(position, out var list) ? list : new List<string>();
        }

        private bool IsLoadingAnswers(int position)
        {
            return _isLoadingAnswers.TryGetValue(position, out var loading) && loading;
        }

        private bool HasMoreAnswers(int position)
        {
            return _hasMoreAnswers.TryGetValue(position, out var hasMore) && hasMore;
        }
        private async Task LoadAnswersAsync(int position)
        {
            if (FormId is null || IsLoadingAnswers(position)) return;

            _isLoadingAnswers[position] = true;
            try
            {
                var currentAnswers = GetLoadedAnswers(position);
                var result = await QuizService.GetQuestionTextAnswersAsync(FormId, position, currentAnswers.Count, 5);
                
                if (result.Success && result.Data != null)
                {
                    if (!_loadedAnswers.ContainsKey(position))
                    {
                        _loadedAnswers[position] = new List<string>();
                    }

                    _loadedAnswers[position].AddRange(result.Data.Answers);
                    _hasMoreAnswers[position] = result.Data.HasMore;
                }
                else
                {
                    ToastService.ShowError("Failed to load additional answers", "Load Error");
                }
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Failed to load answers: {ex.Message}", "Load Error");
            }
            finally
            {
                _isLoadingAnswers[position] = false;
                StateHasChanged();
            }
        }

        private static IEnumerable<BarItem> GetCheckboxBars(QuestionStatisticsDto q)
        {
            var counts = BuildChoiceCounts(q);
            if (counts.Count == 0 || q.TotalAnswers <= 0)
                return Enumerable.Empty<BarItem>();

            return counts
                .OrderByDescending(kv => kv.Value)
                .Select(kv => new BarItem(kv.Key, kv.Value, (int)Math.Round(kv.Value * 100.0 / Math.Max(1, q.TotalAnswers))));
        }

        private static string[] GetPieColors(int count)
        {
            var palette = new[]
            {
                Colors.Blue.Default,
                Colors.Red.Default,
                Colors.Green.Default,
                Colors.Orange.Default,
                Colors.Purple.Default,
                Colors.Teal.Default,
                Colors.Cyan.Default,
                Colors.Brown.Default,
                Colors.DeepOrange.Default,
                Colors.Lime.Default,
                Colors.Pink.Default,
                Colors.Indigo.Default,
                Colors.DeepPurple.Default,
            };

            var colors = new string[count];
            for (int i = 0; i < count; i++)
                colors[i] = palette[i % palette.Length];
            return colors;
        }

        private static IEnumerable<LegendDisplayItem> GetLegendDisplayItems(QuestionStatisticsDto q)
        {
            var labels = ToLabels(q);
            var values = BuildChoiceCounts(q);
            var colors = GetPieColors(labels.Length);
            var total = Math.Max(1, q.TotalAnswers);

            for (int i = 0; i < labels.Length; i++)
            {
                var label = labels[i];
                var count = values.TryGetValue(label, out var c) ? c : 0;
                var percent = (int)Math.Round(count * 100.0 / total);
                yield return new LegendDisplayItem(label, count, percent, colors[i]);
            }
        }
    }
}