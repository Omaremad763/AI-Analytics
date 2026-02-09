using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.DTOS;

using Microsoft.AspNetCore.Http;

namespace Application.Contracts.Data_Ingestion
{
    public interface IData_InegstionService
    {
        Task<Guid> SaveFileAsync(IFormFile file, CancellationToken ct);

        Task ProcessBatchAsync(Guid batchId, string filePath, CancellationToken ct);

        Task<UploadStatusDto?> GetBatchStatusAsync(Guid batchId);
    }

}
