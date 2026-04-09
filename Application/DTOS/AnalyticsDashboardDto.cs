namespace Application.DTOS;

public record AnalyticsDashboardDto(
    decimal TotalIncome,
    decimal TotalExpense,
    decimal NetProfit,
    IEnumerable<DailyTransactionSummaryDto> DailySummaries
);

public record CategoryDistributionDto
{
    public string CategoryName { get; init; } = string.Empty;
    public decimal TotalAmount { get; init; }
    public double Percentage { get; init; }
    public string? Color { get; init; }
}
public record MetricCardDto(string Label, decimal Value, string Icon, string Color);
public record ChartDataDto
{
    public List<string> Labels { get; init; } = new();
    public List<ChartDatasetDto> Datasets { get; init; } = new();
}

public record ChartDatasetDto
{
    public string Label { get; init; } = string.Empty;
    public List<decimal> Data { get; init; } = new();
    public string? BackgroundColor { get; init; }
    public string? BorderColor { get; init; }
}
public record DailyTransactionSummaryDto
{
    public DateTime TransactionDate { get; init; }
    public decimal TotalAmount { get; init; }
    public int TransactionCount { get; init; }
    public DailyTransactionSummaryDto() { }
}