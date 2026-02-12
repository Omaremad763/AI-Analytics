using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

using Application.Contracts;
using Application.Contracts.Ai_Insights;
using Application.DTOS;

using Microsoft.Extensions.Configuration;

namespace Infrastructure.Contracts_Implementation;
    public class Ai_InsightService:IAI_InsightService
    {
    private readonly HttpClient _httpClient;
    private readonly IAI_AnalyticsServices _service;
    public Ai_InsightService(HttpClient httpClient,IAI_AnalyticsServices service)
    {
        _service = service;
        _httpClient = httpClient;
    }
    public async Task<string> GetFinancialInsightsAsync(string financialSummaryJson)
    {
        var groqUrl = "https://api.groq.com/openai/v1/chat/completions";

        var prompt = $"Analyze this financial data (JSON) and give me 3 bullet points of insights and 2 tips to save money: {financialSummaryJson}";

        var requestBody = new
        {
            model = "llama-3.3-70b-versatile",
            messages = new[]
            {
            new { role = "user", content = prompt }
        },
            temperature = 0.5
        };

        var jsonPayload = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        var _apiKey = Environment.GetEnvironmentVariable("GrokKey");
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);
         var response = await _httpClient.PostAsync(groqUrl, content);
        var errorDetails = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode) return $"AI Advisor is resting (Error: {response.StatusCode})";

            var responseData = await response.Content.ReadFromJsonAsync<JsonElement>();
            return responseData
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString() ?? "No insights generated.";
    }
    public async Task<AIInsightDto> GetAIInsightReportAsync(DateTime start, DateTime end)
    {
        var dashboardData = await _service.DashboardService.GetDailyTrendChartAsync(start, end);
        var jsonData = JsonSerializer.Serialize(dashboardData);

        var analysis = await GetFinancialInsightsAsync(jsonData);
        return new AIInsightDto(analysis, DateTime.UtcNow);
    }

}

