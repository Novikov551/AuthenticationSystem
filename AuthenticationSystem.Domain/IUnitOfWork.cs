using AuthenticationSystem.Domain.Core;

namespace AuthenticationSystem.Domain
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        Task SaveAsync(CancellationToken cancellationToken = default);
        Task BulkInsertAsync<T>(IEnumerable<T> data, CancellationToken cancellationToken);
        IRepository<T> Repository<T>() where T : BaseEntity;
    }
}