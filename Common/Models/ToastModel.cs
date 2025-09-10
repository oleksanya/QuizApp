namespace Quiz.Common.Models;
using Quiz.Common.Enums;

public class ToastModel
{
    public string Id { get; init; } = Guid.NewGuid().ToString("N");
    public string? Title { get; init; }
    public required string Message { get; init; }
    public ToastType Type { get; init; } = ToastType.Info;
    public int DurationMs { get; init; } = 5000;
}