using System.Net.Http.Headers;
using System.Text;

namespace Quiz.Features.Services
{
    public class CustomHttpClient
    {
        private readonly HttpClient _httpClient;

        public CustomHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}: {errorContent}");
            }

            return response;
        }
    }
}
