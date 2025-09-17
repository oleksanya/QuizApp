using System.Text.Json;
using Quiz.Common.Interfaces;
using Quiz.Common.Models;

namespace Quiz.Common.Services
{
    public class ErrorLoggerService : IErrorLoggerService
    {
        private readonly CustomHttpClient _client;
        private readonly ToastService _toastService;
        private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);

        private const string LogEndpoint = "/error-logs";

        public ErrorLoggerService(CustomHttpClient client, ToastService toastService)
        {
            _client = client;
            _toastService = toastService;
        }

        public async Task LogAsync(ErrorLog log)
        {
            try
            {
                var payload = new
                {
                    errorInfo = new
                    {
                        message = log.Message,
                        stackTrace = log.StackTrace
                    },
                    source = log.Source,
                    component = log.Component,
                    path = log.Path,
                    userAgent = log.UserAgent,
                    severity = "Error"
                };

                var json = JsonSerializer.Serialize(payload, _jsonOptions);

                HttpResponseMessage? response = null;

                try
                {
                    response = await _client.SendRequestAsync(LogEndpoint, HttpMethod.Post, json);
                }
                catch (Exception ex)
                {
                    _toastService.ShowError($"Failed to send error log: {ex.Message}", "Logger Error");
                }
            }
            catch (Exception ex)
            {
                _toastService.ShowError($"Failed to process error log: {ex.Message}", "Logger Error");
            }
        }
    }
}
