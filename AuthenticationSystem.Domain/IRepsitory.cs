using AuthenticationSystem.Domain.Core;

namespace AuthenticationSystem.Domain
{
    public interface IRepository<T> where T : BaseEntity
    {
        ValueTask<T?> FindAsync(Guid id);
        IQueryable<T> Query { get; }
        T Create(T entity);
        IEnumerable<T> Create(IEnumerable<T> entities);
        T Update(T entity, bool autoVersionUpdate = true);
        IEnumerable<T> Update(IEnumerable<T> entities, bool autoVersionUpdate = true);
        void Remove(T entity);
        void Remove(IEnumerable<T> entities);
        IQueryable<T> FromSqlRaw(string sql);
    }
}
