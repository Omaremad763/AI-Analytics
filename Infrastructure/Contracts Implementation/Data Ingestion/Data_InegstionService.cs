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

    public class Data_InegstionService(
        IDataBatchRepository repository,
        IBackgroundJobClient backgroundJobClient,
        IExcelParserService service,
        IMapper mapper
        ) : IData_InegstionService
    {

        public async Task<UploadStatusDto?> GetBatchStatusAsync(Guid batchId)
        {
            var batch = await repository.GetByIdAsync(batchId);
            if (batch == null) return null;
            var mapped = mapper.Map<UploadStatusDto>(batch);
            return mapped;
        }

        public async Task ProcessBatchAsync(Guid batchId, string filePath, CancellationToken ct)
        {
            await repository.UpdateStatusAsync(batchId, BatchStatus.Processing);
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var records = service.ParseFinancialFile(stream);

                // هنا المفروض سطر حفظ الـ records في الداتابيز (المرحلة الثانية)
                // await repository.SaveRecordsAsync(batchId, records);
            }
            await repository.UpdateStatusAsync(batchId, BatchStatus.Completed);
            if (File.Exists(filePath)) File.Delete(filePath);
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
              await repository.AddAsync(batch);
            backgroundJobClient.Enqueue<IData_InegstionService>(service =>
                service.ProcessBatchAsync(batch.Id, filePath,ct));
            return batch.Id;
        }
    }

