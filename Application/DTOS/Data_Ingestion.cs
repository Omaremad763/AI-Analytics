using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOS;
public record FinancialRecordDto
{
    public string Category { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public DateTime TransactionDate { get; init; }
    public string Description { get; init; } = string.Empty;
}

public record UploadStatusDto(
    Guid Id,
    string FileName,
    string Status, // Pending, Processing, Completed, Failed
    string? ErrorMessage,
    DateTime UploadDate,
    DateTime? ProcessedAt
);