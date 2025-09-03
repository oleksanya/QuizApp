using Quiz.Common.Models;
using Quiz.Common.Services;
using System.Text.Json;
using static Quiz.Features.Quizzes.Models.QuizApiModels;

namespace Quiz.Features.Quizzes.Services
{
    public class QuizService
    {
        private readonly CustomHttpClient _customHttpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public QuizService(CustomHttpClient customHttpClient)
        {
            _customHttpClient = customHttpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<ApiResponse<CreateFormDto>?> SaveFormAsync(CreateFormDto formDto)
        {
            try
            {
                var json = JsonSerializer.Serialize(formDto, _jsonOptions);
                var response = await _customHttpClient.SendRequestAsync("/forms", HttpMethod.Post, json);

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ApiResponse<CreateFormDto>>(responseContent, _jsonOptions);
            }
            catch (HttpRequestException ex)
            {
                return new ApiResponse<CreateFormDto>
                {
                    Success = false,
                    Message = "Failed to create form",
                    Error = ex.Message
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<CreateFormDto>
                {
                    Success = false,
                    Message = "Unexpected error occurred",
                    Error = ex.Message
                };
            }
        }

        public async Task<ApiResponse<List<CreateFormDto>>?> GetFormsAsync()
        {
            try
            {
                var response = await _customHttpClient.SendRequestAsync("/forms", HttpMethod.Get);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                var result = JsonSerializer.Deserialize<ApiResponse<List<CreateFormDto>>>(responseContent, _jsonOptions);
                return result;
            }
            catch (JsonException ex)
            {
                return new ApiResponse<List<CreateFormDto>>
                {
                    Success = false,
                    Message = "Failed to deserialize forms response",
                    Error = ex.Message
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<CreateFormDto>>
                {
                    Success = false,
                    Message = "Failed to retrieve forms",
                    Error = ex.Message
                };
            }
        }

        public async Task<ApiResponse<CreateFormDto>?> GetFormByIdAsync(string formId)
        {
            try
            {
                var response = await _customHttpClient.SendRequestAsync($"/forms/{formId}", HttpMethod.Get);
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ApiResponse<CreateFormDto>>(responseContent, _jsonOptions);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CreateFormDto>
                {
                    Success = false,
                    Message = "Failed to retrieve form",
                    Error = ex.Message
                };
            }
        }

        public async Task<ApiResponse<CreateFormDto>?> UpdateFormAsync(string id, CreateFormDto formDto)
        {
            try
            {
                var json = JsonSerializer.Serialize(formDto, _jsonOptions);
                var response = await _customHttpClient.SendRequestAsync($"/forms/{id}", HttpMethod.Put, json);

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ApiResponse<CreateFormDto>>(responseContent, _jsonOptions);
            }
            catch (HttpRequestException ex)
            {
                return new ApiResponse<CreateFormDto>
                {
                    Success = false,
                    Message = "Failed to update form",
                    Error = ex.Message
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<CreateFormDto>
                {
                    Success = false,
                    Message = "Unexpected error occurred during update",
                    Error = ex.Message
                };
            }
        }

        public async Task<ApiResponse<DeleteFormResponse>?> DeleteFormAsync(string id)
        {
            try
            {
                var response = await _customHttpClient.SendRequestAsync($"/forms/{id}", HttpMethod.Delete);
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ApiResponse<DeleteFormResponse>>(responseContent, _jsonOptions);
            }
            catch (Exception ex)
            {
                return new ApiResponse<DeleteFormResponse>
                {
                    Success = false,
                    Message = "Failed to delete form",
                    Error = ex.Message
                };
            }
        }

        public async Task<ApiResponse<FormResponseSubmissionResponseDto>?> SubmitFormResponseAsync(FormResponseSubmissionDto submission)
        {
            try
            {
                var json = JsonSerializer.Serialize(submission, _jsonOptions);
                var response = await _customHttpClient.SendRequestAsync("/form-responses", HttpMethod.Post, json);

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ApiResponse<FormResponseSubmissionResponseDto>>(responseContent, _jsonOptions);
            }
            catch (HttpRequestException ex)
            {
                return new ApiResponse<FormResponseSubmissionResponseDto>
                {
                    Success = false,
                    Message = "Failed to submit form response",
                    Error = ex.Message
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<FormResponseSubmissionResponseDto>
                {
                    Success = false,
                    Message = "Unexpected error occurred during submission",
                    Error = ex.Message
                };
            }
        }
    }
}