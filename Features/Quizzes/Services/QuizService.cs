using Quiz.Common.Models;
using Quiz.Common.Services;
using Quiz.Features.Quizzes.Models;
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

        public async Task<ApiResponse<CreateFormDto>> SaveFormAsync(CreateFormDto formDto)
        {
            var json = JsonSerializer.Serialize(formDto, _jsonOptions);
            return await _customHttpClient.SendApiRequestAsync<CreateFormDto>("/forms", HttpMethod.Post, json, _jsonOptions);
        }

        public async Task<ApiResponse<List<CreateFormDto>>> GetFormsAsync()
        {
            return await _customHttpClient.SendApiRequestAsync<List<CreateFormDto>>("/forms", HttpMethod.Get, null, _jsonOptions);
        }

        public async Task<ApiResponse<CreateFormDto>> GetFormByIdAsync(string formId)
        {
            return await _customHttpClient.SendApiRequestAsync<CreateFormDto>($"/forms/{formId}", HttpMethod.Get, null, _jsonOptions);
        }

        public async Task<ApiResponse<CreateFormDto>> UpdateFormAsync(string id, CreateFormDto formDto)
        {
            var json = JsonSerializer.Serialize(formDto, _jsonOptions);
            return await _customHttpClient.SendApiRequestAsync<CreateFormDto>($"/forms/{id}", HttpMethod.Patch, json, _jsonOptions);
        }

        public async Task<ApiResponse<DeleteFormResponse>> DeleteFormAsync(string id)
        {
            return await _customHttpClient.SendApiRequestAsync<DeleteFormResponse>($"/forms/{id}", HttpMethod.Delete, null, _jsonOptions);
        }

        public async Task<ApiResponse<FormResponseSubmissionResponseDto>> SubmitFormResponseAsync(FormResponseSubmissionDto submission)
        {
            var json = JsonSerializer.Serialize(submission, _jsonOptions);
            return await _customHttpClient.SendApiRequestAsync<FormResponseSubmissionResponseDto>("/form-responses", HttpMethod.Post, json, _jsonOptions);
        }

        public async Task<ApiResponse<FormStatisticsDto>> GetFormStatisticsAsync(string formId)
        {
            return await _customHttpClient.SendApiRequestAsync<FormStatisticsDto>($"/form-responses/form/{formId}/statistics", HttpMethod.Get, null, _jsonOptions);
        }

        public async Task<ApiResponse<TextAnswersResponseDto>> GetQuestionTextAnswersAsync(string formId, int position, int offset, int limit)
        {
            var url = $"/form-responses/form/{formId}/question/{position}/text-answers?offset={offset}&limit={limit}";
            return await _customHttpClient.SendApiRequestAsync<TextAnswersResponseDto>(url, HttpMethod.Get, null, _jsonOptions);
        }
    }
}