using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Entites;

namespace Application.Contracts
{
    public interface IDataBatchRepository
    {
        Task<DataBatch?> GetByIdAsync(Guid id);
        Task AddAsync(DataBatch batch);
        Task UpdateStatusAsync(Guid id, BatchStatus status, string? error = null);
    }
}
