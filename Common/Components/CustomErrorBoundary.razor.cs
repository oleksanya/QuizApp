using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Quiz.Common.Interfaces;
using Quiz.Common.Models;
using Quiz.Common.Services;

namespace Quiz.Common.Components
{
    public partial class CustomErrorBoundary : ErrorBoundary
    {
        [Inject] private IErrorLoggerService ErrorLogger { get; set; } = default!;
        [Inject] private ToastService ToastService { get; set; } = default!;
        [Inject] private IJSRuntime JS { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;

        protected override async Task OnErrorAsync(Exception ex)
        {
            try
            {
                string? userAgent = null;

                if (JS is not null)
                {
                    userAgent = await JS.InvokeAsync<string>("eval", "navigator.userAgent");
                }

                var componentName = TryExtractComponentFromStackTrace(ex);
                var path = Navigation?.Uri;

                var log = new ErrorLog(ex, path: path, component: componentName, userAgent: userAgent);

                await ErrorLogger.LogAsync(log);
            }
            catch (Exception logEx)
            {
                ToastService?.ShowError($"Failed to report error: {logEx.Message}", "Logger Error");
            }
        }
        private static string? TryExtractComponentFromStackTrace(Exception ex)
        {
            var text = ex.StackTrace ?? ex.ToString();
            if (string.IsNullOrWhiteSpace(text)) return null;

            var lines = text.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
            foreach (var raw in lines)
            {
                var line = raw.Trim();
                if (!line.StartsWith("at ", StringComparison.Ordinal)) continue;
                if (!line.Contains("Quiz.")) continue;

                var upToIn = line;
                var inIndex = line.IndexOf(" in ", StringComparison.Ordinal);
                if (inIndex > 0) upToIn = line[..inIndex];

                var afterAt = upToIn.Substring(3).Trim();
                var parenIndex = afterAt.IndexOf('(');
                if (parenIndex > 0) afterAt = afterAt[..parenIndex];

                var parts = afterAt.Split('.', StringSplitOptions.RemoveEmptyEntries).ToList();
                if (parts.Count == 0) continue;

                // Remove compiler-generated segments and the final member name
                var filtered = parts.Where(p => !p.StartsWith("<", StringComparison.Ordinal)).ToList();
                if (filtered.Count <= 1) continue;

                // Remove the last item which is typically the member name
                filtered.RemoveAt(filtered.Count - 1);

                return string.Join('.', filtered);
            }

            return null;
        }
    }
}
