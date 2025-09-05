namespace Quiz.Features.Quizzes.Components
{
    public partial class QuizResults
    {
        private readonly record struct BarItem(string Label, int Count, int Percent);
        internal readonly record struct LegendDisplayItem(string Label, int Count, int Percent, string Color);
    }
}
