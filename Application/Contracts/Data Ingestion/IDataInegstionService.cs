using Application.DTOS;

using Microsoft.AspNetCore.Http;

namespace Application.Contracts.Data_Ingestion
{
    public interface IDataInegstionService
    {
        Task<Guid> SaveFileAsync(IFormFile file, CancellationToken ct);

        Task ProcessBatchAsync(Guid batchId, string filePath, CancellationToken ct);

        Task<UploadStatusDto?> GetBatchStatusAsync(Guid batchId);
    }
}