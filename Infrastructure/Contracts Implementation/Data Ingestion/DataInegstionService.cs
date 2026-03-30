using Application.Contracts;
using Application.Contracts.Data_Ingestion;
using Application.DTOS;

using AutoMapper;

using Domain.Entites;

using Hangfire;

using Microsoft.AspNetCore.Http;

namespace Infrastructure.Contracts_Implementation;

public class DataInegstionService(IUnitofWork unitofWork,
    IBackgroundJobClient backgroundJobClient,
    IAIAnalyticsServices service,
    IMapper mapper) : IDataInegstionService
{
    public async Task<UploadStatusDto?> GetBatchStatusAsync(Guid batchId)
    {
        var batch = await unitofWork.DataBatchRepository.GetByIdAsync(batchId);
        if (batch == null) return null;
        var mapped = mapper.Map<UploadStatusDto>(batch);
        return mapped;
    }

    [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
    public async Task ProcessBatchAsync(Guid batchId, string filePath, CancellationToken ct)
    {
        await unitofWork.DataBatchRepository.UpdateStatusAsync(batchId, BatchStatus.Processing);
        using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            List<FinancialRecordDto>? records = service.ExcelParserService.ParseFinancialFile(stream);
            List<FinancialRecord>? mapping = mapper.Map<List<FinancialRecord>>(records);
            mapping.ForEach(x => x.DataBatchId = batchId);
            await service.DashboardService.BulkInsertRecordsAsync(mapping);
        }
        await unitofWork.DataBatchRepository.UpdateStatusAsync(batchId, BatchStatus.Completed);
        if (File.Exists(filePath)) File.Delete(filePath);
        await unitofWork.CommitAsync();
    }

    public async Task<Guid> SaveFileAsync(IFormFile file, CancellationToken ct)
    {
        var filePath = Path.Combine("Uploads", $"{Guid.NewGuid()}_{file.FileName}");
        var directoryPath = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath!);
        }
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream, ct);
        }

        var batch = new DataBatch
        {
            FileName = file.FileName,
            Status = BatchStatus.Pending,
            FilePath = filePath
        };
        await unitofWork.DataBatchRepository.AddAsync(batch);
        await unitofWork.CommitAsync();
        backgroundJobClient.Enqueue<DataInegstionService>(x => x.ProcessBatchAsync(batch.Id, filePath, CancellationToken.None));
        return batch.Id;
    }
}