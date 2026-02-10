using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOS;

public record AIInsightDto(
    string AnalysisText,
    DateTime GeneratedAt,
    DateTime PeriodStart,
    DateTime PeriodEnd
);

