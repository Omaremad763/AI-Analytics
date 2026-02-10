using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.DTOS;

using Domain.Entites;

namespace Application.Contracts.Dashboard
{
    public interface IDashboardService
    {
        Task<AnalyticsDashboardDto> GetDashboardDataAsync(DateTime start, DateTime end);

        // للمرحلة الأولى (Ingestion) - حفظ البيانات الضخمة
        Task SaveProcessedRecordsAsync(List<FinancialRecord> records);

        // لجلب السجلات بالتفصيل لو احتجنا نعرض Table
        Task<IEnumerable<FinancialRecordDto>> GetBatchDetailsAsync(Guid batchId);
    }
}
