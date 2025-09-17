namespace Quiz.Common.Models
{
    public class ErrorLog
    {
        public string Message { get; init; } = string.Empty;
        public string? StackTrace { get; init; }
        public string? Source { get; init; }
        public string? Path { get; init; }
        public string? Component { get; init; }
        public string? UserAgent { get; init; }

        public ErrorLog() {  }

        public ErrorLog(Exception ex, string? path = null, string? component = null, string? userAgent = null)
        {
            Message = ex.Message;
            StackTrace = ex.ToString();
            Source = ex.Source;
            Path = path;
            Component = component;
            UserAgent = userAgent;
        }
    }
}
