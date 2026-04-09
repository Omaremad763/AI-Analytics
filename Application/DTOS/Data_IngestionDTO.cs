namespace Application.DTOS;
public record FinancialRecordDto
{
    public string Category { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public DateTime TransactionDate { get; init; } = DateTime.UtcNow;
    public string Description { get; init; } = string.Empty;
    public string Vendor { get; init; } = string.Empty;
    public string Type { get; init; }
}

public record class UploadStatusDto
{
    public Guid Id { get; init; }
    public string FileName { get; init; } = default!;
    public string Status { get; init; } = default!;
    public string? ErrorMessage { get; init; }
    public DateTime UploadDate { get; init; } = DateTime.UtcNow;
    public DateTime? ProcessedAt { get; init; } = DateTime.UtcNow;
    public UploadStatusDto() { }
}