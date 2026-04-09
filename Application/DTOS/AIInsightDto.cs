namespace Application.DTOS;

public record AIInsightDto(
    string AnalysisText,
    DateTime GeneratedAt
);