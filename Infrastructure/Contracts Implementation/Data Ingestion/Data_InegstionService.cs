using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Application.Contracts;
using Application.Contracts.Data_Ingestion;
using Application.CQRS;
using Application.DTOS;

using AutoMapper;

using Domain.Entites;

using Hangfire;

using MediatR;

using Microsoft.AspNetCore.Http;

namespace Infrastructure.Contracts_Implementation;

    public class Data_InegstionService( IUnitofWork unitofWork,IBackgroundJobClient backgroundJobClient,IAI_AnalyticsServices service,IMapper mapper) : IData_InegstionService
    {
        public async Task<UploadStatusDto?> GetBatchStatusAsync(Guid batchId)
        {
            var batch = await unitofWork.DataBatchRepository.GetByIdAsync(batchId);
            if (batch == null) return null;
            var mapped = mapper.Map<UploadStatusDto>(batch);
            return mapped;
        }
        public async Task ProcessBatchAsync(Guid batchId, string filePath, CancellationToken ct)
        {
            await unitofWork.DataBatchRepository.UpdateStatusAsync(batchId, BatchStatus.Processing);
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var records = service.ExcelParserService.ParseFinancialFile(stream);
                var mapping = mapper.Map<List<FinancialRecord>>(records);
                await service.DashboardService.SaveProcessedRecordsAsync(mapping);
            }
            await unitofWork.DataBatchRepository.UpdateStatusAsync(batchId, BatchStatus.Completed);
            if (File.Exists(filePath)) File.Delete(filePath);
            await unitofWork.CommitAsync();
        }
    public async Task<Guid> SaveFileAsync(IFormFile file, CancellationToken ct)
        {
            var filePath = Path.Combine("Uploads", $"{Guid.NewGuid()}_{file.FileName}");
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream, ct);
            }

            var batch = new DataBatch
            {
                FileName = file.FileName,
                Status = BatchStatus.Pending
            };
              await unitofWork.DataBatchRepository.AddAsync(batch);
              await unitofWork.CommitAsync();
        backgroundJobClient.Enqueue<IData_InegstionService>(service =>service.ProcessBatchAsync(batch.Id, filePath,ct));
            return batch.Id;
        }
    }

