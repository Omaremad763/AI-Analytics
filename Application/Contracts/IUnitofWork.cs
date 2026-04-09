using Application.Contracts.Dashboard;

namespace Application.Contracts;

public interface IUnitofWork : IDisposable
{
    IDashboardRepository DashboardRepository { get; }
    IDataBatchRepository DataBatchRepository { get; }

    Task<int> CommitAsync();
}