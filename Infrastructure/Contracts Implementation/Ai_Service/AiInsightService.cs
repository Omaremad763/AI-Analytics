using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

using Application.Contracts;
using Application.Contracts.Ai_Insights;
using Application.DTOS;

using Microsoft.Extensions.Configuration;

namespace Infrastructure.Contracts_Implementation;

public class AiInsightService(
   HttpClient httpClient, 
   IAIAnalyticsServices service,
   IConfiguration configuration
    ) 
 : IAIInsightService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IAIAnalyticsServices _service = service;

    public async Task<string> GetFinancialInsightsAsync(string financialSummaryJson)
    {
        var groqUrl = "https://api.groq.com/openai/v1/chat/completions";

var prompt = $@"Analyze this financial transaction data: {financialSummaryJson}. 
The data includes Categories (like Cloud, Marketing, Services) and Types (Income/Expense).

Please provide:
1. A summary of Total Income vs Total Expenses and the Net Profit.
2. 3 specific insights: Identify the highest expense category and if the income is diversified.
3. 2 strategic tips to improve the cash flow based on these specific vendors and categories.
4. tips to save money.

Provide the response in this EXACT format:
[INSIGHTS]
* insight 1
* insight 2
* insight 3
[TIPS]
* tip 1
* tip 2";
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

        var _apiKey = configuration["GrokKey"];
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);
        var response = await _httpClient.PostAsync(groqUrl, content);
        await response.Content.ReadAsStringAsync();
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