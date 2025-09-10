using System.Text.Json.Serialization;

namespace Quiz.Common.Models
{
    public class ApiResponse<T>
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("data")]
        public T? Data { get; set; }

        [JsonPropertyName("count")]
        public int? Count { get; set; }

        [JsonPropertyName("error")]
        public string? Error { get; set; }

        public ApiResponse() { }

        public ApiResponse(string message, string? error = null)
        {
            Success = false;
            Message = message;
            Error = error;
        }

        public ApiResponse(T? data, string message = "Success")
        {
            Success = true;
            Message = message;
            Data = data;
        }

        public static ApiResponse<T> CreateError(string message, string? error = null)
        {
            return new ApiResponse<T>(message, error);
        }

        public static ApiResponse<T> CreateSuccess(T? data, string message = "Success")
        {
            return new ApiResponse<T>(data, message);
        }
    }
}