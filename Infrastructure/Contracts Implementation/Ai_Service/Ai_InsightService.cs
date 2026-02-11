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
    private readonly string _apiKey;
    private const string GeminiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent";

    public Ai_InsightService(HttpClient httpClient, IConfiguration configuration,IAI_AnalyticsServices service)
    {
        _service = service;
        _httpClient = httpClient;
        _apiKey = configuration["Gemini:ApiKey"]!;
    }
    public async Task<string> GetFinancialInsightsAsync(string financialSummaryJson)
    {
        var prompt = $"Analyze this financial data (JSON) and give me 3 bullet points of insights and 2 tips to save money: {financialSummaryJson}";

        var requestBody = new
        {
            contents = new[]
            {
                new { parts = new[] { new { text = prompt } } }
            }
        };
        var jsonPayload = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{GeminiUrl}?key={_apiKey}", content);
        if (!response.IsSuccessStatusCode) return "Unable to reach AI advisor at the moment.";
        var responseData = await response.Content.ReadFromJsonAsync<JsonElement>();

            return responseData.GetProperty("candidates")[0]
                               .GetProperty("content")
                               .GetProperty("parts")[0]
                               .GetProperty("text").GetString() ?? "No insights generated.";
    }
    public async Task<AIInsightDto> GetAIInsightReportAsync(DateTime start, DateTime end)
    {
        var dashboardData = await _service.DashboardService.GetDashboardDataAsync(start, end);
        var jsonData = JsonSerializer.Serialize(dashboardData);

        var analysis = await GetFinancialInsightsAsync(jsonData);
        return new AIInsightDto(analysis, DateTime.UtcNow, start, end);
    }

}

