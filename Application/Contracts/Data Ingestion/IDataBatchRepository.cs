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