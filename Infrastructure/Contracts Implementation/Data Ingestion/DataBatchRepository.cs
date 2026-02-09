using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.Contracts;

using Domain.Entites;

using Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contracts_Implementation
{
    public class DataBatchRepository : IDataBatchRepository
    {
        private readonly ApplicationDbContext _context;

        public DataBatchRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataBatch?> GetByIdAsync(Guid id)
        {
            return await _context.DataBatchs.FindAsync(id);
        }

        public async Task AddAsync(DataBatch batch)
        {
            await _context.DataBatchs.AddAsync(batch);
        }

        public async Task UpdateStatusAsync(Guid id, BatchStatus status, string? error = null)
        {
            await _context.DataBatchs
                .Where(b => b.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(b => b.Status, status)
                    .SetProperty(b => b.ErrorMessage, error)
                    .SetProperty(b => b.ProcessedAt, DateTime.UtcNow));
        }
    }
}
