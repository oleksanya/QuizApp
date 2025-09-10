using Quiz.Common.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Quiz.Common.Services
{
    public class CustomHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ToastService _toastService;

        public CustomHttpClient(HttpClient httpClient, ToastService toastService)
        {
            _httpClient = httpClient;
            _toastService = toastService;
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<HttpResponseMessage> SendRequestAsync(string url, HttpMethod method, string? json = null)
        {
            var cleanUrl = url.TrimStart('/');
            
            var request = new HttpRequestMessage
            {
                Method = method,
                RequestUri = new Uri($"{_httpClient.BaseAddress?.ToString().TrimEnd('/')}/{cleanUrl}"),
                Content = json != null ? new StringContent(json, Encoding.UTF8, "application/json") : null
            };

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                var errorMessage = $"Request failed with status code {response.StatusCode}: {errorContent}";
                
                var operationType = GetOperationTypeFromMethod(method);
                
                _toastService.ShowError($"Failed to {operationType} resource", "HTTP Error");
            }

            return response;
        }

        public async Task<ApiResponse<T>> SendApiRequestAsync<T>(string url, HttpMethod method, string? json = null, JsonSerializerOptions? jsonOptions = null)
        {
            try
            {
                var response = await SendRequestAsync(url, method, json);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                var result = JsonSerializer.Deserialize<ApiResponse<T>>(responseContent, jsonOptions);
                return result ?? ApiResponse<T>.CreateError("Failed to deserialize response");
            }
            catch (HttpRequestException ex)
            {
                var operationType = GetOperationTypeFromMethod(method);

                return ApiResponse<T>.CreateError($"Failed to {operationType} resource", ex.Message);
            }
            catch (JsonException ex)
            {
                _toastService.ShowError("Failed to process server response", "Parsing Error");
                return ApiResponse<T>.CreateError("Failed to deserialize response", ex.Message);
            }
            catch (Exception ex)
            {
                _toastService.ShowError("An unexpected error occurred", "System Error");
                return ApiResponse<T>.CreateError("Unexpected error occurred", ex.Message);
            }
        }

        private static string GetOperationTypeFromMethod(HttpMethod method)
        {
            return method.Method switch
            {
                "POST" => "create",
                "PUT" or "PATCH" => "update",
                "DELETE" => "delete",
                _ => "retrieve"
            };
        }
    }
}
